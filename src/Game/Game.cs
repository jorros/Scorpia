using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;
using Scorpia.Engine.UI.Style;
using Scorpia.Game.Nodes;
using Scorpia.Game.Scenes;

namespace Scorpia.Game;

public class Game : Engine.Engine
{
    protected override void Init(IServiceCollection services)
    {
        services.AddScene<MainMenuScene>();
    }

    protected override void Load(IServiceProvider serviceProvider)
    {
        var assetManager = serviceProvider.GetRequiredService<AssetManager>();

        assetManager.Load("UI");
        ScorpiaStyle.Setup(assetManager);
        
        var sceneManager = serviceProvider.GetRequiredService<SceneManager>();

        sceneManager.Switch<MainMenuScene>();
    }
}