using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Helper;
using Scorpia.Engine.UI;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Game;

public static class ScorpiaStyle
{
    public static Stylesheet? Stylesheet { get; private set; }
    
    public static void Setup(AssetManager assetManager)
    {
        Stylesheet = new Stylesheet(assetManager);
        
        Stylesheet.SetFont(null, "UI:Montserrat");
        Stylesheet.SetFont("Medium", "UI:Montserrat-Medium");
        Stylesheet.SetFont("SemiBold", "UI:Montserrat-SemiBold");
        
        //
        // LABELS
        //
        
        var defaultLabel = Stylesheet.CreateLabelStyle(null, "UI:Montserrat");
        defaultLabel.Size = 36;

        var debugLabel = Stylesheet.CreateLabelStyle("debug", "UI:Montserrat");
        debugLabel.Size = 36;
        debugLabel.Color = Color.White;

        var cornerText = Stylesheet.CreateLabelStyle("corner", "UI:Montserrat");
        cornerText.Color = Color.White;
        cornerText.Size = 60;
        
        var cornerBoldText = Stylesheet.CreateLabelStyle("corner_bold", "UI:Montserrat-SemiBold");
        cornerBoldText.Color = Color.White;
        cornerBoldText.Size = 60;

        var headerLabel = Stylesheet.CreateLabelStyle("header", "UI:Montserrat");
        headerLabel.Size = 48;
        headerLabel.Color = Color.White;

        var formLabel = Stylesheet.CreateLabelStyle("form", "UI:Montserrat");
        formLabel.Size = 24;
        formLabel.Color = Color.White;
        
        var inputLabel = Stylesheet.CreateLabelStyle("InputLabel", "UI:Montserrat");
        inputLabel.Size = 28;
        inputLabel.Color = Color.White;

        var statLabel = Stylesheet.CreateLabelStyle("StatLabel", "UI:Montserrat");
        statLabel.Size = 28;
        statLabel.Color = Color.FromArgb(132, 111, 98);
        
        var infoNameLabel = Stylesheet.CreateLabelStyle("InfoNameLabel", "UI:Montserrat");
        infoNameLabel.Size = 32;
        infoNameLabel.Color = Color.FromArgb(163, 139, 92);
        infoNameLabel.Outline = 2;
        infoNameLabel.OutlineColor = Color.Black;

        var tooltipHeader = Stylesheet.CreateLabelStyle("TooltipHeader", "UI:Montserrat");
        tooltipHeader.Size = 34;
        tooltipHeader.Color = Color.FromArgb(179, 164, 151);
        
        var tooltipSubHeader = Stylesheet.CreateLabelStyle("TooltipSubHeader", "UI:Montserrat");
        tooltipSubHeader.Size = 26;
        tooltipSubHeader.Color = Color.FromArgb(204, 147, 103);
        
        var tooltipContent = Stylesheet.CreateLabelStyle("TooltipContent", "UI:Montserrat");
        tooltipContent.Size = 28;
        tooltipContent.Color = Color.FromArgb(168, 164, 156);
        
        var topStats = Stylesheet.CreateLabelStyle("TopStats", "UI:Montserrat");
        topStats.Size = 28;
        topStats.Color = Color.White;

        var bigButtonLabel = Stylesheet.CreateLabelStyle("big-button", "UI:Montserrat-SemiBold");
        bigButtonLabel.Color = Color.White;
        bigButtonLabel.Size = 34;
        bigButtonLabel.TextAlign = TextAlign.Center;
        
        var smallButtonLabel = Stylesheet.CreateLabelStyle("small-button", "UI:Montserrat-SemiBold");
        smallButtonLabel.Color = Color.White;
        smallButtonLabel.Size = 26;
        smallButtonLabel.TextAlign = TextAlign.Center;
        
        //
        // INPUTS
        //

        var textInput = Stylesheet.CreateTextInputStyle(null, "UI:input", "InputLabel");
        textInput.Width = 600;
        textInput.Height = 80;
        textInput.Padding = new Point(20, 20);
        
        //
        // RADIO BUTTONS
        //

        var emptyRadioButton =
            Stylesheet.CreateRadioButtonStyle("empty", null, null);
        emptyRadioButton.PressedTint = Color.Gray;
        emptyRadioButton.SelectedTint = Color.Orchid;
        emptyRadioButton.MinWidth = 164;
        emptyRadioButton.MinHeight = 164;

        //
        // BUTTONS
        //

        var loginActionButton =
            Stylesheet.CreateButtonStyle("action_login", "UI:button");
        loginActionButton.MinHeight = 80;
        loginActionButton.MinWidth = 600;
        loginActionButton.ContentPosition = new Point(0, -20);
        loginActionButton.Padding = new Point(0, 0);
        loginActionButton.PressedTint = Color.FromArgb(0, 166, 79);
        loginActionButton.Tint = Color.FromArgb(3, 189, 91);
        loginActionButton.DefaultLabelStyle = "big-button";
        
        var regularActionButton =
            Stylesheet.CreateButtonStyle("action_regular", "UI:button");
        regularActionButton.MinHeight = 52;
        regularActionButton.MinWidth = 180;
        regularActionButton.ContentPosition = new Point(0, -18);
        regularActionButton.Padding = new Point(0, 0);
        regularActionButton.DefaultLabelStyle = "small-button";

        var greenActionButton = Stylesheet.CopyButtonStyle(regularActionButton, "action_green", "UI:button");
        greenActionButton.PressedTint = Color.FromArgb(0, 166, 79);
        greenActionButton.Tint = Color.FromArgb(3, 189, 91);

        var redActionButton = Stylesheet.CopyButtonStyle(regularActionButton, "action_red", "UI:button");
        redActionButton.PressedTint = Color.FromArgb(185, 34, 47);
        redActionButton.Tint = Color.FromArgb(241, 50, 66);

        var cornerButton = Stylesheet.CreateButtonStyle("corner", null);
        cornerButton.FixedHeight = 96;
        cornerButton.FixedWidth = 96;
        cornerButton.PressedTint = Color.Gray;
        cornerButton.ContentPosition = new Point(-48, -48);

        //
        // DIVIDERS
        //
        var horizontalDivider = Stylesheet.CreateHorizontalDividerStyle(null, "UI:horizontal_divider");
        horizontalDivider.Height = 2;

        //
        // PROGRESS BARS
        //
        var loadingProgressBar =
            Stylesheet.CreateProgressBarStyle("loading", "UI:progress_loading_empty", "UI:progress_loading_full");
        loadingProgressBar.Height = 15;
        
        //
        // WINDOWS
        //

        var bothWindow = Stylesheet.CreateWindowStyle("full", "UI:container_both");
        bothWindow.Padding = new Point(40, 40);
        bothWindow.ActionBarHeight = 90;
        bothWindow.ActionBarPadding = new Rectangle(50, -30, 50, 0);
        bothWindow.ActionBarSpaceBetween = 20;
        bothWindow.ActionBarAnchor = UIAnchor.Right;
        bothWindow.HasActionBar = true;
        bothWindow.HasTitle = true;
        bothWindow.TitleHeight = 107;
        bothWindow.TitlePadding = new Rectangle(40, 25, 40, 0);
        bothWindow.TitleSpaceBetween = 30;
        bothWindow.TitleLabelStyle = "header";
        
        var lightWindow = Stylesheet.CreateWindowStyle("light", "UI:container_top");
        lightWindow.Padding = new Point(20, 20);
        lightWindow.HasTitle = true;
        lightWindow.TitleHeight = 107;
        lightWindow.TitlePadding = new Rectangle(40, 25, 40, 0);
        lightWindow.TitleSpaceBetween = 30;
        lightWindow.TitleLabelStyle = "header";

        var simpleWindow = Stylesheet.CreateWindowStyle(null, "UI:container_nobar");
        simpleWindow.Padding = new Point(40, 40);
    }

    public static void SetupInGame(AssetManager assetManager)
    {
        var topCornerButton = Stylesheet!.CreateButtonStyle("top_button_corner", "Game:HUD/top_button_corner");
        topCornerButton.PressedTint = Color.Gray;
        topCornerButton.FixedHeight = 109;
        topCornerButton.FixedWidth = 123;
        topCornerButton.ContentPosition = new Point(-44, -35);
        
        var topButton = Stylesheet!.CreateButtonStyle("top_button", "Game:HUD/top_button");
        topButton.PressedTint = Color.Gray;
        topButton.FixedHeight = 109;
        topButton.FixedWidth = 109;
        topButton.ContentPosition = new Point(-37, -37);
    }
}