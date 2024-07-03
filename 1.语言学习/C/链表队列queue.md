> **概念定义：**
>
> 项：队列的基本单元
>
> 节点：实现队列的链表数据结构的基本单元

## 1、queue.h头文件

```c
#ifndef _QUEUE_H_
#define _QUEUE_H_
#include <stdbool.h>



#define MAXQUEUE 10//定义最大队列长度


typedef int Item;//此处插入队列元素Item的定义，用户可以根据实际情况自定义其数据类型
typedef struct node
{
    Item item;
    struct node * next;
} Node;//定义队列链表节点结构类型
typedef struct queue
{
    Node * front;//队列头
    Node * rear;//队列尾
    int items;//队列中的项数
} Queue;//定义队列信息结构类型
void InitializeQueue(Queue * pq);//初始化队列为空
bool QueueIsFull(const Queue * pq);//判断队列是否已满（因无需对队列进行变更，形参使用const关键字限定）
bool QueueIsEmpty(const Queue * pq);//判断队列是否为空
int QueueItemCount(const Queue * pq);//返回队列的项数
bool EnQueue(Item item, Queue * pq);//在队列尾添加项
bool DeQueue(Item *pitem, Queue *pq);//在队列头删除项
void EmptyTheQueue(Queue * pq);//清空队列

#endif
```

## 2、queue.c文件（队列的接口函数实现）

```c
#include "queue.h"


void InitializeQueue(Queue * pq)//初始化队列为空:将队列首项和尾项的指针设置为NULL
{
    pq->front = pq->rear = NULL;
    pq->items = 0;
}

bool QueueIsFull(const Queue * pq)//判断队列是否已满
{
    return pq->items == MAXQUEUE;
}

bool QueueIsEmpty(const Queue * pq)//判断队列是否为空
{
    return pq->items == 0;
}
int QueueItemCount(const Queue * pq)//返回队列的项数
{
    return pq->items;
}
static void CopyToNode(Item item,Node *pn)//将队列项->链表节点中
{
    pn->item = item;
}
static void CopyToItem(Node *pn,Item *pitem)//链表节点内容->队列项
{
     *pitem = pn->item;
}
bool EnQueue(Item item, Queue * pq)//在队列尾添加项
{
    /*创建新节点*/
    Node * pnew;
    if (QueueIsFull(pq))
        return false;
    pnew = (Node *)malloc(sizeof(Node));//为新节点申请内存空间
    if (pnew == NULL)
    {
        fprintf(stderr,"Unable to allocate memory!\n");//内存不足
        exit(1);
    }
    /*初始化新节点*/
    CopyToNode(item,pnew);
    pnew->next = NULL;
    if (QueueIsEmpty(pq))
		pq->front = pnew;
    else
        pq->rear->next = pnew;
    pq->rear = pnew;
    (pq->items)++;
    return true; 
}
bool DeQueue(Item *pitem, Queue *pq)//在队列头删除项
{
    Node * pt;//用于临时存储队首项
    if (QueueIsEmpty(pq))
        return false;
    CopyToItem(pq->front, pitem);//用于返回被删除的项
    pt = pq->front;
    pq->front = pq->front->next;
    free(pt);
    (pq->items)--;
    if (pq->items == 0)
        pq->rear = NULL;
    return true;  
}
void EmptyTheQueue(Queue * pq)//清空队列
{
    Item dummy;
    while(!QueueIsEmpty(pq))
        DeQueue(&dummy,pq);
}
```

