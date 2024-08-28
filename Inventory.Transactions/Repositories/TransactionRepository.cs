using Inventory.Transactions.Dto;
using Inventory.Transactions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Transactions.Repositories
{
    public class TransactionRepository :
        ITransactionRepository
    {
        private readonly TransactionsDbContext _context;
        
        public TransactionRepository(TransactionsDbContext context)
        { _context = context; }

        public async Task EmptyDB()
        {
            _context.Values.RemoveRange(_context.Values);
            _context.Transactions.RemoveRange(_context.Transactions);
            _context.Fields.RemoveRange(_context.Fields);
            _context.Sections.RemoveRange(_context.Sections);
            _context.Templates.RemoveRange(_context.Templates);
            await _context.SaveChangesAsync();
        }

        public async Task<TemplateDto> GetTemplateAsync(Guid Id)
        {
            var sections = await  GetSectionsAsync(Id);
          
            return   await _context.Templates.Where(o => o.Id == Id)
                                           .Select(i => new TemplateDto(i.Id, i.Name, i.Type, i.Created, sections))
                                           .SingleAsync();
 
        }

       

        public async Task<TemplateDto> AddTemplateAsync(TemplateDto dto)
        {
            var t = new Template() { Id = dto.Id, Created = dto.Created, Name = dto.Name, Type = dto.Type };
            _context.Templates.Add(t);
            await _context.SaveChangesAsync();



            if (dto.Sections != null)
                foreach (var s in dto.Sections)
                {
                    s.TemplateId = t.Id;
                   await AddSection(s);
                }


            return await GetTemplateAsync(t.Id);
        }

        async Task<TemplateDto> ITransactionRepository.EditTemplateAsync(TemplateDto inboundTemplate)
        {
            var inStoreTemplate = await GetTemplateAsync(inboundTemplate.Id);

            foreach (var sec in inboundTemplate.Sections)
            {
                if (inStoreTemplate.Sections.Where(t => sec.TemplateId == t.Id).Any())
                    EditSection(sec);
                else
                     await   AddSection(sec);
            }

            foreach (var storedSection in inStoreTemplate.Sections)
            {
                if (inboundTemplate.Sections.Where(t => storedSection.TemplateId == t.Id).Any() == false)
                    DeleteSection(storedSection);
            }

            await _context.SaveChangesAsync();  
            return await GetTemplateAsync(inStoreTemplate.Id);
        }

        public async Task DeleteTemplateAsync(TemplateDto c)
        {
            Template t = _context.Templates.Where(p => p.Id == c.Id).Single();
            _context.Remove(t);

            List<Field> fs = _context.Fields.Where(p => p.TemplateId == c.Id).ToList();
            foreach (Field f in fs)
                _context.Remove(f);
            await _context.SaveChangesAsync();
        }

        #region Sections 
        public async Task<ICollection<SectionDto>> GetSectionsAsync(Guid TemplateId)
        {
            return await _context.Sections.Where(o => o.TemplateId == TemplateId)
                           .Select(i => new SectionDto()
                           {
                               Id = i.Id,
                               Name = i.Name,
                               TemplateId = i.TemplateId,
                           }).ToListAsync();
        }

        public async Task AddSection(SectionDto c)
        {
            List<Field> fields = new List<Field>();
            Section s = new Section()
            {
                Id = c.Id,
                Name = c.Name,
                TemplateId = c.TemplateId,
                Fields = fields,
                SectionType = c.SectionType
            };
            _context.Sections.Add(s);
            await _context.SaveChangesAsync();

              foreach (var f in c.Fields)
                fields.Add(new Field()
                {
                    Name = f.Name,
                    Expression = f.Expression,
                    Id = f.Id,  
                    SectionId = s.Id,
                    TemplateId= c.TemplateId,   
                    Type    = f.Type,   
                });

            await _context.SaveChangesAsync();

        }

        public void  EditSection(SectionDto inboundSection)
        {

            var inStoreSection = _context.Sections.Where(i => i.Id == inboundSection.Id).Single();

            Section f = new Section()
            { Id = inboundSection.Id, Name = inboundSection.Name, TemplateId = inboundSection.TemplateId, SectionType = inboundSection.SectionType };


            foreach (var field in inboundSection.Fields)
            {
                if (inStoreSection.Fields.Where(t => field.TemplateId == t.Id).Any())
                    EditField(field);
                else
                    AddField(field);
            }

            foreach (var storedField in inStoreSection.Fields)
            {
                if (inboundSection.Fields.Where(t => storedField.TemplateId == t.Id).Any() == false)
                    DeleteField(new FieldDto(storedField.Id));
            }



            _context.Update(f);


        }

        public void DeleteSection(SectionDto dto)
        {
            Section f = _context.Sections.Where(p => p.Id == dto.Id).Single();
            _context.Remove(f);
        }

        #endregion 



        #region Fields 
        public async Task<ICollection<FieldDto>> GetFieldsAsync(Guid TemplateId)
        {
             return await _context.Fields.Where(o => o.TemplateId == TemplateId)
                            .Select(i => new FieldDto() { Id = i.Id,
                                                          Expression =i.Expression, 
                                                          Name = i.Name,
                                                          Type = i.Type,
                                                          TemplateId = i.TemplateId ,
                                                          SectionId = i.SectionId,  
                            }).ToListAsync();
        }

        public void AddField(FieldDto c)
        {
            _context.Fields.Add(new Field()
            {
                Id = c.Id,
                Expression = c.Expression,
                Name = c.Name,
                Type = c.Type,
                SectionId = c.SectionId,    
                TemplateId= c.TemplateId,   
            });
        }

        public void EditField(FieldDto dto)
        {
            Field f = new Field()
            { Id = dto.Id, Name = dto.Name, Expression = dto.Expression, 
              TemplateId = dto.TemplateId, Type = dto.Type, SectionId = dto.SectionId};
            _context.Update(f);
        }

        public void  DeleteField(FieldDto dto)
        {
            Field f = _context.Fields.Where(p => p.Id == dto.Id).Single();
            _context.Remove(f);
        }
        #endregion Fields 

        #region Values

        public async Task<ICollection<ValueDto>> GetTransactionValuesAsync(Guid TransactionId)
        {
            return await _context.Values.Where(o => o.TransactionId == TransactionId)


                //todo use mapper
                           .Select(i => new ValueDto()
                           {
                               Id = i.Id,
                               Field    = new FieldDto() { 
                                                               Expression = i.Field.Expression ,
                                                               Name = i.Field.Name, 
                                                               Type = i.Field.Type,
                                                               Id = i.Id,
                                                               TemplateId = i.Field.TemplateId, 
                                                         },
                               Text = i.Text 


                           })                   
                          .ToListAsync();
        }

        public void AddValue(ValueDto dto)
        { //todo use mapper
            _context.Values.Add(new Value()
            {
                Field = new Field
                {
                    Expression = dto.Field.Expression,
                    Name = dto.Field.Name,
                    Type = dto.Field.Type,
                    Id = dto.Id,
                    TemplateId =dto.Field.TemplateId,
                },
                Text = dto.Text   
            });
        }

        public void EditValue(ValueDto dto)
        {
            //todo use mapper
            Field field = new Field
            {
                Expression = dto.Field.Expression,
                Name = dto.Field.Name,
                Type = dto.Field.Type,
                Id = dto.Id,
                TemplateId = dto.Field.TemplateId,
            };

            Value f = new Value() { Id = dto.Id, Field = field, FieldId = dto.Id };
            _context.Update(f);
        }

        public void DeleteValue(ValueDto dto)
        {
            Value f = _context.Values.Where(p => p.Id == dto.Id).Single();
            _context.Remove(f);
        }

        #endregion Values 


        #region Transaction 

        public async Task<TransactionDto> AddTransactionAsync(TransactionDto dto)
        {
            //todo use mapper
            var t = new Entities.Transaction() { Id = dto.Id, Created = dto.Created, Description = dto.Description };
            _context.Transactions.Add(t);

            foreach (var v in dto.Values)
                AddValue(v);

            await _context.SaveChangesAsync();
            return await GetTransactionAsync(t.Id);

        }

        public async Task<TransactionDto> GetTransactionAsync(Guid Id)
        {
            var values = await GetTransactionValuesAsync(Id);

            return await _context.Transactions.Where(o => o.Id == Id)
                                           .Select(i => new TransactionDto(i.Id, i.Description, i.Created, values))
                                           .SingleAsync();

        }

        public async Task<TransactionDto> EditTransactionAsync(TransactionDto inboundTransaction)
        {
            var inStoreTransaction = await GetTransactionAsync(inboundTransaction.Id);

            foreach (var value in inboundTransaction.Values)
            {
                if (inStoreTransaction.Values.Where(v => value.Id == v.Id).Any())
                    EditValue(value);
                else
                    AddValue(value);
            }

            foreach (var storedTransaction in inStoreTransaction.Values)
            {
                if (inboundTransaction.Values.Where(v => storedTransaction.Id == v.Id).Any() == false)
                    DeleteValue(storedTransaction);
            }


            await _context.SaveChangesAsync();

            return await GetTransactionAsync(inStoreTransaction.Id);
        }

        public async Task DeleteTransactionAsync(TransactionDto dto)
        {
            Entities.Transaction t = _context.Transactions.Where(p => p.Id == dto.Id).Single();   
            _context.Remove(t);

            List<Value> vs = _context.Values.Where(p => p.TransactionId == dto.Id).ToList();
            foreach (Value v in vs)
                _context.Remove(v);
            await _context.SaveChangesAsync();
        }

        #endregion Transaction 

  
    }
}
