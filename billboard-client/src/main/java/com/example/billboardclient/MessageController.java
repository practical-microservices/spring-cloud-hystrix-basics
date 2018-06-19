package com.example.billboardclient;

import com.netflix.hystrix.contrib.javanica.annotation.HystrixCommand;
import com.netflix.hystrix.contrib.javanica.annotation.HystrixProperty;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class MessageController {

    @Autowired
    private MessageService messageService;

    @GetMapping("/")
    public String getMessage(@RequestParam(value = "id", required = false) String billboardId)
    {
      return messageService.getMessage(billboardId);
    }

    @GetMapping("/slow")
    public String slow(){
        return messageService.slowOperation("test");
    }

}
