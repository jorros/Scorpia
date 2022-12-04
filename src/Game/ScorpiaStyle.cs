using System.Drawing;
using Scorpian.Asset;
using Scorpian.Maths;
using Scorpian.UI;
using Scorpian.UI.Style;

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
        debugLabel.Outline = 2;
        debugLabel.OutlineColor = Color.Black;

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

        var tooltipHeader = Stylesheet.CreateLabelStyle("tooltip-header", "UI:Montserrat-SemiBold");
        tooltipHeader.Size = 34;
        tooltipHeader.Color = Color.White;
        
        var tooltipSubHeader = Stylesheet.CreateLabelStyle("tooltip-subheader", "UI:Montserrat");
        tooltipSubHeader.Size = 26;
        tooltipSubHeader.Color = Color.FromArgb(138, 138, 138);
        
        var tooltipContent = Stylesheet.CreateLabelStyle("tooltip-content", "UI:Montserrat");
        tooltipContent.Size = 28;
        tooltipContent.Color = Color.White;
        
        var topStats = Stylesheet.CreateLabelStyle("top", "UI:Montserrat-Medium");
        topStats.Size = 28;
        topStats.Color = Color.White;

        var bigButtonLabel = Stylesheet.CreateLabelStyle("big-button", "UI:Montserrat-SemiBold");
        bigButtonLabel.Color = Color.White;
        bigButtonLabel.Size = 34;
        bigButtonLabel.Alignment = Alignment.Center;
        
        var smallButtonLabel = Stylesheet.CreateLabelStyle("small-button", "UI:Montserrat-SemiBold");
        smallButtonLabel.Color = Color.White;
        smallButtonLabel.Size = 26;
        smallButtonLabel.Alignment = Alignment.Center;
        
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
        regularActionButton.Padding = new Point(15, 0);
        regularActionButton.DefaultLabelStyle = "small-button";
        regularActionButton.Tint = Color.FromArgb(53, 52, 52);
        regularActionButton.PressedTint = Color.FromArgb(40, 40, 40);

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
            Stylesheet.CreateProgressBarStyle("loading", "UI:progress_loading_full", backgroundSprite: "UI:progress_loading_empty");
        loadingProgressBar.Height = 15;
        
        //
        // WINDOWS
        //

        var bothWindow = Stylesheet.CreateWindowStyle("full", "UI:container_both");
        bothWindow.Padding = new Point(40, 40);
        bothWindow.ActionBarHeight = 90;
        bothWindow.ActionBarPadding = new Box(50, 15, 50, 0);
        bothWindow.ActionBarSpaceBetween = 20;
        bothWindow.ActionBarAlign = Alignment.Right;
        bothWindow.HasActionBar = true;
        bothWindow.HasTitle = true;
        bothWindow.TitleHeight = 107;
        bothWindow.TitlePadding = new Box(40, 25, 40, 0);
        bothWindow.TitleSpaceBetween = 30;
        bothWindow.TitleLabelStyle = "header";
        
        var lightWindow = Stylesheet.CreateWindowStyle("light", "UI:container_top");
        lightWindow.Padding = new Point(20, 20);
        lightWindow.HasTitle = true;
        lightWindow.TitleHeight = 107;
        lightWindow.TitlePadding = new Box(40, 25, 40, 0);
        lightWindow.TitleSpaceBetween = 30;
        lightWindow.TitleLabelStyle = "header";

        var simpleWindow = Stylesheet.CreateWindowStyle(null, "UI:container_nobar");
        simpleWindow.Padding = new Point(40, 40);
    }

    public static void SetupInGame(AssetManager assetManager)
    {
        var infoWindow = Stylesheet!.CreateWindowStyle("info", "Game:HUD/info_box");
        infoWindow.Padding = new Point(50, 20);
        infoWindow.HasTitle = true;
        infoWindow.TitleHeight = 114;
        infoWindow.TitlePadding = new Box(50, 4, 40, 0);
        infoWindow.TitleSpaceBetween = 30;
        infoWindow.TitleLabelStyle = "header";
        
        var notificationWindow = Stylesheet.CreateWindowStyle("notification", "Game:HUD/menu_container");
        notificationWindow.Padding = new Point(26, 0);
        notificationWindow.Width = 800;
        notificationWindow.Height = 800;
        notificationWindow.HasTitle = true;
        notificationWindow.TitleHeight = 114;
        notificationWindow.TitlePadding = new Box(60, 24, 60, 0);
        notificationWindow.TitleSpaceBetween = 30;
        notificationWindow.TitleLabelStyle = "header";
        notificationWindow.IsDraggable = true;
        notificationWindow.HasActionBar = true;
        notificationWindow.ActionBarHeight = 162;
        notificationWindow.ActionBarSpaceBetween = 30;
        notificationWindow.ActionBarPadding = new Box(0, 17, 0, 0);
        notificationWindow.ActionBarAlign = Alignment.Center;

        var notificationLabel = Stylesheet.CreateLabelStyle("notification", "UI:Montserrat");
        notificationLabel.Color = Color.White;
        notificationLabel.Size = 30;

        var mapWindow = Stylesheet.CreateWindowStyle("map", "Game:HUD/mini_map");
        mapWindow.HasTitle = true;
        mapWindow.TitleHeight = 60;
        mapWindow.TitlePadding = new Box(5, 5, 5, 5);
        mapWindow.TitleLabelStyle = "map";
        mapWindow.TitleAlign = Alignment.Right;
        
        var mapLabel = Stylesheet.CreateLabelStyle("map", "UI:Montserrat");
        mapLabel.Size = 36;
        mapLabel.Color = Color.White;

        var menuButton = Stylesheet.CreateButtonStyle("menu", "Game:HUD/menu_button");
        menuButton.FixedWidth = 116;
        menuButton.FixedHeight = 117;
        menuButton.PressedTint = Color.DarkGray;
        menuButton.ContentPosition = new Point(-38, -38);
        
        var notificationButton = Stylesheet.CreateButtonStyle("notification", "Game:HUD/notification_button");
        notificationButton.FixedWidth = 132;
        notificationButton.FixedHeight = 132;
        notificationButton.PressedTint = Color.DarkGray;
        notificationButton.ContentPosition = new Point(-65, -65);

        var districtButton = Stylesheet.CreateButtonStyle("district", "Game:HUD/district_button");
        districtButton.FixedWidth = 162;
        districtButton.FixedHeight = 86;
        districtButton.ContentPosition = new Point(-51, -30);
        districtButton.PressedTint = Color.DarkGray;

        var buildingButton = Stylesheet.CreateButtonStyle("building", "Game:HUD/building_button");
        buildingButton.FixedHeight = 83;
        buildingButton.FixedWidth = 83;
        buildingButton.ContentPosition = new Point(-41, -41);
        buildingButton.PressedTint = Color.DarkGray;
        
        var districtLabel = Stylesheet.CreateLabelStyle("district", "UI:Montserrat");
        districtLabel.Size = 48;
        districtLabel.Color = Color.White;
        districtLabel.Alignment = Alignment.Center;

        var actionButton = Stylesheet.CreateButtonStyle("action", "Game:HUD/action_button");
        actionButton.FixedWidth = 103;
        actionButton.FixedHeight = 103;
        actionButton.ContentPosition = new Point(-39, -39);
        actionButton.PressedTint = Color.DarkGray;

        var storageProgress =
            Stylesheet.CreateProgressBarStyle("storage", "Game:HUD/storage_full", foregroundSprite: "Game:HUD/storage_empty");
        storageProgress.Width = 279;
        storageProgress.Height = 279;
        storageProgress.Orientation = Orientation.Vertical;
        storageProgress.FillColor = Color.FromArgb(0, 152, 64);

        var infoLabel = Stylesheet.CreateLabelStyle("info", "UI:Montserrat");
        infoLabel.Size = 30;
        infoLabel.Color = Color.White;

        var infoHeaderLabel = Stylesheet.CreateLabelStyle("info_label", "UI:Montserrat-SemiBold");
        infoHeaderLabel.Size = 30;
        infoHeaderLabel.Color = Color.White;

        var titleLabel = Stylesheet.CreateLabelStyle("title", "Game:HUD/honeyblot_caps");
        titleLabel.Size = 36;
        titleLabel.Color = Color.FromArgb(105, 105, 105);
    }
}