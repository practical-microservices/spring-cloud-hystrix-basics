package com.example.billboardclient;

import com.netflix.hystrix.contrib.javanica.annotation.HystrixCommand;
import com.netflix.hystrix.contrib.javanica.annotation.HystrixProperty;
import org.springframework.stereotype.Service;

@Service
public class MessageService {

    private String defaultMessage(String billboardId)
    {
        return "Default Message: They don't make bugs like Bugs Bunny anymore";
    }

    @HystrixCommand(fallbackMethod = "defaultMessage")
    public String getMessage(String billboardId)
    {
        if( billboardId == null) {
            throw new RuntimeException("I'm totally not working - please fix me");
        } else {
            return "Hello is always a good start";
        }
    }

    // see docs on options @ https://github.com/Netflix/Hystrix/wiki/Configuration
    @HystrixCommand(fallbackMethod = "defaultMessage",
            commandProperties = {
                    @HystrixProperty(name="execution.isolation.strategy", value="THREAD"),
                    @HystrixProperty(name="execution.isolation.thread.timeoutInMilliseconds", value="1000"),
                    @HystrixProperty(name="execution.timeout.enabled", value="true"),
                    @HystrixProperty(name="execution.isolation.thread.interruptOnTimeout", value="true"),
                    @HystrixProperty(name="execution.isolation.semaphore.maxConcurrentRequests", value="10"),
                    @HystrixProperty(name="fallback.isolation.semaphore.maxConcurrentRequests", value="10"),
                    @HystrixProperty(name="fallback.enabled", value="true"),
                    @HystrixProperty(name="circuitBreaker.enabled", value="true"),
                    @HystrixProperty(name="circuitBreaker.requestVolumeThreshold", value="20"),
                    @HystrixProperty(name="circuitBreaker.sleepWindowInMilliseconds", value="5000"),
                    @HystrixProperty(name="circuitBreaker.errorThresholdPercentage", value="50"),
                    @HystrixProperty(name="circuitBreaker.forceOpen", value="false"),
                    @HystrixProperty(name="circuitBreaker.forceClosed", value="false"),
                    @HystrixProperty(name="metrics.rollingStats.timeInMilliseconds", value="10000"),
                    @HystrixProperty(name="metrics.rollingStats.numBuckets", value="10"),
                    @HystrixProperty(name="metrics.rollingPercentile.enabled", value="true"),
                    @HystrixProperty(name="metrics.rollingPercentile.timeInMilliseconds", value="60000"),
                    @HystrixProperty(name="metrics.rollingPercentile.numBuckets", value="6"),
                    @HystrixProperty(name="metrics.rollingPercentile.bucketSize", value="100"),
                    @HystrixProperty(name="metrics.healthSnapshot.intervalInMilliseconds", value="500"),
                    @HystrixProperty(name="requestCache.enabled", value="true"),
                    @HystrixProperty(name="requestLog.enabled", value="true"),
            }
    )
    public String slowOperation(String billboardId)
    {
        try {
            Thread.sleep(2000);
            return "It's a good day";
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }

    }
}
