using RegionOrebroLan;
using StackExchange.Redis;

namespace Project
{
	/// <inheritdoc />
	public class ConnectionMultiplexerFactory : IConnectionMultiplexerFactory
	{
		#region Constructors

		public ConnectionMultiplexerFactory(IParser<ConfigurationOptions> configurationOptionsParser)
		{
			this.ConfigurationOptionsParser = configurationOptionsParser ?? throw new ArgumentNullException(nameof(configurationOptionsParser));
		}

		#endregion

		#region Properties

		protected internal virtual IParser<ConfigurationOptions> ConfigurationOptionsParser { get; }

		#endregion

		#region Methods

		public virtual IConnectionMultiplexer Create(string connectionString)
		{
			try
			{
				return ConnectionMultiplexer.Connect(this.ConfigurationOptionsParser.Parse(connectionString));
			}
			catch(Exception exception)
			{
				throw new ArgumentException($"Could not create a connection-multiplexer from connection-string \"{connectionString}\".", nameof(connectionString), exception);
			}
		}

		#endregion
	}
}