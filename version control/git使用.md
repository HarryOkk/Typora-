## 1. 如何在github上新建一个仓库

<img src="D:\my_documents\学习文件\笔记\图片\image-20240104100259658.png" alt="image-20240104100259658" style="zoom:33%;" />

---

**github创建仓库**

![image-20240104100403751](D:\my_documents\学习文件\笔记\图片\image-20240104100403751.png)

---

**复制仓库的URL**

![image-20240104100956730](D:\my_documents\学习文件\笔记\图片\image-20240104100956730.png)

---



**PC本地进入仓库的文件目录****：**

![image-20240104100439675](D:\my_documents\学习文件\笔记\图片\image-20240104100439675.png)

---

**鼠标邮件打开**`git bash`

![image-20240104100553164](D:\my_documents\学习文件\笔记\图片\image-20240104100553164.png)

---

**依次键入以下命令：**

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



