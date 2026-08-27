using System.Drawing;

namespace VideoOS.LPR.Common.Config;

public interface IRecognitionSettings
{
	RectangleF Area { get; set; }

	byte[] Mask { get; set; }

	Size MaskSize { get; set; }

	float MaxHeight { get; set; }

	float MinHeight { get; set; }

	Size FixedMaskSize { get; }

	void ApplyAll(IRecognitionSettings recognitionSettings);
}
