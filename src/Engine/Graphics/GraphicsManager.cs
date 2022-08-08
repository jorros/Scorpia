using System;
using System.Runtime.InteropServices;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace Scorpia.Engine.Graphics;

internal class GraphicsManager
{
    private readonly UserDataManager _userDataManager;

    public GraphicsManager(UserDataManager userDataManager)
    {
        _userDataManager = userDataManager;
    }
    
    internal IntPtr Window { get; private set; }
    internal IntPtr Renderer { get; private set; }

    internal void Init()
    {
        if (SDL_Init(SDL_INIT_VIDEO) < 0)
        {
            throw new EngineException($"Could not initialise SDL: {SDL_GetError()}");
        }

        var windowWidth = _userDataManager.Get("windowWidth", 1024);
        var windowHeight = _userDataManager.Get("windowHeight", 768);

        Window = SDL_CreateWindow("Scorpia", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, windowWidth, windowHeight,
            SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL_WindowFlags.SDL_WINDOW_METAL | SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);

        if (Window == IntPtr.Zero)
        {
            throw new EngineException($"Could not create window: {SDL_GetError()}");
        }
        
        Renderer = SDL_CreateRenderer(Window, 
            -1, 
            SDL_RendererFlags.SDL_RENDERER_ACCELERATED | 
            SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        if (Renderer == IntPtr.Zero)
        {
            throw new EngineException($"There was an issue creating the renderer: {SDL_GetError()}");
        }
        
        if (IMG_Init(IMG_InitFlags.IMG_INIT_PNG) == 0)
        {
            throw new EngineException($"There was an issue initialising SDL2_Image: {IMG_GetError()}");
        }
    }

    internal void Quit()
    {
        SDL_DestroyRenderer(Renderer);
        SDL_DestroyWindow(Window);
        SDL_Quit();
    }

    public RenderContext CreateContext()
    {
        return new RenderContext(this);
    }

    internal IntPtr LoadTexture(byte[] data, string format)
    {
        var size = Marshal.SizeOf(data[0]) * data.Length;
        var pnt = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, pnt, size);
        
        var rw = SDL_RWFromMem(pnt, size);
        var surface = IMG_LoadTyped_RW(rw, 1, format);
        var texture = SDL_CreateTextureFromSurface(Renderer, surface);
        SDL_FreeSurface(surface);

        return texture;
    }

    internal void RemoveTexture(IntPtr texture)
    {
        SDL_DestroyTexture(texture);
    }

    internal void Clear()
    {
        if (SDL_SetRenderDrawColor(Renderer, 135, 206, 235, 255) < 0)
        {
            Console.WriteLine($"There was an issue with setting the render draw color. {SDL_GetError()}");
        }
        
        if (SDL_RenderClear(Renderer) < 0)
        {
            Console.WriteLine($"There was an issue with clearing the render surface. {SDL_GetError()}");
        }
    }

    internal void Flush()
    {
        SDL_RenderPresent(Renderer);
    }
}