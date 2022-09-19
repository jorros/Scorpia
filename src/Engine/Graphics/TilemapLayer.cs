using System;
using System.Drawing;
using System.Numerics;
using Scorpia.Engine.Asset;

namespace Scorpia.Engine.Graphics;

public class TilemapLayer
{
    private readonly int _width;
    private readonly int _height;
    private readonly Size _size;
    private readonly Matrix3x2 _matrix;
    private readonly Sprite[] _tiles;

    private static readonly Matrix3x2 PointyOrientation = new((float) Math.Sqrt(3.0), 0.0f, (float) Math.Sqrt(3.0) / 2.0f,
        3.0f / 2.0f, 0, 0);
    private static readonly Matrix3x2 FlatOrientation = new(3.0f / 2.0f, (float)Math.Sqrt(3.0) / 2.0f, 0.0f,
        (float)Math.Sqrt(3.0), 0, 0);

    internal TilemapLayer(int width, int height, Size size, TilemapOrientation orientation)
    {
        _width = width;
        _height = height;
        _size = size;
        _tiles = new Sprite[width * height];

        _matrix = orientation == TilemapOrientation.Flat ? FlatOrientation : PointyOrientation;
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
        var result = Vector2.Transform(new Vector2(h.Q, h.R), _matrix);
        result *= new Vector2(_size.Width, _size.Height);

        return new PointF(result.X + _size.Width / 2f, result.Y + _size.Height / 2f);
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