## 1. 如何在github上新建一个仓库

<img src="D:\my_documents\学习文件\笔记\图片\image-20240104100259658.png" alt="image-20240104100259658" style="zoom:33%;" />

---

**github创建仓库**

![image-20240- 104100403751](D:\my_documents\学习文件\笔记\图片\image-20240104100403751.png)

---

**复制仓库的URL**

![image-20240104100956730](D:\my_documents\学习文件\笔记\图片\image-20240104100956730.png)

---



**PC本地进入仓库的文件目录：**

![image-20240104100439675](D:\my_document\笔记\Typora-\6.图片image-20240104100439675.png)

---

**鼠标右键打开**`git bash`

![image-20240104100553164](D:\my_documents\学习文件\笔记\图片\image-20240104100553164.png)

---

**依次键入以下命令：**

> 1. 先提交（commit）到本地仓库之后再推送（push）到远端（github）
> 2. 本地提交的时候要填入message（提交日志信息）

```shell
#git 初始化
git init
#将目录下所有文件添加到路径
git add .
#提交到本地仓库，自动创建默认分支master
git commit -m '第一次提交'
#添加远程仓库（使用前面复制的GitHub仓库URL）
git remote add origin https://github.com/HarryOkk/Typora-.git
#向远程给仓库推送我的更改
git push -u origin main
```

## 2. 新建仓库之后后续如何提交和推送？

**第一步还是在文件目录下本地打开'git bash'**

**键入以下命令：**

```shell
# 将所有文件变更添加到提交
git add .
# 提交变更到本地仓库，注意加入本地提交日志的描述
git commit -m '本次提交日志的描述'
# 推送到远端仓库（github）
git push
```

## 3. 如何新建分支、切换分支，并将本地新建的分支提交到远端

**第一步还是在文件目录下本地打开**'git bash'

键入以下命令：

```shell
# 在本地仓库创建新分支
git branch 分支名
# 将工作目录切换到新创建的分支，默认复制旧分支的内容到新分支
git checkout 分支名
# 将所有内容添加到变更
git add .
# 由于从旧分支继承，本地内容可以直接提交，注意命令中加入origin，自动为gihub也创建对应的分支
git push origin main
```

## 4. 常见问题

#### 1. PC本地git代理端口号与远端不一致，导致访问github仓库地址失败

**报错显示：**

![image-20240104104605319](D:\my_documents\学习文件\笔记\图片\image-20240104104605319.png)

```shell
10010010@SMEC22-0152 MINGW64 /d/my_documents/学习文件/笔记 (master)
$ git push
fatal: unable to access 'https://github.com/HarryOkk/Typora-.git/': HTTP/2 stream 1 was not closed cleanly before end of the underlying stream
```

****

**解决办法：**

优秀博文：[解决使用git时遇到Failed to connect to github.com port 443 after 21090 ms: Couldn‘t connect to server-CSDN博客](https://blog.csdn.net/qq_40296909/article/details/134285451)

该问题可能由于梯子上网VPN代理异常导致，需要在git本地取消代理，之后再进行push

```shell
#取消https协议的代理
git config --global --unset https.proxy
#重新提交变更并推送
git add .
git commit -m '真的是太棒啦！'
git push
```

#### 2. 上传的单个文件过大导致上传失败

github显示上传内容单个文件的大小，单个文件大小不能超过50M

**报错显示：**

```shell
remote: warning: File 4.IDE工具/4.1交叉编译/1. eclipse安装与使用/jre-8u191-windows-x64.exe is 71.16 MB; this is larger than GitHub's recommended maximum file size of 50.00 MB
```

**解决办法：**

> ​	注意，git跟踪的是文件的变化，而不仅仅是文件的存在与否。所以大文件如果本地提交过，上传github的时候都会把这部分文件带上去。

撤销包含大文件的提交：

```shell
# 查看本地提交日志
$ git log
# 找到提交大文件的记录，并记录其commit id，使用git reset回退到提交大文件之前的提交
$ git reset 9fc3bcd9
# 重新提交
$ git add .
$ git commit -m '注释'
$ git push
```

本地切换分支



#### 3. 换行替换警告

**解释：**

CR/LF是不同操作系统上使用的换行符：

- CR（CarriageReturn回车'\r'）：回到一行的开头，ASCII代码是13
- LF（LineFeed换行'\n'）：另起一行，ASCII代码是10

**应用情况：**

- Dos和Windows平台： 使用回车（CR）和换行（LF）两个字符来结束一行，回车+换行(CR+LF)，即“\r\n”；所以我们平时编写文件的回车符应该确切来说叫做回车换行符。
- Mac 和 Linux平台：只使用换行（LF）一个字符来结束一行，即“\n”；

​	许多 Windows 上的编辑器会悄悄把行尾的换行（LF）字符转换成回车（CR）和换行（LF），或在用户按下 Enter 键时，插入回车（CR）和换行（LF）两个字符。

**警告显示：**

```
warning: in the working copy of '1.语言学习/C/常用内存函数.md', LF will be replaced by CRLF the next time Git touches it
```

#### 4. 大文件上传告警
