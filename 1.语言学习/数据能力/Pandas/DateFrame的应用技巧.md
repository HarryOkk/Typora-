## 1.迭代df的每一行

**背景：**



**应用：**



```python
from pandas as pd

for index,row in df.itterrows():
    print("index是该行的索引值")
    print("row是行对象，通过row['列名']可以访问具体的单元格的值")
```

## 2. 字符串SQL集成IN方法

