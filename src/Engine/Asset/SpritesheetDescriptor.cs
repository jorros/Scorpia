using System.Collections.Generic;
using System.Drawing;

namespace Scorpia.Engine.Asset;

internal class SpritesheetFrame
{
    public string Name { get; set; }
    public Point Position { get; set; }
    public Point Size { get; set; }
    public Rectangle? Split { get; set; }
    public bool Rotated { get; set; }
    public Point OriginalSize { get; set; }
    public Point Offset { get; set; }
    public int Index { get; set; }
}

internal class SpritesheetDescriptor
{
    public IEnumerable<SpritesheetFrame> Frames { get; set; }
}