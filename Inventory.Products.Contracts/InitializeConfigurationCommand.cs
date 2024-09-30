using Inventory.Defaults.Contracts;
using MediatR;

namespace Inventory.Products.Contracts
{
        public  record InitializeConfigurationCommand()   
                :  IRequest<List<InitializeConfigurationResponse>>;
        
        public class InitializeConfigurationResponse
        {
            public required ConfigurationType  TypeName  {
                get;
                set;
            }

            public required  Guid Id  { get; set; }

            public required  string Value  { get; set; }
            
        }
}

