using RegionOrebroLan;
using StackExchange.Redis;

namespace Project
{
	/// <inheritdoc />
	public class ConfigurationOptionsParser : BasicParser<ConfigurationOptions>
	{
		#region Properties

		public virtual bool IgnoreUnknownElements { get; set; }

		#endregion

		#region Methods

		public override ConfigurationOptions Parse(string value)
		{
			if(value == null)
				return null;

			if(string.IsNullOrWhiteSpace(value))
				return new ConfigurationOptions();

			try
			{
				return ConfigurationOptions.Parse(value, this.IgnoreUnknownElements);
			}
			catch(Exception exception)
			{
				throw new ArgumentException($"Could not parse the value \"{value}\" to configuration-options.", nameof(value), exception);
			}
		}

		#endregion
	}
}