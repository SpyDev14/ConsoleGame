using ConsoleGame.Rendering.Textures;
using ConsoleGame.Rendering.Textures.Strategies;
using ConsoleGame.Tools;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleGame
{
	internal static class Program
	{
		static readonly char[] visibleObjects = ['#'];
		static int objectsHeightProportion = 4 / 4;
		static (int width, int height) screenSize;
		static int horizontHeightPosition;
		static char[] buffer = [];
		static double framerate;


		public static void Main(string[] args)
		{
			ConsoleInit();

			bool fixFishEyes = true;
			bool fovNowSetting = false;

			screenSize = (Console.WindowWidth, Console.WindowHeight);
			horizontHeightPosition = screenSize.height / 2;
			Texture floorTexture = new(",.".ToCharArray());
			Texture objectTexture = new("█▓▒░:   ".ToCharArray());
			buffer = new string(' ', screenSize.width * screenSize.height).ToCharArray();
			GameMap gameMap = new([
			// "##########",
			// "###      #",
			// "##    #  #",
			// "#        #",
			// "##   P#  #",
			// "##       #",
			// "###      #",
			// "#        #",
			// "#ХУЙ     #",
			// "##########",
				"##################",
				"#  #       #     #",
				"# ###  #   #     #",
				"# P#       ### ###",
				"#  #  #          #",
				"#  ##            #",
				"#  ####          #",
				"#  ###   ### ### #",
				"##     ######    #",
				"##################"
			]);
			var playerPos = gameMap.FindObjectPosition('P');
			gameMap.Map[playerPos.Y, playerPos.X] = ' ';
			Player player = new(playerPos.Y + 0.5, playerPos.X + 0.5);


			Texture noiseTexture = new("▓▒▒░░".ToCharArray(), new RandomStrategy());
			Random rnd = new();

			Task.Run(DrawBufferProcces);
			Task.Run(() =>
				{
					while (true)
					{
						//buffer[rnd.Next(0, buffer.Length)] = noiseTexture.GetTexture();

						WriteInBuffer("P", (int)player.Position.X, (int)player.Position.Y);
						WriteInBufferObject(gameMap.Map, 0, 0);
						WriteInBuffer("P", (int)player.Position.X, (int)player.Position.Y);

						WriteInBuffer("══════════════════╗", 0, screenSize.height - 4);
						for (int i = 3; i > 0; i--)
							WriteInBuffer("║", 18, screenSize.height - i);

						WriteInBufferObject(
							[
								$"Rotation: {Math.Round(player.Rotation.GetNormalizedDegrees(), 1)} ",
								$"X: {Math.Round(player.Position.X, 1)} | Y: {Math.Round(player.Position.Y, 1)}",
								$"FPS: {framerate} | FOV: {player.FOV.Degrees}",
							], 0, screenSize.height - 3
						);


						WriteInBuffer("┼", screenSize.width / 2 - 1, screenSize.height / 2);
						WriteInBuffer($"{(fixFishEyes ? "Human":"Fish")} eyes selected", 0, gameMap.Map.GetLength(0));
						WriteInBuffer("P", (int)player.Position.X, (int)player.Position.Y);
					}
				}
			);
			Task.Run(async () =>
			{
				while (true)
				{
					ConsoleKey key = Console.ReadKey(true).Key;

					switch (key)
					{
						case ConsoleKey.W:
						case ConsoleKey.UpArrow:
							player.MoveStraight();
							break;
						case ConsoleKey.S:
						case ConsoleKey.DownArrow:
							player.MoveStraight(reverse: true);
							break;
						case ConsoleKey.A:
							player.MoveSideway();
							break;
						case ConsoleKey.D:
							player.MoveSideway(reverse: true);
							break;
						case ConsoleKey.LeftArrow:
							player.Rotate();
							break;
						case ConsoleKey.RightArrow:
							player.Rotate(reverse: true);
							break;
						case ConsoleKey.F:
							fixFishEyes = !fixFishEyes;
							break;
						case ConsoleKey.R:
							fovNowSetting = true;
							var input = Console.ReadLine();
							bool success = int.TryParse(input, out int value);
							player.FOV.Degrees = success ? value : player.FOV.Degrees;
							fovNowSetting = true;
							break;
					}

					await Task.Delay(1);
				}
			});


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

			string floorLine = new(rawFloorLine);

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

				for (int x = 0; x < screenSize.width; x++)
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
					const double renderingDistance = 14;

					while (!hit && traveledDistance <= renderingDistance)
					{
						traveledDistance += rayStepSize;

						(int Y, int X) checkPos = (
							(int)(player.Position.Y + rayPosChangeFactor.Y * traveledDistance),
							(int)(player.Position.X + rayPosChangeFactor.X * traveledDistance)
						);

						if (visibleObjects.Contains(gameMap.Map[checkPos.Y, checkPos.X]))
							hit = visibleObjects.Contains(gameMap.Map[checkPos.Y, checkPos.X]);
						// else
							// WriteInBuffer("*", checkPos.X, checkPos.Y);
					}

					if (fixFishEyes)
						traveledDistance *= Math.Cos(rayAngle - player.Rotation.Radians);

					int maxObjectSize = screenSize.height / objectsHeightProportion;
					int objectSize = (int)(maxObjectSize * (1 - traveledDistance / renderingDistance));
					char selectedObjectTexture = objectTexture.GetTexture(
						new DistanceStrategy(0, traveledDistance, renderingDistance)
					);


					sb.Append(new string(' ', (maxObjectSize - objectSize) / 2));
					sb.Append(new string(selectedObjectTexture, objectSize));

					//string str = floorLine[(screenSize.height - sb.Length - horizontHeightPosition)..];
					sb.Append(floorLine[^Math.Min((maxObjectSize - objectSize) / 2, floorLine.Length)..]);
					WriteInBufferVertical(
						sb.Length > screenSize.height ?
							sb.ToString()[..screenSize.height] :
							sb.ToString(),
						x, 0
					);

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
