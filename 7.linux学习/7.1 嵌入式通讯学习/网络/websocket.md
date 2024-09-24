> [万字长文，一篇吃透WebSocket：概念、原理、易错常识、动手实践-腾讯云开发者社区-腾讯云 (tencent.com)](https://cloud.tencent.com/developer/article/1887095)

## 1、websocket简介

 ### 1.1 websocket诞生背景

​	传统上，网站实现推送使用“轮询”或“长轮询”方式，这种方式使用HTTP协议，包含比较长的头部，会消耗比较多的带宽资源。

​	后来先出现的轮询技术Comet也是基于HTTP长连接，还是会消耗较多的带宽资源。

​	再后来就是**HTML5**提出了websocket技术，**主打一个长连接！**

#### websocket URI

`ws://echo.websocket.org` ：基于TCP，使用80端口（和HTTP/HTTPS协议相同），可以绕过大多数防火墙的限制

`wss://echo.websocket.org`：基于TLS，使用443端口

### 1.2 websocket简介

  全双工通讯，

### 1.3 websocket通讯特点（优点）

1、控制开销小：websocket头部较小，带宽占用低

2、实时性好

3、有连接状态

4、更好的二进制支持

5、支持扩展

