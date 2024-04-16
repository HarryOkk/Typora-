## 1. 一般用法

### 1.1 读取sqlite3数据库

```python
import sqlite3
import pandas as pd

# 连接数据库(不存在数据库时自动创建)
conn = sqlite3.connect('example.db')

# 从数据库中读取数据到DateFram
df = pd.read.sql_query("SELECT * FROM table_name;",conn)

# 关闭数据库连接
conn.close()
```

后面只需要通过df对数据库中的数据进行操作即可

### 1.2 将pd中的内容写入到sqlite3数据库

```python
import sqlite3 
import pandas as pd

# 使用上下文管理器with来打开数据库连接，这样可以自动关闭
with sqlite3.connect('example.db') as conn:
    df.to_sql('table_name',conn,if_exists='append',index=False)
"""
注意：
1、关键字参数if_exits有如下几种选择：
'append'：将数据追加到表table_name中
'replace'：如果表已经存在，则将其替换
'fail'：在表存在的时候抛出一个异常
根据实际的应用选择
2、index=False
pandas的索引列不写入数据库中，该关键字参数的默认值为True（也就是默认写入）
''"""
```

