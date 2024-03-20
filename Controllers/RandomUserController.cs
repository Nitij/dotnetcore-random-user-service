using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomUserService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RandomUserController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RandomUserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://randomuser.me/api/");
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode) 
            { 
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }

            return StatusCode((int)response.StatusCode, response.Content.ToString());

        }
    }
}
