using Scorpia.Engine.Asset;

namespace Scorpia.Engine.Graphics;

public class TilemapLayer
{
    private readonly int _width;
    private readonly int _height;
    private readonly OffsetVector _size;
    private readonly TilemapOrientationMatrix _orientationMatrix;
    private readonly Sprite[] _tiles;
    
    internal TilemapLayer(int width, int height, OffsetVector size, TilemapOrientationMatrix orientationMatrix)
    {
        _width = width;
        _height = height;
        _size = size;
        _orientationMatrix = orientationMatrix;
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

    private OffsetVector HexToScreen(OffsetVector pos)
    {
        var h = pos.ToCube();
        
        var x = (_orientationMatrix.f0 * pos.X + _orientationMatrix.f1 * pos.Y) * _size.X;
        var y = (_orientationMatrix.f2 * pos.X + _orientationMatrix.f3 * pos.Y) * _size.Y;
        return new OffsetVector((int)x, (int)y);
    }
    
    public void Render(RenderContext renderContext)
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var pos = new OffsetVector(x, y);
                var sprite = GetTile(pos);
                if (sprite is null)
                {
                    continue;
                }
                
                var position = HexToScreen(pos);
                renderContext.Draw(sprite, position);
            }
        }
    }
}