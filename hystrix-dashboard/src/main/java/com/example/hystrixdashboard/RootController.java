package com.example.hystrixdashboard;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class RootController {
    @GetMapping("/")
    public String get()
    {
        return "redirect:/hystrix";
    }

}

