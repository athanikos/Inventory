using Inventory.Metrics.Endpoints;
using MediatR;
using Inventory.Products.Dto;

namespace Inventory.Metrics.Handlers
{
    internal class AddMetricHandler : IRequestHandler<AddMetricCommand, MetricDto>
    {
        private readonly Products.ProductsDbContext _context;

        public AddMetricHandler(Products.ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<MetricDto> Handle(AddMetricCommand request,
            CancellationToken cancellationToken)
        {
            Products.Entities.Metric m = new Products.Entities.Metric()
            {
                Id = request.Id,
                Code = request.Code,
                Description = request.Description,
                EffectiveDate = request.EffectiveDate,
                Value = request.Value   
            };

            _context.Metrics.Add(m);
            await _context.SaveChangesAsync(cancellationToken);

            return new MetricDto(m.Id,m.Description,m.Value,m.EffectiveDate,m.Code,m.SourceId);
        }
    }
}
