using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NetCoreBase.Controllers
{
    /// <summary>
    /// WeatherForecastController
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly ILog log;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            log = LogManager.GetLogger(typeof(WeatherForecastController));

        }

        /// <summary>
        /// 获取天气
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(GroupName = "User")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //int i = new int[] { }[2];

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 用于consul健康检查
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(GroupName ="Project")]// swagger分组显示
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Health()
        {
            return Ok();
        }
    }
}
