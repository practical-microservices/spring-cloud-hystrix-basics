# Spring Cloud Hystrix Basics

### Overview

This sample demonstrates how to use Spring Cloud Netflix Histrix to build resiliant and performing microservices. Key things to pay attention to in this sample is how to create commands that implement a circuit breaker pattern which provide 4 key things:
* fallback when normal flow doesn't work
* fail-fast behavior - slow operations are aborted and fallback executed
* repeated failures are defaulted to fallback behavior until normal operations resume
* circuit dashboard & aggregation across multiple microservices using Hystrix Turbine

The other Hystrix feature that is demonstrated is the use of collapsers, which allow combining multiple upstream commands into a single downstream execution batch command, and then mapping the reply back on to the original requests. This pattern can greatly reduces stress on your infrastructure and optimize code efficiency. 

### Run the Demo

* Import the root of the repo into your favorite Java IDE
* Run `hystrix-dashboard` application
* Run `hystrix-turbine` application
* Run `billboard-client`
* Access the app at the port Tomcat is started at
 

### Things to try out 
* Access the hystrix dashboard at http://localhost:7979 and put http://localhost:8080/actuator/hystrix.stream into the stream URL
* Try hitting the default endpoint with and without _id_ querystring parameter and notice fallback behavior take effect. Observe how this is tracked on hystrix dashboard
* Rapidly hit the / endpoint without id parameter (error) and observe circuit breaker flip on dashboard (blue).
* Try hitting `http://localhost:8080/slow` endpoint to see how long running operations are treated as failures (fail-fast)
* Try changing `execution.isolation.thread.timeoutInMilliseconds` for slow command to control the timeout when the command is aborted
* Try hitting /getUserById/{1-5} in multiple browser tabs without 5 second window. Observe in app console how requests got combined into a single database query. Examine classes under `billboard-client\src\main\java\com\example\billboardclient\collapser` to see how this works. 
* TODO: Add turbine steps 

 
### Resources to Learn More:
* https://spring.io/guides/gs/circuit-breaker/
* https://cloud.spring.io/spring-cloud-netflix/multi/multi__circuit_breaker_hystrix_clients.html
