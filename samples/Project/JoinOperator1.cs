using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Project
{
    public class JoinOperator1<TEntity>
    {

        public JoinResult<TEntity, TJoinEntity> LefJoin<TJoinEntity>(Expression<Func<TEntity, object>> expression, string fieldName = default)
        {
            return new JoinResult<TEntity, TJoinEntity>() { Link = this };
        }
    }


    public class JoinResult<TEntity,TJoinEntity> {

        public JoinOperator1<TEntity> Link;
        public string TypeName;
        public void Test()
        {
           
        }
        public JoinOperator1<TEntity> ContactResult<TReturn>(Expression<Func<TJoinEntity,TReturn>> expression)
        {
            TypeName = typeof(TReturn).Name;
            return Link;
        }
        public JoinOperator1<TEntity> NoResult()
        {
            return Link;
        }
    
    }


}
