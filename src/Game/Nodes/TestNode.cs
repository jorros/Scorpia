using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Components;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Nodes;

public class TestNode : NetworkedNode
{
    private AssetBundle? _game;
    private RenderComponent? _render;
    
    private float _direction = 0.01f;

    public override void OnInit()
    {
        _game = AssetManager.Load("Game");

        _render = new RenderComponent(_game.Get<TextureSprite>("title_icon_blue"), OffsetVector.Zero)
        {
            Position = new OffsetVector(200, 200)
        };
        // AttachComponent(_render);
    }

    public override void OnUpdate()
    {
        // if (_render.Scale >= 2.0f)
        // {
        //     _direction = -0.01f;
        // }
        // else if (_render.Scale <= 1.0f)
        // {
        //     _direction = 0.01f;
        // }
        //
        // _render.Scale += _direction;
        
        if (Input.IsKeyDown(KeyboardKey.Down))
        {
            Viewport.WorldPosition += new OffsetVector(0, 1);
        }
        if (Input.IsKeyDown(KeyboardKey.Up))
        {
            Viewport.WorldPosition += new OffsetVector(0, -1);
        }
        if (Input.IsKeyDown(KeyboardKey.Right))
        {
            Viewport.WorldPosition += new OffsetVector(1, 0);
        }
        if (Input.IsKeyDown(KeyboardKey.Left))
        {
            Viewport.WorldPosition += new OffsetVector(-1, 0);
        }
    }
}