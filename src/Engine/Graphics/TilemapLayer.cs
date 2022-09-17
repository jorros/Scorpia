using Scorpia.Engine.Asset;

namespace Scorpia.Engine.Graphics;

public class TilemapLayer
{
    private readonly int _width;
    private readonly int _height;
    private readonly int _tileWidth;
    private readonly int _tileHeight;
    private readonly Sprite[] _tiles;
    
    internal TilemapLayer(int width, int height, int tileWidth, int tileHeight)
    {
        _width = width;
        _height = height;
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
        _tiles = new Sprite[width * height];
    }

    public void SetTile(OffsetVector position, Sprite tile)
    {
        _tiles[position.Y * _width + position.X] = tile;
    }

    public void SetTile(CubeVector position, Sprite tile)
    {
        SetTile(position.ToOffset(), tile);
    }

    public Sprite GetTile(OffsetVector position)
    {
        return _tiles[position.Y * _width + position.X];
    }

    public Sprite GetTile(CubeVector position)
    {
        return GetTile(position.ToOffset());
    }
    
    public void Render(RenderContext renderContext)
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var sprite = GetTile(new OffsetVector(x, y));
                if (sprite is null)
                {
                    continue;
                }
                
                var position = new OffsetVector(x * _tileWidth, y * _tileHeight);
                
                renderContext.Draw(sprite, position);
            }
        }
    }
}