using System.Collections.Generic;
using System.Linq;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class RadioGroup : UIElement
{
    private readonly List<RadioButton> _radioButtons = new();

    public void Attach(RadioButton button)
    {
        button.Parent = this;
        _radioButtons.Add(button);
    }

    public void Select(RadioButton radioButton)
    {
        foreach (var button in _radioButtons)
        {
            if (button == radioButton)
            {
                button.IsSelected = true;
                continue;
            }

            button.IsSelected = false;
        }
    }

    public RadioButton GetSelected()
    {
        return _radioButtons.FirstOrDefault(x => x.IsSelected);
    }

    public RadioButton GetButton(object value)
    {
        return _radioButtons.FirstOrDefault(x => x.Value == value);
    }

    public object? GetValue()
    {
        return GetSelected()?.Value;
    }
    
    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        foreach (var button in _radioButtons)
        {
            button.Render(renderContext, stylesheet, inWorld);
        }
    }
}