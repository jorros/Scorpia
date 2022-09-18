using System;
using System.Drawing;
using Scorpia.Engine.Asset;

namespace Scorpia.Engine.Graphics;

public class TilemapLayer
{
    private readonly int _width;
    private readonly int _height;
    private readonly Size _size;
    private readonly TilemapOrientationMatrix _orientationMatrix;
    private readonly Sprite[] _tiles;
    
    internal TilemapLayer(int width, int height, Size size, TilemapOrientationMatrix orientationMatrix)
    {
        _width = width;
        _height = height;
        _size = size;
        _orientationMatrix = orientationMatrix;
        _tiles = new Sprite[width * height];
    }

    public void SetTile(Point position, Sprite tile)
    {
        _tiles[position.Y * _width + position.X] = tile;
    }

    public void SetTile(Hex position, Sprite tile)
    {
        SetTile(position.ToPoint(), tile);
    }

    public Sprite GetTile(Point position)
    {
        return _tiles[position.Y * _width + position.X];
    }

    public Sprite GetTile(Hex position)
    {
        return GetTile(position.ToPoint());
    }

    private PointF HexToScreen(Hex h)
    {
        var x = (_orientationMatrix.f0 * h.Q + _orientationMatrix.f1 * h.R) * _size.Width;
        var y = (_orientationMatrix.f2 * h.Q + _orientationMatrix.f3 * h.R) * _size.Height;
        return new PointF(x + _size.Width / 2f, y + _size.Height / 2f);
    }
    
    public void Render(RenderContext renderContext)
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var pos = new Point(x, y);
                var sprite = GetTile(pos);
                if (sprite is null)
                {
                    continue;
                }

                var q = x - (y >> 1);
                
                var position = HexToScreen(new Hex(q, y, 0));
                renderContext.Draw(sprite, position);
            }
        }
    }
}