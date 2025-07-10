
namespace ConsoleGame;

public class GameMap
{
	public char[,] Map { get; set; }
    public GameMap(string[] blueprint)
	{
		Map = new char[blueprint.Length, blueprint[0].Length];

		for (int i = 0; i < blueprint.Length; i++)
			for (int j = 0; j < blueprint[0].Length; j++)
				Map[i, j] = blueprint[i][j];
	}

	public (int Y, int X) FindObjectPosition(char obj)
	{
		for (int i = 0; i < Map.GetLength(0); i++)
			for (int j = 0; j < Map.GetLength(1); j++)
				if (Map[i, j] == obj)
					return (i, j);

		return (-1, -1);
	}
}
