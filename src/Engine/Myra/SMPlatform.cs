using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Myra.Graphics2D.UI;
using Myra.Platform;
using Scorpia.Engine.Graphics;
using static SDL2.SDL;

namespace Scorpia.Engine.Myra;

internal class SMPlatform : IMyraPlatform
{
    private readonly GraphicsManager _graphicsManager;

    public SMPlatform(GraphicsManager graphicsManager)
    {
        _graphicsManager = graphicsManager;
    }

    public object CreateTexture(int width, int height)
    {
        return SDL_CreateTexture(_graphicsManager.Renderer, SDL_PIXELFORMAT_ARGB8888,
            (int) SDL_TextureAccess.SDL_TEXTUREACCESS_STATIC, width,
            height);
    }

    public Point GetTextureSize(object texture)
    {
        SDL_QueryTexture((IntPtr) texture, out _, out _, out var w, out var h);

        return new Point(w, h);
    }

    public void SetTextureData(object texture, Rectangle bounds, byte[] data)
    {
        var rect = new SDL_Rect
        {
            h = bounds.Height,
            w = bounds.Width,
            x = bounds.X,
            y = bounds.Y
        };

        var size = Marshal.SizeOf(data[0]) * data.Length;
        var pnt = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, pnt, size);

        SDL_UpdateTexture((IntPtr) texture, ref rect, pnt, bounds.Width * Marshal.SizeOf(data[0]));
    }

    public IMyraRenderer CreateRenderer()
    {
        return new SMRenderer(_graphicsManager);
    }

    public MouseInfo GetMouseInfo()
    {
        SDL_GetMouseState(out var x, out var y);

        return new MouseInfo
        {
            Position = new Point(x, y)
        };
    }

    public void SetKeysDown(bool[] keys)
    {
    }

    public TouchCollection GetTouchState()
    {
        return TouchCollection.Empty;
    }

    public Point ViewSize
    {
        get
        {
            SDL_GetRendererOutputSize(_graphicsManager.Renderer, out var w, out var h);
            
            return new Point(w, h);
        }
    }
}