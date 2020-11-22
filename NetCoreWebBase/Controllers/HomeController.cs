using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NetCoreWebBase.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        private IMemoryCache _memoryCache;
        private IDistributedCache _distributedCache;
        //private ICustomMemoryCache _customMemoryCache;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IMemoryCache memoryCache,
            IDistributedCache distributedCache)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._memoryCache = memoryCache;
            this._distributedCache = distributedCache;
            //this._customMemoryCache = customMemoryCache;
        }

        [EnableCors("any")]//支持跨域
        [ResponseCache(Duration =70)]//如果不添加app.UseResponseCaching();，换浏览器缓存就会失效
        public IActionResult Index()
        {
            string key = "memorykey";

            if (this._memoryCache.TryGetValue<string>(key, out string time))
            {

            }
            else
            {
                time = DateTime.Now.ToShortTimeString();
                this._memoryCache.Set(key, time, TimeSpan.FromSeconds(120));
            }
            this._memoryCache.Remove(key);

            //通过redis实现分布式缓存
            {
                time = this._distributedCache.GetString(key);
                if (!string.IsNullOrEmpty(time))
                {

                }
                else
                {
                    time = DateTime.Now.ToShortTimeString();
                    this._distributedCache.SetString(key, time, new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(120)
                    });
                }
            }

            return View();
        }
    }
}