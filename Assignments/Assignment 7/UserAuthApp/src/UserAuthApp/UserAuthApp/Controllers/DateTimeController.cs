using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthApp.BLL.Interfaces;
using UserAuthApp.Models.DTO.Responses;

namespace UserAuthApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DateTimeController
    {
        private readonly IDateTimeProvider now;

        public DateTimeController(IDateTimeProvider dateTimeProvider)
        {
            now = dateTimeProvider;
        }

        [HttpGet]
        public DateTimeResponseDTO GetTime()
        {
            var current = now.UtcNow;

            var response = new DateTimeResponseDTO
            {
                Date = current.ToString("MMMM dd, yyyy"),
                Time = current.ToString("HH:mm:ss")
            };

            return response;
        }
    }
}
