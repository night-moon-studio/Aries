# Aries 原 FreeSql.Natasha.Extension 项目
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fnight-moon-studio%2FAries.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fnight-moon-studio%2FAries?ref=badge_shield)

FreeSql 的 Natasha 扩展

## 前端传值结构（Aries 模型）
![Struct](https://github.com/night-moon-studio/Aries/blob/master/images/Aries1.png)  

## 使用

### Natasha 初始化

  ```C#
  //仅仅注册组件
  NatashaInitializer.Initialize();
  //注册组件+预热组件 , 之后编译会更加快速
  await NatashaInitializer.InitializeAndPreheating();
  ```  
  
<br/>  

### 配置

#### 引用 Provider

该库是对 IFreesql 接口的一个扩展，同时也是一个抽象的实现，因此具体适配什么数据库，需要用户 手动引用 Freesql 的适配库，例如 "FreeSql.Provider.PostgreSQL"。


#### 信息初始化配置

##### 控制台

```C#
//初始化主键等信息
TableInfomation.Initialize(freesql, typeof(Test), typeof(Test2), typeof(Test3)，.....);
```  

<br/>

##### Web

```C#

// AddAriesPgSql / AddAriesMySql / AddAriesSqlServer 等等
services.AddAriesPgSql(

  //链接字符串
  "Host=127.0.0.1;Port=5432;Username=postgres;Password=; Database=test;Pooling=true;Minimum Pool Size=1",
  //对 freesql 的额外操作
   freeSql => { freeSql.Aop.CurdBefore += Aop_CurdBefore; }

 );
```

对实体类进行扫描或者添加 

```C#
 //手动添加个别实体类
 services.AddAriesEntities(typeof(Student), typeof(Teacher)....);
 //直接扫描该程序集瞎的实体类
 services.AddAriesAssembly("TestAssembly");
```


#### 字段使用范围初始化配置

PropertiesCache<Test> 泛型提供了对 更新/条件查询/字段返回 操作的字段限制，允许参与或不参与，详情请看方法注释。
```C#
//配置业务禁止返回的字段 作用于 ToLimitList / ToJoinList
 PropertiesCache<Test>.AllowSelectFields("Name","Age");
 //允许 Name / Age 返回。

```    

<br/>  

### 查询

 - WhereWithEntity(Request.Query.Keys,entity); 通过前端指定的 Key (字段名), 来添加对 entity 指定字段的 Where 查询代码, 翻译成 Where(item=>item.{keys[i]} == entity.{keys[i]})。
 - WhereWithModel(queryModel); 通过前端传来的 Model 进行分页/排序/模糊查询，翻译成 Page() / Orderby("") / Where(item=>item.{field}.Contains({value}))。
 - WherePrimaryKeyFromEntity(entity); 翻译成 Freesql 中 Where(item=>item.{PrimaryKey} == entity.{PrimaryKey})， 生成 Where 主键 = xxx 的查询条件。
 
### 更新

 - UpdateAll(entity); 通过前端传来的实体，进行更新。
 - UpdateWithModel(Request.Query.Keys,entity); 通过前端指定的 Key (字段名), 来添加对 entity 指定字段的 更新, 翻译成 Set(item=>item.{key[i]}==entity.{key[i]})。


### 高度封装的扩展操作入口

一下方法封装了 XXXWithEntity / WhereWithModel 可以在查询的同时完成更新/删除等操作
```C#
//插入实体
AriesInsert<TEntity>(TEntity entity)
//通过 Aries 模型查询并更新实体
AriesModify<TEntity>(SqlModel<TEntity> model);
//通过 Aries 模型查询实体
AriesQuery<TEntity>(SqlModel<TEntity> model);
//通过 Aries 模型查询并删除实体
AriesDelete<TEntity>(SqlModel<TEntity> model);
```    
<br/>  

## 前端操作Model

```C#
var temp = new SqlModel();
temp.QueryInstance.Instance.Id = 1;
temp.ModifyInstance.Instance.Id = 1;
temp.AddAscField("CreateTime");
temp.AddFuzzy("Name","a");
temp.AddUpdateField("Name");
temp.AddWhereField("Age");
console.log(temp);
```
<br/>  

### 外联查询

```C#
_freeSql.Select<Test>().ToJoinList(item => new {
                TestName = item.Name,
                DomainId = item.Domain.AriesInnerJoin<Test2>(c => c.Id).Id,
                DomainName = item.Domain.AriesInnerJoin<Test2>(c => c.Id).Name,
                TypeName = item.Type.AriesInnerJoin<Test2>(c => c.Id).Name,
}));
//翻译成：
SELECT 
  a."Name" AS "TestName",
  Test2_AriesInnerJoin_Domain."Id" AS "DomainId",
  Test2_AriesInnerJoin_Domain."Name" AS "DomainName",
  Test2_AriesInnerJoin_Type."Name" AS "TypeName" 
FROM "Test" a 
  INNER JOIN "Test2" AS Test2_AriesInnerJoin_Domain ON a."Domain" = Test2_AriesInnerJoin_Domain."Id" 
  INNER JOIN "Test2" AS Test2_AriesInnerJoin_Type ON a."Type" = Test2_AriesInnerJoin_Type."Id"
```  

<br/>  

### 发布日志

  - 2020年10月27日，发布 v1.5.0 版本: 1、剪短外联查询的关系构建路径。 2、新增乐观锁操作 API, AriesOptimisticLock，用户可实现 OptimisticLockBase 来实现不同的乐观锁存储和更新等操作。  

<br/>  

## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fnight-moon-studio%2FAries.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Fnight-moon-studio%2FAries?ref=badge_large)
