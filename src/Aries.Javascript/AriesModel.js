

function GetAriesProxy() {

    var sqlModel = {
        QueryModel: {
            Page: 0,
            Size: 0,
            Total: true,
            Orders: [],
            Fuzzy: []
        },
        QueryInstance: {
            Instance: {},
            Contains: [],
            Fields: []
        },
        ModifyInstance: {
            Instance: {},
            Fields: []
        },
        In: function (value) {

            this.QueryInstance.Contains.push(value);

        },
        AddFuzzy: function (field, value) {

            this.QueryModel.Fuzzy.push({ FieldName: field, FuzzyValue: value, IgnoreCase: true, IsOr: true });

        },
        AddAndFuzzy: function (field, value) {

            this.QueryModel.Fuzzy.push({ FieldName: field, FuzzyValue: value, IgnoreCase: false, IsOr: false });

        },
        AddIgnoreAndFuzzy: function (field, value) {

            this.QueryModel.Fuzzy.push({ FieldName: field, FuzzyValue: value, IgnoreCase: false, IsOr: false });

        },
        AddOrFuzzy: function (field, value) {

            this.QueryModel.Fuzzy.push({ FieldName: field, FuzzyValue: value, IgnoreCase: false, IsOr: true });

        },
        AddIgnoreOrFuzzy: function (field, value) {

            this.QueryModel.Fuzzy.push({ FieldName: field, FuzzyValue: value, IgnoreCase: false, IsOr: true });

        },

        //增加升序字段
        AddAscField: function (field) {

            this.QueryModel.Orders.push({ FieldName: field, IsDesc: false });

        },

        //增加降序字段
        AddDescField: function (field) {

            this.QueryModel.Orders.push({ FieldName: field, IsDesc: true });

        }


    };

    sqlModel.ModifyInstance.Instance = new Proxy(sqlModel.ModifyInstance.Instance, {
        
        set(target, key, value, receiver) {

            if(sqlModel.ModifyInstance.Fields.indexOf(key) == -1)
            {
                sqlModel.ModifyInstance.Fields.push(key);
            }
            Reflect.set(target, key, value, receiver);

        }
    });

    sqlModel.QueryInstance.Instance = new Proxy(sqlModel.QueryInstance.Instance, {

        set(target, key, value, receiver) {

            if(sqlModel.QueryInstance.Fields.indexOf(key) == -1)
            {
                sqlModel.QueryInstance.Fields.push(key);
            }
            Reflect.set(target, key, value, receiver);

        }
    });

    return sqlModel;
}

