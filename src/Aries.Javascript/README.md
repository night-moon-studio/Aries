#### 一共3个Model :

1、 queryModel 包含分页/排序/模糊查询  

2、 queryInstance 对字段进行精确查询  

3、 modifyInstance 对字段进行精确修改  


```C#
{
    "queryModel": 
    {
        //当前页    //页码    //是否获取总数    
        "page": 0, "size": 0,"total": true,


        "orders": 
        [{
        
            //按照 id 排序       //是降序
            "fieldName": "id", "isDesc": true
            
        }],
        
        
        "fuzzy": 
        [{
            //根据 title 进行查询
            "fuzzyField": "title",
            
            //模糊匹配 value 字串
            "fuzzyValue": "value",
            
            //是否忽略大小写
            "ignoreCase": true,
            
            //查询之间是否使用 OR 做连接
            "isOr": true
        }]
    },
    

    "queryInstance": 
    {
        //查询实体
        "instance": 
        {
          "id": 0,
          "domain": 0,
          "type": 0,
          "name": "string",
          "address": "string"
        },
      
        //需要查询实体中的哪些字段 比如：只查询 Domain 值 (数组类型)
        "fields": ["Domain"]
    },
    

    "modifyInstance": 
    {
        //新的实体
        "instance": 
        {
          "id": 0,
          "domain": 0,
          "type": 0,
          "name": "string",
          "address": "string"
        },
        
        //需要更新实体中的哪些字段 比如：只查询 domain 和 name 值
        "fields": ["domain","name"]
    }
  }
```
