using System.ComponentModel;
using Cps360.SyncWithCps.Presentation.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cps360.SyncWithCps.Presentation.Adapters.HttpAdapters.CpsPortfolios
{
    [Route("api/csp-portfolios-publisher")]
    [ApiController]
    public class CpsPortfoliosPublisherController : ControllerBase
    {
        private readonly CpsPortfoliosFetchAndPublishHandler _handler;

        public CpsPortfoliosPublisherController(CpsPortfoliosFetchAndPublishHandler handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// fetch and publish all cps portfolios
        /// </summary>
        [Auth("cps360-admin")]
        [HttpGet("fetch-and-publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult FetchAndPublish()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += async (sender, args) => await _handler.Handle();
            worker.RunWorkerAsync();

            return NoContent();
        }
    }
}