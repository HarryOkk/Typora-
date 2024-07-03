## 1、信号灯集的创建和初始化

```c
//本例进创建包含1个信号灯的信号灯集
#include <stdio.h>
#include <linux/sem.h>
#include <sys/ipc.h>
#include <errno.h>


int flag1,flag2;//semget调用标志位
int key;//用于创建IPC的key
int init_ok=0;//semaphore是否完成初始化的标志位
int tmperrno;//保存errno
int i;//循环计数
int my_sem_num = 1;//创建信号灯集中的信号灯数量

struct semid_ds sem_info;//存储查询semaphore是否完成初始化的信息
union semun arg;//用于初始化semaphore

flag1 = IPC_CREAT|IPC_EXCL|0666;// 创建新的semaphore
flag2 = IPC_CREAT|0666;//semaphore存在时返回已存在的

key = ftok(SEM_PATH,'a');
semid = semget(key,my_sem_num,flag1);
if(semid == -1)
{
    tmperrno = errno;
    perror("semget");
    if (temperrno == EEXIST)
    {         
        semid = semget(key,my_sem_num,flag2);
        //flag2 只包含IPC_CREAT标志位，因此不会创建新的信号量集，nsems（这里为1）必须与原来的信号灯数目一致
        arg.buf = &sem_info;
        for(i=0;i<max_tries;i++)
        {
            if (semctl(semid,0,IPC_STAT,arg)==-1)
            {
                perror("semctl error");
                i = max_tries;
            }
            else
            {
                if(arg.buf->sem_otime !=0)
                {
                    //将循环次数i设置为最大值，本次循环结束后会自动退出循环
                    i=max_tries;
                    init_ok = 1;
                }
                else
                {
                    sleep(1);
                }
            } 
        }
        if(!init_ok)//sem在上述循环中初始化未成功的处理
        {
            arg.val = 1;//信号量的初始值（sem的初始化工作）
            if (semctl(semid,0,SETVAL,arg)==-1)
            {
                perror("semctl setval srror");
            }

        }
    }
    else
    {
        perror("semget error,process exit");//会打印出errno的信息，供排查
        exit(1);
    }
}
else //sem的初始化工作
{
    arg.val = 1;
    if (semctl(semid,0,SETVAL,arg)==-1)
    {
        perror("semctl setval error");
    }

}
```

### **关于semaphore初始化的说明：**

1、信号量用于对单一资源占用的判断：使用**二进制信号量**，初始化为1（代表资源可用）

```c
arg.val = 1; // 二进制信号量，初始时资源可用
```

2、信号用于对资源剩余个数的判断：使用**计数信号量**，初始化为初始资源数量（剩余可用资源数量）

```c
arg.val = 10; // 计数信号量，初始时有10个资源可用
```

## 2、获取当前信号灯集中某一信号量的信息

```c

arg.buf = &sem_info;//初始化arg.buf指针的指向，在这里，若sem存在，则sem_info已经有值
if(semctl(semid,0,IPC_STAT,arg)==-1)
{
    perror("semctl IPC_STAT srror!");
}
print("owner's uid is %d\n",arg.buf->sem_perm.uid);
print("owner's gid is %d\n",arg.buf->sem_perm.gid);
print("creater's uid is %d\n",arg.buf->sem_perm.cuid);
print("creater's gid is %d\n",arg.buf->sem_perm.cgid);
```

## 3、获取当前Linux发行版系统对semaphore的限制

```c
struct seminfo sem_info2;

arg.__buf = &sem_info2;
if(semctl(semid,0,IPC_INFO,arg)==-1)
perror("semctl IPC_INFO error!");
printf("the number of entries in semaphore map is %d\n",arg.__buf->semmap);
printf("max number of semaphore identifiers is %d\n",arg.__buf->semmni);
printf("max number of semaphore in system is %d\n",arg.__buf->semmns);
printf("the number of undo structures system wide is %d\n",arg.__buf->semmnu);
printf("max number of semaphore per semid is %d\n",arg.__buf->semmsl);
printf("max number of ops per semop call is %d\n",arg.__buf->semmsl);
printf("max number of undo entries per process is %d\n",arg.__buf->semume);
printf("the size of struct sem_undo is %d\n",arg.__buf->semusz);
printf("the maximum semphore value is %d\n",arg.__buf->semvmx);
```

## 4、通过semaphore标记并申请可用资源

```c
struct sembuf askfor_res;

askfor_res.sem_num = my_sem_num;
askfor_res.sem_op = -1;//申请占用一个信号灯资源
askfor_res.sem_flg = SEM_UNDO;//进程退出后委托内核,也可以使用SEM_UNDO|IPC_NOWAIT，非阻塞式申请资源
if(semop(semdid,&askfor_res,sizeof(askfor_res)/sizeof(askfor_res[0]))==-1)
    perror("semop error");
/*
这部分进行自定义的申请资源操作！！！
*/
printf("now free the resources\n");

```

## 5、通过semaphore标记并释放可用资源

```c
struct sembuf free_res;
free_res.sem_num = 0;
free_res.sem_op = 1;//释放一个共享资源
free_res.sem_flg = SEM_UNDO;//进程退出后委托内核,也可以使用SEM_UNDO|IPC_NOWAIT，非阻塞式申请资源

if(semop(semid,&free_res,sizeof(free_res)/sizeof(free_res[0]))==-1)
    if (error == EIDRM)
    {
        printf("the semaphore set was removed\n");
    }
```

## 6、删除semaphore中的某一信号灯

```c
if(semctl(semid,0,IPC_RMID)==-1)
    perror("semctl IPC_RMID error");
else   
    printf("remove sem ok\n");

```

注意：信号灯集是随内核持续的，所以我们只能删除某一信号灯！