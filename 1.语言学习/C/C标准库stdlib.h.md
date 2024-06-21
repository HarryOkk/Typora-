## 1、随机数相关

### 1.1随机数播种：srand函数

**伪随机：**C中的随机数都是伪随机，为了更加贴近真正的随机，往往使用**`srand((unsigned)time(time_t t))`**来指定随机数种子

**使用方法：**

一般在main函数内开始的地方调用srand函数

```c
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int main()
{
    //定义系统时间对象
    time_t t;
    //设置随机数种子
    srand((unsigned)time(&t));
}
```

### 1.2 伪随机数生成

> 在使用srand函数播种伪随机数之后，便可以使用rand函数生成伪随机数

**函数定义：**

```c
#include <stdlib.h>

int rand(void);
```

**返回值范围：**

0 - RAND_MAX

**函数的使用：**

```c
//产生0-99的随机数
int num = rand()%100;
//产生1-100的随机数
int num = rand()%100+1;
```

