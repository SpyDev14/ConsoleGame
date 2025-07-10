using ConsoleGame.Rendering.Textures.Strategies;

namespace ConsoleGame.Rendering.Textures;

public class Texture
{
	private char[] _textures;
	private ITextureChoiseStrategy? _strategy;

	public Texture(char[] textures, ITextureChoiseStrategy? strategy = null)
	{
		if (textures.Length < 1)
			throw new ArgumentException();

		_textures = textures;
		_strategy = strategy;
	}

	public char? GetTexture(ITextureChoiseStrategy? strategy = null)
	{
		if (_strategy != null)
			return _strategy.ChooseTexture(_textures);
		if (strategy != null)
			return strategy.ChooseTexture(_textures);

		return _textures[0];
	}
}
