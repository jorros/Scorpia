using System;
using System.Collections.Generic;
using Scorpia.Engine.Asset.Markup;
using static SDL2.SDL;

namespace Scorpia.Engine.Asset.Font;

public interface IFontRenderer
{
    Type Type { get; }

    IEnumerable<(IntPtr texture, SDL_Rect target)> Render(Font font, IBlock block, OffsetVector position, ref OffsetVector cursor);

    void CalculateSize(string text, Font font, IBlock block, ref OffsetVector cursor);

    void Clear();
}