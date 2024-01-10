## 1. SQL语句的灵活输入

#### 场景：

​	SQL语句往往有些部分需要根据用户输入灵活变更，而SQL语句往往在C语言中都是字符类型。

​	以更新数据表某一记录的字段值为例，原始SQL如下：

```sql
update kzg_param_t set param_value='123456789012' where param_key='CPD_ID';
```

​	其中，`set param_value='123456789012'`部分的值往往需要根据用户输入灵活变化。

#### 最佳实践

```c
/*
 ============================================================================
 Name        : cpdid_write.c
 Author      : HarryOkk
 Version     :
 Copyright   : HarryOkk
 Description : Hello World in C, Ansi-style
 ============================================================================
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "sqlite3.h"

int main() {
    sqlite3 *db;
    char new_cpdid[13];  // 为字符串结尾符预留一个位置
    char sql[150];

    int rc = sqlite3_open("/mnt/userdata/myDB/JS268_DB.db", &db);
    if (rc) {
        fprintf(stderr, "fail to open db: %s\n", sqlite3_errmsg(db));
        return 1;
    }
    fprintf(stdout, "success to open db\n");
    fprintf(stdout, "PLZ input the value 0f CPDID\n");

    do {
        fgets(new_cpdid, sizeof(new_cpdid), stdin);
        new_cpdid[strcspn(new_cpdid, "\n")] = '\0';  // 去除换行符
        if (strlen(new_cpdid) != 12) {
            fprintf(stdout, "input CPDID error, please retry!\n(ps: CPDID is 12-digit numbers or letters)\n");
        }
    } while (strlen(new_cpdid) != 12);

    fprintf(stdout, "CPDID：%s\nnew_cpdid's size is %zu\n", new_cpdid, strlen(new_cpdid));

    snprintf(sql, sizeof(sql), "INSERT OR REPLACE INTO kzg_param_t (param_type, param_key, param_value) VALUES ('SYS_PARAM', 'CPD_ID', '%s');", new_cpdid);

    fprintf(stdout, "SQL: %s\n", sql);

    rc = sqlite3_exec(db, sql, 0, 0, 0);
    if (rc != SQLITE_OK) {
        fprintf(stderr, "SQL 语句执行失败: %s\n", sqlite3_errmsg(db));
        sqlite3_close(db);
        return 1;
    }
    fprintf(stdout, "成功执行 SQL 语句\n");

    sqlite3_close(db);
    fprintf(stdout, "success to close db\n");
    return 0;
}


```

#### 知识总结

- **strcspn(new_cpdid, "\n")**：该函数返回字符数组中第一个\n的位置，后续便可以根据其返回值索引到\n
- **new_cpdid[strcspn(new_cpdid, "\n")] = '\0';**在字符数组中间添加\0。格式化输出的时候字符数组会被分成多个字符串，并且只输出第一个字符串
- 字符数组和字符串的关系：
  - 字符数组用于存储字符类型变量
  - 字符串一般指const char类型，为字符类型的组合，并以NULL（'\0'）结尾
- snprintf(sql, sizeof(sql), "update kzg_param_t set param_value='%s' where param_key='CPD_ID';", new_cpdid);
  - 向一个缓冲区（字符数组）格式化输入一个字符串，并且因为可以指定缓冲区的大小，在Linux中使用非常安全
- 因目标Linux设备有的需要更新记录，有些需要插入记录，所以使用`INSERT OR REPLACE INTO kzg_param_t (param_type, param_key, param_value) VALUES ('SYS_PARAM', 'CPD_ID', '%s');`并且注意，每个字段的名称和值都要在SQL中指出
