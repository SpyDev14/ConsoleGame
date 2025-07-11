namespace ConsoleGame.Rendering.Textures.Strategies;

public class DistanceStrategy(double minValue, double value, double maxValue, bool reverse = false) : ITextureChoiseStrategy
{
	public char? ChooseTexture(char[] textures)
	{
		if (textures.Length < 1)
			return null;
		//if (value < minValue || maxValue < value)
		//	throw new ArgumentOutOfRangeException();


		double valuesRange = maxValue - minValue;
		if (valuesRange < 0)
			valuesRange += valuesRange * 2;

		int index = (int)Math.Clamp(value * textures.Length / valuesRange, 0, valuesRange);
		if (reverse) index = (textures.Length - 1) - index;

		return textures[Math.Clamp(index, 0, textures.Length-1)];
	}
}
