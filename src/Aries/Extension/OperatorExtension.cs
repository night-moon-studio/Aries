using System.Collections.Generic;

namespace Aries.Extension
{
    public static class OperatorExtension
    {

        public static long ModifyFromQueryModel<TEntity>(this IFreeSql freesql, SqlModel<TEntity> model) where TEntity : class
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


                return handler.ExecuteAffrows();
            }
            return -1;


        }


        public static IEnumerable<TEntity> QueryFromQueryModel<TEntity>(this IFreeSql freesql, SqlModel<TEntity> model, out long total) where TEntity : class
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
            return handler.ToLimitList();

        }


        public static long DeleteFromQueryModel<TEntity>(this IFreeSql freesql, SqlModel<TEntity> model) where TEntity : class
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
            return handler.ExecuteAffrows();
        }

    }
}
