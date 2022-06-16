using System.Diagnostics.CodeAnalysis;
using StackExchange.Redis;

namespace Shared
{
	/// <summary>
	/// This code is from: [Creating a Very Simple Console Chat App using C# and Redis Pub/Sub](https://www.codeproject.com/Articles/1222027/Creating-a-Very-Simple-Console-Chat-App-using-Csha)
	/// </summary>
	public static class Program
	{
		#region Methods

		[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
		private static void OnMessageReceived(string message)
		{
			var initialCursorTop = Console.CursorTop;
			var initialCursorLeft = Console.CursorLeft;

			Console.MoveBufferArea(0, initialCursorTop, Console.WindowWidth, 1, 0, initialCursorTop + 1);
			Console.CursorTop = initialCursorTop;
			Console.CursorLeft = 0;

			WriteReceivedMessage(message);

			Console.CursorTop = initialCursorTop + 1;
			Console.CursorLeft = initialCursorLeft;
		}

		private static void ResetColor()
		{
			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.Cyan;
		}

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		public static void Start(string title)
		{
			Console.Title = title;
			Console.ForegroundColor = ConsoleColor.Cyan;

			const string channelName = "Channel";
			// You need to start redis: docker run --rm -it -p 6379:6379 redis
			const string connectionString = "localhost:6379";

			try
			{
				var connection = ConnectionMultiplexer.Connect(connectionString);

				Console.Write("Enter your name: ");
				var userName = Console.ReadLine();

				var subscriber = connection.GetSubscriber();

				subscriber.Subscribe(channelName, (_, value) =>
				{
					OnMessageReceived(value);
				});

				subscriber.Publish(channelName, $"\"{userName}\" joined the chat room.");

				while(true)
				{
					subscriber.Publish(channelName, $"{userName}: {Console.ReadLine()}  " + $"({DateTime.Now.ToString("HH:mm", null)})");
				}
			}
			catch(Exception exception)
			{
				WriteError(exception);
				Console.Read();
			}
		}

		private static void WriteError(object value)
		{
			WriteLine(ConsoleColor.Red, value);
		}

		private static void WriteLine(ConsoleColor color, object value)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(value);
			ResetColor();
		}

		private static void WriteReceivedMessage(object value)
		{
			WriteLine(ConsoleColor.Yellow, value);
		}

		#endregion
	}
}