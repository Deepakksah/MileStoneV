using System;
using System.Windows.Media.Imaging;

namespace VideoOS.LPR.Common.UI.LPRServiceStatus;

public class VideoSourceStatusModel
{
	private static BitmapImage OkIcon;

	private static BitmapImage WarningIcon;

	private static BitmapImage ErrorIcon;

	public VideoSourceStatusModelState State { get; set; }

	public string Text { get; set; }

	public string ErrorText { get; set; }

	public BitmapSource Icon
	{
		get
		{
			if (OkIcon == null)
			{
				OkIcon = new BitmapImage(new Uri("pack://application:,,,/VideoOS.LPR.Common;component/UI/LPRServiceStatus/Resources/Ok.png"));
				WarningIcon = new BitmapImage(new Uri("pack://application:,,,/VideoOS.LPR.Common;component/UI/LPRServiceStatus/Resources/Warning.png"));
				ErrorIcon = new BitmapImage(new Uri("pack://application:,,,/VideoOS.LPR.Common;component/UI/LPRServiceStatus/Resources/Error.png"));
				OkIcon.Freeze();
				WarningIcon.Freeze();
				ErrorIcon.Freeze();
			}
			return State switch
			{
				VideoSourceStatusModelState.Warning => WarningIcon, 
				VideoSourceStatusModelState.Error => ErrorIcon, 
				_ => OkIcon, 
			};
		}
	}
}
