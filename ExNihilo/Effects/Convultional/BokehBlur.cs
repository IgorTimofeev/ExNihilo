﻿using ExNihilo.Base;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ExNihilo.Effects;

/// <summary>
/// Defines effect that allow the application of bokeh blur on an <see cref="Visual"/>
/// </summary>
public class BokehBlur : Effect
{
    public override void Render(Image image, GraphicsOptions graphicsOptions) =>
        image.Mutate(x => x.BokehBlur());
}