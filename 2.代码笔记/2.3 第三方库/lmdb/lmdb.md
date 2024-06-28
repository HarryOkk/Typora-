## 1、简介

​	高性能**嵌入式数据库**，常用于需要快速访问数据的场景。

**特点：**

1、内存数据库：读写速度快，不占用外存；掉电丢失、容量有限（依赖于进程空间的虚拟内存地址映射）

2、ACID（原子性、一直选哪个、隔离性、持久性）

3、支持多个读**事务**同时进行，但同一时刻只支持一个写事务

4、跨平台友好：Linux、Windows、macOS

5、记录为key-value形式，不支持SQL语句

**典型应用：**

嵌入式设备、缓存系统、高性能计算

## 2、使用

> lmdb的使用需要理解环境、事务、游标的概念
>
> 1、环境：指定数据库目录以及数据库的大小，并返回环境句柄指针
>
> 2、事务：一次读或写事务，需指定环境；同一时刻，读事务可以有多个，写事务只能有一个；创建事务之后就可以进行简单的读写删除操作了
>
> 3、游标：对数据进行批量处理时需要使用游标

### 2.1 环境创建与打开

```c
//创建MDB_env类型指针
MDB_env *env;
//创建数据库环境
mdb_env_creat(&env);
//打开环境
mdb_env_open(env,"./testdb",0,0664);
```

### 2.2 创建事务

```c
//创建MDB_txn类型指针
MDB_txn *txn;
//创建事务
mdb_txn_begin(env,NULL,0,&txn);
```

### 2.3 打开数据库

```c
MDB_dbi dbi;
mdb_dbi_open(txn,NULL,0,&dbi);
```

### 2.4 写入数据库

**普通写入：**

```c
//数据库记录准备
MDB_val key, data;
char strKey[50],srtValue[50];
key.mv_size = strlen(strKey)*sizeof(char);
key.mv_data = strKey;
key.mv_size = strlen(srtValue)*sizeof(char);
key.mv_data = srtValue;

//写入数据库
mdb_put(txn,dbi,&key.&data,0);
```

**游标写入：**

```c
//打开游标
MDB_cursor *cursor;
mdb_cursor_open(txn,dbi,&cursor);
//数据库记录准备
MDB_val key, data;
char strKey[50],srtValue[50];
//读取数据
mdb_cursor_get(cursor, &key, &data, MDB_NEXT) == 0);
memset(strKey, 0, sizeof(strKey));
memset(strValue, 0, sizeof(strValue));
strncpy(strKey, (const char*)key.mv_data, (int)key.mv_size);
strncpy(strValue, (const char*)data.mv_data, (int)data.mv_size);
printf("key:%s, value:%s\n",strKey, strValue);
```

### 2.5 善后清理工作

```c
mdb_cursor_close(cursor);//游标关闭
mdb_dbi_close(env, dbi);//数据库关闭
mdb_txn_abort(txn);//事务关闭
mdb_env_close(env);//环境关闭
```

