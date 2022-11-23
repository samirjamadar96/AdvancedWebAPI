using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AdvancedWebAPI.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> OrderByCustom<TEntity>(this IQueryable<TEntity> items, string sortBy, string sortOrder)
        {
            var defaultLookup = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            var type = typeof(TEntity);
            var expression2 = Expression.Parameter(type, "t");
            var property = type.GetProperty(sortBy, defaultLookup);
            if (property != null)
            {
                var expression1 = Expression.MakeMemberAccess(expression2, property);
                var lambda = Expression.Lambda(expression1, expression2);
                var result = Expression.Call(
                    typeof(Queryable),
                    sortOrder == "desc" ? "OrderByDescending" : "OrderBy",
                    new Type[] { type, property.PropertyType },
                    items.Expression,
                    Expression.Quote(lambda));

                return items.Provider.CreateQuery<TEntity>(result);
            }
            else
                throw new Exception("Property is not present in DB to sort by");
            
        }
    }
}

