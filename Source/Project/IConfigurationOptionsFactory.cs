using StackExchange.Redis;

namespace Project
{
	public interface IConfigurationOptionsFactory
	{
		#region Methods

		ConfigurationOptions Create(string connectionStringName);

		#endregion
	}
}