# 包含comm.c和comm.h

>C代码无非几个重要内容：
>
>​	1.结构类型定义
>
>​	2.变量定义和常量定义：数据类型：变量名称含义
>
>​	3.功能函数声明
>
>​	4.业务代码（函数）声明

## 1. 结构类型定义

### 枚举类型：通讯方式硬件协议

```c
enum if_name
{
	/* 4个串口必须放前面 */
	IF_RS485_1,
	IF_RS485_2,
	IF_RS422_1,
	IF_RS422_2,

	IF_CAN1,
	IF_CAN2,
	IF_LAN1,
	IF_LAN2,

};
```

### 枚举类型：功能定义

```c
enum if_func
{
	IF_FUNC_NONE,

	IF_FUNC_DIGLINK,
	IF_FUNC_ELSC,
	IF_FUNC_BACNET,
	IF_FUNC_MODBUS,
	IF_FUNC_SPEC485,

	IF_FUNC_ELE,
	IF_FUNC_ELE_MOP,
	IF_FUNC_ELE_OLD_MOP,
	IF_FUNC_ELE_CRT,
	IF_FUNC_ELE_GRM,
	IF_FUNC_ELE_JP,/* 引进梯用PD17（NCR。AAN，BA等数据） */
	IF_FUNC_ELE_CS,/* 778和758平台 */
	IF_FUNC_GC,
	IF_FUNC_BA,
	IF_FUNC_CAN1_TEST_MODE,
	IF_FUNC_CAN2_TEST_MODE,
	IF_FUNC_485_1_TEST_MODE,
	IF_FUNC_485_2_TEST_MODE,
	IF_FUNC_422_1_TEST_MODE,
	IF_FUNC_422_2_TEST_MODE,
	IF_FUNC_BA_IC, /* 通过485BA板实现IC卡 */
	IF_FUNC_ESC, /* 扶梯 */	
	
	IF_FUNC_DIGLINK_JSON, /* 测试：使用串口进行JSON格式通信 */
};
```

### 结构体类型：硬件通讯协议的配置信息

```c
typedef struct
{
    //硬件通讯协议
	enum if_name if_name;
    //功能名称
	enum if_func if_func;
    //波特率值
	char baud[8];
    //奇偶校验方式（无校验、偶校验、奇校验）
	char parity[8]; /* none, even, odd */
} if_cfg_t;
```

### 结构体类型：通讯配置信息

```c
typedef struct
{
    //硬件接口协议的配置信息（名称、功能、波特率、奇偶校验）
	if_cfg_t rs485_1;
	if_cfg_t rs485_2;
	if_cfg_t rs422_1;
	if_cfg_t rs422_2;
	if_cfg_t can_1;
	if_cfg_t can_2;
	//通讯使用的端口号
	int32_t elsca_port; /* elsca server使用的port */
	int32_t diglink_port; /* diglink server使用的port */
	int32_t web_server_port; /* web server使用的port */
	int32_t fvr_server_port; /* 人脸装置 server（默认DigTel-II监控室装置）使用的port */
	int32_t emids_server_port; /* EMIDS-II Server(默认DigTel-II监控室装置)使用的port */
	int32_t gc_server_port; /*gc server使用的port*/
	int32_t top_server_port;  /* 15.6寸触摸屏  Server(默认DigTel-II监控室装置)使用的port */
    //网络通讯IP
	char dst_server_ip[16]; /* 本装置作为client连接的server（默认对应监控室装置） */
	char eth0_ip[16]; /* 控制柜装置外网口IP */
	char eth1_ip[16]; /* 控制柜装置内网口IP */
	//通讯对象的URI
	char diglink_uri[128]; /* web server提供的diglink功能对应uri */
	char vibration_sensor_uri[128]; /* 振动传感器对应uri */
	char fvr_uri[128]; /* 人脸识别装置对应uri */
	char fvr_server_uri[128]; /* 人脸装置 server（默认DigTel-II监控室装置）使用的uri */
	char ivrd_uri[128]; /*  uri */
	char emids_uri[128]; /* EMIDS-II uri */
	char emids_server_uri[128]; /* EMIDS-II Server(默认DigTel-II监控室装置)uri */
	char top_uri[128]; /* 15.6寸触摸屏 uri */
	char top_server_uri[128]; /* 15.6寸触摸屏 Server(默认DigTel-II监控室装置)uri */
	char camera_status_uri[128]; /*  */
	char ebike_settings_uri[128]; /*  */
	char people_status_uri[128]; /*  */
	char camera_area_usage_uri[128];
} comm_cfg_t;
```

