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

​	一般我们会查看原理图，然后通过原理图确认CPU中的GPIO引脚编号，再转换成Linux中的引脚编号。这样就可以在驱动程序中直接操作引脚了。

###### a. 分组方式

```shell
cat /sys/kernel/debug/gpio # 在该文件中有对于Linux-gpio编号和CPU-gpio分组的对应关系
```

###### b. 编号分组方式

```shell
cd /sys/class/gpio # 在该目录下，每个文件夹名称为cpu某一组gpio的首个引脚编号
```

## 2 子系统方式使用GPIO

### 2.1 GPIO子系统函数

###### a.GPIO子系统函数有新、老两套：

| **descriptor-based**       | **legacy**            |
| -------------------------- | --------------------- |
| **获得GPIO**               |                       |
| **gpiod_get**              | gpio_request          |
| **gpiod_get_index**        |                       |
| **gpiod_get_array**        | gpio_request_array    |
| **devm_gpiod_get**         |                       |
| **devm_gpiod_get_index**   |                       |
| **devm_gpiod_get_array**   |                       |
| **设置方向**               |                       |
| **gpiod_direction_input**  | gpio_direction_input  |
| **gpiod_direction_output** | gpio_direction_output |
| **读值、写值**             |                       |
| **gpiod_get_value**        | gpio_get_value        |
| **gpiod_set_value**        | gpio_set_value        |
| **释放GPIO**               |                       |
| **gpio_free**              | gpio_free             |
| **gpiod_put**              | gpio_free_array       |
| **gpiod_put_array**        |                       |
| **devm_gpiod_put**         |                       |
| **devm_gpiod_put_array**   |                       |

###### b.注意

- 申请gpio即占用gpio资源（gpio_request），使用完需要释放gpio（gpio_free）
- 设置gpio的值需要1、申请gpio引脚（gpio_set_value）；2、设置方向为输出（gpio_direction_output）；3、释放gpio（gpio_free）
- 读取gpio的值不需要申请，直接运行gpio_get_value
- 设置gpio方向为输入后（gpio_direction_input），就可以设置使用gpio对应的中断了，无需申请、释放
- **每个函数中使用gpio最好都包含申请和释放的过程，这样不容易弄混**

> 每次使用都包含申请和释放过程不好吗？

### 2.2 注意

