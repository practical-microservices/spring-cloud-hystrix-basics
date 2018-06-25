using BillboardClient.Models;
using BillboardClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;

namespace BillboardClient.Controllers
{
    public class UserController : Controller
    {
        private readonly ILoggerFactory _loggerFactory;

        public UserController(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        [Route("/getById/{id}")]
        public User GetById(int id)
        {
            var collapserOptions = new HystrixCollapserOptions(HystrixCollapserKeyDefault.AsKey("UserCollapser"), RequestCollapserScope.GLOBAL)
            {
                TimerDelayInMilliseconds = 5000, 
                MaxRequestsInBatch = 10,
                RequestCacheEnabled = true
            };
            var collapser = new UserServiceCollapser(collapserOptions, _loggerFactory.CreateLogger<UserServiceCollapser>());
            collapser.UserId = id;
            return collapser.Execute();
        }
    }
}