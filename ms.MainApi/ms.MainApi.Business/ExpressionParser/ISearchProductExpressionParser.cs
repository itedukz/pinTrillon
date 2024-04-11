using ms.MainApi.Entity.Models.Pages.SearchPages;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ms.MainApi.Business.ExpressionParser;

public interface ISearchProductExpressionParser
{
    public Expression<Func<T, bool>>? ParseExpressionOf<T>(SearchProduct doc);
}



public class SearchProductExpressionParser : ISearchProductExpressionParser
{

    public Expression<Func<T, bool>>? ParseExpressionOf<T>(SearchProduct doc)
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

    private Expression ParseTree<T>(SearchProduct condition, ParameterExpression parm)
    {
        Expression left = null;

        try
        {
            Binder binder = (Binder)Expression.And;
            Expression bind(Expression left, Expression right) => left == null ? right : binder(left, right);
            
            if (condition.query!.catalogsId != null && condition.query!.catalogsId.Count > 0 && condition.query!.catalogsId[0] != 0)
            {
                var property = Expression.Property(parm, "catalogId");

                //object val = condition.catalogsId.AsEnumerable();
                var contains = MethodContains.MakeGenericMethod(typeof(int));
                var right = Expression.Call(contains, Expression.Constant(condition.query!.catalogsId), property);

                left = bind(left, right);
            }

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


            //if (condition.materialsId != null && condition.materialsId.Count > 0)
            //{
            //    var property = Expression.Property(parm, "materialsId");

            //    object val = condition.materialsId.AsEnumerable();
            //    var intersect = MethodIntersect.MakeGenericMethod(typeof(int));
            //    var right = Expression.Call(intersect, Expression.Constant(condition.catalogsId), property);

            //    left = bind(left, right);
            //}


        }
        catch { }

        return left;
    }
}