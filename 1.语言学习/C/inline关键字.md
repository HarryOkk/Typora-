# 内联函数定义 inline

## 特点：

1. 在C99和C11标准之后支持
2. 省去函数建立调用、传递参数、跳转到函数代码并返回的开销
3. 用函数体的代码替换函数调用

## 注意事项：

1. 内联函数应该比较短小：把较长的函数变成内联并未节约多少时间，因为执行函数体的时间比调用函数的时间长得多。
2. 内联函数定义与函数调用必须在同一个文件中。若其他文件想调用内联函数，则应把内联函数的定义放在头文件中，其他文件引用该头文件。



## 定义说明：

#### inline static 定义：

编译器可以优化代码，且函数为内联定义，外部不可使用

```c
inline static double square(double);
double square(double x) { return x * x; }
int main()
{
　　 double q = square(1.3);
　　 ...
```



#### inline定义：

可替换的外部定义：外部可使用并且外部同名函数定义可将其替换

```c
inline double square(double x) { return (int) (x * x + 0.5); }
void masp(double w)
{
　　 double kw = square(w);
　　 ...
```



#### 普通定义：

该函数具有外部链接，外部可使用

```c
double square(double x) { return (int) (x*x); }
void spam(double v)
{
　　 double kv = square(v);
　　 ...
```

