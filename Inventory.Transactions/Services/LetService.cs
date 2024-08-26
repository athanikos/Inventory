namespace Inventory.Transactions.Services
{
    public  class LetService
    {
        public LetService() { }

        public void ReserveDates(DateTime from, DateTime to) 
        { 
            // validate reserve
            throw new NotImplementedException();
        }

        public void MultiReserveDates(DateTime from, DateTime to)
        {
            // validate reserve accross rooms and break intervals
            // start from larger then decrement by 1 
            // left or right ?
            throw new NotImplementedException();
        }

        
        public bool ValidateReservation(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }


}
