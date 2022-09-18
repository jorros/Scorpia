using System.Collections.Generic;
using System.Drawing;

namespace Scorpia.Engine.Graphics;

public class Tilemap
{
    private readonly int _width;
    private readonly int _height;
    private readonly Size _size;
    private readonly List<TilemapLayer> _layers;
    private readonly TilemapOrientationMatrix _orientationMatrix;

    public Tilemap(int width, int height, Size size, TilemapOrientation orientation)
    {
        _width = width;
        _height = height;
        _size = size;
        _layers = new List<TilemapLayer>();
        _orientationMatrix = orientation == TilemapOrientation.Flat
            ? TilemapOrientationMatrix.Flat
            : TilemapOrientationMatrix.Pointy;
    }

    public TilemapLayer AddLayer()
    {
        var layer = new TilemapLayer(_width, _height, _size, _orientationMatrix);
        _layers.Add(layer);

        return layer;
    }
    
    public void Render(RenderContext renderContext)
    {
        foreach (var layer in _layers)
        {
            layer.Render(renderContext);
        }
    }
}