using Inventory.Transactions.Contracts;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Entities;
using Inventory.Transactions.Entities.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Inventory.Transactions.Repositories.Postgres
{
    public class PostgresTransactionRepository(TransactionsDbContext context) :
        ITransactionRepository
    {
        public async Task EmptyDb()
        {
            context.Values.RemoveRange(context.Values);
            context.Fields.RemoveRange(context.Fields);
            context.Sections.RemoveRange(context.Sections);
       
            context.TransactionSectionGroups.RemoveRange(context.TransactionSectionGroups);
            context.TransactionSections.RemoveRange(context.TransactionSections);
            context.Transactions.RemoveRange(context.Transactions);
            context.Templates.RemoveRange(context.Templates);

            await context.SaveChangesAsync();
        }

        #region Templates
        public async Task<TemplateDto> GetTemplateAsync(Guid id)
        {
            var sections = await GetSectionsAsync(id);

            return await context.Templates
                .Include(t => t.Sections)
                .ThenInclude(s => s.Fields)
                .Where(o => o.Id == id)
                .Select
                (
                    i => new TemplateDto(i.Id,
                        i.Name,
                        i.Type,
                        i.Created,
                        i.Sections.Select
                        (
                            s=>new SectionDto()
                            {
                                Fields = s.Fields.Select(
                                    f=> new FieldDto()
                                    {
                                        Id  =f.Id,
                                        SectionId = f.SectionId,
                                        Name = f.Name,
                                        Expression =f.Expression,
                                        Type =f.Type
                                    }).ToList(),
                                Id = s.Id,
                                Name = s.Name,
                                SectionType = s.SectionType,
                                TemplateId = s.TemplateId
                            }
                        ).ToList()
                    ))
                .SingleAsync();

        }

        public async Task<List<TemplateDto>> GetTemplatesAsync()
        {
            return await context.Templates
                .Include(t => t.Sections)
                .ThenInclude(s => s.Fields)
                .Select
                (
                    i => new TemplateDto(i.Id,
                    i.Name,
                    i.Type,
                    i.Created,
                    i.Sections.Select
                    (
                        s=>new SectionDto()
                        {
                                Fields = s.Fields.Select(
                                        f=> new FieldDto()
                                        {
                                              Id  =f.Id,
                                              SectionId = f.SectionId,
                                              Name = f.Name,
                                              Expression =f.Expression,
                                              Type =f.Type
                                        }).ToList(),
                                Id = s.Id,
                                Name = s.Name,
                                SectionType = s.SectionType,
                                TemplateId = s.TemplateId
                        }
                    ).ToList()
                )).ToListAsync();

        }
        
        public async Task<TemplateDto> AddTemplateAsync(TemplateDto dto)
        {
            var t = new Template() {
                                        Id = dto.Id, 
                                        Created = dto.Created, 
                                        Name = dto.Name,
                                         Type = dto.Type 
                                    };
            context.Templates.Add(t);
            await context.SaveChangesAsync(); //todo do work in context avoid saving per level 

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
                if (inStoreTemplate.Sections.Any(t => sec.TemplateId == t.Id))
                    EditSection(sec);
                else
                    await AddSection(sec);
            }

            foreach (var storedSection in inStoreTemplate.Sections)
            {
                if (inboundTemplate.Sections.Any(t => storedSection.TemplateId == t.Id) == false)
                    DeleteSection(storedSection);
            }

            await context.SaveChangesAsync();
            return await GetTemplateAsync(inStoreTemplate.Id);
        }

        public async Task DeleteTemplateAsync(TemplateDto c)
        {
            Template t = context.Templates.Include(template => template.Sections).Single(p => p.Id == c.Id);
            foreach (var item in t.Sections)
                DeleteSection(new SectionDto(t.Id));

            context.Remove(t);
            await context.SaveChangesAsync();
        }
        #endregion Templates 

        #region Sections 
        public async Task<ICollection<SectionDto>> GetSectionsAsync(Guid templateId)
        {
            return await context.Sections.Where(o => o.TemplateId == templateId)
                           .Select(i => new SectionDto()
                           {
                               Id = i.Id,
                               Name = i.Name,
                               TemplateId = i.TemplateId,
                               SectionType = i.SectionType,
                           }).ToListAsync();
        }

        private async Task AddSection(SectionDto c)
        {
            List<Field> fields = [];
            Section s = new Section()
            {
                Id = c.Id,
                Name = c.Name,
                TemplateId = c.TemplateId,
                Fields = fields,
                SectionType = c.SectionType

            };
            context.Sections.Add(s);
            await context.SaveChangesAsync();

            foreach (var f in c.Fields)
                fields.Add(new Field()
                {
                    Name = f.Name,
                    Expression = f.Expression,
                    Id = f.Id,
                    SectionId = s.Id,
                     Type = f.Type,
                });

            await context.SaveChangesAsync();

        }

        private void EditSection(SectionDto inboundSection)
        {

            var inStoreSection = context.Sections.Include(section => section.Fields).Single(i => i.Id == inboundSection.Id);

            Section f = new ()
            { Id = inboundSection.Id, Name = inboundSection.Name, TemplateId = inboundSection.TemplateId, SectionType = inboundSection.SectionType };


            foreach (var field in inboundSection.Fields)
                if (field.Id == Guid.Empty)
                    AddField(field);
                else
                    EditField(field);
            
            foreach (var storedField in inStoreSection.Fields)
            {
                if (inboundSection.Fields.Any(t => storedField.SectionId == t.SectionId) == false)
                    DeleteField(new FieldDto(storedField.Id));
            }
            context.Update(f);
        }

        public void DeleteSection(SectionDto dto)
        {
            Section s = context.Sections.
                Include(section => section.Fields).
                Single(p => p.Id == dto.Id);
            context.Remove(s);

            foreach (Field f in s.Fields)
                context.Remove(f);

        }

        #endregion 

        #region Fields 
        public async Task<ICollection<FieldDto>> GetFieldsAsync(Guid templateId)
        {
            var SectionId = await context.Sections
                            .Where(o => o.TemplateId == templateId)
                            .Select(i=>i.Id).SingleOrDefaultAsync();  


            return await context.Fields.Where(o => o.SectionId == SectionId)
                           .Select(i => new FieldDto()
                           {
                               Id = i.Id,
                               Expression = i.Expression,
                               Name = i.Name,
                               Type = i.Type,
                               TemplateId = templateId,
                               SectionId = i.SectionId,
                           }).ToListAsync();
        }

        private void AddField(FieldDto c)
        {
            context.Fields.Add(new Field()
            {
                Id = c.Id,
                Expression = c.Expression,
                Name = c.Name,
                Type = c.Type,
                SectionId = c.SectionId,    
            });
        }

        private void EditField(FieldDto dto)
        {
            Field f = new Field()
            {
                Id = dto.Id,
                Name = dto.Name,
                Expression = dto.Expression,
                Type = dto.Type,
                SectionId = dto.SectionId
            };
            context.Update(f);
        }

        public void DeleteField(FieldDto dto)
        {
            Field f = context.Fields.Single(p => p.Id == dto.Id);
            context.Remove(f);
        }
        #endregion Fields 

        #region Values

        public async Task<ICollection<ValueDto>>
            GetTransactionValuesAsync(Guid transactionId)
        {

            var query = from value in context.Values
                        where value.TransactionId == transactionId
                        join sectionGroup in context.TransactionSectionGroups
                        on value.TransactionSectionGroupId equals sectionGroup.Id
                        join section in context.TransactionSections
                        on sectionGroup.Id equals section.Id
                        select new ValueDto()
                        {
                            Id = value.Id,
                            FieldId = value.FieldId,
                            GroupValue = sectionGroup.GroupValue,
                            Text = value.Text,
                            TransactionId = transactionId,
                            TransactionSectionGroupId = value.TransactionSectionGroupId,
                            TransactionSectionType = section.TransactionSectionType
                        };


            return await query.ToListAsync();

        }

        private void AddValue(ValueDto dto, TransactionSectionGroup tsg)
        { //todo use mapper
            tsg.Values.Add
            (
                            new Value()
                            {
                                FieldId = dto.FieldId,
                                Text = dto.Text,
                                TransactionId = dto.TransactionId,
                                TransactionSectionGroupId = dto.TransactionSectionGroupId,
                            }
            );

        }

        private void EditValue(ValueDto dto, TransactionSectionGroup tsg)
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
            context.Entry(v).State = EntityState.Modified;
        }

        public void DeleteValue(ValueDto dto)
        {
            Value f = context.Values.Single(p => p.Id == dto.Id);
            context.Remove(f);
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
            context.Transactions.Add(t);
            
            if (dto.Sections!=null)
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

            await context.SaveChangesAsync();
            return await GetTransactionAsync(t.Id);
        }

        public async Task<TransactionDto> GetTransactionAsync(Guid id)
        {
            var transactionSections = await GetTransactionSectionsAsync(id);

            return await context.Transactions.
                Where(o => o.Id == id).
                //todo include 
                Select(i =>
                                                        new TransactionDto(
                                                        i.Id,
                                                        i.Description,
                                                        i.Created,
                                                        i.TemplateId,
                                                        transactionSections)
                                                 )
                                           .SingleAsync();

        }

        public async Task<List<TransactionDto>> GetTransactionsAsync()
        {
       
            return await context.Transactions.
                Include(o=>o.TransactionSections).
             
             
                Select(i =>
                    new TransactionDto(
                        i.Id,
                        i.Description,
                        i.Created,
                        i.TemplateId, null)//i.TransactionSections.Select() )
                )
                .ToListAsync();

        }
        
        public async Task<TransactionDto> EditTransactionAsync(TransactionDto inboundTransaction)
        {
            var inStoreTransaction = await context.Transactions
                .Where(p => p.Id == inboundTransaction.Id).Include(transaction => transaction.TransactionSections).SingleAsync();

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
                        .Any(s => storedSection.TransactionId == s.TransactionId) == false)
                    DeleteTransactionSection(storedSection);
            }

            await context.SaveChangesAsync();
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
            var query = from ts in context.TransactionSections
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
                    Id = Guid.Empty,
                    Values = sg.Values,
                    TransactionSectionId = sg.TransactionSectionId
                };
                if (tsg.Id == Guid.Empty)
                    AddTransactionSectionGroup(tsg, ts);
                else
                    EditTransactionSectionGroup(tsg, ts);
            }
            t.TransactionSections.Add(ts);
        }

        private void EditTransactionSection(TransactionSectionDto dto, 
                                            Transaction t )
        {
            TransactionSection ts = new TransactionSection()
            {
                Id = dto.Id,
                TransactionId = t.Id,
                TransactionSectionType = dto.TransactionSectionType,
            };

            context.Attach(ts); //test 
     
            
            foreach (var sg in dto.SectionGroups)
                if (sg.Id == Guid.Empty)
                   AddTransactionSectionGroup(sg,ts);
                else
                   EditTransactionSectionGroup(sg,ts);
        }

        private void  DeleteTransactionSection(Entities.TransactionSection ts  )
        {
            foreach (var sg in ts.SectionGroups)
               context.Remove(sg);
            context.Remove(ts);
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

            context.Add(tsg);
            await context.SaveChangesAsync();

            foreach (var v in dto.Values)
            {
                v.TransactionSectionGroupId = tsg.Id;
                if (v.Id == Guid.Empty)
                    AddValue(v,tsg);
                else
                    EditValue(v,tsg);
            }

            await context.SaveChangesAsync();
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
                v.Id = Guid.Empty;
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
            context.Update(tsg);
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

        public async Task<Guid> RoomsPrepareAsync()
        {
            return await AddRoomsLetTemplate();
        }
        
        /// <summary>
        /// Template creation for Let Room app
        /// with one template
        /// having two sections individual entity and rooms let 
        /// </summary>
        /// <returns></returns>
        private async Task<Guid> AddRoomsLetTemplate()
        {
            var t = new Template()
            {
                Id = Guid.NewGuid(),
                Name = "Rooms To Let Template",
                Sections =
                    {
                        AddRoomsLetSection(),
                        AddRoomsLetIndividualEntitySection()
                     }

            };

            context.Add(
               t
            );

            await context.SaveChangesAsync();
            return t.Id;
        }

        private static Section AddRoomsLetIndividualEntitySection()
        {
            return new Section()
            {
                Id = Guid.Empty,
                Name = "Rooms Individual Entity Template",
                SectionType = Contracts.SectionType.IndividualEntity,
                Fields = [

                                new()
                                {
                                    Expression = string.Empty,
                                    Name="ID",
                                    Type= FieldType.String
                                },
                                new()
                                {
                                    Expression = string.Empty,
                                    Name="Name",
                                    Type= FieldType.String
                                },
                                new()
                                {
                                  Expression  = string.Empty,
                                  Name="FullName",
                                  Type= FieldType.String
                                },
                                  new()
                                {
                                  Expression  = string.Empty,
                                  Name="Email",
                                  Type= FieldType.String
                                },
                            ]
            };
        }

        private static Section AddRoomsLetSection() => new()
        {
            Id = Guid.Empty,
            Name = "Rooms Let Section",
            SectionType = Contracts.SectionType.ProductLet,
            Fields = [

                                            new()
                                            {
                                                Expression = string.Empty,
                                                Name="DateFrom",
                                                Type= FieldType.Date
                                            },
                                            new()
                                            {
                                                Expression = string.Empty,
                                                Name="DateTo",
                                                Type= FieldType.Date
                                            },
                                            new()
                                            { Expression = "DateTo - DateFrom",
                                              Name="Days",
                                              Type= FieldType.Date
                                            },
                                        ]
        };
    }
}
