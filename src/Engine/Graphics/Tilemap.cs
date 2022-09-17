using System.Collections.Generic;

namespace Scorpia.Engine.Graphics;

public class Tilemap
{
    private readonly int _width;
    private readonly int _height;
    private readonly int _tileWidth;
    private readonly int _tileHeight;
    private readonly List<TilemapLayer> _layers;

    public Tilemap(int width, int height, int tileWidth, int tileHeight)
    {
        _width = width;
        _height = height;
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
        _layers = new List<TilemapLayer>();
    }

    public TilemapLayer AddLayer()
    {
        var layer = new TilemapLayer(_width, _height, _tileWidth, _tileHeight);
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