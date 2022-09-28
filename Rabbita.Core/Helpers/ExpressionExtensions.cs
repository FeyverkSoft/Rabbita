namespace Rabbita.Core.Helpers;

public static class ExpressionExtensions
{
    /// <summary>
    /// Извлекает PropertyInfo по LambdaExpression
    /// </summary>
    /// <param name="propertyAccessExpression"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static PropertyInfo GetPropertyAccess(this LambdaExpression propertyAccessExpression)
    {
        var parameterExpression = propertyAccessExpression.Parameters.Single();
        var propertyInfo = parameterExpression.MatchSimplePropertyAccess(propertyAccessExpression.Body);

        if (propertyInfo == null)
            throw new ArgumentException($"Invalid property expression {propertyAccessExpression}",
                nameof(propertyAccessExpression));

        var declaringType = propertyInfo.DeclaringType;
        var parameterType = parameterExpression.Type;

        if (declaringType == null ||
            declaringType == parameterType ||
            !declaringType.GetTypeInfo().IsInterface ||
            !declaringType.GetTypeInfo().IsAssignableFrom(parameterType.GetTypeInfo()))
        {
            return propertyInfo;
        }

        var propertyGetter = propertyInfo.GetMethod;
        var interfaceMapping = parameterType.GetTypeInfo().GetRuntimeInterfaceMap(declaringType);
        var index = Array.FindIndex(interfaceMapping.InterfaceMethods, p => propertyGetter.Equals(p));
        var targetMethod = interfaceMapping.TargetMethods[index];
        return parameterType.GetRuntimeProperties().FirstOrDefault(runtimeProperty => targetMethod.Equals(runtimeProperty.GetMethod), propertyInfo);
    }

    private static PropertyInfo? MatchSimplePropertyAccess(this Expression parameterExpression, Expression propertyAccessExpression)
    {
        var propertyInfos = MatchPropertyAccess(parameterExpression, propertyAccessExpression);

        return propertyInfos.Count == 1 ? propertyInfos[0] : null;
    }

    private static IReadOnlyList<PropertyInfo> MatchPropertyAccess(
        this Expression parameterExpression, Expression propertyAccessExpression)
    {
        var propertyInfos = new List<PropertyInfo>();

        MemberExpression? memberExpression;

        do
        {
            memberExpression = RemoveTypeAs(RemoveConvert(propertyAccessExpression)) as MemberExpression;

            if (memberExpression?.Member is not PropertyInfo propertyInfo)
            {
                return ArraySegment<PropertyInfo>.Empty;
            }

            propertyInfos.Insert(0, propertyInfo);

            propertyAccessExpression = memberExpression.Expression;
        } while (RemoveTypeAs(RemoveConvert(memberExpression.Expression)) != parameterExpression);

        return propertyInfos;
    }

    private static Expression? RemoveTypeAs(this Expression expression)
    {
        while (expression?.NodeType == ExpressionType.TypeAs)
        {
            expression = ((UnaryExpression)RemoveConvert(expression)).Operand;
        }

        return expression;
    }

    private static Expression RemoveConvert(Expression expression)
    {
        if (expression is UnaryExpression unaryExpression
            && (expression.NodeType == ExpressionType.Convert
                || expression.NodeType == ExpressionType.ConvertChecked))
        {
            return RemoveConvert(unaryExpression.Operand);
        }

        return expression;
    }
}