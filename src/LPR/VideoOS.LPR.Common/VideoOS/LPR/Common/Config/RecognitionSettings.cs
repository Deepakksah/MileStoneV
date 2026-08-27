using System;
using System.Drawing;

namespace VideoOS.LPR.Common.Config;

public class RecognitionSettings : IRecognitionSettings
{
	public RectangleF Area { get; set; }

	public byte[] Mask { get; set; }

	public Size MaskSize { get; set; }

	public float MaxHeight { get; set; }

	public float MinHeight { get; set; }

	public Size FixedMaskSize => new Size(32, 32);

	public RecognitionSettings()
	{
	}

	public RecognitionSettings(IRecognitionSettings recognitionSettings)
	{
		ApplyAll(recognitionSettings);
	}

	public void ApplyAll(IRecognitionSettings recognitionSettings)
	{
		if (recognitionSettings == null)
		{
			throw new ArgumentException("The parameter recognitionSettings should not be null");
		}
		Area = recognitionSettings.Area;
		Mask = recognitionSettings.Mask;
		MaskSize = recognitionSettings.MaskSize;
		MinHeight = recognitionSettings.MinHeight;
		MaxHeight = recognitionSettings.MaxHeight;
	}
}
