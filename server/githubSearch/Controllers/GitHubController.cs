using githubSearch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace githubSearch.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        // GET: api/<GitHubController>
        [HttpGet]
        public string Get()
        {
            return "hello from git search";
        }

        [HttpGet("tokenXX")]
        public ActionResult GetToken()
        {

            // Create the signing credentials using the secret key
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Models.User.SECRET));

            // Generate a token using the signing key
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Set other token parameters (issuer, audience, claims, etc.)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string access_token = tokenHandler.WriteToken(token);
            // Save the token in the user's session
            HttpContext.Session.SetString("access-token", access_token);

            return Ok(new { token = access_token });
        }

        // GET api/<GitHubController>/5
        [Authorize]
        [HttpGet("search/")]
        public async Task<Repos> Get([FromQuery] string query)
        {
            string url = $"https://api.github.com/search/repositories?q={query}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");//Set the User Agent to "request"

            using (HttpResponseMessage response = client.GetAsync(url).Result)
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Repos>(responseBody);
            }
        }

        // POST api/<GitHubController>
        [Authorize]
        [HttpPost("Bookmark")]
        public void Post([FromBody] Repo repo)
        {
        }

    }
}
