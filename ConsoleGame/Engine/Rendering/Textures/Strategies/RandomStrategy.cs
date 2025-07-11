namespace ConsoleGame.Engine.Rendering.Textures.Strategies;

public class RandomStrategy : BaseStrategy, ITextureStrategy
{
	private Random _rnd = new();
	public char ChooseTexture(char[] textures)
	{
		validateTextures(textures);

		return textures[_rnd.Next(0, textures.Length)];
	}
}
