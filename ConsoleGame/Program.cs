using System.Numerics;

namespace ConsoleGame
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			Init();


			(int width, int height) screenSize = (Console.WindowWidth, Console.WindowHeight);


			char[] visibleObjectTextureGrade = "█▓▒░ ".ToCharArray();
			char[] floorTextureGrade = ".,:*".ToCharArray();
			char[] visibleObjects = ['#'];
			string[] gameMap = [
				"##########",
				"#     #  #",
				"#     P# #",
                "#        #",
                "###      #",
                "#   #    #",
                "##########",
			];
			bool[,] visibleObjectsMap = MakeVisibleObjectsMap(gameMap, visibleObjects);

			char[] buffer = new string(' ', screenSize.width * screenSize.height).ToCharArray();


			var playerOnMapPosition = FindPlayerPosition(gameMap);
			var player = new Player() {
				Position = new Vector2(playerOnMapPosition.X, playerOnMapPosition.Y),
			};

			double drawningDistance = 7;

			int horizontHeightPosition = screenSize.height / 2;
			int wallHeight = (screenSize.height / 4) * 2; // Чтобы было понятнее


			Task.Run(() => DrawBufferProcces(buffer, screenSize));
			Task.Run(() => PlayerController(player));

			while (true)
			{
				var infoStr = $"X: {player.Position.X}; Y: {player.Position.Y}; Rotate: {Math.Round(player.Rotation, 5)}     ";
				for (int i = 0; i < infoStr.Length; i++)
					buffer[i] = infoStr[i];

				for (int x = 0;  x < screenSize.width; x++)
				{
					double rayDirection = player.Rotation + player.FOV / 2 - x * player.FOV / screenSize.width;


					(double X, double Y) rayPos = (
						Math.Sin(rayDirection),
						Math.Cos(rayDirection)
					);

					double traveledDistance = 0;
					bool hit = false;
					const double rayStep = 0.1;

					while (!hit && traveledDistance < drawningDistance)
					{
						traveledDistance += rayStep;

						(int X, int Y) checkPos = (
							(int)(player.Position.X + rayPos.X * traveledDistance),
							(int)(player.Position.Y + rayPos.Y * traveledDistance)
						);

						hit = visibleObjectsMap[checkPos.Y, checkPos.X];
					}
					double deltaAngle = rayDirection - player.Rotation;
					//traveledDistance = traveledDistance * Math.Cos(deltaAngle);

					if (traveledDistance < 1)
						traveledDistance = 1;


					int textureIndex = (int)(traveledDistance / drawningDistance * visibleObjectTextureGrade.Length);
					textureIndex = Math.Clamp(textureIndex, 0, visibleObjectTextureGrade.Length - 1);

					char wallTexture = visibleObjectTextureGrade[textureIndex];

					var wallStr = new string(wallTexture, (int)Math.Round(wallHeight / traveledDistance));
					wallStr = wallStr + new string(' ', wallHeight - wallStr.Length);

					//int floorHeight = screenSize.height - horizontHeightPosition;
					//for (int y = horizontHeightPosition;  y < screenSize.height; y++)
					//{
					//	int floorTextureIndex = (int)((y - floorHeight) / (screenSize.height - floorHeight) * floorTextureGrade.Length);

						
					//}
					for (int i = 0; i < wallStr.Length; i++)
					{
						buffer[screenSize.width * (horizontHeightPosition - i) + x] = wallStr[i];
						buffer[screenSize.width * (horizontHeightPosition-1 + i) + x] = wallStr[i];
                    }
				}
			}
		}

		private static (int Y, int X) FindPlayerPosition(string[] gameMap)
		{
			for (int row = 0; row < gameMap.Length; row++)
			{
				int column = gameMap[row].IndexOf('P');
				if (column != -1)
					return (row, column);
			}
			return (-1, -1); // Если 'P' не найден
		}

		private static bool[,] MakeVisibleObjectsMap(string[] gameMap, char[] visibleObjects)
		{
			bool[,] visibleObjectsMap = new bool[gameMap.Length, gameMap[0].Length];
			for (int i = 0; i < gameMap.Length; i++)
				for (int j = 0; j < gameMap[0].Length; j++)
					visibleObjectsMap[i, j] = visibleObjects.Contains(gameMap[i][j]);

			return visibleObjectsMap;
		}

		private static void Init()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.InputEncoding = System.Text.Encoding.UTF8;
		}

		private static void PlayerController(Player player)
		{
			// мне похуй, я поржать
			while (true)
			{
				var key = Console.ReadKey(true).Key;

				switch (key)
				{
					case ConsoleKey.W:
						player.MoveStraight();
						break;

					case ConsoleKey.S:
						player.MoveStraight(-1);
						break;

					case ConsoleKey.A:
						player.MoveSideway();
						break;

					case ConsoleKey.D:
						player.MoveSideway(-1);
						break;

					case ConsoleKey.LeftArrow:
						player.Rotate();
						break;

					case ConsoleKey.RightArrow:
						player.Rotate(-1);
						break;
				}
			}
		}

		private static async Task DrawBufferProcces(char[] buffer, (int width, int height) screenSize)
		{
			while (true)
			{
				Console.SetCursorPosition(0, 0);

				if (Console.CursorVisible)
					Console.CursorVisible = false;

				if (Console.WindowWidth != screenSize.width)
					Console.WindowWidth = screenSize.width;

				Console.Write(buffer);

				await Task.Delay(20);
			}
		}
	}
}
