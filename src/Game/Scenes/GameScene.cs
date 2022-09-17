using Scorpia.Engine.Asset;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.Scenes;

public partial class GameScene : NetworkedScene
{
    private MapNode _map;
    
    protected override void OnLoad(AssetManager assetManager)
    {
        SetupUI(assetManager);
        
        _map = CreateNode<MapNode>();
    }

    public void InitMap(int seed)
    {
        _map.Generate(seed);
    }
}