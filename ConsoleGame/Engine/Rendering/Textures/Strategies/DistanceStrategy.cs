namespace ConsoleGame.Engine.Rendering.Textures.Strategies;

public class DistanceStrategy : BaseStrategy, ITextureStrategy
{
	public double MinValue { get; set; }
	public double MaxValue { get; set; }
	public bool Reverse { get; set; }

	public double Value
	{
		get => _value;
		set
		{
			if (value < MinValue || MaxValue < value)
				throw new ArgumentOutOfRangeException();

			_value = value;
		}
	}
	private double _value;

	public DistanceStrategy(double minValue, double value, double maxValue, bool reverse = false)
	{
		MinValue = minValue;
		Value = value;
		MaxValue = maxValue;
		Reverse = reverse;
	}

	public char ChooseTexture(char[] textures)
	{
		validateTextures(textures);

		double value = Value - MinValue;
		double maxValue = MaxValue - MinValue;
		// minValue = 0


		int index = (int)(textures.Length * (value / maxValue));
		if (Reverse) index = textures.Length - 1 - index;

		return textures[index];
	}
}
