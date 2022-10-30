using System.Collections.Generic;
using System.Drawing;
using Scorpia.Engine.Maths;

namespace Scorpia.Engine.Asset;

internal class SpritesheetFrame
{
    public string Name { get; set; }
    public Point Position { get; set; }
    public Size Size { get; set; }
    public Box? Split { get; set; }
    public bool Rotated { get; set; }
    public Size OriginalSize { get; set; }
    public Point Offset { get; set; }
    public int Index { get; set; }
}

internal class SpritesheetDescriptor
{
    public IEnumerable<SpritesheetFrame> Frames { get; set; }
}