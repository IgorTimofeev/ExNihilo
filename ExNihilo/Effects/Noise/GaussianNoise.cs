﻿using ExNihilo.Base;
using ExNihilo.Extensions.Processors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ExNihilo.Effects;

/// <summary>
/// Defines effect that allow the application of gaussian noise on an <see cref="Visual"/>
/// </summary>
public class GaussianNoise : Effect
{
    /// <summary>
    /// Seed for noise randomizer.
    /// </summary>
    public IntProperty Seed { get; set; } = new(0) { Min = 0, Max = 10 };
    /// <summary>
    /// Amount of noise (0-255).
    /// </summary>
    public ByteProperty Amount { get; set; } = new(0, byte.MaxValue, byte.MaxValue) { Min = 0, Max = byte.MaxValue };
    /// <summary>
    /// Define is noise monochrome or not.
    /// </summary>
    public BoolProperty Monochrome { get; set; } = new();

    /// <summary>
    /// <inheritdoc cref="GaussianNoise"/>
    /// </summary>
    public GaussianNoise() { }

    /// <summary>
    /// <inheritdoc cref="GaussianNoise"/>
    /// </summary>
    /// <param name="amount"><inheritdoc cref="Amount" path="/summary"/></param>
    /// <param name="isMonochome"><inheritdoc cref="Monochrome" path="/summary"/></param>
    public GaussianNoise(byte amount, bool isMonochome)
    {
        Amount.Value = amount;
        Monochrome.Value = isMonochome;
    }

    /// <summary>
    /// Set Seed value
    /// </summary>
    /// <param name="value"><inheritdoc cref="Seed" path="/summary"/></param>
    public GaussianNoise WithSeed(int value)
    {
        Seed.Value = value;
        return this;
    }
    /// <summary>
    /// Set Seed randomization parameters.
    /// </summary>
    /// <param name="min">Minimal randomization value. <inheritdoc cref="Seed" path="/summary"/></param>
    /// <param name="max">Maximal randomization value. <inheritdoc cref="Seed" path="/summary"/></param>
    public GaussianNoise WithRandomizedSeed(int min, int max)
    {
        Seed.WithRandomizedValue(min, max);
        return this;
    }

    /// <summary>
    /// Set Amount value
    /// </summary>
    /// <param name="value"><inheritdoc cref="Amount" path="/summary"/></param>
    public GaussianNoise WithAmount(byte value)
    {
        Amount.Value = value;
        return this;
    }
    /// <summary>
    /// Set Amount randomization parameters.
    /// </summary>
    /// <param name="min">Minimal randomization value. <inheritdoc cref="Amount" path="/summary"/></param>
    /// <param name="max">Maximal randomization value. <inheritdoc cref="Amount" path="/summary"/></param>
    public GaussianNoise WithRandomizedAmount(byte min, byte max)
    {
        Amount.WithRandomizedValue(min, max);
        return this;
    }

    /// <summary>
    /// Set Monochrome value
    /// </summary>
    /// <param name="value"><inheritdoc cref="Monochrome" path="/summary"/></param>
    public GaussianNoise IsMonochrome(bool value)
    {
        Monochrome.Value = value;
        return this;
    }

    public override void Render(Image image, GraphicsOptions graphicsOptions)
    {
        image.Mutate(x => { x.GaussianNoise(Seed, Amount, Monochrome); });
    }
}
