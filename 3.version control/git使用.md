## 1. 如何在github上新建一个仓库

<img src="D:\my_documents\学习文件\笔记\图片\image-20240104100259658.png" alt="image-20240104100259658" style="zoom:33%;" />

---

**github创建仓库**

![image-20240104100403751](D:\my_documents\学习文件\笔记\图片\image-20240104100403751.png)

---

**复制仓库的URL**

![image-20240104100956730](D:\my_documents\学习文件\笔记\图片\image-20240104100956730.png)

---



**PC本地进入仓库的文件目录：**

![image-20240104100439675](D:\my_documents\学习文件\笔记\图片\image-20240104100439675.png)

---

**鼠标邮件打开**`git bash`

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

**现象：**

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

