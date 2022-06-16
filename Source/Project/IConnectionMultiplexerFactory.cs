using StackExchange.Redis;

namespace Project;

public interface IConnectionMultiplexerFactory
{
	#region Methods

	IConnectionMultiplexer Create(string connectionString);

	#endregion
}