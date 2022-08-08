using System.Collections.Generic;

namespace Scorpia.Engine.Asset;

internal class SpritesheetFrame
{
    public string Filename { get; set; }
    
    public Rect Frame { get; set; }
    
    public bool Rotated { get; set; }
    
    public bool Trimmed { get; set; }
    
    public Rect SpriteSourceSize { get; set; }
    
    public Size SourceSize { get; set; }
    
    public Point Pivot { get; set; }

    public class Point
    {
        public float X { get; set; }
        
        public float Y { get; set; }
    }

    public class Size
    {
        public int W { get; set; }
        
        public int H { get; set; }
    }

    public class Rect
    {
        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int W { get; set; }
        
        public int H { get; set; }
    }
}

internal class SpritesheetDescriptor
{
    public IEnumerable<SpritesheetFrame> Frames { get; set; }
}