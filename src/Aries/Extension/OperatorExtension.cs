using FreeSql;
using System.Linq;

namespace Aries
{
    public static class OperatorExtension
    {

        public static IUpdate<TEntity> AriesModify<TEntity>(this IFreeSql freesql, SqlModel<TEntity> model) where TEntity : class
        {

            if (model.ModifyInstance != null)
            {


                var handler = freesql.UpdateWithEntity(model.ModifyInstance.Fields, model.ModifyInstance.Instance);
                var instance = model.QueryInstance;
                if (instance != null)
                {

                    if (instance.Contains != null && instance.Contains.Length > 0)
                    {
                        handler.Where(InQueryOperator<TEntity>.InHandler(instance.Contains));
                    }
                    handler.WhereWithEntity(instance.Fields, instance.Instance);
                    foreach (var item in model.GetWhereExpressions())
                    {
                        handler.Where(item);
                    }

                }
                if (model.QueryModel != null)
                {

                    handler.WhereWithModel(model.QueryModel);

                }

                return handler;
            }
            return null;


        }


        public static ISelect<TEntity> AriesQuery<TEntity>(this IFreeSql freesql, SqlModel<TEntity> model,out long total) where TEntity : class
        {

            var handler = freesql.Select<TEntity>();
            var instance = model.QueryInstance;
            if (instance != null)
            {

                if (instance.Contains!=null && instance.Contains.Length>0)
                {
                    handler.Where(InQueryOperator<TEntity>.InHandler(instance.Contains));
                }
                handler.WhereWithEntity(instance.Fields, instance.Instance);
                foreach (var item in model.GetWhereExpressions())
                {
                    handler.Where(item);
                }

            }

            if (model.QueryModel != null)
            {

                handler.WhereWithModel(model.QueryModel);
                if (model.QueryModel.Total)
                {
                    handler.Count(out total);
                }
                else
                {
                    total = 0;
                }

                if (model.QueryModel.Size != 0)
                {
                    handler.Page(model.QueryModel.Page, model.QueryModel.Size);
                }
            }
            else
            {
                total = 0;
            }

            return handler;

        }


        public static IDelete<TEntity> AriesDelete<TEntity>(this IFreeSql freesql, SqlModel<TEntity> model) where TEntity : class
        {

            var handler = freesql.Delete<TEntity>();
            var instance = model.QueryInstance;
            if (instance != null)
            {

                if (instance.Contains != null && instance.Contains.Length > 0)
                {
                    handler.Where(InQueryOperator<TEntity>.InHandler(instance.Contains));
                }
                handler.WhereWithEntity(instance.Fields, instance.Instance);
                foreach (var item in model.GetWhereExpressions())
                {
                    handler.Where(item);
                }

            }
            if (model.QueryModel != null)
            {

                handler.WhereWithModel(model.QueryModel);

            }
            return handler;
        }

    }
}
