## 1、文档说明

​	本例主要是说明：

- 如何在ubuntu中编译u-boot，得到u-boot二进制文件和u-boot镜像文件
- 如何将uboot镜像文件烧写到目标开发板，使其在下次开发板上电后生效

## 2、主要过程

### 2.1 ubuntu下编译u-boot

以下是编译和配置U-Boot引导加载程序的详细步骤和解释：

#### 1. 进入SDK目录

```sh
cd /home/book/100ask_imx6ull-sdk
```

这是你的工作目录，包含所有相关的源码和工具。

#### 2. 进入U-Boot源码目录

```sh
cd Uboot-2017.03
```

进入U-Boot源码目录。U-Boot是一个广泛使用的开源引导加载程序，支持多种架构和板级支持包（BSP）。

#### 3. 清理之前的构建

```sh
make distclean
```

`make distclean`命令会清理U-Boot源码目录中的所有编译生成的文件和配置文件，恢复到干净的状态。这是一个好的做法，确保没有之前构建的残留文件干扰新的编译过程。

#### 4. 配置U-Boot

```sh
make mx6ull_14x14_evk_defconfig
```

`make mx6ull_14x14_evk_defconfig`命令使用`mx6ull_14x14_evk_defconfig`配置文件来配置U-Boot。这一步会生成一个适用于你的硬件平台（例如i.MX6ULL 14x14 EVK）的默认配置文件`.config`。

#### 5. 编译U-Boot

```sh
make
```

`make`命令会根据`.config`文件编译U-Boot源码，生成U-Boot引导加载程序。编译过程可能需要一些时间，具体取决于你的硬件和配置。

### 编译结果

编译成功后，你会在`Uboot-2017.03`目录下找到以下几个重要的文件：

- **u-boot-dtb.imx**：包含设备树信息的U-Boot镜像文件。这个文件通常用于烧录到eMMC、SD卡或其他存储设备中。
- **u-boot.bin**：编译生成的U-Boot二进制文件。
- **SPL**（如果你的配置使用了SPL）：二级引导加载程序（Secondary Program Loader），在一些系统中用于初始化硬件和加载主引导加载程序。
- 

### 2.2 将u-boot镜像文件烧写到开发板

将上述文件上传到开发板的用户路径下（可以使用nfs的方式），然后执行以下烧写命令：

### 1. 禁用只读模式

```sh
echo 0 > /sys/block/mmcblk1boot0/force_ro
```

#### 目的

这条命令将`/sys/block/mmcblk1boot0/force_ro`文件的值设置为0，禁用`mmcblk1boot0`分区的只读模式，使其变为可写状态。这是必要的一步，因为默认情况下，这些特殊分区通常是只读的，以防止意外写操作。

### 2. 写入U-Boot引导加载程序

```sh
dd if=u-boot-dtb.imx of=/dev/mmcblk1boot0 bs=512 seek=2
```

#### 目的

`dd`命令用于将`u-boot-dtb.imx`文件（U-Boot引导加载程序）写入`/dev/mmcblk1boot0`设备（SD卡的特定分区）。

- `if=u-boot-dtb.imx`：指定输入文件为`u-boot-dtb.imx`。
- `of=/dev/mmcblk1boot0`：指定输出文件为`/dev/mmcblk1boot0`，即目标设备。
- `bs=512`：设置块大小为512字节。
- `seek=2`：从目标设备的第2个块开始写入，这通常是因为引导加载程序需要特定的对齐或位置。

### 3. 启用只读模式

```sh
echo 1 > /sys/block/mmcblk1boot0/force_ro
```

#### 目的

这条命令将`/sys/block/mmcblk1boot0/force_ro`文件的值设置为1，重新启用`mmcblk1boot0`分区的只读模式。这样可以保护分区，防止意外的写操作。
