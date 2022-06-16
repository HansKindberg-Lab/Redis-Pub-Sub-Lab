using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RegionOrebroLan;
using RegionOrebroLan.Configuration.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Project.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddRedisMessaging(this IServiceCollection services, IConfiguration configuration)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			services.TryAddSingleton<IConfigurationOptionsFactory, ConfigurationOptionsFactory>();
			services.TryAddSingleton<IConnectionMultiplexerFactory, ConnectionMultiplexerFactory>();
			services.TryAddSingleton<IParser<ConfigurationOptions>, ConfigurationOptionsParser>();

			services.AddConfigurationMonitor(configuration);

			return services;
		}

		#endregion
	}
}