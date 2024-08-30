using Inventory.Transactions.Dto;
using Inventory.Transactions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Timers;

namespace Inventory.Transactions.Repositories.Postgres
{
    public class PostgresTransactionRepository :
        ITransactionRepository
    {
        private readonly TransactionsDbContext _context;

        public PostgresTransactionRepository(TransactionsDbContext context)
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
            var sections = await GetSectionsAsync(Id);

            return await _context.Templates.Where(o => o.Id == Id)
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
                               SectionType = i.SectionType,
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
                    TemplateId = c.TemplateId,
                    Type = f.Type,
                });

            await _context.SaveChangesAsync();

        }

        public void EditSection(SectionDto inboundSection)
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
                           .Select(i => new FieldDto()
                           {
                               Id = i.Id,
                               Expression = i.Expression,
                               Name = i.Name,
                               Type = i.Type,
                               TemplateId = i.TemplateId,
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
                TemplateId = c.TemplateId,
            });
        }

        public void EditField(FieldDto dto)
        {
            Field f = new Field()
            {
                Id = dto.Id,
                Name = dto.Name,
                Expression = dto.Expression,
                TemplateId = dto.TemplateId,
                Type = dto.Type,
                SectionId = dto.SectionId
            };
            _context.Update(f);
        }

        public void DeleteField(FieldDto dto)
        {
            Field f = _context.Fields.Where(p => p.Id == dto.Id).Single();
            _context.Remove(f);
        }
        #endregion Fields 

        #region Values

        public async Task<ICollection<ValueDto>> GetTransactionValuesAsync(Guid TransactionId)
        {

            var query = from value in _context.Values
                        where value.TransactionId == TransactionId
                        join sectionGroup in _context.TransactionSectionGroups
                        on value.TransactionSectionGroupId equals sectionGroup.Id
                        join section in _context.TransactionSections
                        on sectionGroup.Id equals section.Id
                        select new ValueDto()
                        {
                            Id = value.Id,
                            FieldId = value.FieldId,
                            GroupValue = sectionGroup.GroupValue,
                            Text = value.Text,
                            TransactionId = TransactionId,
                            TransactionSectionGroupId = value.TransactionSectionGroupId,
                            TransactionSectionType = section.TransactionSectionType
                        };


            return await query.ToListAsync();

        }

        public void AddValue(ValueDto dto)
        { //todo use mapper
            _context.Values.Add(new Value()
            {
                FieldId = dto.FieldId,
                Text = dto.Text,
                TransactionId = dto.TransactionId,
                TransactionSectionGroupId = dto.TransactionSectionGroupId,

            });
        }

        public void EditValue(ValueDto dto)
        {
            _context.Update(new Value()
            {
                FieldId = dto.FieldId,
                Text = dto.Text,
                TransactionId = dto.TransactionId,
                TransactionSectionGroupId = dto.TransactionSectionGroupId,
            });
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
            var t = new Entities.Transaction()
            {
                Id = dto.Id,
                Created = dto.Created,
                Description = dto.Description,
                TemplateId = dto.TemplateId

            };
            _context.Transactions.Add(t);
            await _context.SaveChangesAsync(); // to save id 

            foreach (var ts in dto.Sections)
                await AddTransactionSectionAsync(
                                    new TransactionSectionDto()
                                    {
                                        Id = ts.Id,
                                        TransactionId = t.Id,
                                        TransactionSectionType = ts.TransactionSectionType,
                                        SectionGroups = ts.SectionGroups,
                                    }
                    );

            await _context.SaveChangesAsync();
            return await GetTransactionAsync(t.Id);

        }

        public async Task<TransactionDto> GetTransactionAsync(Guid Id)
        {
            var transactionSections = await GetTransactionSectionsAsync(Id);

            return await _context.Transactions.Where(o => o.Id == Id)
                                           .Select(i =>
                                                        new TransactionDto(
                                                        i.Id,
                                                        i.Description,
                                                        i.Created,
                                                        i.TemplateId,
                                                        transactionSections)
                                                 )
                                           .SingleAsync();

        }

        public async Task<TemplateDto> EditTemplateAsync(TemplateDto inboundTemplate)
        {
            var inStoreTemplate = await GetTemplateAsync(inboundTemplate.Id);

            foreach (var sec in inboundTemplate.Sections)
            {
                if (inStoreTemplate.Sections.Where(t => sec.TemplateId == t.Id).Any())
                    EditSection(sec);
                else
                    await AddSection(sec);
            }

            foreach (var storedSection in inStoreTemplate.Sections)
            {
                if (inboundTemplate.Sections.Where(t => storedSection.TemplateId == t.Id).Any() == false)
                    DeleteSection(storedSection);
            }

            await _context.SaveChangesAsync();
            return await GetTemplateAsync(inStoreTemplate.Id);
        }

        public async Task<TransactionDto> EditTransactionAsync(TransactionDto inboundTransaction)
        {
            var inStoreTransaction = await GetTransactionAsync(inboundTransaction.Id);

            foreach (var sec in inboundTransaction.Sections)
            {
                if (inStoreTransaction.Sections.Where(v => sec.Id == v.Id).Any())
                    await EditTransactionSectionAsync(sec);
                else
                    await AddTransactionSectionAsync(sec);
            }

            foreach (var storedSection in inStoreTransaction.Sections)
            {
                if (inboundTransaction.Sections.Where(s => storedSection.Id == s.TransactionId).Any() == false)
                    await DeleteTransactionSectionAsync(storedSection);
            }

            await _context.SaveChangesAsync();
            return await GetTransactionAsync(inStoreTransaction.Id);
        }

        //public async Task DeleteTransactionAsync(TransactionDto dto)
        //{
        //    Entities.Transaction t = _context.Transactions.Where(p => p.Id == dto.Id).Single();   
        //    _context.Remove(t);

        //    List<Value> vs = _context.Values.Where(p => p.TransactionId == dto.Id).ToList();
        //    foreach (Value v in vs)
        //        _context.Remove(v);
        //    await _context.SaveChangesAsync();
        //}


        #endregion Transaction 


        #region TransactionSection 
        public async Task<ICollection<TransactionSectionDto>> GetTransactionSectionsAsync(Guid TransactionId)
        {
            var query = from ts in _context.TransactionSections
                        where ts.TransactionId == TransactionId
                        select new TransactionSectionDto()
                        {
                            Id = ts.Id,
                            TransactionId = TransactionId,
                            TransactionSectionType = ts.TransactionSectionType,
                            // Transaction = null
                        };


            return await query.ToListAsync();

        }

        public async Task<TransactionSectionDto> AddTransactionSectionAsync(TransactionSectionDto dto)
        {
            TransactionSection ts = new TransactionSection()
            {
                Id = dto.Id,
                TransactionId = dto.TransactionId,
                TransactionSectionType = dto.TransactionSectionType,
            };
            _context.Add(ts);
            await _context.SaveChangesAsync();

            foreach (var sg in dto.SectionGroups)
            {
                sg.TransactionSectionId = ts.Id;
                await AddTransactionSectionGroupAsync(sg);
            }
            return dto;
        }

        public async Task<TransactionSectionDto> EditTransactionSectionAsync(TransactionSectionDto dto)
        {
            TransactionSection ts = new TransactionSection()
            {
                Id = dto.Id,
                TransactionId = dto.TransactionId,
                TransactionSectionType = dto.TransactionSectionType,
            };
            _context.Update(ts);

            foreach (var sg in dto.SectionGroups)
            {
                if (sg.Id == Guid.Empty)                
                    await AddTransactionSectionGroupAsync(sg);
                else
                    await EditTransactionSectionGroupAsync(sg);

            }

            return dto;


        }


        public async Task DeleteTransactionSectionAsync(TransactionSectionDto dto)
        {
            throw new NotImplementedException();    
            //List<Value> vs = _context.Values.Where(p => p.TransactionId == dto.Id).ToList();
            //foreach (Value v in vs)
            //    _context.Remove(v);
            //await _context.SaveChangesAsync();
        }

        #endregion



        #region TransactionSectionGroup 
        public async Task AddTransactionSectionGroupAsync(TransactionSectionGroupDto dto)
        {
            var e = new TransactionSectionGroup()
            {
                GroupValue = dto.GroupValue,
                Id = dto.Id,
                TransactionSectionId = dto.TransactionSectionId,
                TransactionSection = null
            };

            _context.Add(e);
            await _context.SaveChangesAsync();

            foreach (var v in dto.Values)
            {
                v.TransactionSectionGroupId = e.Id;
                if (v.Id == Guid.Empty)
                    AddValue(v);
                else
                    EditValue(v);
            }
            await _context.SaveChangesAsync();
        }

        public async Task EditTransactionSectionGroupAsync(TransactionSectionGroupDto dto)
        {
            var e = new TransactionSectionGroup()
            {
                GroupValue = dto.GroupValue,
                Id = dto.Id,
                TransactionSectionId = dto.TransactionSectionId,
                TransactionSection = null
            };

            _context.Update(e);
            await _context.SaveChangesAsync();

            foreach (var v in dto.Values)
            {
                v.TransactionSectionGroupId = e.Id;
                AddValue(v);
            }
            await _context.SaveChangesAsync();
        }

        public Task DeleteTransactionAsync(TransactionDto c)
        {
            throw new NotImplementedException();
        }

        #endregion





    }
}
