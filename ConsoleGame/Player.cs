using ConsoleGame.Tools;

namespace ConsoleGame;

public class Player(double y = 0, double x = 0, double fovDegrees = 75)
{
	public double StepSize { get; } = 0.1;
	public double RotateStepSize { get; } = 0.1;

	public (double Y, double X) Position { get; set; } = (y, x);

	public Angle FOV { get; set; } = new(fovDegrees);
	public Angle Rotation { get; set; } = new();


	public void MoveStraight()
	{

	}

	public void MoveSideway()
	{

	}

	public void Rotate(double degrees = 5)
	{
		Rotation.Degrees += degrees;
	}
}