### 枚举类型：通讯状态

```c
enum comm_state
{
	COMM_STATE_INIT = 0,

	/* 基本通信状态（一般仅使用这两个） */
	COMM_STATE_DISCONNECTED,
	COMM_STATE_CONNECTED,

	/* 其它的一些状态 */
	COMM_STATE_CONNECTING,
	COMM_STATE_DISABLED,

	COMM_STATE_DISCONNECTED_TIMEOUT, /* 断开超时 */
};
```

### 枚举类型：通讯对象

```c
enum comm_obj
{
	COMM_OBJ_NONE,
	COMM_OBJ_ORDER,
	COMM_OBJ_TEST,
	COMM_OBJ_TRANS_TRANS,
	COMM_OBJ_TRANS_COMM,
	COMM_OBJ_CAN1,
	COMM_OBJ_CAN2,
	COMM_OBJ_RS485_1,
	COMM_OBJ_RS485_2,
	COMM_OBJ_RS422_1,
	COMM_OBJ_RS422_2,
	COMM_OBJ_CAMERA_EBIKE,
	COMM_OBJ_SMART_DOOR_F,
	COMM_OBJ_SMART_DOOR_R,
	COMM_OBJ_IMDS1,
	COMM_OBJ_IMDS2,
	COMM_OBJ_REMES,
	COMM_OBJ_ELSC1,
	COMM_OBJ_ELSC2,
	COMM_OBJ_FVR, /* 人脸识别装置 */
	COMM_OBJ_BUSSINESS_NCR, /* 新增业务中的NCR */
	COMM_OBJ_DIGLINK_SERVER, /* diglink server */
	COMM_OBJ_ELSCA, /* ELSCA */
	COMM_OBJ_DM, /* 物模型 */
	COMM_OBJ_ISVC, /* 智慧电梯物模型服务 */
	COMM_OBJ_ESC, /* 扶梯 */
	COMM_OBJ_GC,  /*群控板*/
	COMM_OBJ_CNT, /* COMM_DEV_CNT必须确保放在最后一个 */
};
```

### 枚举类型：通讯传输的数据类型

```c
enum comm_data_type
{
	COMM_DATA_TYPE_NONE, /* 无 */
	COMM_DATA_TYPE_ORDER_ID, /* 订单ID */
	COMM_DATA_TYPE_ORDER_INFO, /* 订单信息 */
	COMM_DATA_TYPE_RAW, /* 原始数据内容 */
	COMM_DATA_TYPE_STRUCT, /* 返回结构体即可 */
	COMM_DATA_TYPE_BINARY, /* diglink二进制协议格式 */
	COMM_DATA_TYPE_JSON, /* JSON格式应答 */
};
```

### 枚举类型：通讯事件类型

```c
enum comm_ev_type
{
	/* 订单相关通知 */
	COMM_EV_STATE_CHANGED, /* 订单状态变动 */
	COMM_EV_STEP_CHANGED, /* 订单执行阶段变动 */

	/* TODO: 后续完善 */
	COMM_EV_NONE,
	COMM_EV_ACCEPTED,
	COMM_EV_PENDING,
	COMM_EV_DOING,
	COMM_EV_DONE,
	COMM_EV_FAILED,
	COMM_EV_TIMEOUT,

	COMM_EV_SEND_REQ, /* 发送数据请求 */
	COMM_EV_SEND_ELSC_DATA,/* ELSC的应答数据 */
	COMM_EV_SEND_REMES_FRAME, /* 发送REMES数据帧 */
	COMM_EV_SEND_NCR_FRAME, /* 发送NCR数据帧 */
	COMM_EV_SEND_REMES_TRANS_FRAME, /*以REMES形式发送给TRANS的数据帧，主要用于查询摄像头的状态*/
	COMM_EV_SEND_SMART_CAR_FRAME, /* 发送智能轿厢数据（支持物模型数据） */
};
```

