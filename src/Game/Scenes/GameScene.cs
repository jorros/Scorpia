using Scorpia.Engine.Asset;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Scenes;

public partial class GameScene : NetworkedScene
{
    protected override void OnLoad(AssetManager assetManager)
    {
        SetupUI(assetManager);
    }
}