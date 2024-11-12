## 1 基础

### 1.1 驱动程序与硬件交互的方法

方法一：通过子系统函数库

方法二：通过ioremap，把实际物理地址转换为虚拟地址，直接访问寄存器

> 注意：Linux系统驱动开发除了保证框架之外，和裸机/HAL库开发没有本质区别

### 1.2 访问CPU的GPIO引脚

###### a. 一般CPU组成与定位GPIO引脚

​	CPU的外设由各种模块组成：UART模块、GPIO1模块（一组引脚，包括GPIO1.1、GPIO1.2、......）、GPIO2模块（GPIO2.1、GPIO2.2、......）。

​	所以定位一个CPU的GPIO引脚需要两件事：

​		**1、确定为哪一组引脚；**

​		**2、确定为哪一组引脚的第几个**

###### b. Linux软件中GPIO的编号统一化

​	Linux软件中，所有GPIO模块的所有引脚都进行统一编号，比如

​		GPIO0-15：GPIO1模块（GPIO1.1、GPIO1.2、......）

​		GPIO16-31：GPIO2模块（GPIO2.1、GPIO2.2、......）

###### c. 一般Linux的GPIO编程过程

​	1、获得引脚编号：get_gpio

​	2、设置引脚方向（输入/输出）

​	3、读/写value

### 1.3 确定CPU中某一GPIO引脚在Linux中编号值的方法

###### a. 分组方式

```shell
cat /sys/kernel/debug/gpio # 在该文件中有对于Linux-gpio编号和CPU-gpio分组的对应关系
```

###### b. 编号分组方式

```shell
cd /sys/class/gpio # 在该目录下，每个文件夹名称为cpu某一组gpio的首个引脚编号
```