using Elsa.Webhooks.Models;
using ElsaServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElsaServer.Controllers
{
    [ApiController]
    [Route("api/webhooks")]
    public class WebhookController : Controller
    {

        //[HttpPost("run-task")]
        //public async Task<IActionResult> RunTask(WebhookEvent webhookEvent)
        //{
        //    var payload = webhookEvent.Payload;
        //    //var taskPayload = payload.TaskPayload;
        //    var employee = taskPayload.Employee;

        //    var task = new UserModel
        //    {
        //        //ProcessId = payload.WorkflowInstanceId,
        //        //ExternalId = payload.TaskId,
        //        //Name = payload.TaskName,
        //        //Description = taskPayload.Description,
        //        Email = employee.Email,
        //        Name = employee.Name,
        //        //CreatedAt = DateTimeOffset.Now
        //    };

        //    return Ok();
        //}
    }
}
