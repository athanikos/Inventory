namespace Inventory.Transactions.Dto;

public class TransactionDto
{
    public TransactionDto(
       Guid Id)
    {
        this.Id = Id;
        this.Sections = null;
    }

    public TransactionDto(
        Guid Id ,
        string Description,
        DateTime Created,
        Guid TemplateId,
        ICollection<TransactionSectionDto> Sections
        )
    {
        this.Id = Id;
        this.Description = Description;
        this.Created = Created; 
        this.TemplateId = TemplateId;   
        this.Sections = Sections;       
    }


    public Guid Id { get; set; } = Guid.Empty;

    public string Description { get; set; } = string.Empty; 

    public DateTime Created { get; set; }

    public Guid TemplateId { get; set; }

    public ICollection<TransactionSectionDto> Sections { get; set; } = null;  

}