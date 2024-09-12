namespace Inventory.Products.Services
{
    public class ModifyQuantityException : Exception
    {

        public ModifyQuantityException()
        {
        }

        public ModifyQuantityException(string message)
            : base(message)
        {
        }

        public ModifyQuantityException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }

    public class InvalidDiffException : ModifyQuantityException
    {

    }
}