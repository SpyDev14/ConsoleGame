using ConsoleGame.Tools;

namespace ConsoleGame;

public class Player(double y = 0, double x = 0, double fovDegrees = 75)
{
	public double StepSize { get; } = 0.05;
	public double RotateStepSize { get; } = 3;

	public (double Y, double X) Position { get; set; } = (y, x);

	public Angle FOV { get; set; } = new(fovDegrees);
	public Angle Rotation { get; set; } = new();


	public void MoveStraight(bool reverse = false)
	{
		double distance = StepSize * (reverse ? -1 : 1);
		Position = (
			Position.Y + distance * Math.Cos(Rotation.Radians),
			Position.X + distance * Math.Sin(Rotation.Radians)
		);
	}

	public void MoveSideway(bool reverse = false)
	{
		double distance = StepSize * (reverse ? -1 : 1);
		Angle angle = new(Rotation.Degrees + 90);

		Position = (
			Position.Y + distance * Math.Cos(angle.Radians),
			Position.X + distance * Math.Sin(angle.Radians)
		);
	}

	public void Rotate(bool reverse = false)
	{
		Rotation.Degrees += RotateStepSize * (reverse ? -1 : 1);
	}
}
