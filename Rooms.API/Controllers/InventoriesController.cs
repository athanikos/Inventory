using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Rooms.API.Controllers
{
    public class InventoriesController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly string _url;

        public InventoriesController (IConfiguration configuration)
        {
            _configuration = configuration;
            _url =_configuration["CoreApi:Url"];

        }

        [HttpPost(ApiEndpoints.Inventories.Create)]
        public async Task<IActionResult> Create([FromBody] Inventory inventory)
        {
          
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(inventory);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

                using (var response = await httpClient.PostAsync
                    (_url + "inventories", stringContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    inventory = JsonConvert.DeserializeObject<Inventory>(apiResponse);
                }
            }

            return CreatedAtAction("get", new { id = inventory.Id }, inventory);

        }


      
    }
}
