using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Platform.UI.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Strings
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.Platform.UI.Resources.Strings", typeof(Strings).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string ItemPickerViewModel_DefaultEmptyListOverlayText => ResourceManager.GetString("ItemPickerViewModel_DefaultEmptyListOverlayText", resourceCulture);

	public static string ItemPickerViewModel_DefaultErrorText => ResourceManager.GetString("ItemPickerViewModel_DefaultErrorText", resourceCulture);

	public static string ItemPickerViewModel_DefaultSearchPlaceholderText => ResourceManager.GetString("ItemPickerViewModel_DefaultSearchPlaceholderText", resourceCulture);

	public static string ItemPickerViewModel_DefaultTableHeaderContent => ResourceManager.GetString("ItemPickerViewModel_DefaultTableHeaderContent", resourceCulture);

	public static string ItemPickerViewModel_RemoveAllButtonText => ResourceManager.GetString("ItemPickerViewModel_RemoveAllButtonText", resourceCulture);

	public static string ItemPickerViewModel_RemoveButtonTooltip => ResourceManager.GetString("ItemPickerViewModel_RemoveButtonTooltip", resourceCulture);

	public static string ItemPickerWindow_DefaultAcceptBtnContent => ResourceManager.GetString("ItemPickerWindow_DefaultAcceptBtnContent", resourceCulture);

	public static string ItemPickerWindow_DefaultCancelBtnContent => ResourceManager.GetString("ItemPickerWindow_DefaultCancelBtnContent", resourceCulture);

	public static string ItemPickerWindow_DefaultDescriptionText => ResourceManager.GetString("ItemPickerWindow_DefaultDescriptionText", resourceCulture);

	public static string ItemPickerWindow_DefaultTitle => ResourceManager.GetString("ItemPickerWindow_DefaultTitle", resourceCulture);

	public static string VideoOSActionList_OverflowButtonTooltip => ResourceManager.GetString("VideoOSActionList_OverflowButtonTooltip", resourceCulture);

	public static string VideoOSBadge_CountExceededLimitedText => ResourceManager.GetString("VideoOSBadge_CountExceededLimitedText", resourceCulture);

	public static string VideoOSCalendar_ButtonToday => ResourceManager.GetString("VideoOSCalendar_ButtonToday", resourceCulture);

	public static string VideoOSContextMenu_ClearCheckedItems => ResourceManager.GetString("VideoOSContextMenu_ClearCheckedItems", resourceCulture);

	public static string VideoOSContextMenu_NoMatchingItems => ResourceManager.GetString("VideoOSContextMenu_NoMatchingItems", resourceCulture);

	public static string VideoOSContextMenu_SearchPlaceHolderText => ResourceManager.GetString("VideoOSContextMenu_SearchPlaceHolderText", resourceCulture);

	public static string VideoOSDropDown_SelectedItemsValue => ResourceManager.GetString("VideoOSDropDown_SelectedItemsValue", resourceCulture);

	public static string VideoOSMessageBox_ButtonCancel => ResourceManager.GetString("VideoOSMessageBox_ButtonCancel", resourceCulture);

	public static string VideoOSMessageBox_ButtonClose => ResourceManager.GetString("VideoOSMessageBox_ButtonClose", resourceCulture);

	public static string VideoOSMessageBox_ButtonNo => ResourceManager.GetString("VideoOSMessageBox_ButtonNo", resourceCulture);

	public static string VideoOSMessageBox_ButtonOK => ResourceManager.GetString("VideoOSMessageBox_ButtonOK", resourceCulture);

	public static string VideoOSMessageBox_ButtonYes => ResourceManager.GetString("VideoOSMessageBox_ButtonYes", resourceCulture);

	public static string VideoOSMessageBox_CheckBoxRememberChoice => ResourceManager.GetString("VideoOSMessageBox_CheckBoxRememberChoice", resourceCulture);

	public static string VideoOSTabControl_OverflowButtonTooltip => ResourceManager.GetString("VideoOSTabControl_OverflowButtonTooltip", resourceCulture);

	public Strings()
	{
	}
}
