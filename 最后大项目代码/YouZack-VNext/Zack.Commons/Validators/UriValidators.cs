using System;

namespace FluentValidation;
public static class UriValidators
{
    /// <summary>
    /// 验证一个 Uri 对象不是 null 且其原始字符串不为空或空白
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, Uri> NotEmptyUri<T>(this IRuleBuilder<T, Uri> ruleBuilder)
    {
        return ruleBuilder.Must(p => p == null || !string.IsNullOrWhiteSpace(p.OriginalString))
            .WithMessage("The Uri must not be null nor empty.");
    }

    public static IRuleBuilderOptions<T, Uri> Length<T>(this IRuleBuilder<T, Uri> ruleBuilder, int min, int max)
    {
        //为空则跳过检查，因为有专门的NotEmptyUri判断，也许一个东西允许空，但是不为空的时候再限制长度
        return ruleBuilder.Must(p => string.IsNullOrWhiteSpace(p.OriginalString)
            || (p.OriginalString.Length >= min && p.OriginalString.Length <= max))
            .WithMessage($"The length of Uri must not be between {min} and {max}.");
    }
}