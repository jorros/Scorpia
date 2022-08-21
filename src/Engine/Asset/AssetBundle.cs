using System;
using System.Collections.Generic;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.Asset;

public class AssetBundle : IDisposable
{
    private readonly Dictionary<string, IAsset> _assets;
    private readonly GraphicsManager _graphicsManager;

    internal AssetBundle(Dictionary<string, IAsset> assets, GraphicsManager graphicsManager)
    {
        _assets = assets;
        _graphicsManager = graphicsManager;
    }

    public T Get<T>(string name) where T : class, IAsset
    {
        return _assets[name] as T;
    }

    public void Dispose()
    {
        foreach (var asset in _assets.Values)
        {
            switch (asset)
            {
                case TextureSprite sprite:
                    _graphicsManager.RemoveTexture(sprite.Texture);
                    break;
                
                case Font font:
                    font.Dispose();
                    break;
                
                default:
                    Console.WriteLine("Unknown asset in bundle. Could not remove from memory.");
                    break;
            }
        }
        
        _assets.Clear();
    }
}