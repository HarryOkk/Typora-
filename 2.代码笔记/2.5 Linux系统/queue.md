> 本笔记的来源是《高质量嵌入式Linux C编程》，ISBN:

## 1、创建消息队列

```c
#include <stdio.h>
#include <stdlib.h>
#include <sys/msg.h>
#include <unistd.h>
#include <time.h>

const char *msgpath = "/unix/msgqueue";
key_t key;
int gflags,msgid;

key = ftok(msgpath,'a');
gflags = IPC_CREAT|IPC_EXCEL;
msgid = msgget(key,gflags|00666);
if (msgid == -1)
{
    printf("msgget error\n");
}
```

## 2、向消息队列发送消息

```c
//发送标志（非阻塞式）
int sflags = IPC_NOWAIT;
//定义发送函数的执行返回
int reval;
//定义发送消息的消息结构体，并创建对应的结构体变量msg_sbuf
struct msgsbuf
{
    int mtype;
    char mtext[1];
}msg_sbuf;
//为消息体赋值
msg_sbuf.mtype = 10;
msg_sbuf.mtext[0] = 'a';
reval = msgsnd(msgid,&msg_sbuf,sizeof(msg.sbuf.mtext),sflags);
if(reval == -1)
{
 	printf("msgsnd errror\n");
}
```

## 3、从消息队列接收消息

```c
int reval;

//接受函数的执行标志
int rflags = IPC_NOWAIT|MSG_NOERROR;
//定义消息接收结构
struct msgmbuf
{
    int mtype;
    char mtext[10];
}msg_rbuf;
//接收消息
reval = msgrcv(msgid,&msg_rbuf,sizeof(msg_rbuf.mtext),0,rflags);
if(reval == -1)
{
    printf("msgrcv errror\n");
}
```

## 4、设置消息队列属性

> 主要对消息**队列头**进行操作，队列头变量类型为**struct msqid_ds**

```c
int reval;

//初始化队列头
struct msqid_ds msg_cinfo;
msg_cinfo.msg_perm.uid = 8;
msg_cinfo.msg_perm.gid = 8;
msg_cinfo.msg_qtypes = 16388;//设置消息队列的大小（常用）
reval = msgctl(msgid,IPC_SET,&msg_cinfo);
if(reval == -1)
{
    printf("msg info set errror\n");
}
```

## 5、删除消息队列

```c
int reval;
reval = msg_ctl(msgid,IPC_RMID,NULL);
if (reval == -1)
{
    printf("msg remove failure\n");
}
```

## 6、读取消息队列属性

```c
int reval;
//初始化队列头
struct msqid_ds msg_info;
reval = msgctl(msgid,IPC_STAT,&msg_info);
if(reval == -1)
{
    print("msg info get error\n");
}
//打印输出消息队列的状态和属性
printf("\n");
printf("消息队列的ID:%d\n",msgid);
printf("消息队列的读写权限：%u\n",msg_info.msg_perm.mode);
printf("消息队列的uid：%u\n",msg_info.msg_perm.uid);
printf("当前消息队列大小：%lu bytes\n",msg_info.msg_cbytes);
printf("当前消息队列中消息数量：%lu\n",msg_info.msg_qnum);
printf("当前消息队列的最大容量：%lu bytes\n",msg_info.msg_qbytes);
printf("最新发送消息到队列的进程pid：%d \n",msg_info.msg_lspid);
printf("最新接收消息进程pid：%d \n",msg_info.msg_lrpid);
printf("最新消息发送时间：%s\n",ctime(&msg_info.msg_stime));
printf("最新消息接收时间：%s\n",ctime(&msg_info.msg_rtime));
printf("最新消息队列属性变化时间：%s\n",ctime(&msg_info.msg_ctime));
```