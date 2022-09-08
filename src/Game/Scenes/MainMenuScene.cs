using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.Network;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class MainMenuScene : NetworkedScene
{
    protected override void OnLoad(AssetManager? assetManager)
    {
        if (assetManager is not null)
        {
            assetManager.Load("UI");
            ScorpiaStyle.Setup(assetManager);
            SetupUI(assetManager);

            _quitButton!.OnClick += QuitButtonOnOnClick;
            _joinButton!.OnClick += JoinButtonOnOnClick;
        }
    }

    private void JoinButtonOnOnClick(object sender, MouseButtonEventArgs e)
    {
        Invoke(nameof(Test), new TestParam
        {
            Param = 123
        });
    }

    [ClientRpc]
    public void Abc(ushort senderId)
    {
        Console.Write(senderId);
    }
    
    private void QuitButtonOnOnClick(object sender, MouseButtonEventArgs e)
    {
        SceneManager.Quit();
    }

    protected override void OnTick()
    {
        if (NetworkManager.IsServer)
        {
            return;
        }
        
        var renderContext = ServiceProvider.GetService<RenderContext>();
        if (renderContext is not null)
        {
            _fpsLabel!.Text = renderContext.FPS.ToString();
        }
        
        if (!NetworkManager.IsConnected())
        {
            NetworkManager.Connect("127.0.0.1", 1992);
            _serverStatus!.Text = "Server <text color='red' size='70'>OFFLINE</text>";
        }
        else
        {
            _serverStatus!.Text = "Server <text color='green' size='70'>ONLINE</text>";
        }
    }

    protected override void OnRender(RenderContext context)
    {
        _layout!.Render(context, false);
    }
}