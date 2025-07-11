namespace ConsoleGame.Engine.Tools;

public class Angle
{
	private const double TwoPI = Math.PI * 2;
	private double _radians;
	public double Radians
	{
		get => _radians;
		set => _radians = NormalizeRadians(value);
	}

	public double Degrees
	{
		get => RadiansToDegrees(Radians);
		set => Radians = DegreesToRadians(value);
	}

	public Angle(double degrees)
		=> Degrees = degrees;

	public static double RadiansToDegrees(double radians) => 180 / Math.PI * radians;
	public static double DegreesToRadians(double degrees) => Math.PI / 180 * degrees;


	public static double NormalizeDegrees(double degrees)
	{
		degrees %= 360;
		return degrees < 0 ? degrees + 360 : degrees;
	}

	public static double NormalizeRadians(double radians)
	{
		radians %= TwoPI;
		return radians < 0 ? radians + TwoPI : radians;
	}

	public override string ToString() => $"{Degrees:F1}° ({Radians:F4}r)";
}
