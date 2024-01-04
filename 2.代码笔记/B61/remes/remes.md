# remes.c和remes.h

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

## 1. 结构类型定义

### 结构体类型：电梯当前状态句柄

```c
typedef struct
{
	uint8_t msg_version[8];
	uint32_t op_id;
	uint32_t ele_id;
	uint32_t dev_id;
	uint8_t recv_time[7];
	uint8_t complete_time[7];
	uint8_t present_data[24];
} remes_ele_present_t;
```

### 枚举类型：远程操作结果

```c
REMES-IV平台远程操作的处理结果
enum remes_op_result
{
	REMES_OP_RESULT_WAIT = 1,/* 等待中 */
	REMES_OP_RESULT_SUCCESS = 2,/*成功*/
	REMES_OP_RESULT_FAIL = 3, /* 失败 */
	REMES_OP_RESULT_TIMEOUT = 4,/* 超时 */
	REMES_OP_RESULT_DEV_MISMATCH = 5,/* 装置编号不一致 */
	REMES_OP_RESULT_NO_ELE_ID = 6,/* 电梯编号 不一致*/
	REMES_OP_RESULT_NO_PORT = 8, /* 通信端口号不存在 */
	REMES_OP_RESULT_REPEATED_CMD = 9, /*重复的命令  */
	REMES_OP_RESULT_MODE_DISMATCH = 10,/* 现场模式不匹配 */
	REMES_OP_RESULT_DEV_MNT = 11, /* 装置维保中 */
	REMES_OP_RESULT_DEV_BUSY = 12,/* 装置忙碌中 */
	REMES_OP_RESULT_FUNC_UNSUPPORTED = 13,/* 该功能不支持 */
	REMES_OP_RESULT_PARA_EXIST = 14,/* 服务已经开通，参数无法生效 */
	REMES_OP_RESULT_NO_HARDWARE = 15,/* 硬件不存在 */
	REMES_OP_RESULT_ELE_COMM_FAIL = 16,/* 与电梯通信失败 */
	REMES_OP_RESULT_ELE_SW_BUG = 17,/* 电梯软件bug */
	REMES_OP_RESULT_PARA_ERROR = 18,/* 参数错误 */
	REMES_OP_RESULT_PARA_NOT_EXIST = 19,/* 参数不存在 */
	REMES_OP_RESULT_SERVICE_CLOSE = 20, /* 服务关闭中，电梯远程操作无效 */
};
```



## 2. 变量定义和常量定义





## 3. 功能函数声明

### REMES类型管道消息处理函数

```c
/*
函数输入：管道接收缓存数据指针、管道接收缓存长度、发送数据缓存指针、发送数据长度指针（指针可以实现函数修改函数外部变量）
函数返回：无
函数动作：
1.根据REMES协议内容解析REMES数据，返回接收帧解析的结果；
2.准备发送帧的基本内容：
3.接收帧解析结果OK
	1.接收帧消息类型小于128：根据REMES消息类型执行对应的处理函数
	2.消息类型大于等于128：为摄像头状态应答，调用对应的处理函数
4.接收帧解析结果为应答：todo
5.接收帧解析结果为失败：打印解析错误日志
6.如果发送帧记录不为空，则将处理好的发送帧内容赋值给发送数据缓存（通过指针访问）
主要功能：解析REMES接收帧的内容并执行对应消息类型的处理，处理完成后准备发送返回REMES帧的内容
*/

void remes_handle_data(uint8_t *rx, uint16_t len_rx, uint8_t *tx, uint16_t *len_tx)
{
	/* rx_frame准备 */
	remes_frame_t rx_frame;
	rx_frame.frame_len = len_rx;
	memcpy(rx_frame.frame_data, rx, len_rx);
	enum remes_frame_result remes_result = remes_parse_frame(&rx_frame); /* remes的帧解析 */

	/* tx_frame准备 */
	remes_frame_t tx_frame;
	remes_prepare_frame(&tx_frame);
	tx_frame.answer = remes_result; /* 后续可能修改为处理失败 */
	tx_frame.dst = rx_frame.src;
	tx_frame.src = rx_frame.dst;
	tx_frame.rec_cnt = rx_frame.rec_cnt;
	tx_frame.rec_seq = rx_frame.rec_seq;

	/* 解析结果处理 */
	if (remes_result == REMES_FRAME_RESULT_OK)
	{
		if (rx_frame.msg_type < 128) /* 远程下发指令处理 */
		{
			log_i("remes handle msg type: %d", rx_frame.msg_type);
			/* 各处理仅需负责其中的应答结果、记录个数、记录的具体内容，其它帧格式后续统一处理 */
			switch (rx_frame.msg_type)
			{
				case REMES_TYPE_READ_DATA:
					remes_handle_read_address(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_ELE_PRESENT:
					remes_handle_ele_present(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_ELE_MNT_HAND:
				case REMES_TYPE_ELE_MNT_HAND_NV5X1:
					remes_handle_ele_mnt_hand(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_ESC_MNT_HAND:
					remes_handle_esc_mnt_hand(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_RUN_STATUS_SET:
					remes_handle_run_status_command(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_ESC_SET_MODE_SW_TIME:
					remes_handle_esc_mode_switch_time(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_ELE_RUN_STATUS:
				case REMES_TYPE_ELE_RUN_STATUS_EXT:
					remes_handle_ele_run_status(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_ELE_HMI_AUTH:
					remes_handle_set_ele_ui(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_ELE_REQNO_FILTER:
					remes_handle_ele_filter_reqno(&rx_frame, &tx_frame);
					break;
					/* 新增：监控室装置与控制柜SMEC模块之间通信协议 */
				case REMES_TYPE_DIGTEL_SERVICE_SET:
					remes_handle_service_setting(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_DIGTEL_PARA_DOWNLOAD:
					remes_handle_para_download(&rx_frame, &tx_frame);
					break;
				case REMES_TYPE_DIGTEL_ELE_INFO_REQ:
					remes_handle_digtel_ele_info_query(&rx_frame, &tx_frame);
					break;
				default:
					break;
			}
		}
		else /* 远程应答结果处理 */
		{
			switch (rx_frame.msg_type)
			{
				case REMES_TYPE_CAMERA_STATUS_RESP: /* type111的吉盛应答结果 */
					remes_handle_camera_status_resp(&rx_frame, &tx_frame);
					break;
				default:
					break;
			}
		}
	}
	else if (remes_result == REMES_FRAME_RESULT_REPLY)
	{
		/* ignore */
	}
	else
	{
		log_i("remes frame parse result: %d", remes_result);
	}

	/* 返回结果设置 */

	if (tx_frame.rec_data_len > 0)
	{
		remes_pack_frame(&tx_frame);
		memcpy(tx, tx_frame.frame_data, tx_frame.frame_len);
	}

	*len_tx = tx_frame.frame_len;
}
```

