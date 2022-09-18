using System.Drawing;
using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Helper;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Game;

public static class ScorpiaStyle
{
    public static Stylesheet? Stylesheet { get; private set; }
    
    public static void Setup(AssetManager assetManager)
    {
        Stylesheet = new Stylesheet(assetManager);
        
        //
        // LABELS
        //
        
        var defaultLabel = Stylesheet.CreateLabelStyle(null, "UI:MYRIADPRO-REGULAR");
        defaultLabel.Size = 36;
        
        var inputLabel = Stylesheet.CreateLabelStyle("InputLabel", "UI:MYRIADPRO-REGULAR");
        inputLabel.Size = 28;
        inputLabel.Color = Color.FromArgb(202, 178, 148);

        var formLabel = Stylesheet.CreateLabelStyle("Form", "UI:MYRIADPRO-BOLD");
        formLabel.Size = 34;
        formLabel.Color = Color.FromArgb(107, 95, 82);
        formLabel.Outline = 1;
        formLabel.OutlineColor = Color.Black;
        
        var playerPreview = Stylesheet.CreateLabelStyle("PlayerPreview", "UI:MYRIADPRO-BOLD");
        playerPreview.Size = 50;
        playerPreview.Color = Color.FromArgb(154, 132, 108);
        playerPreview.Outline = 1;
        playerPreview.OutlineColor = Color.Black;
        
        var serverStatus = Stylesheet.CreateLabelStyle("Header", "UI:MYRIADPRO-BOLD");
        serverStatus.Size = 54;
        serverStatus.Color = Color.FromArgb(107, 95, 82);
        serverStatus.Outline = 1;
        serverStatus.OutlineColor = Color.Black;
        
        var buttonLabel = Stylesheet.CreateLabelStyle("Button", "UI:MYRIADPRO-BOLD");
        buttonLabel.Color = ColorHelper.HtmlToColor("#cab294");
        buttonLabel.Outline = 1;
        buttonLabel.OutlineColor = Color.Black;
        buttonLabel.Size = 40;
        
        //
        // INPUTS
        //

        var textInput = Stylesheet.CreateTextInputStyle(null, "UI:input", "InputLabel");
        textInput.Width = 550;
        textInput.Height = 90;
        textInput.Padding = new Point(30, 30);
        
        //
        // RADIO BUTTONS
        //

        var contentRadioButton =
            Stylesheet.CreateRadioButtonStyle("content", "UI:radio_button_content_selected", "UI:radio_button_content");
        contentRadioButton.PressedTint = Color.Gray;
        contentRadioButton.MinWidth = 250;
        contentRadioButton.MinHeight = 250;
        contentRadioButton.Padding = new Rectangle(9, -4, 0, 0);
        
        //
        // BUTTONS
        //

        var regularActionButton =
            Stylesheet.CreateButtonStyle("action_regular", "UI:button_regular", "Button");
        regularActionButton.MinHeight = 130;
        regularActionButton.MinWidth = 350;
        regularActionButton.TextPosition = new Point(0, -18);
        regularActionButton.Padding = new Point(40, 0);
        regularActionButton.PressedTint = Color.Gray;

        Stylesheet.CopyButtonStyle(regularActionButton, "action_green", "UI:button_green");
        Stylesheet.CopyButtonStyle(regularActionButton, "action_red", "UI:button_red");
        
        //
        // PROGRESS BARS
        //
        var loadingProgressBar =
            Stylesheet.CreateProgressBarStyle("loading", "UI:loading_progress", "UI:loading_progress_fill");
        loadingProgressBar.Height = 121;
        
        //
        // WINDOWS
        //

        var defaultWindow = Stylesheet.CreateWindowStyle(null, "UI:container", "UI:button_bar");
        defaultWindow.Padding = new Point(80, 80);
        defaultWindow.Height = 1186;
        defaultWindow.Width = 2073;
        defaultWindow.ActionBarHeight = 118;
        defaultWindow.ActionBarPadding = new Rectangle(215, 0, 200, 0);
        defaultWindow.ActionBarMinWidth = 560;
        defaultWindow.ActionBarSpaceBetween = 0;
        defaultWindow.ActionBarMargin = new Point(0, 35);

        var defaultDivider = Stylesheet.CreateHorizontalDividerStyle(null, "UI:horizontal_divider");
        defaultDivider.Height = 26;
        defaultDivider.MinWidth = 50;

        Stylesheet.SetFont(null, "UI:MYRIADPRO-REGULAR");
    }
}