using Inventory.Transactions.Dto;
using Inventory.Transactions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Transactions.Repositories
{
    public class TransactionRepository :
        ITransactionRepository
    {
        private readonly Transactions.TransactionsDbContext _context;
        
        public TransactionRepository(TransactionsDbContext context)
        { _context = context; }



        public async Task<TemplateDto> GetTemplateAsync(Guid Id)

        {
         
            throw new NotImplementedException();      
        }

        
        public async Task<TemplateDto> AddTemplateAsync(TemplateDto dto)
        {
            _context.Templates.Add(new Entities.Template() { Id = dto.Id, Created = dto.Created, Name = dto.Name, Type = dto.Type });
            await _context.SaveChangesAsync();

            foreach (var field in dto.fields)
                AddField(field);
            

            await _context.SaveChangesAsync();

            //todo fetch template 
            return new TemplateDto(dto.Id, dto.Name, dto.Type, dto.Created,dto.fields);
        }

        Task<TemplateDto> ITransactionRepository.EditTemplateAsync(TemplateDto c)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteTemplateAsync(TemplateDto c)
        {
            Entities.Template t = _context.Templates.Where(p => p.Id == c.Id).Single();
            _context.Remove(t);

            List<Entities.Field> fs = _context.Fields.Where(p => p.TemplateId == c.Id).ToList();
            foreach (Entities.Field f in fs)
                _context.Remove(f);
            await _context.SaveChangesAsync();
        }





        #region Fields 
        public async Task<ICollection<FieldDto>> GetFieldsAsync(Guid TemplateId)
        {
             return await _context.Fields.Where(o => o.TemplateId == TemplateId)
                            .Select(i => new FieldDto() { Id = i.Id,
                                                          Expression =i.Expression, 
                                                          Name = i.Name,
                                                          Type = i.Type,
                                                          TemplateId = i.TemplateId 
                            }).ToListAsync();
        }

        public void AddField(FieldDto c)
        {
            _context.Fields.Add(new Field()
            {
                Id = c.Id,
                Expression = c.Expression,
                Name = c.Name,
                Type = c.Type
            });
        }

        public void EditField(FieldDto dto)
        {
            Field f = new Field()
            { Id = dto.Id, Name = dto.Name, Expression = dto.Expression, TemplateId = dto.TemplateId, Type = dto.Type};
            _context.Update(f);
        }

        public void  DeleteField(FieldDto dto)
        {
            Field f = _context.Fields.Where(p => p.Id == dto.Id).Single();
            _context.Remove(f);
        }
        #endregion Fields 
        
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
            //List<Entities.TransactionItem> tis = _context.TransactionItems.Where(p=>p.TransactionId == c.Id).ToList();
            //foreach (Entities.TransactionItem ti in tis)
            //    _context.Remove(ti);
            //todo for values 

            Entities.Transaction t = _context.Transactions.Where(p => p.Id == c.Id).Single();
            _context.Remove(t);
             await _context.SaveChangesAsync();           
        }

        public Task<TransactionItemTemplateDto> AddTransactionItemTemplateAsync(TransactionItemTemplateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionItemDto> AddTransactionItemAsync(TransactionItemDto ti)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionItemDto> EditTransactionItemAsync(TransactionItemDto ti)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTransactionItemAsync(TransactionItemDto ti)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionDto> AddEntityAsync(EntityDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionDto> EditEntityAsync(EntityDto c)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEntityAsync(EntityDto c)
        {
            throw new NotImplementedException();
        }
    }
}
