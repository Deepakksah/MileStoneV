namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class GenericEntryCountParentBox : GenericEntryCountBox
{
	public override bool CanHaveChildren => true;

	public GenericEntryCountParentBox(string boxType)
		: base(boxType)
	{
	}

	public override void AddChild(IBoxParse child)
	{
		base.AddChild(child);
		EntryCount = (uint)base.Children.Count;
	}
}
