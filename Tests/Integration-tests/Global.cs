using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Project;
using TestHelpers.Mocks.Logging;

namespace IntegrationTests
{
	// ReSharper disable All
	[TestClass]
	public static class Global
	{
		#region Fields

		private static IConfiguration _configuration;
		private static IHostEnvironment _hostEnvironment;
		public const string DefaultEnvironmentName = "Integration-test";
		public static readonly string ProjectDirectoryPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
		public static readonly string TemporaryTestDirectoryPath = Path.Combine(Path.GetTempPath(), $".{typeof(ConfigurationOptionsParser).Assembly.GetName().Name}.{typeof(Global).Namespace}-{Guid.NewGuid()}");

		#endregion

		#region Properties

		public static IConfiguration Configuration
		{
			get
			{
				return _configuration ??= CreateConfiguration("appsettings.json");
			}
		}

		public static IHostEnvironment HostEnvironment => _hostEnvironment ??= CreateHostEnvironment(DefaultEnvironmentName);

		#endregion

		#region Methods

		[AssemblyCleanup]
		public static async Task Cleanup()
		{
			await Task.CompletedTask;

			if(Directory.Exists(TemporaryTestDirectoryPath))
				Directory.Delete(TemporaryTestDirectoryPath, true);
		}

		public static IConfiguration CreateConfiguration(params string[] jsonFilePaths)
		{
			return CreateConfigurationBuilder(jsonFilePaths).Build();
		}

		public static IConfigurationBuilder CreateConfigurationBuilder(params string[] jsonFilePaths)
		{
			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.SetFileProvider(HostEnvironment.ContentRootFileProvider);

			foreach(var path in jsonFilePaths ?? Array.Empty<string>())
			{
				configurationBuilder.AddJsonFile(path, false, true);
			}

			return configurationBuilder;
		}

		public static IHostEnvironment CreateHostEnvironment(string environmentName)
		{
			return new HostingEnvironment
			{
				ApplicationName = typeof(Global).Assembly.GetName().Name,
				ContentRootFileProvider = new PhysicalFileProvider(ProjectDirectoryPath),
				ContentRootPath = ProjectDirectoryPath,
				EnvironmentName = environmentName
			};
		}

		public static IServiceCollection CreateServices()
		{
			return CreateServices(Configuration);
		}

		public static IServiceCollection CreateServices(IConfiguration configuration)
		{
			var services = new ServiceCollection();

			services.AddSingleton(configuration);
			services.AddSingleton(HostEnvironment);
			services.AddSingleton<ILoggerFactory, LoggerFactoryMock>();
			services.AddSingleton<LoggerFactory>();
			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
				loggingBuilder.AddConsole();
				loggingBuilder.AddDebug();
				loggingBuilder.AddEventSourceLogger();
				loggingBuilder.Configure(options => { options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId | ActivityTrackingOptions.ParentId; });
			});

			return services;
		}

		public static async Task<string> CreateTemporaryTestClassDirectory(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			await Task.CompletedTask;

			var temporaryTestClassDirectoryPath = Path.Combine(TemporaryTestDirectoryPath, $"{type.Name}");

			return temporaryTestClassDirectoryPath;
		}

		[AssemblyInitialize]
		public static async Task Initialize(TestContext _)
		{
			await Task.CompletedTask;

			Directory.CreateDirectory(TemporaryTestDirectoryPath);
		}

		#endregion
	}
	// ReSharper restore All
}