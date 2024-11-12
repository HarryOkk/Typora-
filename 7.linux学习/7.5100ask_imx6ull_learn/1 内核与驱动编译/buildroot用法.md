## 1、文档说明

buildroot是很棒的Linux系统构建框架（工具）。



## 2、buildroot的作用

配置编译内核

配置编译u-boot（bootloader）

配置编译文件系统



## 3、典型使用

### 3.1 韦东山6ull buildroot

```sh
# 进入buildroot源码
cd /home/book/100ask_imx6ull-sdk/Buildroot_2020.02.x

# 清理编译环境
make clean

# 配置.config
make 100ask_imx6ull_pro_ddr512m_systemv_qt5_defconfig
#或者也可以使用 make menuconfig来进行手动配置
make menuconfig

# 使用buildroot进行编译
make all -j4
```

**编译结果**

结果在buildroot源码路径的 output/image/ 路径下，包括：

u-boot镜像：

内核镜像：

文件系统：

整个文件系统的镜像（包含上述所有内容）：



**镜像文件的烧录：**

韦东山使用的自己开发的烧写工具，方法是将上述编译好的映像文件替换烧写工具指定的路径中的文件。所以不具有参考价值，这里面不再提及。



