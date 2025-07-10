using ConsoleGame.Tools;

namespace ConsoleGame;

public class Player
{
	public double StepSize { get; } = 0.1;
	public double RotateStepSize { get; } = 0.1;

	public (double Y, double X) Position { get; set; }

	public Angle FOV { get; set; } = new();
	public Angle Rotation { get; set; } = new();


	public Player(double y = 0, double x = 0)
		=> Position = (y, x);


	public void MoveStraight()
	{

	}

	public void MoveSideway()
	{

	}

	public void Rotate()
	{

	}
}
