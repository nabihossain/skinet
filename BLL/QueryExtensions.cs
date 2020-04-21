using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLL
{
    public static class QueryExtensions
    {
        public static Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderByFunc<T>(KeyValuePair<string, string> keyValuePair)
        {
            Func<IQueryable<T>, IOrderedQueryable<T>> result = null;
            ParameterExpression param = Expression.Parameter(typeof(T), "arg");
            Expression member = Expression.Property(param, keyValuePair.Key);
            if (member.Type.IsValueType)
            {
                member = Expression.Convert(member, typeof(object));
            }
            var lambda = Expression.Lambda<Func<T, object>>(member, param);
            switch (keyValuePair.Value)
            {
                case "Ascending":
                    result = q => q.OrderBy(lambda);
                    break;
                case "Descending":
                    result = q => q.OrderByDescending(lambda);
                    break;
                default:
                    result = q => q.OrderBy(lambda);
                    break;
            }
            return result;
        }
        public static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy<TEntity>(string orderColumn, string orderType)
        {
            Type typeQueryable = typeof(IQueryable<TEntity>);
            ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "p");
            var outerExpression = Expression.Lambda(argQueryable, argQueryable);
            string[] props = orderColumn.Split('.');
            IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");

            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            LambdaExpression lambda = Expression.Lambda(expr, arg);
            string methodName = orderType == "asc" ? "OrderBy" : "OrderByDescending";

            MethodCallExpression resultExp =
                Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TEntity), type }, outerExpression.Body, Expression.Quote(lambda));
            var finalLambda = Expression.Lambda(resultExp, argQueryable);
            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
        }
        public interface IOrderByExpression<TEntity> where TEntity : class
        {
            IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query);
            IOrderedQueryable<TEntity> ApplyThenBy(IOrderedQueryable<TEntity> query);
        }

        public class OrderByExpression<TEntity, TOrderBy> : IOrderByExpression<TEntity>
    where TEntity : class
        {
            private Expression<Func<TEntity, TOrderBy>> _expression;
            private bool _descending;

            public OrderByExpression(Expression<Func<TEntity, TOrderBy>> expression,
                bool descending = false)
            {
                _expression = expression;
                _descending = descending;
            }

            public IOrderedQueryable<TEntity> ApplyOrderBy(
                IQueryable<TEntity> query)
            {
                if (_descending)
                    return query.OrderByDescending(_expression);
                else
                    return query.OrderBy(_expression);
            }

            public IOrderedQueryable<TEntity> ApplyThenBy(
                IOrderedQueryable<TEntity> query)
            {
                if (_descending)
                    return query.ThenByDescending(_expression);
                else
                    return query.ThenBy(_expression);
            }
        }
    }
}