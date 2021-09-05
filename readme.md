> #  sql 转换RESTful脚手架



YSqler是一个微型数据表转换restful接口，用c#语言编写，以便任何人不需要编写任何代码能执行查询、删除、修改。



> ## YSqler功能

- 依赖dapper；
- 支持多种数据可类型，包括：SQL Server, MYSQL, SQLITE, PostgreSQL, Cockroachdb等；
  



> # YSQLer使用

方式一：全安装

下载源码 +编译部署

①修改配置文件appsetting.json

```json
"ConnectionStrings": {
    "YSQLerConnection": "server=localhost;uid=root;pwd=123456;port=3306;database=sqltotable;sslmode=Preferred;",
    "YSQLerDbType": "MySQL", //MySQL,MSSQL
    "YSQLerAllowOperation": "query,add,update,delete", //不建议开放add,update,delete
    "YSQLerAllowTables": "sysconst,sysconstgroup" //*代表所有表，允许"Order,SysUser"
  }
```



方式二：嵌入当前项目

①下载sdk

②appsetting.json 增加配置



```json
"ConnectionStrings": {
    "YSQLerConnection": "server=localhost;uid=root;pwd=123456;port=3306;database=sqltotable;sslmode=Preferred;",
    "YSQLerDbType": "MySQL", //MySQL,MSSQL
    "YSQLerAllowOperation": "query,add,update,delete", //不建议开放add,update,delete
    "YSQLerAllowTables": "sysconst,sysconstgroup" //*代表所有表，允许"Order,SysUser"
  }
```

③：注入应用配置

```c#
public Startup(IConfiguration configuration)
{
    Configuration = configuration;  
    //增加此代码注入
    YSQLerAppSettings.Configuration = configuration;
}
```







# 1、查询 POST

```json
http://localhost:4000/books #books表名

【Get】 http://localhost:4000/books/2 #通过主键id=2获取
【Post】http://localhost:4000/books?offset=1&limit=10 #offset第一页，limit取10条

【Post】http://localhost:4000/books?offset=1&limit=10 #offset第一页，limit取10条
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



返回：

```json
{
    "data": {
        "totalCount": 1,
        "records": [
            {
                "id": 1,
                "constKey": "333",
                "constValue": "55",
                "status": false,
                "orderBy": 0,
                "groupId": 1
            }
        ]
    },
    "success": true,
    "msg": "请求成功"
}
```



# 2、修改 POST

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

返回：

```json
{
    "success": true,
    "msg": "请求成功"
}
```





# 3、添加 POST

```json
http://localhost:4000/books/Add

{
    add:{ //需要添加字段
      "id":2,
      "name":"数学",
    }
}
```

返回：

```json
{
    "data": {
        "id": 6 //返回主键，字段名id是固定值，不会因为主键名称不一样而返回不一样
    },s
    "success": true,
    "msg": "插入成功"
}
```





# 4、删除 POST

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



返回：

```json
{
    "success": true,
    "msg": "请求成功"
}
```

