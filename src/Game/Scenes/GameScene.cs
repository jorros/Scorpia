using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.Scenes;

public partial class GameScene : NetworkedScene
{
    private MapNode _map = null!;
    public override Color BackgroundColor => Color.FromArgb(105, 105, 108);

    protected override void OnLoad(AssetManager assetManager)
    {
        SetupUI(assetManager);
        
        Input.OnKeyboard += InputOnOnKeyboard;
        
        _map = CreateNode<MapNode>();
    }

    private void InputOnOnKeyboard(object? sender, KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case KeyboardKey.A:
                Camera.ZoomIn(0.05f);
                break;
            
            case KeyboardKey.D:
                Camera.ZoomOut(0.05f);
                break;
        }
    }

    public void InitMap(int seed)
    {
        _map.Generate(seed);
    }
}