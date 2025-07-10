
namespace ConsoleGame.Tools;

public class Angle
{
	private double _value; //Radians
	public double Radians => _value;

	public double Degrees
	{
		get => RadiansToDegrees(_value);
		set => _value = DegreesToRadians(value);
	}

	public Angle(double degrees = 0)
		=> Degrees = degrees;

	public static Angle FromRadians(double radians)
		=> new Angle(RadiansToDegrees(radians));

	public double GetNormalizedDegrees() => NormalizeDegrees(Degrees);
	public static double RadiansToDegrees(double radians)
		=> 180 / Math.PI * radians;
	public static double DegreesToRadians(double degrees)
		=> Math.PI / 180 * degrees;
	public static double NormalizeDegrees(double degrees)
	{
		degrees %= 360;
		if (degrees < 0)
			degrees = (degrees + 360 * 2) % 360;

		return degrees;
	}
}
