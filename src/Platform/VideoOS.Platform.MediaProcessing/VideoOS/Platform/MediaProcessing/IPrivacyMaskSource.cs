using System.Collections.Generic;
using System.Drawing;

namespace VideoOS.Platform.MediaProcessing;

public interface IPrivacyMaskSource
{
	public enum GridSize
	{
		Size_0,
		Size_1x1,
		Size_8x8,
		Size_16x16,
		Size_32x32,
		Size_64x64
	}

	public abstract class MaskSelectorBase
	{
		public IEnumerable<Point> Coordinates;

		protected MaskSelectorBase(IEnumerable<Point> coordinates)
		{
			Coordinates = coordinates;
		}
	}

	public class SolidMaskSelector : MaskSelectorBase
	{
		public Color Color;

		public SolidMaskSelector(Color color, IEnumerable<Point> coordinates)
		{
			Coordinates = coordinates;
			Color = color;
		}
	}

	public class BlurryMaskSelector : MaskSelectorBase
	{
		public int Level;

		public BlurryMaskSelector(int level, IEnumerable<Point> coordinates)
		{
			Coordinates = coordinates;
			Level = level;
		}
	}

	IEnumerable<MaskSelectorBase> Selectors { get; }

	GridSize Size { get; }
}
