using Project;
using StackExchange.Redis;

namespace UnitTests
{
	[TestClass]
	public class ConfigurationOptionsParserTest
	{
		#region Methods

		[TestMethod]
		public async Task IgnoreUnknownElements_ShouldReturnFalseByDefault()
		{
			await Task.CompletedTask;

			Assert.IsFalse(new ConfigurationOptionsParser().IgnoreUnknownElements);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task Parse_IfTheValueParameterContainsUnknownElementsAndIgnoreUnknownElementsIsDefaultFalse_ShouldThrowAnArgumentException()
		{
			await Task.CompletedTask;
			const string value = "localhost,UnknownElement=True";

			try
			{
				new ConfigurationOptionsParser().Parse(value);
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.Message.StartsWith($"Could not parse the value \"{value}\" to configuration-options.", StringComparison.Ordinal) && argumentException.InnerException is ArgumentException)
					throw;
			}
		}

		[TestMethod]
		public async Task Parse_IfTheValueParameterContainsUnknownElementsAndIgnoreUnknownElementsIsSetToTrue_ShouldWorkProperly()
		{
			await Task.CompletedTask;
			const string value = "localhost,UnknownElement=True";

			Assert.AreNotEqual("localhost", new ConfigurationOptionsParser { IgnoreUnknownElements = true }.Parse(value));
		}

		[TestMethod]
		public async Task Parse_IfTheValueParameterIsAnEmptyString_ShouldReturnAnEmptyConfiguration()
		{
			await Task.CompletedTask;

			var configurationOptions = new ConfigurationOptionsParser().Parse(string.Empty);

			Assert.IsFalse(configurationOptions.EndPoints.Any());
		}

		[TestMethod]
		public async Task Parse_IfTheValueParameterIsNull_ShouldReturnNull()
		{
			await Task.CompletedTask;

			Assert.IsNull(new ConfigurationOptionsParser().Parse(null));
		}

		[TestMethod]
		public async Task Parse_IfTheValueParameterIsValidUpperCase_ShouldWorkProperly()
		{
			await Task.CompletedTask;

			var configurationOptions = ConfigurationOptions.Parse("LOCALHOST");
			configurationOptions.AllowAdmin = true;
			configurationOptions.Ssl = true;

			var value = configurationOptions.ToString();

			var parsedConfigurationOptions = new ConfigurationOptionsParser().Parse(value.ToUpperInvariant());

			Assert.AreEqual(value, parsedConfigurationOptions.ToString());
		}

		[TestMethod]
		public async Task Parse_IfTheValueParameterIsWhitespacesOnly_ShouldReturnAnEmptyConfiguration()
		{
			await Task.CompletedTask;

			var configurationOptions = new ConfigurationOptionsParser().Parse("    ");

			Assert.IsFalse(configurationOptions.EndPoints.Any());
		}

		[TestMethod]
		public async Task Parse_ShouldWorkProperly()
		{
			await Task.CompletedTask;

			var configurationOptions = ConfigurationOptions.Parse("localhost");
			configurationOptions.AllowAdmin = true;
			configurationOptions.Ssl = true;

			var value = configurationOptions.ToString();

			configurationOptions = new ConfigurationOptionsParser().Parse(value);

			Assert.AreEqual(value, configurationOptions.ToString());
		}

		#endregion
	}
}