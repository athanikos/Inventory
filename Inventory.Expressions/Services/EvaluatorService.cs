﻿using System.Globalization;
using Hangfire;
using Inventory.Expressions.Repositories;
using Inventory.Notifications.Contracts;
using Inventory.Products.Contracts;
using MediatR;
using Serilog;

namespace Inventory.Expressions.Services
{
    public class EvaluatorService(IMediator mediator, IExpressionRepository repo) : IEvaluatorService
    {
        private const string OpenBracket = "[";
        private const string ClosingBracket = "]";
        private const char Comma = ',';
        private const string OpenParenthesis = "(";
        private const string Sum = "SUM";
        private readonly char[] _operators = { '*', '/', '+', '-', '>', '<' };
        private readonly string[] _aggregateFunctions = { "SUM", "AVG" };
        private readonly string _allSpecifier = "[ALL]";
        private string _expression = string.Empty;
        private List<string> _allProductCodes = new();
        private List<string> _allMetricCodes = new();
        private Guid _inventoryId;

        public async Task<EvaluatorResult> Execute(Guid inventoryId, string expression)
        {
            _expression = expression;
            _inventoryId = inventoryId;
            await GetCodes();
            return await ComputeTokens(BreakExpressionIntoListOfStrings());
        }

        private List<string> BreakExpressionIntoListOfStrings()
        {
            List<string> resultedList = new List<string>();
            string currentToken = string.Empty;

            foreach (var t in _expression)
            {
                if (_operators.Contains(t))
                {
                    if (!string.IsNullOrEmpty(currentToken))
                        resultedList.Add(currentToken);

                    currentToken = string.Empty;
                    resultedList.Add(t.ToString());
                }
                else
                {
                    currentToken += t.ToString();
                }
            }

            if (!string.IsNullOrEmpty(currentToken))
                resultedList.Add(currentToken);

            return resultedList;
        }

        private async Task<EvaluatorResult> ComputeTokens(List<string> tokens)
        {
            var resultedExpression = string.Empty;
            foreach (var token in tokens)
            {
                if (IsNumeric(token) || IsOperator(token))
                {
                    resultedExpression += token.Trim();
                }
                else if (IsInventoryBasedFormula(token))
                {
                    var er = await ComputeComplexFunction(token, ExtractAggregateFunction(token));
                    resultedExpression += er.Result.Trim();
                }
                else if (IsProductBasedFormula(token))
                {
                    var er2 = await ComputeSimpleFunction(_inventoryId, token);
                    resultedExpression += er2.Result.Trim();
                }
            }

            if (string.IsNullOrEmpty(resultedExpression))
                return EvaluatorResult.NewUndefinedResult();

            try
            {
                var value = new NCalc.Expression(resultedExpression).Evaluate().ToString();
                return EvaluatorResult.NewEvaluatorResult(value);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return EvaluatorResult.NewUndefinedResult();
            }
        }

        private async Task GetCodes()
        {
            var response = await mediator.Send(new CodesQuery(_inventoryId));
            if (response == null)
                throw new ArgumentNullException();

            _allProductCodes = response.ProductCodes;
            _allMetricCodes = response.MetricCodes;
        }

        private string ExtractAggregateFunction(string token)
        {
            int indexOfParenthesis = token.IndexOf(OpenParenthesis, StringComparison.Ordinal);
            return token.Substring(0, indexOfParenthesis);
        }

        private async Task<EvaluatorResult> ComputeComplexFunction(string token, string aggregateFunction)
        {
            List<string> productCodes;
            string metricCode;

            try
            {
                metricCode = ExtractMetricCode(token);
                productCodes = ExtractProductCodes(token);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return EvaluatorResult.NewUndefinedResult();
            }

            string result = string.Empty;
            foreach (var productCode in productCodes)
            {
                if (aggregateFunction == Sum)
                {
                    try
                    {
                        var dto = await mediator.Send(new GetProductMetricQuery(productCode, metricCode));
                        result += !string.IsNullOrEmpty(result) ? "+" + dto.Value.ToString("F2", CultureInfo.InvariantCulture) : dto.Value.ToString("F2", CultureInfo.InvariantCulture);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        return EvaluatorResult.NewUndefinedResult();
                    }
                }
                else
                {
                    throw new ArgumentException("aggregate function not supported: " + aggregateFunction);
                }
            }
            return EvaluatorResult.NewEvaluatorResult(result);
        }

        private string ExtractMetricCode(string token)
        {
            foreach (var item in _allMetricCodes)
                if (token.Contains(item))
                    return item;

            throw new ArgumentException(token);
        }

        private List<string> ExtractProductCodes(string token)
        {
            List<string> items = new List<string>();
            if (token.Contains(_allSpecifier))
                items.AddRange(_allProductCodes);
            else
                items.AddRange(ExtractProducts(token).Split(Comma));
            return items;
        }

        private static string ExtractProducts(string token)
        {
            int startIndexOfProducts = token.IndexOf(OpenBracket, StringComparison.Ordinal) + 1;
            int endIndexOfProducts = token.IndexOf(ClosingBracket, StringComparison.Ordinal) - 1;
            var length = endIndexOfProducts - startIndexOfProducts;
            return token.Substring(startIndexOfProducts, length + 1);
        }

