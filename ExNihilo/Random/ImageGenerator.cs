﻿using ExNihilo.Base;

namespace ExNihilo.Rnd;

/// <summary>
/// Allows you to automate randomization and generation of captchas.
/// </summary>
public class ImageGenerator
{
    private Container _template;
    private int[]? _seeds;
    private Dictionary<int, string[]> _captchaText = new();

    /// <summary>
    /// <inheritdoc cref="ImageGenerator"/>
    /// </summary>
    public ImageGenerator(Container template)
    {
        _template = template;
    }

    /// <summary>
    /// Set template for generator.
    /// </summary>
    public ImageGenerator WithTemplate(Container template)
    {
        _template = template;
        return this;
    }
    /// <summary>
    /// Set seeds for randomization.
    /// </summary>
    public ImageGenerator WithSeeds(int[] seeds)
    {
        _seeds = seeds;
        return this;
    }

    /// <summary>
    /// Set seeds for randomization.
    /// </summary>
    public ImageGenerator WithSeed(int seed)
    {
        _seeds = new int[]
        {
            seed
        };

        return this;
    }

    /// <summary>
    /// Set count of seeds for randomization.
    /// </summary>
    public ImageGenerator WithSeedsCount(int count)
    {
        _seeds = Enumerable.Range(0, count).ToArray();
        return this;
    }
    /// <summary>  Set manual input for captchas. </summary>
    public ImageGenerator WithCaptchaInput(string[] input, int index = 0)
    {
        _captchaText[index] = input;
        return this;
    }
    /// <summary>
    /// Generate collection of captchas by defined seed and/or input.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when serial number is outside valid     range</exception>
    public IEnumerable<ImageResult> Generate()
    {
        if (_seeds is null || _seeds.Length == 0)
            WithSeedsCount(_captchaText.Values.FirstOrDefault()?.Length ?? 1);

        ValidateFields();

        return _captchaText.Keys.Count > 0 ? GenerateManualCaptcha() : GenerateRandomizedCaptcha();
    }

    private IEnumerable<ImageResult> GenerateRandomizedCaptcha()
    {
        Dictionary<int, List<ICaptcha>> captchaIndexMapping = GetContainerCaptchas(_template);

        for (int seedId = 0; seedId < _seeds!.Length; seedId++)
        {
            int seed = _seeds[seedId];
            _template.Randomize(new Random(seed));

            List<string> captchaStrings = new();
            foreach (int captchaIndex in captchaIndexMapping.Keys)
            {
                foreach (var captchaVisual in captchaIndexMapping[captchaIndex])
                    captchaStrings.Add(captchaVisual.Text);
            }

            yield return new(seed, _template.Render(), captchaStrings.ToArray());
        }
    }

    private IEnumerable<ImageResult> GenerateManualCaptcha()
    {
        Dictionary<int, List<ICaptcha>> captchaIndexMapping = GetContainerCaptchas(_template);
        int seed;
        List<string> captchaStrings = new();

        for (int seedID = 0; seedID < _seeds!.Length; seedID++)
        {
            seed = _seeds[seedID];
            _template.Randomize(new Random(seed));

            captchaStrings.Clear();

            foreach (int captchaIndex in captchaIndexMapping.Keys.OrderBy(x => x))
            {
                foreach (var captchaVisual in captchaIndexMapping[captchaIndex])
                {
                    if (_captchaText.ContainsKey(captchaIndex))
                        captchaVisual.Text = _captchaText[captchaIndex][seedID];

                    captchaStrings.Add(captchaVisual.Text);
                }
            }

            yield return new(seed, _template.Render(), captchaStrings.ToArray());
        }
    }

    private void ValidateFields()
    {
        int min, max;

        min = _captchaText.Values.Min(x => (int?) x.Length) ?? 0;
        max = _captchaText.Values.Max(x => (int?) x.Length) ?? 0;

        if (min != max)
            throw new ArgumentException("Captcha inputs should have same input array size. ");

        if (max > 0 && _seeds?.Length != max)
            throw new ArgumentException("Seed length must be equal captcha input array size");

        if (min == 0 && _seeds?.Length == 0)
            throw new ArgumentException("For captcha generation you should specify captha input or seeds");
    }

    protected static Dictionary<int, List<ICaptcha>> GetContainerCaptchas(Container container)
    {
        Dictionary<int, List<ICaptcha>> captchas = new();
        
        foreach (var visual in container.Children)
        {
            if (visual is ICaptcha captcha)
            {
                if(captchas.ContainsKey(captcha.Index))
                    captchas[captcha.Index].Add(captcha);
                else
                    captchas[captcha.Index] = new List<ICaptcha>() {
                        captcha
                    };
            }
        }

        return captchas;
    }
}