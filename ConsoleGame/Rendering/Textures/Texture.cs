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

	public char GetTexture(ITextureChoiseStrategy? strategy = null)
	{
		char? texture = null;
		if (_strategy != null)
			texture = _strategy.ChooseTexture(_textures);
		else if (strategy != null)
			texture = strategy.ChooseTexture(_textures);

		if (texture != null)
			return (char)texture;

		return _textures[0];
	}
}
