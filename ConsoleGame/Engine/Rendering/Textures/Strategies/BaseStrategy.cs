namespace ConsoleGame.Engine.Rendering.Textures.Strategies;

public abstract class BaseStrategy()
{
	protected void validateTextures(char[] textures)
	{
		if (textures.Length < 1)
			throw new ArgumentOutOfRangeException("Textures cannot be empty.");
	}
}
