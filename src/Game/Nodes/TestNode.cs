using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Components;
using Scorpia.Engine.InputManagement;

namespace Scorpia.Game.Nodes;

public class TestNode : Node
{
    private AssetBundle _game;
    private RenderComponent _render;

    public override void OnInit()
    {
        _game = AssetManager.Load("Game");

        _render = new RenderComponent(_game.Get<Sprite>("title_icon_blue"), OffsetVector.Zero);
        AttachComponent(_render);
    }

    public override void OnUpdate()
    {
        if (Input.IsKeyDown(KeyboardKey.KEY_DOWN))
        {
            _render.Position += new OffsetVector(0, 1);
        }
        if (Input.IsKeyDown(KeyboardKey.KEY_UP))
        {
            _render.Position += new OffsetVector(0, -1);
        }
        if (Input.IsKeyDown(KeyboardKey.KEY_RIGHT))
        {
            _render.Position += new OffsetVector(1, 0);
        }
        if (Input.IsKeyDown(KeyboardKey.KEY_LEFT))
        {
            _render.Position += new OffsetVector(-1, 0);
        }
    }
}