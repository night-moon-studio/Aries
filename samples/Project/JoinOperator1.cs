using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Project
{
    public class JoinOperator1<TEntity>
    {

        public JoinResult<TEntity, TJoinEntity> LefJoin<TJoinEntity>()
        {

        }
    }


    public class JoinResult<TEntity,TJoinEntity> {

        public JoinOperator1<TEntity> Link;
        public string TypeName;
        public void Test()
        {
            return WithResult(new {  })
        }
        public JoinOperator1<TEntity> WithResult(object instance)
        {
            TypeName = instance.GetType().Name;
            return Link;
        }
    
    }


}
