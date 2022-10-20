using Microsoft.Extensions.Configuration;

namespace ListingsCalculator.Extensions;
public static class ConfigurationExtensions
{
    public static T GetSetting<T>(this IConfigurationRoot configuration)
        where T : class =>
        configuration.GetSection(typeof(T).Name).Get<T>();
}