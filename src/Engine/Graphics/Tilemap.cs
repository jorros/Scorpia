using System.Collections.Generic;
using System.Drawing;

namespace Scorpia.Engine.Graphics;

public class Tilemap
{
    private readonly int _width;
    private readonly int _height;
    private readonly Size _size;
    private readonly TilemapOrientation _orientation;
    private readonly List<TilemapLayer> _layers;

    public Tilemap(int width, int height, Size size, TilemapOrientation orientation)
    {
        _width = width;
        _height = height;
        _size = size;
        _orientation = orientation;
        _layers = new List<TilemapLayer>();
    }

    public TilemapLayer AddLayer()
    {
        var layer = new TilemapLayer(_width, _height, _size, _orientation);
        _layers.Add(layer);

        return layer;
    }

    public Size GetSize()
    {
        return new Size(_width * _size.Width + _size.Width / 2, _height * _size.Height);
    }
    
    public void Render(RenderContext renderContext)
    {
        foreach (var layer in _layers)
        {
            layer.Render(renderContext);
        }
    }
}