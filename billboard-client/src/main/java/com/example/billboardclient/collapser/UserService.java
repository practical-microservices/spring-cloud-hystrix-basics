package com.example.billboardclient.collapser;

import com.netflix.hystrix.contrib.javanica.annotation.HystrixCollapser;
import com.netflix.hystrix.contrib.javanica.annotation.HystrixCommand;
import com.netflix.hystrix.contrib.javanica.annotation.HystrixProperty;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;


@Service
public class UserService {

    private final Logger log = LoggerFactory.getLogger(UserService.class);

    /**
     * Call to this method that arrive within 5000 millisecond will be combined into
     * a single call to getUsers. This can be used reduce the number of remote
     * calls made a service thus increasing performance and reducing the load
     * on the remote service.
     *
     * @param id
     * @return
     */
    @HystrixCollapser(
            scope = com.netflix.hystrix.HystrixCollapser.Scope.GLOBAL,
            batchMethod = "getUsers",
            collapserProperties = {@HystrixProperty(name="timerDelayInMilliseconds", value="5000")})
    public User getUserById(String id) {
        return null;
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

    @HystrixCommand
    public List<User> getUsers(List<String> ids) {
        // the list of return type and paramter must have the same number of elements
        // order matters - used to establish mapping between request/response

        log.info("Collapsing " + ids.size() + " requests into 1");
        List<User> users = new ArrayList<>();

        String sql = "SELECT * FROM USERS WHERE ID in (" + ids.stream().distinct().reduce((id, predicate) -> predicate + "," + id).get() + ")";
        log.info(sql);

        for (String id : ids) {
            users.add(new User(id, "Name is " + id));
        }
        return users;
    }
}
