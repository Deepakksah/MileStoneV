using System;

namespace VideoOS.Platform.Client.Hid;

public sealed class HidButtonInput : HidInput
{
	public static readonly Guid ActionStartRecording = new Guid("2C8C6992-72F3-41f4-AACD-11CBFD24E86D");

	public static readonly Guid ActionStopRecording = new Guid("3CF50B99-26DD-4E04-A0A8-52E1180E205E");

	public static readonly Guid ActionToggleRecording = new Guid("A76DE0FF-3123-478A-ADD0-8128FF58DF7C");

	public static readonly Guid ActionPlayForward = new Guid("144F4A1D-FC2C-41a5-B978-F8D42151F27F");

	public static readonly Guid ActionPlayBackwards = new Guid("0B0AE2A6-CD42-4d74-9803-5DD0C11DABCF");

	public static readonly Guid ActionStopPlayback = new Guid("7D721F53-ECD0-4736-B108-3AE369595444");

	public static readonly Guid ActionTogglePlayback = new Guid("FFC30AFF-D155-4b91-A234-8375033EA4E3");

	public static readonly Guid ActionNextFrame = new Guid("04610607-C457-4ba7-B3BD-2168A6B7E263");

	public static readonly Guid ActionPreviousFrame = new Guid("BCB98436-DA4C-491d-8789-A534D87C731A");

	public static readonly Guid ActionNextSequence = new Guid("E8598AB4-88EB-49b7-AE57-0DF063A46D02");

	public static readonly Guid ActionPreviousSequence = new Guid("AFCDEF26-D34C-49f0-BDAF-87815731B531");

	public static readonly Guid ActionOutgoingAudio = new Guid("6DF0A15F-CFDA-4eba-A3C9-71B47E3955D8");

	public static readonly Guid ActionDatabaseStart = new Guid("CE891D7A-0C96-429b-A438-7C584C41DC78");

	public static readonly Guid ActionDatabaseEnd = new Guid("1AF3F763-99BC-4837-9586-CA66A0327B9F");

	public static readonly Guid ActionMoveViewItemSelectionUp = new Guid("D155348B-E990-4332-862F-51D160FFF250");

	public static readonly Guid ActionMoveViewItemSelectionLeft = new Guid("16DD9CC6-03A0-4330-890B-1F72B2DED290");

	public static readonly Guid ActionMoveViewItemSelectionDown = new Guid("B183C00A-3A6C-4a50-B5DC-D61847FC2625");

	public static readonly Guid ActionMoveViewItemSelectionRight = new Guid("D5C26632-1492-4eca-A546-5ADAC7551288");

	public static readonly Guid ActionToggleSelectedViewItemMaximizedState = new Guid("CF3931AF-ECE1-46a7-91C2-DF5BDFEC1A4F");

	public static readonly Guid ActionZoomInStepwise = new Guid("D06DDA54-09C8-4330-9EFB-32DBE7E17B24");

	public static readonly Guid ActionZoomOutStepwise = new Guid("6A7F1F06-71DD-42b2-B39A-F41DBC0E69BC");

	public static readonly Guid ActionBrowsePlayForwardToggle = new Guid("F309634B-4DED-4BEA-BDDD-9CA7D6472060");

	public static readonly Guid ActionMoveTabSelectionPrevious = new Guid("461BBF7B-4B9B-496C-A78B-48F0176D8817");

	public static readonly Guid ActionMoveTabSelectionNext = new Guid("34AA0246-8BBF-4DF4-A742-BB88AF909A25");

	public static readonly Guid ActionIncreaseTimelineSpan = new Guid("B537B73D-AE8A-413F-9021-BA892541626E");

	public static readonly Guid ActionDecreaseTimelineSpan = new Guid("11D8A5D4-6059-4DB9-8541-DD82FFE2E71F");

	private HidButtonValueData _valueData = new HidButtonValueData();

	public bool SupportValueParameters { get; }

	public HidButtonValueData Value
	{
		get
		{
			return _valueData;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullMIPException("Value");
			}
			if (!_valueData.Equals(value))
			{
				_valueData = value;
				FireValueChangedEvent(new HidButtonValueChangedEventArgs(this, value));
			}
		}
	}

	public HidButtonInput(Guid id, string name, Guid defaultActionId)
		: base(id, name, defaultActionId)
	{
	}

	public HidButtonInput(Guid id, string name)
		: base(id, name, HidInput.ActionNoAction)
	{
	}

	public HidButtonInput(Guid id, string name, Guid defaultActionId, bool supportValueParameters)
		: base(id, name, defaultActionId)
	{
		SupportValueParameters = supportValueParameters;
	}
}
