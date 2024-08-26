namespace Inventory.Transactions.Dto;


            public class EntityDto
            {
                public Guid Id { get; set; } = Guid.NewGuid();

                public string Description { get; set; } = string.Empty;

                public DateTime Created { get; set; }

                public ICollection<ValueDto> Values { get; set; } = new List<ValueDto>();
            }

