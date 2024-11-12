## 1、在Ubuntu虚拟机上的完全开发环境

a. 串口直接连接到虚拟机，使用Ubuntu中的Winterm串口工具访问开发板（imx6ull），Winterm软件也可以访问Ubuntu虚拟机的shell

b. 使用虚拟机跨过Windows，直接访问开发板的网口：

①如果你直接通过Windows网口连接开发板，需要在Ubuntu虚拟机上配置一个桥接网卡

②使用USB网卡：

​	Ubuntu Setting → 配置net → USB_ethernet 中IP地址为固定IP：（IP：192.168.5.11，MSAK：255.255.255.0）

## 2、问题

a. 如果串口连接到了Windows：

​	在Ubuntu中忘记连接规则 → 重新插拔串口 → 重新设置连接规则







