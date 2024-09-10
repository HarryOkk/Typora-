## 1、df的遍历

```python
for index,row in df.itterrows():
    row['column'] = 2
```

注意，这里面的index和row都是创建的副本，对row的修改不会影响df中的值

所以如果要通过row的判断然后反过来修改df中的值，需要：

```python
# 通过df['column1'] == row['column1']索引，修改column2列的值为value
df.loc[df['column1'] == row['column1'], 'column2'] = value
```

