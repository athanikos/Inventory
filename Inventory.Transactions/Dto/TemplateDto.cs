using Inventory.Transactions.Dto;
using System.Text.Json.Serialization;


public class TemplateDto
{

    public TemplateDto(Guid Id)
    { this.Id = Id; }


    public TemplateDto(Guid Id , string Name , TemplateType Type, DateTime Created, ICollection<SectionDto> Sections) 
    {
        this.Id = Id;
        this.Name = Name;
        this.Type = Type;   
        this.Created = Created;
        this.Sections = Sections;
    }    


    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
 
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TemplateType Type { get; set; }

    public DateTime Created { get; set; }
    
    public ICollection<SectionDto> Sections { get; set; }

}

