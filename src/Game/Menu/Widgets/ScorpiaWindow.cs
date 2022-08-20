using Myra.Attributes;
using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;

namespace Scorpia.Game.Menu.Widgets;

public class ScorpiaWindow : SingleItemContainer<VerticalStackPanel>, IContent
{
    private Widget _content;

    [Content]
    public Widget Content
    {
        get => _content;
        set
        {
            if (value == Content)
            {
                return;
            }

            // Remove existing
            if (_content != null)
            {
                InternalChild.Widgets.Remove(_content);
            }

            if (value != null)
            {
                InternalChild.Widgets.Insert(0, value);
            }

            _content = value;
        }
    }

    public ScorpiaWindow()
    {
        IsModal = true;

        InternalChild = new VerticalStackPanel();
        
        HorizontalAlignment = HorizontalAlignment.Left;
        VerticalAlignment = VerticalAlignment.Top;
        
        InternalChild.Spacing = 8;

        InternalChild.Proportions.Add(new Proportion(ProportionType.Auto));
        InternalChild.Proportions.Add(new Proportion(ProportionType.Fill));

        Width = 2048;
        Height = 1328;

        // Background = Stylesheet.Current.Atlas.Regions["container"];
    }
}