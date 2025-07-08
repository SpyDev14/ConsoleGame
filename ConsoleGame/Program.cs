using System.Numerics;

namespace ConsoleGame
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			Init();


			(int width, int height) screenSize = (Console.WindowWidth, Console.WindowHeight);

			char[] visibleObjects = ['#'];
			string[] gameMap = [
				"##########",
				"#        #",
				"#     P  #",
				"#        #",
				"#        #",
				"##########",
			];
			bool[,] visibleObjectsMap = MakeVisibleObjectsMap(gameMap, visibleObjects);

			char[] buffer = new string('░', screenSize.width * screenSize.height).ToCharArray();


			var playerOnMapPosition = FindPlayerPosition(gameMap);
			var player = new Player() {Position = new Vector2(playerOnMapPosition.X, playerOnMapPosition.Y)};

			double drawningDistance = 7;

			int horizontHeight = screenSize.height / 2;
			int wallHeight = (screenSize.height / 4) * 2; // Чтобы было понятнее


			var drawningProccess = Task.Run(() => DrawBufferProcces(buffer, screenSize));

			while (true)
			{
				//var key = Console.ReadKey(true);

				double[] distances = new double[screenSize.width];

				for (int x = 0;  x < screenSize.width; x++)
				{
					double rayDirection = player.Rotate + player.FOV / 2 - x * player.FOV / 2;

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

						if (checkPos.X < 0 || checkPos.Y < 0 ||
							checkPos.X >= drawningDistance + player.Position.X || // Хз зачем
							checkPos.Y >= drawningDistance + player.Position.Y)   // Хз зачем
						{
							hit = true;
							traveledDistance = drawningDistance;
							break;
						}

						hit = visibleObjectsMap[checkPos.Y, checkPos.X];
					}

					distances[x] = traveledDistance;

					//....
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
