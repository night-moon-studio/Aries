using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSql.Natasha.Extension
{
    public static class InsertOperator<TEntity>
    {
        public static Action<TEntity> InsertInitFunc;
    }
}
