using Inventory.Transactions.Dto;
using Inventory.Transactions.Entities;
using Microsoft.EntityFrameworkCore;

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

        public void AddValue(ValueDto dto, TransactionSectionGroup tsg)
        { //todo use mapper
            tsg.Values.Add(

                new Value()
                {
                    FieldId = dto.FieldId,
                    Text = dto.Text,
                    TransactionId = dto.TransactionId,
                    TransactionSectionGroupId = dto.TransactionSectionGroupId,
                }


                );

        }

        public void EditValue(ValueDto dto, TransactionSectionGroup tsg)
        {
            var v = new Value()
            {
                Id = dto.Id,
                FieldId = dto.FieldId,
                Text = dto.Text,
                TransactionId = dto.TransactionId,
                TransactionSectionGroupId = tsg.Id
            };

            tsg.Values.Add(v);
            _context.Entry(v).State = EntityState.Modified;
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
            foreach (var ts in dto.Sections)
                AddTransactionSection(
                                   t
                                   , new TransactionSectionDto()
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

        public async Task<TransactionDto> EditTransactionAsync(TransactionDto inboundTransaction)
        {
            Entities.Transaction inStoreTransaction = await _context.Transactions.Where(p => p.Id == inboundTransaction.Id).SingleAsync();

            foreach (var sec in inboundTransaction.Sections)
            {
                if (sec.Id == Guid.Empty)
                    AddTransactionSection(inStoreTransaction, sec);
                else
                    EditTransactionSection(sec, inStoreTransaction);

            }

            foreach (var storedSection in inStoreTransaction.TransactionSections)
            {
                if (inboundTransaction.Sections
                                      .Where(s => storedSection.TransactionId == s.TransactionId)
                                      .Any() == false)
                    DeleteTransactionSection(storedSection);
            }

            await _context.SaveChangesAsync();
            return await GetTransactionAsync(inStoreTransaction.Id);
        }

        public async Task  DeleteTransactionAsync(TransactionDto c)
        {
             throw new NotImplementedException();
             await Task.CompletedTask;
        }

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
                            //Transaction = null
                        };
           return await query.ToListAsync();

        }

        private void  AddTransactionSection(Entities.Transaction t, TransactionSectionDto tsdto)
        {
            var ts = new TransactionSection()
            {
                Id = Guid.Empty,
                Name = tsdto.Name,
                TransactionId = tsdto.TransactionId,
                TransactionSectionType = tsdto.TransactionSectionType,
            };

            foreach (var sg in tsdto.SectionGroups)
            {
                var tsg = new TransactionSectionGroupDto()
                {
                    GroupValue = sg.GroupValue,
                    Id = sg.Id,
                    Values = sg.Values,
                    TransactionSectionId = sg.TransactionSectionId
                };
                if (sg.Id == Guid.Empty)
                    AddTransactionSectionGroup(tsg, ts);
                else
                    EditTransactionSectionGroup(tsg, ts);
            }
            t.TransactionSections.Add(ts);
        }

        private void EditTransactionSection(TransactionSectionDto dto, 
                                           Transactions.Entities.Transaction t )
        {
            TransactionSection ts = new TransactionSection()
            {
                Id = dto.Id,
                TransactionId = dto.TransactionId,
                TransactionSectionType = dto.TransactionSectionType,
            };

            _context.Attach(ts); //test 
     
            
            foreach (var sg in dto.SectionGroups)
                if (sg.Id == Guid.Empty)
                   AddTransactionSectionGroup(sg,ts);
                else
                   EditTransactionSectionGroup(sg,ts);
        }

        private void  DeleteTransactionSection(Entities.TransactionSection ts  )
        {
            foreach (var sg in ts.SectionGroups)
               _context.Remove(sg);
            _context.Remove(ts);
         }

        #endregion

        #region TransactionSectionGroup 

        public async Task AddTransactionSectionGroupAsync(TransactionSectionGroupDto dto)
        {
            var tsg = new TransactionSectionGroup()
            {
                GroupValue = dto.GroupValue,
                Id = dto.Id,
                TransactionSectionId = dto.TransactionSectionId,
                TransactionSection = null
            };

            _context.Add(tsg);
            await _context.SaveChangesAsync();

            foreach (var v in dto.Values)
            {
                v.TransactionSectionGroupId = tsg.Id;
                if (v.Id == Guid.Empty)
                    AddValue(v,tsg);
                else
                    EditValue(v,tsg);
            }

            await _context.SaveChangesAsync();
        }

        private void AddTransactionSectionGroup(TransactionSectionGroupDto dto, 
            Entities.TransactionSection ts )
        {
            var tsg = new TransactionSectionGroup()
            {
                GroupValue = dto.GroupValue,
                Id = dto.Id,
                TransactionSectionId = ts.Id,
                TransactionSection = null
            };
            ts.SectionGroups.Add(tsg);
                        
            foreach (var v in dto.Values)
            {
                v.TransactionSectionGroupId = tsg.Id;
                if (v.Id == Guid.Empty)
                    AddValue(v,tsg);
                else
                    EditValue(v, tsg);
            }
        }

        private void EditTransactionSectionGroup(TransactionSectionGroupDto dto, 
            Entities.TransactionSection ts)
        {
            var tsg = new TransactionSectionGroup()
            {
                GroupValue = dto.GroupValue,
                Id = dto.Id,
                TransactionSectionId = dto.TransactionSectionId,
                TransactionSection  = ts 
            };
            _context.Update(tsg);
            foreach (var v in dto.Values)
            {
                v.TransactionSectionGroupId = tsg.Id;
                if (v.Id == Guid.Empty)
                    AddValue(v,tsg);
                else
                    EditValue(v, tsg    );
            }
        }

        


        #endregion





    }
}
