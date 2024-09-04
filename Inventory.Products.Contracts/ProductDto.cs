
namespace Inventory.Products.Contracts.Dto;

public class ProductDto
{

    public ProductDto(Guid Id, string Description, string Code, Guid InventoryId,
        List<ProductMetricDto> Metrics)
    {
        this.Id = Id;   
        this.Description = Description;
        this.Code = Code;
        this.InventoryId = InventoryId;
        this.Metrics = Metrics;
    }

    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public string Code { get; private set; }
    public Guid InventoryId { get; private set; }
    public List<ProductMetricDto> Metrics { get; private set; }

    public  static ProductDto NewProductDto(Guid InventoryId, string productCode)
    {
        return new ProductDto(Guid.NewGuid(),"",
                                     productCode,
                                        InventoryId,
                                     new List<ProductMetricDto>());
    }
}
