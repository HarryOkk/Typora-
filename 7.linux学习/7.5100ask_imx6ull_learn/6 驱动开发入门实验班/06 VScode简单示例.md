## 1 使用工作区WorkSpace

在Vscode中如何将两个目录中的代码关联起来？并实现索引？

​	**使用工作区！**

## 2 Vscode工作区的创建与扩展

### 2.1 创建工作区

a. 打开一个文件夹

b. Save WorkSpace as

### 2.2 扩展工作区

a. Add Folder to WorkSpace

## 3 Clangd插件的使用

Clangd用于索引代码，实现我们驱动、APP代码与内核源码的跳转（解析目录、生成索引）

### 3.1 使用方法

###### a. 下载Clangd插件

###### b. 确保冲突插件被禁用：

Setting → Setting.json：

```JSON
"C_Cpp.intelliSenseEngine":"Disabled"
```

###### c. 在已添加到WorkSpace的目录下执行

> 注意：工程下存在MakeFile

```shell
bear make #当前目录下生成compile_commands.json索引文件
```

###### d. 修改compile_commands.json文件

替换里面所有的"cc" 为 目标板的交叉编译工具链（"arm_buildroot......-gcc"）

###### e. 在Vscode中打开任意一个.c文件

Clangd会自动索引，并在目录中创建**.cache**数据库文件

## 4 常用快捷键

ctrl + P：查找文件并打开（在工作区中全局查找）

ctrl + G：跳转行号

ctrl + 鼠标左键：跳转到定义

ctrl + shift + -：前进（不方便，修改快捷键为ctrl + →）

ctrl + Alt + -：后退（不方便，修改快捷键为ctrl + ←）

ctrl + `(Tab上面)：打开/隐藏命令行

> 注意：
>
> ​	修改快捷键的界面：file → Preference → Keyboard Shortcuts
>
> ​	或者直接使用快捷键ctrl+k ctrl+s，即可快速打开快捷键设置界面

