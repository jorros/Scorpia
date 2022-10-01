using System;
using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Engine.Components;

public class RenderComponent : Component
{
    public Sprite Sprite { get; set; }
    public Point Position { get; set; }
    public int AnimationFps { get; set; }

    private int _lastFrame;
    
    public RenderComponent(Sprite sprite, Point position)
    {
        Sprite = sprite;
        Position = position;
    }

    public override void OnRender(RenderContext context, float dT)
    {
        if (Sprite is AnimatedSprite animated)
        {
            var framesToUpdate = (int)Math.Floor(dT / (1.0f / AnimationFps));
            
            if (framesToUpdate > 0) {
                _lastFrame += framesToUpdate;
                _lastFrame %= animated.FramesCount;
            }
            
            context.Draw(Sprite, new RectangleF(Position, Sprite.Size), 0, Color.White, 255, _lastFrame);
            return;
        }
        
        context.Draw(Sprite, Position);
    }
}