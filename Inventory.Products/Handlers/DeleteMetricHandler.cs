using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using FastEndpoints;
using Inventory.Products.Entities;
using Inventory.Metrics.Endpoints;


namespace Inventory.Products.Handlers
{ 
    internal class DeleteMetricHandler :
        IRequestHandler<DeleteMetricCommand, MetricDto>
    {
        private readonly ProductsDbContext  _context;

        public DeleteMetricHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<MetricDto> Handle(DeleteMetricCommand request,
            CancellationToken cancellationToken)
        {
            Entities.Metric metric =
                new Entities.Metric()
            {
                         Id= request.Id
                     
            };

            _context.Metrics.Add(metric);
            await _context.SaveChangesAsync(cancellationToken);

            return new MetricDto(metric.Id,metric.Description,metric.Value, metric.EffectiveDate, metric.Code,  metric.SourceId);   
        }
    }
}
