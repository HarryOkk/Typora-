## 1. 参考博文

[JSON入门看这一篇就够了 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/33792109)

[JSON 教程 | 菜鸟教程 (runoob.com)](https://www.runoob.com/json/json-tutorial.html)

## 2. JSON学习

### 2.1 什么是JSON

JSON：JavaScript Object Notation

​	是存储和交换信息的语法，类似XML，并且JSON独立于任何语言，很通用。

### 2.2 JSON的特点

1. 更容易创建JavaScript对象
2. 和XML同样是存储和交换文本信息的手段（协议消息格式），但更轻量，解析速度会很快
3. XML解析成DOM对象时，浏览器（IE和firefox）会有差异
4. JSON的内容更简单

### 2.3 JSON语法

**基本格式：**

1. JSON用`{}`表示对象
2. 用`[]`表示数据
3. 数据在`键值对`中
4. 数据用`,`分隔
5. 使用`\`来转义字符

**对象：**大括号 **{}** 保存的对象是一个**无序**的**名称/值**对集合。一个对象以左括号 **{** 开始， 右括号 **}** 结束。每个"键"后跟一个冒号 **:**，**名称/值**对使用逗号 **,** 分隔。

**数组：**中括号 [] 保存的数组是值（value）的**有序**集合。一个数组以左中括号 [ 开始， 右中括号 ] 结束，值之间使用逗号 , 分隔。



**键：**双引号括起来的字符串



**值：**

- 数据：双引号括起来的字符串（string）、数值（number，可以是整型或浮点型）、布尔（bull,true\false）、null
- JSON结构：对象（object）、数组（array）

