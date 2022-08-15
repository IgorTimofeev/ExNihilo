﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using ExNihilo.Base.Interfaces;
using ExNihilo.Extensions.Processors;
using ExNihilo.Processors;
using ExNihilo.Base.Properties;

namespace ExNihilo.Effects;

/// <summary>
/// Defines effect that allow the translate Euclidean coordinates to Polar coordinats and vise versa on an <see cref="Drawable"/>
/// </summary>
public class PolarCoordinates : Effect
{
    /// <summary>
    /// The <see cref="PolarConversionType"/> to perform the translate.
    /// </summary>
    public EnumProperty<PolarConversionType> ConversionType { get; } = new(PolarConversionType.CartesianToPolar);

    /// <summary>
    /// <inheritdoc cref="PolarCoordinates"/>
    /// </summary>
    public PolarCoordinates() { }

    /// <summary>
    /// <inheritdoc cref="PolarCoordinates"/>
    /// </summary>
    /// <param name="conversionType"><inheritdoc cref="ConversionType" path="/summary"/></param>
    public PolarCoordinates(PolarConversionType conversionType)
    {
        ConversionType.Value = conversionType;
    }
    /// <summary>
    /// Set polar conversion Type value
    /// </summary>
    /// <param name="value"><inheritdoc cref="Type" path="/summary"/></param>
    public PolarCoordinates WithType(PolarConversionType value)
    {
        ConversionType.Value = value;
        return this;
    }

    public override void Render(Image image, GraphicsOptions graphicsOptions) =>
        image.Mutate(x => x.PolarCoordinates(ConversionType));
}
