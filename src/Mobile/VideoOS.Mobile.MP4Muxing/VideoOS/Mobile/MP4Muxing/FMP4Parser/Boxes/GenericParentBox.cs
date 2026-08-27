namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class GenericParentBox : Box
{
	public override bool CanHaveChildren => true;

	public GenericParentBox(string boxType)
	{
		base.BoxType = boxType;
	}
}
