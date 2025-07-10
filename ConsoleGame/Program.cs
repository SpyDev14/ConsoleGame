using ConsoleGame.Rendering.Textures;
using ConsoleGame.Rendering.Textures.Strategies;
using ConsoleGame.Tools;
using System.Diagnostics;
using System.Text;

namespace ConsoleGame
{
	internal static class Program
	{
		static readonly char[] visibleObjects = ['#'];
		static int objectsHeightProportion = 4 / 4;
		static (int width, int height) screenSize;
		static int horizontHeightPosition;
		static Texture floorTexture;
		static Texture objectTexture;
		static char[] buffer;
		static Player player;
		static GameMap gameMap;
		static double framerate;


		public static void Main(string[] args)
		{
			ConsoleInit();

			screenSize = (Console.WindowWidth, Console.WindowHeight);
			horizontHeightPosition = screenSize.height / 2;
			floorTexture = new("*:.".ToCharArray());
			objectTexture = new("█▓▒░: ".ToCharArray());
			buffer = new string(' ', screenSize.width * screenSize.height).ToCharArray();
			gameMap = new([
				"##########",
				"###      #",
				"##    #  #",
				"#        #",
				"##   P   #",
				"##       #",
				"###      #",
				"#        #",
				"#ХУЙ     #",
				"##########",
			]);
			var playerPos = gameMap.FindObjectPosition('P');
			player = new(playerPos.Y + 0.5, playerPos.X + 0.5);


			Texture noiseTexture = new("▓▒▒░░".ToCharArray(), new RandomStrategy());
			Random rnd = new();

			Task.Run(DrawBufferProcces);
			Task.Run(() =>
				{
					while (true)
					{
						//buffer[rnd.Next(0, buffer.Length)] = noiseTexture.GetTexture();
						WriteInBufferObject(gameMap.Map, 0, 0);
						
						WriteInBuffer("═══════════════╗", 0, screenSize.height - 3);
						for (int i = 2; i > 0; i--)
							WriteInBuffer("║", 15, screenSize.height - i);

						WriteInBufferObject(
							[
								$"X: {player.Position.X} | Y: {player.Position.Y}",
								$"Rotation: {Math.Round(player.Rotation.GetNormalizedDegrees(), 1)} "
							], 0, screenSize.height - 2
						);


						WriteInBuffer("┼", screenSize.width / 2 - 1, screenSize.height / 2);
					}
				}
			);
			Task.Run(async () => {
				while (true)
				{
					player.Rotation.Degrees += 1;
					await Task.Delay(10);
				}
			});


			Task changeTitleTask = new Task(() => {});


			int frames = 0;
			double lastTime = 0;
			Stopwatch timer = Stopwatch.StartNew();

			StringBuilder sb = new();

			char[] rawFloorLine = new char[screenSize.height - horizontHeightPosition];
			for (int i = 0; i < rawFloorLine.Length; i++)
			{
				rawFloorLine[i] = floorTexture.GetTexture(
					new DistanceStrategy(0, i, rawFloorLine.Length, reverse: true)
				);
			}

			string floorLine = rawFloorLine.ToString();

			while (true)
			{
				frames++;
				double currentTime = timer.Elapsed.TotalSeconds;
				double elapsed = currentTime - lastTime;

				if (elapsed >= 1.0)
				{
					framerate = Math.Round(frames / elapsed);
					frames = 0;
					lastTime = currentTime;
				}

				if (changeTitleTask.IsCompleted)
					changeTitleTask = Task.Run(() => { Console.Title = $"Console Engine | FPS: {framerate}"; });



				for (int x = 0;  x < screenSize.width; x++)
				{
					double rayAngle =
						player.Rotation.Radians
						+ player.FOV.Radians / 2
						- x * player.FOV.Radians / screenSize.width;

					(double Y, double X) rayPosChangeFactor = (
						Math.Cos(rayAngle),
						Math.Sin(rayAngle)
					);

					double traveledDistance = 0;
					bool hit = false;
					const double rayStepSize = 0.01;
					const double renderingDistance = 7;

					while (!hit && traveledDistance <= renderingDistance)
					{
						traveledDistance += rayStepSize;

						(int Y, int X) checkPos = (
							(int)(player.Position.Y + rayPosChangeFactor.Y * traveledDistance),
							(int)(player.Position.X + rayPosChangeFactor.X * traveledDistance)
						);

						if (visibleObjects.Contains(gameMap.Map[checkPos.Y, checkPos.X]))
							hit = visibleObjects.Contains(gameMap.Map[checkPos.Y, checkPos.X]);
						else
							WriteInBuffer("*", checkPos.X, checkPos.Y);


					}
					int maxObjectSize = screenSize.height / objectsHeightProportion;
					int objectSize = (int)(maxObjectSize * (1 - traveledDistance / renderingDistance));
					char selectedObjectTexture = objectTexture.GetTexture(
						new DistanceStrategy(0, traveledDistance, renderingDistance)
					);


					sb.Append(new string(' ', (maxObjectSize - objectSize) / 2));
					sb.Append(new string(selectedObjectTexture, objectSize));
					//string str = floorLine[(screenSize.height - sb.Length - horizontHeightPosition)..];
					//sb.Append(str);
					sb.Append(new string(' ', (maxObjectSize - objectSize) / 2));

					WriteInBufferVertical(sb.ToString(), x, 0);

					sb.Clear();
				}
			}
		}

		static void ConsoleInit()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.InputEncoding = System.Text.Encoding.UTF8;
		}

		private static void WriteInBuffer(string text, int pos, int line)
		{
			for (int i = 0; i < text.Length; i++)
				buffer[screenSize.width * line + pos + i] = text[i];
		}
		private static void WriteInBufferVertical(string text, int pos, int line)
		{
			for (int i = 0; i < text.Length; i++)
				buffer[screenSize.width * (line + i) + pos] = text[i];
		}
		private static void WriteInBufferObject(char[,] strings, int pos, int line)
		{
			for (int i = 0; i < strings.GetLength(0); i++)
				for (int j = 0; j < strings.GetLength(1); j++)
					buffer[screenSize.width * (line + i) + pos + j] = strings[i, j];
		}
		private static void WriteInBufferObject(string[] strings, int pos, int line)
		{
			for (int i = 0; i < strings.Length; i++)
				WriteInBuffer(strings[i], pos, line + i);
		}

		private static async Task DrawBufferProcces()
		{
			while (true)
			{
				Console.SetCursorPosition(0, 0);

				if (Console.CursorVisible)
					Console.CursorVisible = false;

				if (Console.WindowWidth != screenSize.width)
				{
					Console.WindowWidth = screenSize.width;
					Console.BufferWidth = screenSize.width;
				}
				if (Console.WindowHeight != screenSize.height)
				{
					Console.WindowHeight = screenSize.height;
				}

				if (Console.WindowWidth == screenSize.width)
					Console.Write(buffer);

				await Task.Delay(20);
			}
		}
	}
}
