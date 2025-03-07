using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Rooms.API.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _url;

        public RoomsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration["CoreApi:Url"];
        }
        [HttpPost(ApiEndpoints.Rooms.Create)]
        public async  Task<IActionResult> Create([FromBody] Room room)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(room);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
                    await httpClient.PostAsync(_url + "products", stringContent);
                }
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.StackTrace, title: ex.Message);
            }
            return Ok();


        }


        [HttpGet(ApiEndpoints.Rooms.Get)]
        public async Task<IActionResult> Get([FromBody] Guid Id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jsonResponse = string.Empty;
                    try
                    {
                        using HttpResponseMessage response = await
                            httpClient.GetAsync(_url + "rooms/" + Id.ToString());
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            jsonResponse = await response.Content.ReadAsStringAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        return Problem(detail: ex.StackTrace, title: ex.Message);
                    }
                    return Ok(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.StackTrace, title: ex.Message);
            }

        }

        [HttpGet(ApiEndpoints.Rooms.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jsonResponse = string.Empty;
                    try
                    {
                        using HttpResponseMessage response = await
                            httpClient.GetAsync(_url + "rooms/");
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            jsonResponse = await response.Content.ReadAsStringAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        return Problem(detail: ex.StackTrace, title: ex.Message);
                    }
                    return Ok(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.StackTrace, title: ex.Message);
            }

        }

    }
}
