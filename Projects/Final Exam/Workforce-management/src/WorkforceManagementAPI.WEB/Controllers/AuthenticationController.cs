using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Create a TOKEN for AUTHENTICATION, using existing username and password.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        [HttpPost, Route("Login")]
        public async Task<ActionResult<OAuthTokenResponseDTO>> Login(AuthenticationLoginRequestDTO loginModel)
        {
            var client = new HttpClient();

            var url = "https://localhost:5001/connect/token";

            var identityServerParameters = new List<KeyValuePair<string, string>>();
            identityServerParameters.Add(new KeyValuePair<string, string>("grant_type", "password"));
            identityServerParameters.Add(new KeyValuePair<string, string>("username", loginModel.Username));
            identityServerParameters.Add(new KeyValuePair<string, string>("password", loginModel.Password));
            identityServerParameters.Add(new KeyValuePair<string, string>("client_id", "WorkforceManagementAPI"));
            identityServerParameters.Add(new KeyValuePair<string, string>("client_secret", "seasharp_BareM1n1mum"));
            identityServerParameters.Add(new KeyValuePair<string, string>("scope", "users offline_access WorkforceManagementAPI"));

            using (client)
            {
                HttpResponseMessage response = await client.PostAsync(url, new FormUrlEncodedContent(identityServerParameters));
                var content = await response.Content.ReadAsStringAsync();
                var json = GetFormattedJson(content);

                return StatusCode((int)response.StatusCode, json);
            }
        }

        private string GetFormattedJson(string content)
        {
            var jsonObject = JsonConvert.DeserializeObject<object>(content);
            return JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        }
    }
}
