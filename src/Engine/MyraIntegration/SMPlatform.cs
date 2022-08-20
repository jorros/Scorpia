using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Myra.Graphics2D.UI;
using Myra.Platform;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using static SDL2.SDL;

namespace Scorpia.Engine.MyraIntegration;

internal class SMPlatform : IMyraPlatform
{
    private readonly GraphicsManager _graphicsManager;
    private readonly RenderContext _renderContext;

    private static readonly Dictionary<Keys, KeyboardKey> _keysMap = new();

    public SMPlatform(GraphicsManager graphicsManager, RenderContext renderContext)
    {
        _graphicsManager = graphicsManager;
        _renderContext = renderContext;

        var keysValues = Enum.GetValues(typeof(Keys));
        foreach (Keys keys in keysValues)
        {
            var name = Enum.GetName(typeof(Keys), keys);

            KeyboardKey key;
            if (Enum.TryParse(name, out key))
            {
                _keysMap[keys] = key;
            }
        }

        _keysMap[Keys.Back] = KeyboardKey.Backspace;
    }

    public object CreateTexture(int width, int height)
    {
        var texture = SDL_CreateTexture(_graphicsManager.Renderer, SDL_PIXELFORMAT_ARGB8888,
            (int) SDL_TextureAccess.SDL_TEXTUREACCESS_STATIC, width,
            height);

        SDL_SetTextureBlendMode(texture, SDL_BlendMode.SDL_BLENDMODE_BLEND);
        SDL_SetRenderDrawBlendMode(texture, SDL_BlendMode.SDL_BLENDMODE_BLEND);

        return texture;
    }

    public Point GetTextureSize(object texture)
    {
        SDL_QueryTexture((IntPtr) texture, out _, out _, out var w, out var h);

        return new Point(w, h);
    }

    public void SetTextureData(object texture, Rectangle bounds, byte[] data)
    {
        // for (var i = 0; i < data.Length; i += 4)
        // {
        //     var r = data[i];
        //     var g = data[i + 1];
        //     var b = data[i + 2];
        //     var a = data[i + 3];
        //
        //     data[i] = b;
        //     data[i + 1] = g;
        //     data[i + 2] = r;
        //     data[i + 3] = a;
        // }

        var rect = new SDL_Rect
        {
            h = bounds.Height,
            w = bounds.Width,
            x = bounds.X,
            y = bounds.Y
        };

        var size = Marshal.SizeOf(typeof(byte)) * data.Length;
        var pnt = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, pnt, size);

        SDL_UpdateTexture((IntPtr) texture, ref rect, pnt, 4 * bounds.Width * Marshal.SizeOf(typeof(byte)));
        Marshal.FreeHGlobal(pnt);
    }

    public IMyraRenderer CreateRenderer()
    {
        return new SMRenderer(_graphicsManager, _renderContext);
    }

    public MouseInfo GetMouseInfo()
    {
        var state = SDL_GetMouseState(out var x, out var y);
        var button = SDL_BUTTON(state);

        return new MouseInfo
        {
            Position = new Point(x, y),
            IsLeftButtonDown = button == (uint) MouseButton.Left,
            IsMiddleButtonDown = button == (uint) MouseButton.Middle,
            IsRightButtonDown = button == (uint) MouseButton.Right,
            Wheel = 0
        };
    }

    public void SetKeysDown(bool[] keys)
    {
        var ptr = SDL_GetKeyboardState(out var num);
        var keyArr = new byte[num];
        Marshal.Copy(ptr, keyArr, 0, num);

        for (var i = 0; i < keys.Length; ++i)
        {
            var ks = (Keys) i;
            if (_keysMap.TryGetValue(ks, out var key))
            {
                keys[i] = keyArr[(int) key] == 1;
            }
            else
            {
                keys[i] = false;
            }
        }
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