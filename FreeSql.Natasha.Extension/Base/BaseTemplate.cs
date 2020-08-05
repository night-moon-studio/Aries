using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSql.Natasha.Extension
{
    public class BaseTemplate<T>
    {
        public readonly IFreeSql SqlHandler;
        public BaseTemplate(IFreeSql freeSql)
        {
            SqlHandler = freeSql;
        }
    }
}
