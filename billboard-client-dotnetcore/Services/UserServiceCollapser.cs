using System.Collections.Generic;
using System.Linq;
using BillboardClient.Models;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;

namespace BillboardClient.Services
{

    public class UserServiceCollapser : HystrixCollapser<List<User>, User, int>
    {
        readonly ILogger<UserServiceCollapser> _logger;

        public static Dictionary<int, User> UserStore { get; set; } = new Dictionary<int, User>()
        {
            {1, new User() {Id = 1, Name = "Andrew"}},
            {2, new User() {Id = 2, Name = "Alex"}},
            {3, new User() {Id = 2, Name = "John"}},
            {4, new User() {Id = 3, Name = "Bob"}},
            {5, new User() {Id = 4, Name = "Seth"}},
        };

        public UserServiceCollapser(IHystrixCollapserOptions options, ILogger<UserServiceCollapser> logger) : base(options)
        {
            _logger = logger;
        }

        public int UserId { get; set; }

        public override int RequestArgument => UserId;

        protected override HystrixCommand<List<User>> CreateCommand(ICollection<ICollapsedRequest<User, int>> requests)
        {
            _logger.LogInformation("Creating batch command to handle {0} number of requests", requests.Count);
            return new HystrixCommand<List<User>>(
                group: HystrixCommandGroupKeyDefault.AsKey("UserCollapser"), 
                run: () =>  RunBatch(requests), 
                fallback: () => new List<User>() );
        }
        /**
         *  A bulk loading method that looks up multiple users at the same time.
         *  methods like this are useful when you can query multiple items from
         *  a remote service faster than you can make individual queries
         *  for each item.
         *
         * @param ids List of ID's to lookup
         * @return A list User Objects if an user was not found null will be returned
         */
        /// <summary>
        /// A bulk loading method that looks up multiple users at the same time.
        /// methods like this are useful when you can query multiple items from
        /// a remote service faster than you can make individual queries
        /// for each item.
        /// </summary>
        /// <param name="requests">ids List of ID's to lookup</param>
        /// <returns>A list User Objects if an user was not found null will be returned</returns>
        private List<User> RunBatch(ICollection<ICollapsedRequest<User, int>> requests)
        {
            _logger.LogInformation($"SELECT * FROM Users WHERE Id IN ({string.Join(',',requests.Select(x => x.Argument))})");
            return requests.Select(x => UserStore.GetValueOrDefault(x.Argument)).ToList();
        }
        
        /// <summary>
        /// Method that maps bulk response to the requests that went into it
        /// </summary>
        /// <param name="batchResponse">Collection of items that came back as part of bulk response</param>
        /// <param name="requests">Orignal requests that need to be populated from bulk response</param>
        protected override void MapResponseToRequests(List<User> batchResponse, ICollection<ICollapsedRequest<User, int>> requests)
        {
            var responses = batchResponse.ToDictionary(x => x.Id);
            foreach (var request in requests)
            {
                request.Response = responses.GetValueOrDefault(request.Argument);
            }
        }
    }
}