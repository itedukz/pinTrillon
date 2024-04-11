using ms.MainApi.Entity.Models.Pages.SearchPages;
using System.Linq.Expressions;
using System.Reflection;

namespace ms.MainApi.Business.ExpressionParser;

public interface ISearchProjectExpressionParser
{
    public Expression<Func<T, bool>>? ParseExpressionOf<T>(SearchProject doc);
}


public class SearchProjectExpressionParser : ISearchProjectExpressionParser
{
    public Expression<Func<T, bool>>? ParseExpressionOf<T>(SearchProject doc)
    {
        var itemExpression = Expression.Parameter(typeof(T));
        var conditions = ParseTree<T>(doc, itemExpression);

        if (conditions != null)
        {
            if (conditions.CanReduce)
            {
                conditions = conditions.ReduceAndCheck();
            }

            var query = Expression.Lambda<Func<T, bool>>(conditions, itemExpression);
            return query;
        }
        return null;
    }

    private readonly MethodInfo MethodContains = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2);
    private readonly MethodInfo MethodIntersect = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Single(m => m.Name == nameof(Enumerable.Intersect) && m.GetParameters().Length == 2);

    private delegate Expression Binder(Expression left, Expression right);

    private Expression ParseTree<T>(SearchProject condition, ParameterExpression parm)
    {
        Expression left = null;

        try
        {
            Binder binder = (Binder)Expression.And;
            Expression bind(Expression left, Expression right) => left == null ? right : binder(left, right);

            #region catalogsId
            if (condition.query!.catalogsId != null && condition.query!.catalogsId.Count > 0 && condition.query!.catalogsId[0] != 0)
            {
                var property = Expression.Property(parm, "projectCatalogId");

                var contains = MethodContains.MakeGenericMethod(typeof(int));
                var right = Expression.Call(contains, Expression.Constant(condition.query!.catalogsId), property);

                left = bind(left, right);
            }
            #endregion


            #region priceMin && priceMax
            if (condition.query!.priceMin > 0)
            {
                var property = Expression.Property(parm, "price");

                object val = condition.query!.priceMin;
                var toCompare = Expression.Constant(val);
                var right = Expression.GreaterThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            if (condition.query!.priceMax > 0)
            {
                var property = Expression.Property(parm, "price");

                object val = condition.query!.priceMax;
                var toCompare = Expression.Constant(val);
                var right = Expression.LessThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            #endregion


            #region quadratureMin && quadratureMax
            if (condition.query!.quadratureMin > 0)
            {
                var property = Expression.Property(parm, "quadrature");

                object val = condition.query!.quadratureMin;
                var toCompare = Expression.Constant(val);
                var right = Expression.GreaterThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            if (condition.query!.quadratureMax > 0)
            {
                var property = Expression.Property(parm, "quadrature");

                object val = condition.query!.quadratureMax;
                var toCompare = Expression.Constant(val);
                var right = Expression.LessThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            #endregion


            #region widthMin && widthMax
            if (condition.query!.widthMin > 0)
            {
                var property = Expression.Property(parm, "width");

                object val = condition.query!.widthMin;
                var toCompare = Expression.Constant(val);
                var right = Expression.GreaterThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            if (condition.query!.widthMax > 0)
            {
                var property = Expression.Property(parm, "width");

                object val = condition.query!.widthMax;
                var toCompare = Expression.Constant(val);
                var right = Expression.LessThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            #endregion


            #region lengthMin && lengthMax
            if (condition.query!.lengthMin > 0)
            {
                var property = Expression.Property(parm, "length");

                object val = condition.query!.lengthMin;
                var toCompare = Expression.Constant(val);
                var right = Expression.GreaterThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            if (condition.query!.lengthMax > 0)
            {
                var property = Expression.Property(parm, "length");

                object val = condition.query!.lengthMax;
                var toCompare = Expression.Constant(val);
                var right = Expression.LessThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            #endregion


            #region heightMin && heightMax
            if (condition.query!.heightMin > 0)
            {
                var property = Expression.Property(parm, "height");

                object val = condition.query!.heightMin;
                var toCompare = Expression.Constant(val);
                var right = Expression.GreaterThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            if (condition.query!.heightMax > 0)
            {
                var property = Expression.Property(parm, "height");

                object val = condition.query!.heightMax;
                var toCompare = Expression.Constant(val);
                var right = Expression.LessThanOrEqual(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));

                left = bind(left, right);
            }
            #endregion

        }
        catch { }

        return left;
    }
}