using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;

namespace Scorpia.Engine.Asset;

public class MyraMarkup : IAsset
{
    public Project Project { get; }
    
    public Stylesheet Stylesheet { get; }

    public MyraMarkup(Project project)
    {
        Project = project;
    }

    public MyraMarkup(Stylesheet stylesheet)
    {
        Stylesheet = stylesheet;
    }
}