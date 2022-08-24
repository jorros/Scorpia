using System;
using static SDL2.SDL;
using static SDL2.SDL_image;
using static SDL2.SDL_ttf;

namespace Scorpia.Engine.Graphics;

public class GraphicsManager
{
    private readonly UserDataManager _userDataManager;

    public GraphicsManager(UserDataManager userDataManager)
    {
        _userDataManager = userDataManager;
    }
    internal IntPtr Window { get; private set; }
    internal IntPtr Renderer { get; private set; }

    internal void Init(IntPtr? handle = null)
    {
        if (SDL_Init(SDL_INIT_VIDEO) < 0)
        {
            throw new EngineException($"Could not initialise SDL: {SDL_GetError()}");
        }

        if (handle is not null)
        {
            Window = SDL_CreateWindowFrom(handle.Value);
        }
        else
        {
            var windowWidth = _userDataManager.Get("windowWidth", 1024);
            var windowHeight = _userDataManager.Get("windowHeight", 768);

            Window = SDL_CreateWindow("Scorpia", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, windowWidth, windowHeight,
                SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL_WindowFlags.SDL_WINDOW_METAL | SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);
        }
        
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
            throw new EngineException($"There was an issue initialising SDL_Image: {IMG_GetError()}");
        }

        if (TTF_Init() == -1)
        {
            throw new EngineException($"There was an issue initialising SDL_TTF: {TTF_GetError()}");
        }
    }

    internal void Quit()
    {
        TTF_Quit();
        IMG_Quit();
        SDL_DestroyRenderer(Renderer);
        SDL_DestroyWindow(Window);
        SDL_Quit();
    }

    internal void Clear()
    {
        if (SDL_SetRenderDrawColor(Renderer, 0, 0, 235, 255) < 0)
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