namespace Inventory.Products.Dto;

public record TransactionItemDto(
                    Guid    TransactionId,
                    Guid    Id,
                    string  Description ,
                    string  TransactionType,
                    decimal UnitPrice,
                    decimal Quantity,
                    decimal Price,
                    decimal VatPercentage,
                    decimal PriceAfterVat,
                    decimal Discount,
                    decimal DiscountAmount ,
                    decimal TransactionFees,
                    decimal DeliveryFees,
                    decimal FinalPrice  );


