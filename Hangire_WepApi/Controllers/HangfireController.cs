using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangire_WepApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    { 

        [HttpGet("welcome")] 
        public IActionResult Welcome()
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to our app"));
            return Ok($"Job Id : {jobId}. Welcome email sent to the user");
        }

        [HttpGet("discount")]
        public IActionResult Discount()
        {
            int timeInSecond = 30;
            var jobId = BackgroundJob.Schedule(() => SendWelcomeEmail("Welcome to our app"), TimeSpan.FromSeconds(timeInSecond));
            return Ok($"Job Id : {jobId}. Discount email will be sent in 30 second");
        }

        [HttpGet("update")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database updated"), Cron.Minutely);
            return Ok("Database check job initiated!");
        }

        [HttpGet("confirm")]
        public IActionResult Confirm()
        {
            int timeInSecond = 30;
            var parentJobId = BackgroundJob.Schedule(() => SendWelcomeEmail("You asked to be Unsubcribe"), TimeSpan.FromSeconds(timeInSecond));
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were unsubscribe!"));
            return Ok("Confirm job created!");
        }

        public void SendWelcomeEmail(string text)
        {
            Console.WriteLine(text);
        }
    }
}
