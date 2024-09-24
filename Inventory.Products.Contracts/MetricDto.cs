
namespace Inventory.Products.Contracts.Dto;

public  class MetricDto
 {
    public MetricDto(Guid id, string description, string code, Guid sourceId)
    {
        Id = id;
        Description = description;
        Code = code;
        SourceId = sourceId;
    }

    public MetricDto(Guid id)
    {
        Id = id;
    }

    public static MetricDto NewMetricDto(Guid SourceId, string MetricCode)
    {
        return new MetricDto(Guid.NewGuid(), "", MetricCode, SourceId);
    }

    public Guid Id { get; internal set; }  = Guid.NewGuid();
    public string Description { get; internal set; }  = string.Empty;
    public string Code { get; internal set; }   = string.Empty;
    /// <summary>
    /// System the attribute value came from 
    /// </summary>
    public Guid SourceId { get; set; }
  }