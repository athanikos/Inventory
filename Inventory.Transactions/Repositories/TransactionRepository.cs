using Inventory.Transactions.Dto;

namespace Inventory.Transactions.Repositories
{
    public class TransactionRepository :
        ITransactionRepository
    {
        private readonly Transactions.TransactionsDbContext _context;
        
        public TransactionRepository(TransactionsDbContext context)
        { _context = context; }



        public async Task<TransactionDto> AddTransactionAsync(TransactionDto c)
        {
            _context.Transactions.Add(new Entities.Transaction() { Id = c.Id, Created = c.Created, Description = c.Description });
            await _context.SaveChangesAsync();
            return new TransactionDto(c.Id, c.Description,c.Created);
            
         }

        public async Task<TransactionDto> EditTransactionAsync(TransactionDto c)
        {
            Entities.Transaction e = new Entities.Transaction() { Id = c.Id, Created = c.Created, Description = c.Description };
            _context.Update(e);
             await _context.SaveChangesAsync();
            return new TransactionDto(c.Id, c.Description, c.Created);
        }


        public async Task DeleteTransactionAsync(TransactionDto c)
        {
            List<Entities.TransactionItem> tis = _context.TransactionItems.Where(p=>p.TransactionId == c.Id).ToList();
            foreach (Entities.TransactionItem ti in tis)
                _context.Remove(ti);

            Entities.Transaction t = _context.Transactions.Where(p => p.Id == c.Id).Single();
            _context.Remove(t);
             await _context.SaveChangesAsync();           
        }

        public async Task<TransactionItemDto> AddTransactionItemAsync(TransactionItemDto ti)
        {
            throw new NotImplementedException();
          //  Entities.TransactionItem trans =
          //new Entities.TransactionItem()
          //{
          //    //Description = ti.Description
          //   // ,
          //    Id = ti.Id
          //    ,
          //    TransactionId = ti.TransactionId
          //    ,
          //    //TransactionType = ti.TransactionType
          //    //,
          //    //UnitPrice = ti.UnitPrice
          //    //,
          //    //Quantity = ti.Quantity
          //    //,
          //    //Price = ti.Price
          //    //,
          //    //VatPercentage = ti.VatPercentage
          //    //,
          //    //PriceAfterVat = ti.PriceAfterVat
          //    //,
          //    //Discount = ti.Discount
          //    //,
          //    //DiscountAmount = ti.DiscountAmount
          //    //,
          //    //TransactionFees = ti.TransactionFees
          //    //,
          //    //DeliveryFees = ti.DeliveryFees
          //    //,
          //    //FinalPrice = ti.FinalPrice
          //};

            //_context.TransactionItems.Add(trans);
            await _context.SaveChangesAsync();

            //return new TransactionItemDto(
            //           trans.TransactionId,
            //           trans.Id
            //           //trans.Description,
            //           //trans.TransactionType,
            //           //trans.UnitPrice,
            //           //trans.Quantity,
            //           //trans.Price,
            //           //trans.VatPercentage,
            //           //trans.PriceAfterVat,
            //           //trans.Discount,
            //           //trans.DiscountAmount,
            //           //trans.TransactionFees,
            //           //trans.DeliveryFees,
            //           //trans.FinalPrice
            //           );
        }


        public async Task<TransactionItemDto> 
            EditTransactionItemAsync(TransactionItemDto ti)
        {

            throw new NotImplementedException();
         //Entities.TransactionItem trns =
         //new Entities.TransactionItem()
         //{
         //    //Description = ti.Description
         //    //,
         //    Id = ti.Id
         //    ,
         //    TransactionId = ti.TransactionId
             
         //    //TransactionType = ti.TransactionType
         //    //,
         //    //UnitPrice = ti.UnitPrice
         //    //,
         //    //Quantity = ti.Quantity
         //    //,
         //    //Price = ti.Price
         //    //,
         //    //VatPercentage = ti.VatPercentage
         //    //,
         //    //PriceAfterVat = ti.PriceAfterVat
         //    //,
         //    //Discount = ti.Discount
         //    //,
         //    //DiscountAmount = ti.DiscountAmount
         //    //,
         //    //TransactionFees = ti.TransactionFees
         //    //,
         //    //DeliveryFees = ti.DeliveryFees
         //    //,
         //    //FinalPrice = ti.FinalPrice
         //};

         //_context.TransactionItems.Add(trns);
         //await _context.SaveChangesAsync();

         //   return new TransactionItemDto(
         //              trns.TransactionId,
         //              trns.Id
         //              //trns.Description,
         //              //trns.TransactionType,
         //              //trns.UnitPrice,
         //              //trns.Quantity,
         //              //trns.Price,
         
         //              //trns.VatPercentage,
         //              //trns.PriceAfterVat,
         //              //trns.Discount,
         //              //trns.DiscountAmount,
         //              //trns.TransactionFees,
         //              //trns.DeliveryFees,
         //              //trns.FinalPrice
         //              );
        }

        public async Task DeleteTransactionItemAsync(TransactionItemDto ti)
        {
            throw new NotImplementedException();
            //var itemToRemove = _context.TransactionItems.
            //    SingleOrDefault(x => x.Id == ti.Id);
            //if (itemToRemove != null)
            //{
            //    _context.TransactionItems.Remove(itemToRemove);
            //    await _context.SaveChangesAsync();
            //}
        }


    }
}
