namespace Inventory.Transactions.Repositories
{
    public  interface IDataPrepareRepository
    {
        Task< Guid> RoomsPrepareAsync();
     
    }
}
