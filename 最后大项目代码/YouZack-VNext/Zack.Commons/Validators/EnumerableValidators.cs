using System.Collections.Generic;
using System.Linq;

namespace FluentValidation;
public static class EnumerableValidators
{

    /// FluentValidation 用于额外的集合验证逻辑

    /// <summary>
    /// 集合中没有重复元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> NotDuplicated<T, TItem>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder)
    {
        return ruleBuilder.Must(p => p == null || p.Distinct().Count() == p.Count());
    }

    /// <summary>
    /// 集合中不包含指定的值comparedValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="comparedValue">待匹配的值</param>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> NotContains<T, TItem>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder, TItem comparedValue)
    {
        return ruleBuilder.Must(p => p == null || !p.Contains(comparedValue));
    }

    /// <summary>
    /// 集合中包含指定的值comparedValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="comparedValue">待匹配的值</param>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> Contains<T, TItem>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder, TItem comparedValue)
    {
        return ruleBuilder.Must(p => p == null || p.Contains(comparedValue));
    }
}