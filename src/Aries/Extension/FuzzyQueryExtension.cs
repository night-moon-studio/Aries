using FreeSql;

namespace Aries
{

    public static class FuzzyQueryExtensions
    {

        /// <summary>
        /// 模糊查询模型
        /// 会受到 PropertiesCache<TEntity>.BlockWhereFields 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="select">查询操作句柄</param>
        /// <param name="model">模糊查询模型</param>
        public static void FuzzyQuery<TEntity>(this ISelect<TEntity> select, FuzzyModel model) where TEntity : class
        {

            FuzzyQueryOperator<TEntity>.FuzzyQueryModel(select, model);

        }
    }
}
