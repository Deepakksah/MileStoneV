using System.Linq;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;

public class HevcSampleEntry : VisualSampleEntry
{
	public HevcDecoderConfigurationBox HevcDecoderConfigurationBox
	{
		get
		{
			return base.Children[0] as HevcDecoderConfigurationBox;
		}
		set
		{
			if (!base.Children.Any() || !(base.Children[0] is HevcDecoderConfigurationBox))
			{
				base.Children.Insert(0, value);
			}
			else
			{
				base.Children[0] = value;
			}
		}
	}

	public HevcSampleEntry(string boxType)
		: base(boxType)
	{
	}
}
