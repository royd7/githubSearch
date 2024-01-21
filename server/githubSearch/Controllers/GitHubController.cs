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

        // GET api/<GitHubController>/5
        [Authorize]
        [HttpGet("search/")]
        public async Task<Repos> Get([FromQuery] string query)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var user = Sessions.Sessions.GetString(token);

            string url = $"https://api.github.com/search/repositories?q={query}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");//Set the User Agent to "request"

            using (HttpResponseMessage response = client.GetAsync(url).Result)
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var repos = JsonConvert.DeserializeObject<Repos>(responseBody);
                foreach (var item in repos.items)
                {
                    item.isBookMark = user.Bookmarks.FirstOrDefault(e => e == item.id) > 0;
                }
                return repos;
            }
        }

        // POST api/<GitHubController>
        [Authorize]
        [HttpPost("Bookmark")]
        public bool Post([FromBody] Repo repo)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var user = Sessions.Sessions.GetString(token);
            var bookMark = user.Bookmarks.FirstOrDefault(e => e == repo.id);
            if (bookMark > 0)
            {
                user.Bookmarks.Remove(bookMark);
                return false;
            }
            else
            {
                user.Bookmarks.Add(repo.id);
                return true;
            }

        }

    }
}
