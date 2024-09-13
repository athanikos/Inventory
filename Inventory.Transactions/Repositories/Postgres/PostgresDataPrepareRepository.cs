using Inventory.Transactions.Dto;
using Inventory.Transactions.Entities;
using Npgsql.Internal.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Transactions.Repositories.Postgres
{
    public class PostgresDataPrepareRepository :
         IDataPrepareRepository
    {
        private readonly TransactionsDbContext _context;

        public PostgresDataPrepareRepository(TransactionsDbContext context)
        { _context = context; }


        public void RoomsPrepare()
        {
            AddRoomsLetTemplate();

        }



        private void AddRoomsLetTemplate()
        {
            _context.Add(
                new Entities.Template()
                {
                    Id = Guid.NewGuid(),
                    Name = "Rooms To Let Template",
                    Sections =
                    {
                        AddRoomsLetSection(),
                        AddRoomsLetIndividualEntitySection()

                     }

                }
            );
        }

        private static Section AddRoomsLetIndividualEntitySection()
        {
            return new Entities.Section()
            {
                Id = Guid.Empty,
                Name = "Rooms Individual Entity Template",
                SectionType = Contracts.SectionType.IndividualEntity,
                Fields = new List<Entities.Field> {

                                new Entities.Field()
                                {
                                    Expression = string.Empty,
                                    Name="ID",
                                    Type= FieldType.String
                                },
                                new Entities.Field()
                                {
                                    Expression = string.Empty,
                                    Name="Name",
                                    Type= FieldType.String
                                },
                                new Entities.Field()
                                { 
                                  Expression  = string.Empty,
                                  Name="FullName",
                                  Type= FieldType.String
                                },
                                  new Entities.Field()
                                {
                                  Expression  = string.Empty,
                                  Name="Email",
                                  Type= FieldType.String
                                },
                            }
            };
        }

        private static Section AddRoomsLetSection()
        {
            return new Entities.Section()
            {
                Id = Guid.Empty,
                Name = "Rooms Let Section",
                SectionType = Contracts.SectionType.ProductLet,
                Fields = new List<Entities.Field> {

                                            new Entities.Field()
                                            {
                                                Expression = string.Empty,
                                                Name="DateFrom",
                                                Type= FieldType.Date
                                            },
                                            new Entities.Field()
                                            {
                                                Expression = string.Empty,
                                                Name="DateTo",
                                                Type= FieldType.Date
                                            },
                                            new Entities.Field()
                                            { Expression = "DateTo - DateFrom",
                                              Name="Days",
                                              Type= FieldType.Date
                                            },
                                        }
            };
        }



        public async Task SaveChangesAsync()
        {
           await  _context.SaveChangesAsync();    
        }
    }
}
