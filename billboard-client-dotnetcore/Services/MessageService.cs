using System;
using System.Threading;
using Steeltoe.CircuitBreaker.Hystrix;

namespace BillboardClient.Services
{
    public class MessageService
    {
        private string DefaultMessage() => "Default Message: They don't make bugs like Bugs Bunny anymore";

        public string GetMessage(string billboardId)
        {
            var cmd = new HystrixCommand<string>(HystrixCommandGroupKeyDefault.AsKey("GetMessage"), () => RunGetMessage(billboardId), DefaultMessage);
            return cmd.Execute();
        }
        
        private string RunGetMessage(string billboardId)
        {
            if( billboardId == null) {
                throw new Exception("I'm totally not working - please fix me");
            } else {
                return "Hello is always a good start";
            }
        }

        public string SlowOperation(string billboardId)
        {
            var commandOptions = new HystrixCommandOptions(HystrixCommandGroupKeyDefault.AsKey("SlowOperation"), HystrixCommandKeyDefault.AsKey("SlowOperation"))
            {
                CircuitBreakerEnabled = true, // Whether to use a HystrixCircuitBreaker or not.
                CircuitBreakerForceClosed = false, // If true the HystrixCircuitBreaker.allowRequest() will always return true to allow requests regardless of the error percentage from HystrixCommandMetrics.getHealthCounts().
                CircuitBreakerForceOpen = false, // If true the HystrixCircuitBreaker.allowRequest() will always return false, causing the circuit to be open (tripped) and reject all requests.
                CircuitBreakerErrorThresholdPercentage = 50, // Error percentage threshold (as whole number such as 50) at which point the circuit breaker will trip open and reject requests.
                CircuitBreakerRequestVolumeThreshold = 20, // Minimum number of requests in the metricsRollingStatisticalWindowInMilliseconds() that must exist before the HystrixCircuitBreaker will trip.
                CircuitBreakerSleepWindowInMilliseconds = 5000, // The time in milliseconds after a HystrixCircuitBreaker trips open that it should wait before trying requests again.
                ExecutionIsolationSemaphoreMaxConcurrentRequests = 10, // Number of concurrent requests permitted to HystrixCommand.run().
                //ExecutionIsolationThreadPoolKeyOverride = "MyThreadpoolKey", // Allow a dynamic override of the HystrixThreadPoolKey that will dynamically change which HystrixThreadPool a HystrixCommand executes on.
                ExecutionIsolationStrategy = ExecutionIsolationStrategy.THREAD, //What isolation strategy HystrixCommand.run() will be executed with.
                ExecutionTimeoutEnabled = true, // Whether the timeout mechanism is enabled for this command
                ExecutionTimeoutInMilliseconds = 1000, // Time in milliseconds at which point the command will timeout and halt execution.
                FallbackEnabled = true, // whether fallback should be attempted when failure occurs.
                FallbackIsolationSemaphoreMaxConcurrentRequests = 10, // Number of concurrent requests permitted to HystrixCommand.getFallback().
                MetricsHealthSnapshotIntervalInMilliseconds = 500, // Time in milliseconds to wait between allowing health snapshots to be taken that calculate success and error percentages and affect HystrixCircuitBreaker.isOpen() status.
                MetricsRollingPercentileBucketSize = 100, //Maximum number of values stored in each bucket of the rolling percentile.
                MetricsRollingPercentileEnabled = true, // Whether percentile metrics should be captured using HystrixRollingPercentile inside HystrixCommandMetrics.
                MetricsRollingPercentileWindowBuckets = 6, // Number of buckets the rolling percentile window is broken into.
                MetricsRollingPercentileWindowInMilliseconds = 60000, // Duration of percentile rolling window in milliseconds.
                MetricsRollingStatisticalWindowBuckets = 10, // Number of buckets the rolling statistical window is broken into.
                MetricsRollingStatisticalWindowInMilliseconds = 10000, // Duration of statistical rolling window in milliseconds.
                RequestCacheEnabled = true, // Whether AbstractCommand.getCacheKey() should be used with HystrixRequestCache to provide de-duplication functionality via request-scoped caching.
                RequestLogEnabled = true, // Whether HystrixCommand execution and events should be logged to HystrixRequestLog.
            };
            var cmd = new HystrixCommand<string>(commandOptions, () => RunSlowOperation(billboardId), DefaultMessage);
            return cmd.Execute();
        }
        
        private string RunSlowOperation(string billboardId)
        {
            Thread.Sleep(2000);
            return "It's a good day";
        }
    }
}