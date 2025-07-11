using ConsoleGame.Engine.Rendering.Textures.Strategies;

namespace ConsoleGame.Engine.Rendering.Textures;

public class Texture
{
	private char[] _textures;
	public ITextureStrategy? Strategy { get; set; }

	public Texture(char[] textures, ITextureStrategy? strategy = null)
	{
		if (textures.Length < 1)
			throw new ArgumentException();

		_textures = textures;
		Strategy = strategy;
	}

	public char GetTexture(ITextureStrategy? strategy = null)
	{
		if (Strategy != null)
			return Strategy.ChooseTexture(_textures);

		if (strategy != null)
			return strategy.ChooseTexture(_textures);

		return _textures[0];
	}
}
