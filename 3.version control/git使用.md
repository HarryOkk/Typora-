## 1. 如何在github上新建一个仓库

适用场景：github无仓库，需要创建远程仓库并和本地代码建立关联

---

### 1.1 **github创建仓库**

注意创建仓库的时候不要选择自动创建README.md文件，因为创建这个文件就会导致自动提交一次，后面本地提交的时候会出问题

---

### 1.2**复制仓库的URL**

---



### 1.3 **PC本地进入仓库的文件目录：**

---

### 1.4**鼠标右键打开**`git bash`

---

### 1.5 **依次键入以下命令：**                                   

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

### 1.6**【异常说明】如果在github上创建仓库时选择了README.md文件**

依次键入以下命令：

> 1. 先拉取云端分支，并忽略冲突
> 2. 本地重新进行提交

```shell
#git 初始化
git init
#将目录下所有文件添加到路径
git add .
#提交到本地仓库，自动创建默认分支master
git commit -m '第一次提交'
#添加远程仓库（使用前面复制的GitHub仓库URL）
git remote add origin https://github.com/HarryOkk/Typora-.git
#从远程仓库拉取，并忽略不相关的历史
git pull origin master --allow-unrelated-histories
#提交本地内容到远端分支
git push -u origin master
```

## 2. 新建仓库之后后续如何提交和推送？

### 2.1 在原有分支上提交

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

### 2.2 在新建分支上提交

```shell
# 本地新建分支
git branch new_feature
# 切换到new_feature
git checkout new_fearure
# 添加并提交当前内容
git add .
git commit -m ''
# 提交到远端的新分支（新分支在远端自动创建）
git push origin -u new_fearure
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



## 4. 如何合并github上的两个分支

### 4.1 合并远程两个分支中的内容

> 将**master**分支中的内容合并到**main**分支，并在main分支上创建一个合并提交
>
> 这里面用的方法还是通过拉取到本地合并再提交的方式

1、首先确定本地分支情况

```shell
git branch -a
```

2、假设当前分支为master，拉取当前分支最新的代码

```shell
git pull
```

3、如果本地只有你自己的分支，这时候你需要创建main分支，并切换到main

```shell
git checkout -b main
```

4、本地git创建好main分支之后，拉取远端对应分支的代码

```shell
git pull origin main
```

5、将master分支合并到main分支上（注意都是本地分支）

```shell
git merge master
```

6、合并后的本地main分支内容提交到远端

```shell
git push origin main
```

7、**关于冲突：**

代码的合并过程可能会出现意想不到的冲突，比如：

- 两个分支对同一个文件做了不同的修改
- 同一段代码两个分支做了不同的修改

```shell
# 查看冲突文件
git satus
# 手动编辑冲突文件解决冲突

# 将解决冲突后的文件标记为已解决
git add
# 继续合并过程
git merge --continue
```

### 4.2 将远端分支修改的内容添加到本地分支

场景：当团队成员在远程仓库上创建了新的分支并进行了更改，您可能需要将这些更改合并到本地仓库中。

```shell
git fetch origin
git merge origin/remote-brach
```



## 5. 如何从远程仓库中拉取特定分支的更新？

> 场景：在本地和远端默认分支建立好连接之后，想在本地创建另外一个分支对应远端另外一个分支，从而拉取对应远端分支的代码

```shell
# 从远端仓库拉取特定分支的更新
git fetch origin <远端分支名称>
# 本地切换分支并与远端分支关联
git checkout -b <本地分支名称> origin/<远端分支名称>
```

## 6. 如何查看本地分支对应的远端分支情况

- **查看本地分支**:

```text
git branch
```

- **查看远程分支**:

```text
git branch -r
```

- **查看所有分支**:

```text
git branch -a
```

- **查看本地和远程分支的关联情况**:

```text
git branch -vv
```

## 7 如果清除本地修改

​	有时拉取代码时，发现本地做了一些修改，和远端分支有冲突，这个时候可以使用命令

```shell
git stash
```

清除本地修改，并将修改入栈。

然后就可以正常使用`git pull`命令拉取远端分支了。

## 常见问题

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