### REMES帧句柄固定内容准备（初始化）函数

```c
/*
函数输入：REMES数据帧结构句柄
函数返回：无
函数动作：
1.
2.
主要功能：准备一个REMES数据帧的固定内容
*/

static void remes_prepare_frame(remes_frame_t *frame)
{
	frame->frame_len = 0;
	frame->rec_data_len = 0;
	frame->rec_data = &(frame->frame_data[19]);
	frame->all_rec_data = &(frame->frame_data[15]);

	/* 默认值 */
	frame->answer = REMES_FRAME_RESULT_OK;
	frame->src = REMES_OBJ_ERMD;
	frame->dst = REMES_OBJ_WEB;
	frame->rec_cnt = 1;
	frame->rec_seq = 1;
}
```

## 4.业务代码（函数）声明

### 电梯当前状态处理函数

```c
/*
函数输入：REMES接收数据帧结构句柄指针、处理后的发送帧句柄指针
函数返回：无
函数动作：
1.订单信息free（初始化、置0）
	1.订单信息中信息版本、操作编号、电梯编号、装置编号的初始化
	2.使用装置系统时间为接收时间赋值
2.判断操作结果1：判断电梯状态并返回操作结果；
3.判断操作结果2：判断电梯系列并返回操作结果；
4.操作结果为成功：准备订单信息并提交订单（所以这部分返回帧不是同步的，是异步的，需要订单处理完成）
5.操作结果为失败：将内容填写到应答帧句柄指针中
主要功能：根据电梯的状态判断REMES远程操作结果，并确定是否执行电梯订单任务。若执行：异步返回发送帧；若不执行，保存失败操作结果到发送帧
*/

void remes_handle_ele_present(remes_frame_t *rx_frame, remes_frame_t *tx_frame)
{
	/* 内容解析 */
	remes_ele_present_t remes_req; /* 原始订单信息 */
	memset(&remes_req, 0, sizeof(remes_req));
	enum remes_op_result remes_result;/* 远程操作结果 */

	memcpy(remes_req.msg_version, rx_frame->rec_data, 8);
	remes_req.op_id = get_uint32(&(rx_frame->rec_data[8])); /* 操作编号 */
	remes_req.ele_id = get_uint32(&(rx_frame->rec_data[12])); /* 电梯编号 */
	remes_req.dev_id = get_uint32(&(rx_frame->rec_data[16])); /* 装置编号 */
	get_localtime_bcd(remes_req.recv_time);

#if 0
	if (remes_op_id_duplicated(rx_frame->msg_type, remes_req.op_id))
	{
		return;
	}
#endif

	remes_result = remes_check_op_result(remes_req.ele_id);
	remes_result = remes_check_unsupported_series(remes_result);

	if (remes_result == REMES_OP_RESULT_SUCCESS)
	{
		order_request_t order_req;
		mop_order_t mop_order = { MOP_TASK_GET_ELE_PRESENT_STATUS };
		/* 仅需附带原始订单信息 */
		if (order_prepare_order(&order_req, DIGLINK_OBJ_TYPE_DEFAULT, 0, DIGLINK_OBJ_TYPE_DEFAULT,
				0, ORDER_TYPE_NEW_MOP_TASK, &mop_order, sizeof(mop_order), &remes_req,
				sizeof(remes_req)))
		{
			log_v("new ele present order");
			order_req.head.ev_notify = true;
			order_submit(&order_req, COMM_OBJ_REMES, COMM_DATA_TYPE_ORDER_ID);
		}
	}
	else
	{
		/* 异常情况下的应答*/
		tx_frame->msg_type = REMES_TYPE_ELE_PRESENT + REMES_TYPE_ANS;
		memcpy(tx_frame->rec_data, rx_frame->rec_data, 12);
		tx_frame->rec_data[12] = remes_result; /*远程操作结果(错误) */
		memcpy(&(tx_frame->rec_data[13]), remes_req.recv_time, 7); /* 收到命令时间 */
		tx_frame->rec_data_len = 20;/*错误时的长度  */
	}
}

```

