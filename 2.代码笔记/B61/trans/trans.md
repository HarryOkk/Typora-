# 包含trans.c和trans.h

>**C代码无非几个重要内容：**
>
>​	1.结构类型定义
>
>​	2.变量定义和常量定义：形式为**数据类型：变量名称含义**
>
>​	3.功能函数声明
>
>​	4.业务代码（函数）声明
>
>**C代码的编写思考方式**：指针是拿到值得方式。两大板块：如何组织和拿到值、如何处理值。
>
>**这么看画数据流图是理解代码逻辑的重要方式了**

---

说明：

> trans部分代码用于进程间通讯
>
> 



## 1. 结构类型定义

### 宏定义：trans缓存大小

```c
#define TRANS_BUFFER_LEN (EXTRANET_FRAME_LEN_MAX*2)
```

### 结构体类型：trans通讯结构类

```c
typedef struct
{
    //libuv库的管道句柄，用于管道通信
	uv_pipe_t handle;
    //libuv库的连接句柄，用于建立连接的异步操作
	uv_connect_t connect;
    //libuv的异步事件句柄，可能用于异步地关闭事件
	uv_async_t close_ev;
    //libuv的定时器句柄
	uv_timer_t reconnect_timer;
	uv_timer_t heartbeat_timer;
	//队列句柄：用结构体实现的队列类型，其中包括缓冲区首地址指针、缓冲区总长度、当前缓冲区长度、队列进位置、队列出位置
	data_fifo_t rx_fifo;
    //字节数组，用于给队列句柄（rx_fifo）的实际内容缓冲区赋值
	uint8_t fifo_buf[TRANS_BUFFER_LEN];

	uint8_t domain; /* 本机设备域 */
	uint8_t module_id; /* trans通信的直连模块ID，注意使用中与目的模块ID区分（不一定一致） */
	uint8_t heartbeat_seq; /* 心跳用seq */
	char server_sock_name[32]; /* NOTE: 名称长度不要超过31个字符 */
	char client_sock_name[32]; /* NOdm_gc_strTE: 名称长度不要超过31个字符 */
	char name[16]; /* NOTE: 名称长度不要超过15个字符 */

	char tmp_str[128]; /* NOTE: 打印用临时字符串缓存，不要超过127字符 */
	char allocbuf[EXTRANET_FRAME_LEN_MAX]; /* NOTE: 用于trans_alloc_buffer()，不使用动态申请 */

	uint8_t buf[EXTRANET_FRAME_LEN_MAX]; /* 其它临时使用的buf */
	uint16_t buf_len; /* 临时使用的发送数据长度 */

	parse_result_t parse_result;
} trans_comm_t;
```

## 2. 变量定义和常量定义

## 3. 功能函数声明

### trans接收回调函数

```c
/*
主要功能：将外网通信数据（comm进程通过管道发送的）加入队列
函数输入：uv文件流句柄、读取到的字节数、常量uv缓存句柄
函数返回：无
函数动作：
1.通过nread判断管道通信是否出错。若出错，打印、停掉定时器并重新连接
2.正常读取数据：将缓存数据入队列
3.判断是否有更多的数据需要读取，如果有，继续加入队列并继续判断
4.没有更多的数据则不做处理
*/

void trans_recv_cb(uv_stream_t *handle, ssize_t nread, const uv_buf_t *buf)
{
	trans_comm_t *self = (trans_comm_t*) (handle->data);

	if (nread < 0)
	{
		sprintf(self->tmp_str, "[%s recv]", self->name);
		uv_err(nread, self->tmp_str);

		if (nread == UV_EOF)
		{
			/* 断开连接后处理及重连 */
			uv_read_stop(handle);
			uv_timer_stop(&self->heartbeat_timer);
			uv_close((uv_handle_t*) handle, NULL);
			uv_timer_start(&self->reconnect_timer, trans_reconnect_cb, 500, 0);
		}
	}
	else if (nread > 0)
	{
#if 0 /* 打印数据 */
		fprintf(stderr, "[%s recv %d] ", self->name, nread);
		for (int i = 0; i < nread; ++i)
		{
			fprintf(stderr, "%02X ", buf->base[i]);
		}
		fprintf(stderr, "\n");
#endif
		/* 将数据放入data_fifo */
		data_fifo_put(&self->rx_fifo, nread, (const uint8_t*) (buf->base));
		/* NOTE: 由于采用固定缓存，不需要也不能free */
		bool has_more_frame;
		do
		{
			has_more_frame = extranet_handle_data(&self->rx_fifo, &self->parse_result);
			/* TODO: 根据handle函数返回的parse_result来处理需要应答的情况：uv_write() */
			uint16_t len_tx = self->parse_result.len_tx;
			if (len_tx > 0)
			{
#if 0
				fprintf(stderr, "[trans send %d] ", len_tx);
				for (int i = 0; i < len_tx; ++i)
				{
					fprintf(stderr, "%02X ", self->parse_result.tx[i]);
				}
				fprintf(stderr, "\n");
#endif
				/* 实时应答数据的处理 */
				new_uv_write(handle, self->parse_result.tx, len_tx);
			}
		} while (has_more_frame);
	}
	else
	{
		/* 不需要处理 */
	}
}
```



## 4. 业务代码（函数）声明

### trans初始化trans函数

```c
/*
函数动作：
trans句柄的初始化
libuv连接定时器的初始化
libuv管道初始化
*/
void trans_init_trans(trans_comm_t *self, char *name, char *server_sock_name, char *client_sock_name, uint8_t module_id)
{
    //trans通讯句柄置零
	memset(self, 0, sizeof(trans_comm_t));
	//将函数传入的名字、socket服务端名字、socket客户端名字、进程模块名保存到trans通讯句柄中（因对象为字符类型，所以使用字符处理函数，而不是内存处理函数）
	self->module_id = module_id;
	strncpy(self->name, name, sizeof(self->name) - 1);
	strncpy(self->server_sock_name, server_sock_name, sizeof(self->server_sock_name) - 1);
	if (client_sock_name != NULL)
	{
		strncpy(self->client_sock_name, client_sock_name, sizeof(self->client_sock_name) - 1);
	}
	//通讯队列的初始化
	data_fifo_init(&self->rx_fifo, TRANS_BUFFER_LEN, self->fifo_buf);
	//libuv管道句柄
	self->handle.data = self;
    //libuv连接句柄
	self->connect.data = self;
    //libuv定时器句柄
	self->reconnect_timer.data = self;
	self->heartbeat_timer.data = self;
	//libuv定时器初始化
	uv_timer_init(loop, &(self->reconnect_timer));
	uv_timer_init(loop, &(self->heartbeat_timer));
	//初始化管道并发起管道连接
	trans_start_connect(self);
}
```

### trans初始化函数

```c
/*
函数动作：
定时器初始化
comm通信对象回调函数注册
*/
void trans_init(void)
{
    //当前进程和trans进程的管道通信、libuv定时器初始化
	trans_init_trans(&trans_trans, "trans_trans", "/server_transSocket", "/smec_t", DIGTEL_MODULE_TRANS);
    //当前进程和comm进程的管道通信、libuv定时器初始化
	trans_init_trans(&trans_comm, "trans_comm", "/server_commSocket", "/smec", DIGTEL_MODULE_EXTRANET);
	//comm通信回调函数注册
	comm_set_response_cb(COMM_OBJ_TRANS_TRANS, &trans_trans, trans_resp_cb);
	comm_set_response_cb(COMM_OBJ_TRANS_COMM, &trans_comm, trans_resp_cb);
}
```

