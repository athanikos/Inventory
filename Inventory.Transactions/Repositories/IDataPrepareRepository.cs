namespace Inventory.Transactions.Repositories
{
    public  interface IDataPrepareRepository
    {
        void RoomsPrepare();
        Task SaveChangesAsync();
    }
}
