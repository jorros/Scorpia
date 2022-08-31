using System.Drawing;
using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Helper;
using Scorpia.Engine.SceneManagement;
using Scorpia.Engine.UI;
using Scorpia.Engine.UI.Style;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public class MainMenuScene : Scene
{
    private readonly AssetManager _assetManager;

    private readonly BasicLayout _layout;

    public MainMenuScene(IServiceProvider serviceProvider, AssetManager assetManager) : base(serviceProvider)
    {
        _assetManager = assetManager;
        assetManager.Load("UI");

        CreateNode<TestNode>();
        
        var stylesheet = new Stylesheet(assetManager);
        
        var defaultLabel = stylesheet.CreateLabelStyle(null, "UI:MYRIADPRO-REGULAR");
        defaultLabel.Size = 36;
        
        var buttonLabel = stylesheet.CreateLabelStyle("Button", "UI:MYRIADPRO-BOLD");
        buttonLabel.Color = ColorHelper.HtmlToColor("#cab294");
        buttonLabel.Outline = 1;
        buttonLabel.OutlineColor = Color.Black;
        buttonLabel.Size = 40;
        
        var regularActionButton =
            stylesheet.CreateButtonStyle("action_regular", "UI:button_regular", "Button");
        regularActionButton.MinHeight = 130;
        regularActionButton.MinWidth = 350;
        regularActionButton.TextPosition = new OffsetVector(0, -18);
        regularActionButton.Padding = new OffsetVector(40, 0);
        regularActionButton.PressedTint = Color.Gray;

        stylesheet.CopyButtonStyle(regularActionButton, "action_green", "UI:button_green");
        stylesheet.CopyButtonStyle(regularActionButton, "action_red", "UI:button_red");

        var defaultWindow = stylesheet.CreateWindowStyle(null, "UI:container", "UI:button_bar");
        defaultWindow.Padding = new OffsetVector(80, 80);
        defaultWindow.Height = 1186;
        defaultWindow.Width = 2073;
        defaultWindow.ActionBarHeight = 118;
        defaultWindow.ActionBarPadding = new Rectangle(215, 0, 200, 0);
        defaultWindow.ActionBarMinWidth = 560;
        defaultWindow.ActionBarSpaceBetween = 0;
        defaultWindow.ActionBarMargin = new OffsetVector(0, 35);

        stylesheet.SetFont(null, "UI:MYRIADPRO-REGULAR");

        _layout = new BasicLayout(stylesheet)
        {
            Background = _assetManager.Get<Sprite>("UI:background")
        };

        var versionLabel = new Label
        {
            Text = "<outline color=\"#000\" size=\"2\">Version 0.1 Pre-Alpha</outline>",
            Anchor = UIAnchor.BottomRight,
            Color = Color.White,
            Size = 42,
            Position = new OffsetVector(20, 20)
        };
        _layout.Attach(versionLabel);

        var window = new Window
        {
            Anchor = UIAnchor.Center
        };
        _layout.Attach(window);

        var quitButton = new Button
        {
            Position = new OffsetVector(0, 0),
            Text = "<outline color=\"#000\" size=\"1\">QUIT</outline>",
            Type = "action_red"
        };
        window.AttachAction(quitButton);

        var joinButton = new Button
        {
            Position = new OffsetVector(0, 0),
            Text = "JOIN",
            Type = "action_green"
        };
        window.AttachAction(joinButton);

        var settingsButton = new Button
        {
            Position = new OffsetVector(0, 0),
            Text = "SETTINGS",
            Type = "action_regular"
        };
        window.AttachAction(settingsButton);
    }

    protected override void OnRender(RenderContext context)
    {
        _layout.Render(context, false);
    }
}