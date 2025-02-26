using Microsoft.AspNetCore.Mvc;

namespace Rooms.API.Controllers
{
    public class RoomsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]  
        public async  Task<IActionResult> Create([FromBody] Room room)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync
                    ("https://localhost:44324/api/Reservation"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiResponse);
                }
            }

           return CreatedAtAction("get", new {id = room.Id}, room);

        }


        //[HttpGet(ApiEndpoints.Rooms.Get)]
        //public async Task<IActionResult> Get([FromRoute] Guid Id)
        //{
        //    throw new NotImplementedException();

        //}

        //[HttpGet(ApiEndpoints.Rooms.GetAll)]
        //public async Task<IActionResult> GetAll()
        //{
        //    throw new NotImplementedException();

        //}

        //[HttpPut(ApiEndpoints.Rooms.Put)]
        //public async Task<IActionResult> Edit([FromBody] Room room)
        //{
        //    throw new NotImplementedException();

        //}

    }
}
