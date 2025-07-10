namespace ConsoleGame.Rendering.Textures.Strategies;

public class RandomStrategy : ITextureChoiseStrategy
{
	private Random _rnd = new();
	public char? ChooseTexture(char[] textures)
	{
		if (textures.Length < 1) return null;
		
		return textures[_rnd.Next(0, textures.Length)];
	}
}
