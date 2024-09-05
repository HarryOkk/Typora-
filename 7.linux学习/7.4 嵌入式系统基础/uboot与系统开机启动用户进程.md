## 1、uboot（Universal Boot Loader）的作用

初始化硬件

加载并启动操作系统

提供命令行接口，用于引导设置和调试

**总之：uboot是用来加载操作系统的**

## 2、系统开机启动用户进程

在uboot结束之后，往往需要启动一些用户进程来运行业务代码。(**推荐使用Systemd，其提供了更多的控制和管理功能，但目前DigTel-II使用的是cron和/etc/rc.local)**

### 2.1 crontab

'cron'是Unix/Linux系统中的定时任务调度工具。

可以设置定时任务、设置在设备重启后执行的任务。

```shell
#打开‘crontab’编辑器
crontab -e
```

可以在编辑器内设置Linux服务，在什么条件下启动系统中的某一可执行文件。注意可执行文件要具备对应的权限（用chmod 777赋予权限）。





### 2.2 使用Systemd服务





### 2.3 在‘/etc/rc.local’中添加命令

说明：

1. 编辑/etc/rc.local文件，可以让指定路径下的进程在Linux系统进入多用户模式后运行
2. 该方法比较传统，很多现代的Linux发行版默认禁用，但可以启用它



```shell
# 编辑/etc/rc/local文件
sudo nano /etc/rc.local
#在文件中添加运行可执行文件的命令
/path/to/your/rxrcutable &
#确保/etc/rc.local文件具有可执行权限
sudo chmod 777 /etc/rc.local
```

**下面是一个典型的应用：**

```shell
#!/bin/sh -e
#
# rc.local
#
# This script is executed at the end of each multiuser runlevel.
# Make sure that the script will "exit 0" on success or any other
# value on error.
#
# In order to enable or disable this script just change the execution
# bits.
#
# By default this script does nothing.

# Custom script to run at startup
/path/to/chg_ota_ver.sh

echo 30000 > /proc/sys/vm/min_free_kbytes

exit 0

```

其中，/path/to/chg_ota_ver.sh是指定的可执行文件（具有可执行权限，chmod 777）



### 2.4 在用户的~/.bashrc或~/.profile中添加命令

该种方法在用户登录时自动运行命令

```shell
# 编辑~/.bashrc或~/.profile文件
nano ~/.bashrc
#在文件末尾添加运行可执行文件的命令
/path/to/your/executable &
```

