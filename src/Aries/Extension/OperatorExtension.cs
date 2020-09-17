using FreeSql;
using System.Collections.Generic;

namespace Aries
{
    public static class OperatorExtension
    {

        public static IUpdate<TEntity> ModifyFromSqlModel<TEntity>(this IFreeSql freesql, SqlModel<TEntity> model) where TEntity : class
        {

            if (model.ModifyInstance != null)
            {


                var handler = freesql.UpdateWithHttpModel(model.ModifyInstance.Fields, model.ModifyInstance.Instance);
                if (model.QueryModel != null)
                {

                    handler.QueryWithModel(model.QueryModel);

                }


                if (model.QueryInstance != null)
                {

                    handler.QueryWithHttpEntity(model.QueryInstance.Fields, model.QueryInstance.Instance);

                }


                return handler;
            }
            return null;


        }


        public static ISelect<TEntity> QueryFromSqlModel<TEntity>(this IFreeSql freesql, SqlModel<TEntity> model, out long total) where TEntity : class
        {

            var handler = freesql.Select<TEntity>();
            if (model.QueryModel != null)
            {

                handler.QueryWithModel(model.QueryModel,out total);

            }
            else
            {

                total = 0;

            }


            if (model.QueryInstance != null)
            {

                handler.QueryWithHttpEntity(model.QueryInstance.Fields, model.QueryInstance.Instance);

            }
            return handler;

        }


        public static IDelete<TEntity> DeleteFromSqlModel<TEntity>(this IFreeSql freesql, SqlModel<TEntity> model) where TEntity : class
        {

            var handler = freesql.Delete<TEntity>();
            if (model.QueryModel!=null)
            {

                handler.QueryWithModel(model.QueryModel);

            }


            if (model.QueryInstance!=null)
            {

                handler.QueryWithHttpEntity(model.QueryInstance.Fields, model.QueryInstance.Instance);

            }   
            return handler;
        }

    }
}
