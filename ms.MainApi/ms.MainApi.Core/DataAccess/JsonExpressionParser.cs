using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace ms.MainApi.Core.DataAccess;

public class JsonExpressionParser
{
    private const string StringStr = "string";
    private readonly string BooleanStr = nameof(Boolean).ToLower();
    private readonly string Number = nameof(Number).ToLower();
    private readonly string Integer = "integer";
    private readonly string In = nameof(In).ToLower();
    private readonly string And = nameof(And).ToLower();

    private readonly MethodInfo MethodContains = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2);

    private delegate Expression Binder(Expression left, Expression right);

    private Expression ParseTree<T>(JsonElement condition, ParameterExpression parm)
    {
        Expression left = null;
        try
        {
            var gate = condition.GetProperty(nameof(condition)).GetString();

            JsonElement rules = condition.GetProperty(nameof(rules));

            Binder binder = gate == And ? (Binder)Expression.And : Expression.Or;

            Expression bind(Expression left, Expression right) => left == null ? right : binder(left, right);

            foreach (var rule in rules.EnumerateArray())
            {
                if (rule.TryGetProperty(nameof(condition), out JsonElement check))
                {
                    var right = ParseTree<T>(rule, parm);
                    left = bind(left, right);
                    continue;
                }

                string @operators = rule.GetProperty(nameof(@operators)).GetString()!;
                string type = rule.GetProperty(nameof(type)).GetString()!;
                string field = rule.GetProperty(nameof(field)).GetString()!;

                JsonElement value = rule.GetProperty(nameof(value));
                JsonElement values = rule.GetProperty(nameof(values));

                var property = Expression.Property(parm, field);

                if (operators == In)
                {
                    var rightExpression = getFieldValueExpression(operators, type, values, property);
                    left = bind(left, rightExpression);
                }
                else
                {
                    var rightExpression = getFieldValueExpression(operators, type, value, property);
                    left = bind(left, rightExpression);
                }
            }
        }
        catch { }

        return left;
    }

    private Expression getFieldValueExpression(string operators, string type, JsonElement values, MemberExpression property = null)
    {
        object val = new object();
        if (operators == In)
        {
            if (type == StringStr || type == BooleanStr)
                val = values.EnumerateArray().Select(e => e.GetString()).ToList();
            else if (type == Integer)
                val = values.EnumerateArray().Select(e => int.Parse(e.GetString()!)).ToList();
            else
                val = values.EnumerateArray().Select(e => decimal.Parse(e.GetString()!)).ToList();

            var contains = MethodContains.MakeGenericMethod(typeof(string));
            var right = Expression.Call(contains, Expression.Constant(val), property);

            return right;
        }
        else
        {
            if (type == StringStr || type == BooleanStr)
            {
                val = values.GetString()!;
                var toCompare = Expression.Constant(val);

                return Expression.Equal(property, toCompare);
            }
            else if (type == Integer)
            {
                val = int.Parse(values.GetString()!);
                var toCompare = Expression.Constant(val);

                return Expression.Equal(property, Expression.Convert(toCompare, typeof(System.Int32)));
            }
            else
            {
                val = decimal.Parse(values.GetString()!);
                var toCompare = Expression.Constant(val);

                return Expression.Equal(Expression.Convert(property, typeof(decimal)), Expression.Convert(toCompare, typeof(decimal)));
            }
        }
    }


    public Expression<Func<T, bool>>? ParseExpressionOf<T>(JsonDocument doc)
    {
        var itemExpression = Expression.Parameter(typeof(T));
        var conditions = ParseTree<T>(doc.RootElement, itemExpression);

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

    public Func<T, bool>? ParsePredicateOf<T>(JsonDocument doc)
    {
        var query = ParseExpressionOf<T>(doc);

        var result = query?.Compile();
        return result;
    }
}