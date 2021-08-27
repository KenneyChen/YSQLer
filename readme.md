> #  sql 转换RESTful 脚手架



YSqler是一个微型叫数据库表转换restful接口，用c#语言编写，以便任何人能执行查询、删除、修改，非标准restful定义。



> ## YSqler功能

- 依赖dapper；
- 支持多种数据可类型，包括：SQL Server, MYSQL, SQLITE, PostgreSQL, Cockroachdb等；
  



> # YSQLer安装



> # 使用



> # 1、查询

```json
http://localhost:4000/books #books表名
http://localhost:4000/books/2 #通过主键id=2获取
http://localhost:4000/books?offset=1&limit=10 #offset第一页，limit取10条

{
    query:{
        "fileds":["id","name"], //查询字段，空，默认全部
        "filter":{ //过滤条件,传入主键 忽略这个fitler条件
            "id":2,
            "name":"数学"   
        },
    }
}
```



> # 2、修改

```json
http://localhost:4000/books/2 #通过主键id=2

{
    update:{ //需要更新字段
      "id":2,
      "name":"数学",
      "filter":{ //传入主键 忽略这个fitler条件
            "id":2,
            "name":"数学"   
        },
    }
}
```





> # 3、添加

```json
http://localhost:4000/books

{
    add:{ //需要添加字段
      "id":2,
      "name":"数学",
    }
}
```



> # 4、删除

```json
http://localhost:4000/books/2 #通过主键id=2 删除

{
    "delete":{
       "filter":{ //传入主键 忽略这个fitler条件
          "id":2,
          "name":"数学"   
        },
    }
}
```

