using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebScraperApiApp.Scraper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebScraperApiApp.Controllers
{
    
    
    [ApiController, Route("~/")]
    public class AppController : ControllerBase
    {
        public async Task<ActionResult> Index([FromServices] HzzoHtmlScraper scraper)
        {
            var startTime = DateTime.Now;
            // TODO: implement scraper and parser logic
            var meds = await scraper.Run();

            var totalTime = startTime - DateTime.Now;

            return Ok(
                $"Done! Handler duration: {totalTime.Duration()}" +
                Environment.NewLine +
                Environment.NewLine +
                string.Join(Environment.NewLine, meds.Select(x => x.FileName))
                );
        }
    }

    
}
