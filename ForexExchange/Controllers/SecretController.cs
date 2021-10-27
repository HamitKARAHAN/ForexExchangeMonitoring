using Microsoft.AspNetCore.Mvc;
using ForexExchange.Filters;
using System;

namespace ForexExchange.Controllers
{
    [ApiKeyAuth]
    public class SecretController : ControllerBase
    {
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            return Ok("ı have no secret");
        }
    }
}