### 结构类型：保存通讯对象对应通讯状态

```c
typedef struct
{
	enum comm_state remes_comm_state; /* REMES通信状态 */
	enum comm_state smos_comm_state; /* SMOS通信状态 */
	enum comm_state smos_gc_comm_state; /* SMOS GC通信状态 */
	enum comm_state ncr_comm_state; /* NCR通信状态 */
	enum comm_state ele_comm_state; /* 与电梯通信状态 */
	enum comm_state mop_comm_state; /* 与电梯MOP通信状态 */
	enum comm_state jp_ele_comm_state; /* 与引进梯PD17通信状态 */
	enum comm_state cs_comm_state; /* 与cs通信状态 */
	enum comm_state gc_ncr_comm_state; /* 与群控柜NCR通信状态 */
	enum comm_state gc_hop_comm_state; /* 与群控柜hop通信状态 */
	enum comm_state acs_comm_state; /* 与ACS通信状态 */
	enum comm_state camera_comm_state; /* 与camera通信状态 */
	enum comm_state smartdoor_f_comm_state; /* 与smartdoor_f通信状态 */
	enum comm_state smartdoor_r_comm_state; /* 与smartdoor_r通信状态 */
	enum comm_state grm_comm_state; /* 与grm通信状态 */
	enum comm_state esc_comm_state; /* 与扶梯通信状态 */
	enum comm_state md_wired_comm_state; /* 与监控室有线连接的通信状态 */
	enum comm_state deep_sleep_comm_state; /*与深度待机印版通信状态 */
	enum comm_state gc_mts_comm_state; /* 与群控板有线连接的通信状态 */

	enum comm_state elsgw_comm_state; /*与elsgw装置通信状态 */
	uint8_t ncr_comm_protocol; /* NCR通信协议类型 */
} comm_info_t;
```



## 2. 变量和常量定义

### 结构体数组：功能定义表（用于保存功能的回调）

```c
value_name_t if_func_def_table[] =
{
	{ IF_FUNC_NONE, "none"},
	{ IF_FUNC_DIGLINK, "diglink"},
	{ IF_FUNC_ELSC, "elsc"},
	{ IF_FUNC_BACNET, "bacnet"},
	{ IF_FUNC_MODBUS, "modbus"},
	{ IF_FUNC_SPEC485, "spec485"},
	{ IF_FUNC_422_1_TEST_MODE,"422_test_1"}, /*新增*/
	{ IF_FUNC_422_2_TEST_MODE,"422_test_2"},/*新增*/
	{ IF_FUNC_ELE, "ele"},
	{ IF_FUNC_GC, "gc"},
	{ IF_FUNC_ELE_MOP, "ele_mop"}, /* RS422 mop专用 */
	{ IF_FUNC_ELE_OLD_MOP, "ele_old_mop"}, /* RS422 老mop专用 */
	{ IF_FUNC_ELE_CRT, "ele_crt"},
	{ IF_FUNC_ELE_JP, "ele_jp"}, /* 引进梯PD17 的通信 */
	{ IF_FUNC_ELE_CS, "ele_cs"}, /* cs的通信 */
	{ IF_FUNC_ESC, "esc"},/* 扶梯 */
	{ IF_FUNC_BA, "ba"},
	{ IF_FUNC_DIGLINK_JSON, "diglink_json"},
};
```

### 结构体对象：功能定义索引（用于保存功能的回调）

```c
name_def_t if_func_def =
{	
    //功能定义表指针（数组名对象即指向数组第一个对象的指针，可以直接用作指针变脸传入）
	.def_table = if_func_def_table,
    //功能定义表数组成员个数
	.item_cnt = TABLE_ITEM_CNT(if_func_def_table),
    //功能定义表数组的默认索引
	.default_index = 0
};
```

