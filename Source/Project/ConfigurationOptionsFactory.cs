using Microsoft.Extensions.Configuration;
using RegionOrebroLan;
using StackExchange.Redis;

namespace Project
{
	/// <inheritdoc />
	public class ConfigurationOptionsFactory : IConfigurationOptionsFactory
	{
		#region Constructors

		public ConfigurationOptionsFactory(IConfiguration configuration, IParser<ConfigurationOptions> configurationOptionsParser)
		{
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.ConfigurationOptionsParser = configurationOptionsParser ?? throw new ArgumentNullException(nameof(configurationOptionsParser));
		}

		#endregion

		#region Properties

		protected internal virtual IConfiguration Configuration { get; }
		protected internal virtual IParser<ConfigurationOptions> ConfigurationOptionsParser { get; }

		#endregion

		#region Methods

		public virtual ConfigurationOptions Create(string connectionStringName)
		{
			if(connectionStringName == null)
				throw new ArgumentNullException(nameof(connectionStringName));

			var connectionString = this.Configuration.GetConnectionString(connectionStringName);

			if(connectionString == null)
				throw new ArgumentException($"The connection-string \"{connectionStringName}\" does not exist.", nameof(connectionStringName));

			try
			{
				return this.ConfigurationOptionsParser.Parse(connectionString);
			}
			catch(Exception exception)
			{
				throw new ArgumentException($"Could not create configuration-options from the connection-string \"{connectionStringName}\" with value \"{connectionString}\".", nameof(connectionStringName), exception);
			}
		}

		#endregion
	}
}