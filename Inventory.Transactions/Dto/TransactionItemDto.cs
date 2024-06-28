namespace Inventory.Transactions.Dto;

public record TransactionItemDto(
                    Guid Id,
                    Guid TransactionId,
                    string  Description = "" ,
                    string  TransactionType="",
                    decimal UnitPrice = 0,
                    decimal Quantity = 0,
                    decimal Price = 0,
                    decimal VatPercentage = 0,
                    decimal PriceAfterVat = 0,
                    decimal Discount = 0,
                    decimal DiscountAmount = 0 ,
                    decimal TransactionFees = 0,
                    decimal DeliveryFees = 0,
                    decimal FinalPrice =0  );


