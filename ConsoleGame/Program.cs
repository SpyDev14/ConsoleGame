using ConsoleGame.Rendering.Textures;
using ConsoleGame.Tools;

namespace ConsoleGame
{
	internal static class Program
	{
		static char[] visibleObjects = ['#'];
		static int objectsHeightProportion = 2 / 4;
		static (int width, int height) screenSize;
		static int horizontHeightPosition;
		static Texture floorTexture;
		static Texture objectTexture;
		static char[] buffer;
		static Player player;
		static GameMap gameMap;


		public static void Main(string[] args)
		{
			Init();

			Task.Run(DrawBufferProcces);


			while (true)
            {
				WriteInBuffer("HELLO", 10, 10);

                WriteInBufferVertical("HELLO", 10, 11);

				WriteInBufferObject(gameMap.Map, 0, 0);
            }
		}

		static void Init()
		{
			screenSize = (Console.WindowWidth, Console.WindowHeight);
			horizontHeightPosition = screenSize.height / 2;
			floorTexture = new(".:*".ToCharArray());
			objectTexture = new(" ░▒▓█".ToCharArray());
			buffer = new string('.', screenSize.width * screenSize.height).ToCharArray();
			gameMap = new([
				"##########",
				"#        #",
				"#     P  #",
				"#        #",
				"#        #",
				"#        #",
				"#        #",
				"#        #",
				"#        #",
				"##########",
			]);
			var playerPos = gameMap.FindObjectPosition('P');
			player = new(playerPos.Y, playerPos.X);
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
				WriteInBuffer(strings[i], pos, line * i);
        }

        private static async Task DrawBufferProcces()
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
