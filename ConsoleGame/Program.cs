using System.Diagnostics;

namespace ConsoleGame
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			ConsoleInit();

			Stopwatch stopwatch = new();
			int framesCount = 0;
			float fps = 0;

			while (true)
			{
				stopwatch.Start();

				framesCount++;

				Thread.Sleep(1);
				

				if (stopwatch.ElapsedMilliseconds >= 1000)
				{
					fps = framesCount / (stopwatch.ElapsedMilliseconds / 1000);
					framesCount = 0;
					Console.Write($"FPS: {fps}");
					Console.SetCursorPosition(0, 0);
					stopwatch.Reset();
				}
			}
		}

		static void ConsoleInit()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.InputEncoding = System.Text.Encoding.UTF8;

			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.Clear();
		}
	}
}
