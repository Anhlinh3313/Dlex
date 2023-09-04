using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class SignalRController : Controller
    {
        private readonly IHubContext<NotifyHub> _hubContext;

        public SignalRController(IHubContext<NotifyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody]ModelTest model)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", model.Message);

            return new JsonResult(new { data = model });
        }
    }

    public class ModelTest
    {
        public string Message { get; set; }
    }
}
