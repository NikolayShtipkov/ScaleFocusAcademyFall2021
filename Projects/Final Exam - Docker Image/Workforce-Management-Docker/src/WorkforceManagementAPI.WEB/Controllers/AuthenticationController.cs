using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Net.Http;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController(IWebHostEnvironment env)
        {
            CurrentEnvironment = env;
        }

        public IWebHostEnvironment CurrentEnvironment { get; }

        /// <summary>
        /// Create a TOKEN for AUTHENTICATION, using existing username and password.
        /// </summary>
        /// <param name="loginModel"></param>
        /// 
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        [HttpPost, Route("Login")]
        public string Login(AuthenticationLoginRequestDTO loginModel)
        {
            var client = new HttpClient();

            var url = "";

            url = "http://host.docker.internal:8000/connect/token";

            var IdentityServerParameters = new List<KeyValuePair<string, string>>();
            IdentityServerParameters.Add(new KeyValuePair<string, string>("grant_type", "password"));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("username", loginModel.Username));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("password", loginModel.Password));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("client_id", "WorkforceManagementAPI"));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("client_secret", "seasharp_BareM1n1mum"));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("scope", "users offline_access WorkforceManagementAPI"));
            
            using (client)
            {
                HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(IdentityServerParameters)).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
