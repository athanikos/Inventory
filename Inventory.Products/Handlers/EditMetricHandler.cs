using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;


namespace Inventory.Products.Handlers
{
    public class EditMetricHandler :
        IRequestHandler<EditMetricCommand, MetricDto>
    {
        private readonly ProductsDbContext  _context;

        public EditMetricHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<MetricDto> Handle(EditMetricCommand request,
            CancellationToken cancellationToken)
        {
            Entities.Metric metric =
                new Entities.Metric()
            {
                         Id= request.Id,
                         Description=request.Description,
                         Value=request.Value,
                         EffectiveDate=request.EffectiveDate,
                         Code = request.Code,
                         SourceId = request.SourceId
            };

            _context.Metrics.Add(metric);
            await _context.SaveChangesAsync(cancellationToken);

            return new MetricDto(metric.Id,metric.Description,metric.Value, metric.EffectiveDate, metric.Code,  metric.SourceId);   
        }
    }
}