### 结构体数据类型定义：通讯响应回调结构对象（用于保存功能的回调）

``` c
typedef struct
{
    //一个指针对象，目前看还不知道干啥的
	void *parent;
    //comm返回回调函数对象resp_cb
	comm_response_cb resp_cb;
} comm_resp_t;
```

### 结构体数据类型定义：通讯管理结构对象（用于保存功能的回调）

```c
typedef struct
{
    //通讯对象结构数组，该数组中应该可以找到各个通讯对象的回调函数
	comm_resp_t comm_resp[COMM_OBJ_CNT];
} comm_mgt_t;
```

## 3.功能函数声明

### 通讯测试处理回调函数

```c
/*
指定通讯数据类型、事件类型等，在终端打印具体的值。
*/

void comm_handle_test_resp(void *self, enum comm_data_type data_type, enum comm_ev_type ev_type, uint8_t *data, uint16_t len)
{
	switch (data_type)
	{
        //二进制类型数据直接以16进制进行打印
		case COMM_DATA_TYPE_BINARY:
			fprintf(stderr, "[comm] test response: ");
			for (int i = 0; i < len; ++i)
			{
				fprintf(stderr, "%02X ", data[i]);
			}
			fprintf(stderr, "\n");
			break;
        //JSON对象直接输出
		case COMM_DATA_TYPE_JSON:
			fprintf(stderr, "[comm] test response: %s\n", (char*)data);
			break;
        //非二进制和JSON对象直接输出到日志
		default:
			log_v("data_type: %d, ev_type: %d", data_type, ev_type);
			elog_hexdump("comm test", 16, data, len);
			break;
	}
}
```

### 通讯管理结构对象函数列表注册（数组赋值的形式）

```c
/*调用该函数给数组对象赋值*/
void comm_set_response_cb(enum comm_obj comm_dev, void *parent, comm_response_cb cb)
{
	comm_mgt.comm_resp[comm_dev].parent = parent;
	comm_mgt.comm_resp[comm_dev].resp_cb = cb;
}
```

### 通讯管理结构对象初始化

```c
/*初始化（全部置0）并赋值测试通讯对象*/
void comm_resp_init(void)
{
	memset(&comm_mgt, 0, sizeof(comm_mgt_t));
	comm_set_response_cb(COMM_OBJ_TEST, NULL, comm_handle_test_resp);
}
```

### 通讯对象的事件处理

```c
/*根据通讯对象调用对应的回调函数*/
void comm_handle_response(enum comm_obj comm_dev, enum comm_data_type data_type, enum comm_ev_type ev_type, uint8_t *data, uint16_t len)
{
	comm_resp_t *comm_resp = &(comm_mgt.comm_resp[comm_dev]);

	if (comm_resp->resp_cb != NULL)
	{
		comm_resp->resp_cb(comm_resp->parent, data_type, ev_type, data, len);
	}
#if 1
	else
	{
		fprintf(stderr, "[comm] handle resp error: comm_dev %d cb not set\n", comm_dev);
	}
#endif
}
```

### CAN通讯的初始化

```c
/*根据硬件功能的对CAN通讯进行初始化设置*/
void comm_can_init(char *can_name, if_cfg_t *if_cfg)
{
	switch (if_cfg->if_func)
	{
		case IF_FUNC_ELE:
			can_init_ele_comm(can_name);
			break;
		case IF_FUNC_GC:
			can_init_gc_can(can_name);
			break;
		case IF_FUNC_ELE_JP:
			can_init_jp_ele_can(can_name);
			break;
		case IF_FUNC_ELE_CS:
			can_init_ele_cs_can(can_name);
			break;
		case IF_FUNC_CAN1_TEST_MODE:
			can_init_test_can1(can_name);
			break;
		case IF_FUNC_CAN2_TEST_MODE:
			can_init_test_can2(can_name);
			break;
		case IF_FUNC_ESC:
			can_init_esc_can(can_name);
			break;
		case IF_FUNC_NONE:
		default:
			break;
	}
}

```

