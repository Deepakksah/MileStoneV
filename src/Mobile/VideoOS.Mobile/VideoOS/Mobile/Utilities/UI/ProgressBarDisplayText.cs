using System;

namespace VideoOS.Mobile.Utilities.UI;

[Flags]
public enum ProgressBarDisplayText
{
	Percentage = 1,
	CustomText = 2,
	CustomTextAndPercentage = 3
}
