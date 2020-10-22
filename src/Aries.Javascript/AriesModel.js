function SqlModel() {
}
SqlModel.prototype.QueryModel = {};
SqlModel.prototype.QueryModel.Page = 0;
SqlModel.prototype.QueryModel.Size = 0;
SqlModel.prototype.QueryModel.Total = false;
SqlModel.prototype.QueryModel.Orders = [];
SqlModel.prototype.QueryModel.Fuzzy = [];
SqlModel.prototype.QueryInstance = {};
SqlModel.prototype.QueryInstance.Instance = {};
SqlModel.prototype.QueryInstance.Fields = [];
SqlModel.prototype.ModifyInstance = {};
SqlModel.prototype.ModifyInstance.Instance = {};
SqlModel.prototype.ModifyInstance.Fields = [];
 

//增加模糊查询
SqlModel.prototype.AddFuzzy = function (field,value) 
{

    this.QueryModel.Fuzzy.push({FieldName: field, FuzzyValue : value });

}


//增加升序字段
SqlModel.prototype.AddAscField = function (field) 
{

    this.QueryModel.Orders.push({ FieldName: field, IsDesc: false });

}


//增加降序字段
SqlModel.prototype.AddDescField = function (field) 
{

    this.QueryModel.Orders.push({ FieldName: field, IsDesc: true });

}


//增加条件查询字段
SqlModel.prototype.AddWhereField = function (field) {

    this.QueryInstance.Fields.push(field);

}


//增加被更新的字段
SqlModel.prototype.AddUpdateField = function (field) {

    this.ModifyInstance.Fields.push(field);

}

