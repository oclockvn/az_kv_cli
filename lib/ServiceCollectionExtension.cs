using az_kv.lib.Readers;
using Microsoft.Extensions.DependencyInjection;

namespace az_kv.lib;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddLib(this IServiceCollection services)
    {
        return services
            .AddScoped<ILocalSettingReader, LocalSettingReader>()
            .AddScoped<IVaultSecretReader, VaultSecretReader>()
            ;
    }
}

