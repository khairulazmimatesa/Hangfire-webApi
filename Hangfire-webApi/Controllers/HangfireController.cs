using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire_webApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase {

        [HttpGet]
        public IActionResult Get() {
            return Ok("Hello from hangfire webapi!");
        }

        //Fire and Forget Jobs
        [HttpPost]
        [Route("[action]")]
        public IActionResult Welcome() {

           var jobId =  BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to our app"));

            return Ok($"Job ID: {jobId}. Welcome Email sent to the user!");
        }
        
        //Delayed Jobs
        [HttpPost]
        [Route("[action]")]
        public IActionResult Discount() {

            int timeInSeconds = 30;
           var jobId =  BackgroundJob.Schedule(() => SendWelcomeEmail("Welcome to our app"),TimeSpan.FromSeconds(timeInSeconds));

            return Ok($"Job ID: {jobId}. Discount Email will sent in {timeInSeconds} seconds!");
        }

        //Recurring Jobs
        [HttpPost]
        [Route("[action]")]
        public IActionResult DatabaseUpdate() {

            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database Updated"), Cron.Minutely);
            return Ok("Database check job initiated");

        }
        //Continous Jobs
        [HttpPost]
        [Route("[action]")]
        public IActionResult Confirm() {

            int timeInSeconds = 30;
            var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("You asked to be Unsubscribed!"), TimeSpan.FromSeconds(timeInSeconds));

            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were Unsubscribed!"));

            return Ok("Confirmation app created");
        }

        public void SendWelcomeEmail(string text) {
            Console.WriteLine(text);
        }
    }
}
