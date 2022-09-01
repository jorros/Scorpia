#nullable enable
using System;
using System.Collections.Generic;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Asset.Font;

namespace Scorpia.Engine.UI.Style;

public class Stylesheet
{
    private readonly Dictionary<string, Font> _fonts = new();
    private readonly Dictionary<string, ButtonStyle> _buttons = new();
    private readonly Dictionary<string, LabelStyle> _labels = new();
    private readonly Dictionary<string, WindowStyle> _windows = new();
    private readonly Dictionary<string, TextInputStyle> _inputs = new();

    private Font? _defaultFont;
    private ButtonStyle? _defaultButton;
    private LabelStyle? _defaultLabel;
    private WindowStyle? _defaultWindow;
    private TextInputStyle? _defaultInput;
    private readonly AssetManager _assetManager;

    public Stylesheet(AssetManager assetManager)
    {
        _assetManager = assetManager;
        ScaleModifier = assetManager.HighRes ? 1.0 : 0.5;
    }
    
    public double ScaleModifier { get; }

    public int Scale(int val)
    {
        return (int)Math.Floor(val * ScaleModifier);
    }

    public OffsetVector Scale(OffsetVector val)
    {
        return new OffsetVector(Scale(val.X), Scale(val.Y));
    }

    public LabelStyle CreateLabelStyle(string? name, string fontAsset)
    {
        var style = new LabelStyle
        {
            Font = _assetManager.Get<Font>(fontAsset)
        };

        if (name is null)
        {
            _defaultLabel = style;
            return style;
        }

        _labels[name] = style;
        return style;
    }
    
    public TextInputStyle CreateTextInputStyle(string? name, string spriteAsset, string? labelStyle)
    {
        var style = new TextInputStyle
        {
            Background = _assetManager.Get<Sprite>(spriteAsset),
            Text = GetLabel(labelStyle)
        };

        if (name is null)
        {
            _defaultInput = style;
            return style;
        }

        _inputs[name] = style;
        return style;
    }

    public ButtonStyle CreateButtonStyle(string? name, string spriteAsset, string? labelStyle)
    {
        var style = new ButtonStyle
        {
            Button = _assetManager.Get<Sprite>(spriteAsset),
            LabelStyle = GetLabel(labelStyle)
        };

        if (name is null)
        {
            _defaultButton = style;
            return style;
        }
        
        _buttons[name] = style;
        return style;
    }

    public ButtonStyle CopyButtonStyle(ButtonStyle toCopy, string name, string spriteAsset)
    {
        var style = toCopy with {Button = _assetManager.Get<Sprite>(spriteAsset)};

        _buttons[name] = style;
        return style;
    }
    
    public WindowStyle CreateWindowStyle(string? name, string backgroundAsset, string actionBarAsset)
    {
        var style = new WindowStyle
        {
            Background = _assetManager.Get<Sprite>(backgroundAsset),
            ActionBarBackground = _assetManager.Get<Sprite>(actionBarAsset)
        };

        if (name is null)
        {
            _defaultWindow = style;
            return style;
        }
        
        _windows[name] = style;
        return style;
    }

    public void SetFont(string? name, string assetName)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _defaultFont = _assetManager.Get<Font>(assetName);
            return;
        }

        _fonts[name] = _assetManager.Get<Font>(assetName);
    }

    public ButtonStyle GetButton(string? name = null)
    {
        if (name is not null && _buttons.ContainsKey(name))
        {
            return _buttons[name];
        }

        if (_defaultButton is null)
        {
            throw new EngineException("No default button style set in stylesheet.");
        }

        return _defaultButton;
    }
    
    public TextInputStyle GetTextInput(string? name = null)
    {
        if (name is not null && _inputs.ContainsKey(name))
        {
            return _inputs[name];
        }

        if (_defaultInput is null)
        {
            throw new EngineException("No default input style set in stylesheet.");
        }

        return _defaultInput;
    }
    
    public WindowStyle GetWindow(string? name = null)
    {
        if (name is not null && _windows.ContainsKey(name))
        {
            return _windows[name];
        }

        if (_defaultWindow is null)
        {
            throw new EngineException("No default window style set in stylesheet.");
        }

        return _defaultWindow;
    }

    public LabelStyle GetLabel(string? name = null)
    {
        if (name is not null && _labels.ContainsKey(name))
        {
            return _labels[name];
        }
        
        if (_defaultLabel is null)
        {
            throw new EngineException("No default label style set in stylesheet.");
        }

        return _defaultLabel;
    }

    public Font GetFont(string? name = null)
    {
        if (name is not null && _fonts.ContainsKey(name))
        {
            return _fonts[name];
        }

        if (_defaultFont is null)
        {
            throw new EngineException("No default font set in stylesheet.");
        }

        return _defaultFont;
    }
}