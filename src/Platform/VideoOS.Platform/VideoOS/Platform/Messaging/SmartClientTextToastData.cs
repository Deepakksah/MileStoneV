using System;
using System.Drawing;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.Util.Images;

namespace VideoOS.Platform.Messaging;

public class SmartClientTextToastData : SmartClientToastData
{
	private VideoOSIconSourceBase _iconSource;

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use IconSource instead.", false)]
	public Icon Icon { get; }

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return _iconSource ?? VideoOS.Platform.Util.Images.ImageConverter.ConvertIconToIconSourceBase(Icon);
		}
		private set
		{
			_iconSource = value;
		}
	}

	public string PrimaryText { get; }

	public string SecondaryText { get; }

	public string CountText { get; }

	[Obsolete("This constructor is deprecated. Windows Forms controls are no longer supported. Use the SmartClientTextToastData(Guid, TimeSpan, Action<SmartClientToastData>, Action<SmartClientToastData>, Action<SmartClientToastData>, VideoOSIconSourceBase, string, string, string) instead.", false)]
	public SmartClientTextToastData(Guid id, TimeSpan duration, Action<SmartClientToastData> dismissed, Action<SmartClientToastData> activated, Action<SmartClientToastData> expired, Icon icon, string primaryText, string secondaryText, string countText)
		: base(id, duration, dismissed, activated, expired)
	{
		Icon = icon;
		PrimaryText = primaryText;
		SecondaryText = secondaryText;
		CountText = countText;
	}

	public SmartClientTextToastData(Guid id, TimeSpan duration, Action<SmartClientToastData> dismissed, Action<SmartClientToastData> activated, Action<SmartClientToastData> expired, VideoOSIconSourceBase iconSource, string primaryText, string secondaryText, string countText)
		: base(id, duration, dismissed, activated, expired)
	{
		IconSource = iconSource;
		PrimaryText = primaryText;
		SecondaryText = secondaryText;
		CountText = countText;
	}
}