        private async Task<EvaluatorResult> ComputeSimpleFunction(Guid inventoryId, string token)
        {
            string productCode = string.Empty;
            string metricCode = string.Empty;

            foreach (var item in _allProductCodes)
                if (token.Contains(item))
                    productCode = item;

            foreach (var item in _allMetricCodes)
                if (token.Contains(item))
                    metricCode = item;

            try
            {
                var result = (await mediator.Send(new GetProductMetricQuery(productCode, metricCode))).Value.ToString("F2", CultureInfo.InvariantCulture);
                return EvaluatorResult.NewEvaluatorResult(result);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return EvaluatorResult.NewUndefinedResult();
            }
        }

        private ExpressionType _type = ExpressionType.Undefined;

        private enum ExpressionType
        {
            Undefined = -1,
            ProductBased = 0,
            InventoryBased = 1
        }

        private bool IsProductBasedFormula(string token)
        {
            if (IsInventoryBasedFormula(token))
                _type = ExpressionType.InventoryBased;

            foreach (var item in _allProductCodes)
                if (token.Contains(item))
                    _type = ExpressionType.ProductBased;

            foreach (var item in _allMetricCodes)
                if (token.Contains(item))
                    _type = ExpressionType.ProductBased;

            return _type == ExpressionType.ProductBased;
        }

        private bool IsOperator(string token)
        {
            return token.Length == 1 && _operators.Contains(token[0]);
        }

        private bool IsNumeric(string token)
        {
            return decimal.TryParse(token, out _);
        }

        private bool IsInventoryBasedFormula(string token)
        {
            foreach (var item in _aggregateFunctions)
                if (token.Contains(item))
                {
                    _type = ExpressionType.InventoryBased;
                    return true;
                }

            if (token.Contains(_allSpecifier))
            {
                _type = ExpressionType.InventoryBased;
                return true;
            }

            int numberOfProducts = 0;
            foreach (var item in _allProductCodes)
                if (token.Contains(item))
                    numberOfProducts++;

            if (numberOfProducts > 1)
            {
                _type = ExpressionType.InventoryBased;
                return true;
            }

            return false;
        }

        public void ScheduleJobs(IServiceProvider serviceProvider)
        {
            var productExpressions = repo.GetProductExpressions();
            foreach (var p in productExpressions)
                RecurringJob.AddOrUpdate(p.Id.ToString(), () => DoScheduledWork(p), Cron.Minutely);

            var inventoryExpressions = repo.GetInventoryExpressions();
            foreach (var i in inventoryExpressions)
                RecurringJob.AddOrUpdate(i.Id.ToString(), () => DoScheduledWork(i), Cron.Minutely);

            var booleanExpressions = repo.GetBooleanExpressions();
            foreach (var i in booleanExpressions)
                RecurringJob.AddOrUpdate(i.Id.ToString(), () => DoScheduledWork(i), Cron.Minutely);
        }

        public void DoScheduledWork(global::Inventory.Expressions.Entities.InventoryExpression ie)
        {
            try
            {
                DoScheduledWorkAsync(ie).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        public void DoScheduledWork(global::Inventory.Expressions.Entities.ProductExpression pe)
        {
            try
            {
                DoScheduledWorkAsync(pe).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        public void DoScheduledWork(global::Inventory.Expressions.Entities.BooleanExpression be)
        {
            try
            {
                DoScheduledWorkAsync(be).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        public async Task DoScheduledWorkAsync(global::Inventory.Expressions.Entities.InventoryExpression p)
        {
            var result = await Execute(p.TargetInventoryId, p.Expression);

            if (_type == ExpressionType.Undefined)
                throw new ArgumentException(_type.ToString());

            if (_type == ExpressionType.InventoryBased)
            {
                var command = new AddInventoryMetricCommand(p.TargetInventoryId, p.TargetMetricId, decimal.Parse(result.Result), DateTime.Now);
                await mediator.Send(command);
            }
        }

        public async Task DoScheduledWorkAsync(global::Inventory.Expressions.Entities.ProductExpression p)
        {
            var result = await Execute(p.InventoryId, p.Expression);

            if (_type == ExpressionType.Undefined)
                throw new ArgumentException(_type.ToString());

            if (result.Type == EvaluatorResult.EvaluatorResultType.undefined)
                return;

            if (_type == ExpressionType.ProductBased)
            {
                var command = new AddProductMetricCommand(p.TargetProductId, p.TargetMetricId, decimal.Parse(result.Result), DateTime.Now, Constants.EmptyUnityOfMeasurementId);
                await mediator.Send(command);
            }
        }

        public async Task DoScheduledWorkAsync(global::Inventory.Expressions.Entities.BooleanExpression p)
        {
            var result = await Execute(p.InventoryId, p.Expression);

            if (_type == ExpressionType.Undefined)
                throw new ArgumentException(_type.ToString());

            if (result.Type == EvaluatorResult.EvaluatorResultType.undefined)
                return;

            if (_type == ExpressionType.ProductBased)
            {
                var command = new UpdateNotificationExpressionValueCommand
                {
                    ExpressionValue = bool.Parse(result.Result),
                    BooleanExpressionId = p.Id
                };
                await mediator.Send(command);
            }
        }

        public void DoScheduledWork()
        {
            try
            {
                foreach (var p in repo.GetProductExpressions())
                    DoScheduledWork(p);

                foreach (var i in repo.GetInventoryExpressions())
                    DoScheduledWork(i);

                foreach (var b in repo.GetBooleanExpressions())
                    DoScheduledWork(b);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }
    }

}
