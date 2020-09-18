# Aries 原 FreeSql.Natasha.Extension 项目
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fnight-moon-studio%2FAries.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fnight-moon-studio%2FAries?ref=badge_shield)

FreeSql 的 Natasha 扩展

## 前端传值结构（Aries 模型）
![Struct](https://github.com/night-moon-studio/Aries/blob/master/images/Aries1.png)  

## 使用

### 配置

#### 信息初始化配置
```C#
//初始化主键等信息
TableInfomation.Initialize(freesql, typeof(Test), typeof(Test2), typeof(Test3)，.....);
```

#### 字段使用范围初始化配置
```C#
//配置业务禁止返回的字段 作用于 ToLimitList / ToJoinList
PropertiesCache<Test>.SetSelectBlockFields("Domain", "Address");

//配置业务允许更新的字段 作用于 UpdateWithHttpModel / UpdateAll 
PropertiesCache<Test>.SetUpdateAllowFields("Domain");

//配置业务允许查询的字段 作用于 QueryWithHttpEntity / QueryWithModel / FuzzyQuery
PropertiesCache<Test>.SetWhereBlockFields("Domain");
```

#### 实体写操作初始化配置
```C#
//更新时对实体进行单独处理
PropertiesCache<Test>.SetUpdateInit(item => item.Address = "null");//多次添加可以累加
//插入时对实体进行单独处理
PropertiesCache<Test>.SetInsertInit(item => item.Domain = 2);
```

### 查询

 - QueryWithHttpEntity(Request.Query.Keys,entity); 通过前端指定的 Key (字段名), 来添加对 entity 指定字段的 Where 查询代码。
 - QueryWithModel(queryModel); 通过前端传来的 Model 进行分页/排序/模糊查询。
 - WherePrimaryKeyFromEntity(entity); 生成 Where 主键 = xxx 的查询条件代码。
 
### 更新

 - UpdateAll(entity); 通过前端传来的实体，进行更新。
 - UpdateWithHttpModel(Request.Query.Keys,entity); 通过前端指定的 Key (字段名), 来添加对 entity 指定字段的 更新。


### 高度封装的扩展操作入口

一下方法封装了 XXXWithHttpEntity / QueryWithModel 可以在查询的同时完成更新/删除等操作
```C#
//插入实体
InsertWithInited<TEntity>(TEntity entity)
//通过 Aries 模型更新实体
ModifyFromSqlModel<TEntity>(SqlModel<TEntity> model);
//通过 Aries 模型查询实体
QueryFromSqlModel<TEntity>(SqlModel<TEntity> model);
//通过 Aries 模型删除实体
DeleteFromSqlModel<TEntity>(SqlModel<TEntity> model);
```  


## 链表查询

### 关系初始化配置

```C#
//配置关联关系
OrmNavigate<Test>.Connect<Test2>(test => test.Domain, test2 => test2.Id);
//test=>test.Type, test3=>test3.Id
OrmNavigate<Test>.Connect<Test3>("Type", "Id"); 
```

### 使用 ToJoinList
```C#
[HttpPost("join")]
public ApiReturnPageResult GetJoinList(SqlModel<Test> sqlModel)
{
    return Result( _freeSql.QueryFromSqlModel(sqlModel,out long total).ToJoinList(item => new
        {
            item.Id,
           TestName = item.Name,
           DomainName = InnerJoin<Test2>.MapFrom(item => item.Name),
           TypeName = InnerJoin<Test3>.MapFrom(item => item.TypeName)
        }),total);
}
```

## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fnight-moon-studio%2FAries.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Fnight-moon-studio%2FAries?ref=badge_large)
