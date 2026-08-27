using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Common.SharedComponent.Options;
using VideoOS.Common.SharedComponent.Resources;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Common.SharedComponent;

public static class ResourcesUtility
{
	private const string ResourceFilename = "resourcesSharedComponent.xml";

	private static readonly OemResourcePackage OemResourcePackage;

	public static string ResourceFilePath
	{
		get
		{
			string text = typeof(ResourcesUtility).Assembly.GetName().CodeBase.ToLowerInvariant();
			if (text.StartsWith("file:///"))
			{
				text = text.Substring("file:///".Length);
			}
			return Path.Combine(Path.GetDirectoryName(text), "resourcesSharedComponent.xml");
		}
	}

	static ResourcesUtility()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(ResourceFilePath);
		OemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!OemResourcePackage.ValidateSignature())
		{
			MessageBox.Show(Strings.ResourceTamperedDialogText, Strings.ResourceTamperedDialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static string GetResourceString(string key)
	{
		string text = ClientProfileResources.ResourceManager.GetString(key, ClientProfileResources.Culture);
		if (!string.IsNullOrWhiteSpace(text))
		{
			text = ReplaceOemResource(text);
		}
		return text;
	}

	public static string GetOemResource(string name)
	{
		if (OemResourcePackage.GetResourceItem(name) is OemResourceItemString oemResourceItemString)
		{
			return oemResourceItemString.ResourceValue;
		}
		return string.Empty;
	}

	public static string ReplaceOemResource(string rawResourceString)
	{
		foreach (Match item in Regex.Matches(rawResourceString, "\\{\\S*\\}"))
		{
			string text = item.Value;
			if (!string.IsNullOrWhiteSpace(text))
			{
				text = text.Replace("{", string.Empty);
				text = text.Replace("}", string.Empty);
			}
			string oemResource = GetOemResource(text);
			rawResourceString = rawResourceString.Replace(item.Value, oemResource);
		}
		return rawResourceString;
	}

	public static Bitmap GetResourceImage(string key)
	{
		object obj = ClientProfileResources.ResourceManager.GetObject(key, CultureInfo.InvariantCulture);
		if (obj is Icon icon)
		{
			return icon.ToBitmap();
		}
		return obj as Bitmap;
	}

	public static string GetResourceString(OptionId optionId)
	{
		return GetResourceString("ClientProfileSetting" + optionId);
	}

	public static string GetResourceString(OptionShowHide optionShowHide)
	{
		return GetResourceString("ClientProfileValue" + optionShowHide);
	}

	public static string GetResourceString(OptionAvailable optionAvailable)
	{
		return GetResourceString("ClientProfileValue" + optionAvailable);
	}

	public static string GetResourceString(OptionYesNo optionYesNo)
	{
		return GetResourceString("ClientProfileValue" + optionYesNo);
	}

	public static string GetResourceString(OptionInclude optionInclude)
	{
		return GetResourceString("ClientProfileValue" + optionInclude);
	}

	public static string GetResourceString(OptionEmptySpaces optionEmptySpaces)
	{
		string key = string.Empty;
		switch (optionEmptySpaces)
		{
		case OptionEmptySpaces.CompanyLogo:
			key = "ClientProfileValueLogoMilestone";
			break;
		case OptionEmptySpaces.CustomLogo:
			key = "ClientProfileValueLogoCustom";
			break;
		case OptionEmptySpaces.Nothing:
			key = "ClientProfileValueNothing";
			break;
		case OptionEmptySpaces.BlackImage:
			key = "ClientProfileValueLogoBlackImage";
			break;
		}
		return GetResourceString(key);
	}

	public static string GetResourceString(OptionCameraErrors optionCameraErrors)
	{
		string key = string.Empty;
		switch (optionCameraErrors)
		{
		case OptionCameraErrors.ShowAsOverlay:
			key = "ClientProfileValueOverlayShow";
			break;
		case OptionCameraErrors.BlackImageWithOverlay:
			key = "ClientProfileValueOverlayBlackImage";
			break;
		case OptionCameraErrors.Hidden:
			key = "ClientProfileValueHideSeeHelp";
			break;
		}
		return GetResourceString(key);
	}

	public static string GetResourceString(OptionServerErrors optionServerErrors)
	{
		string key = string.Empty;
		switch (optionServerErrors)
		{
		case OptionServerErrors.ShowAsOverlay:
			key = "ClientProfileValueOverlayShow";
			break;
		case OptionServerErrors.Hidden:
			key = "ClientProfileValueHide";
			break;
		}
		return GetResourceString(key);
	}

	public static string GetResourceString(OptionViewGridSpacer optionViewGridSpacer)
	{
		string key = string.Empty;
		switch (optionViewGridSpacer)
		{
		case OptionViewGridSpacer.Hide:
			key = "ClientProfileValueHide";
			break;
		case OptionViewGridSpacer.Pixel1:
			key = "ClientProfileValuePixelOne";
			break;
		case OptionViewGridSpacer.Pixel2:
			key = "ClientProfileValuePixelTwo";
			break;
		case OptionViewGridSpacer.Pixel3:
			key = "ClientProfileValuePixelThree";
			break;
		case OptionViewGridSpacer.Pixel4:
			key = "ClientProfileValuePixelFour";
			break;
		}
		return GetResourceString(key);
	}

	public static string GetResourceString(OptionApplicationMaximization optionApplicationMaximization)
	{
		string key = string.Empty;
		switch (optionApplicationMaximization)
		{
		case OptionApplicationMaximization.FullScreen:
			key = "ClientProfileValueMaximizeFullscreen";
			break;
		case OptionApplicationMaximization.NormalWindow:
			key = "ClientProfileValueMaximizeNormal";
			break;
		}
		return GetResourceString(key);
	}

	public static string GetResourceString(OptionDefaultImageQuality optionDefaultImageQuality)
	{
		return GetResourceString("ClientProfileValue" + optionDefaultImageQuality);
	}

	public static string GetResourceString(OptionDefaultFrameRate optionDefaultFrameRate)
	{
		return GetResourceString("ClientProfileValue" + optionDefaultFrameRate);
	}

	public static string GetResourceString(OptionApplicationStartMode optionApplicationStartMode)
	{
		return GetResourceString("ClientProfileValue" + optionApplicationStartMode);
	}

	public static string GetResourceString(OptionStartView optionStartView)
	{
		return GetResourceString("ClientProfileValue" + optionStartView);
	}

	public static string GetResourceString(OptionBookmarkMode optionBookmarkMode)
	{
		string key = string.Empty;
		switch (optionBookmarkMode)
		{
		case OptionBookmarkMode.Quick:
			key = "ClientProfileValueBookmarkQuick";
			break;
		case OptionBookmarkMode.Detail:
			key = "ClientProfileValueBookmarkAddDetail";
			break;
		case OptionBookmarkMode.Unavailable:
			key = "ClientProfileValueUnavailable";
			break;
		}
		return GetResourceString(key);
	}

	public static string GetResourceString(OptionExportPath optionExportPath)
	{
		return GetResourceString("ClientProfileValue" + optionExportPath);
	}

	public static string GetResourceString(OptionPrintReportHeadline optionPrintReportHeadline)
	{
		return GetResourceString("ClientProfileValue" + optionPrintReportHeadline);
	}

	public static string GetResourceString(OptionMaxDecodingThreads optionMaxDecodingThreads)
	{
		return GetResourceString("ClientProfileValue" + optionMaxDecodingThreads);
	}

	public static string GetResourceString(OptionDeinterlacing optionDeinterlacing)
	{
		string key = string.Empty;
		switch (optionDeinterlacing)
		{
		case OptionDeinterlacing.Default:
			key = "ClientProfileValueNoFilter";
			break;
		case OptionDeinterlacing.Bob_Top:
			key = "ClientProfileValueBobTop";
			break;
		case OptionDeinterlacing.Bob_Bottom:
			key = "ClientProfileValueBobBottom";
			break;
		case OptionDeinterlacing.Content_Adaptive_Vertical_Temporal:
			key = "ClientProfileValueContentAdaptiveVerticalTemporal";
			break;
		}
		return GetResourceString(key);
	}

	public static string GetResourceString(OptionTimeZone optionTimeZone)
	{
		return GetResourceString("ClientProfileValueTimeZone" + optionTimeZone);
	}

	public static string GetResourceString(OptionDestinationTypes optionDestinationTypes)
	{
		string key = string.Empty;
		switch (optionDestinationTypes)
		{
		case OptionDestinationTypes.ToDisk:
			key = "ClientProfileValueToDisk";
			break;
		case OptionDestinationTypes.ToMediaBurner:
			key = "ClientProfileValueToMediaBurner";
			break;
		case OptionDestinationTypes.Both:
			key = "ClientProfileValueToDiskMediaBurner";
			break;
		}
		return GetResourceString(key);
	}

	public static string GetResourceString(OptionRequired optionRequired)
	{
		return GetResourceString("ClientProfileValue" + optionRequired);
	}

	public static string GetResourceString(OptionEncryptionStrength optionEncryptionStrength)
	{
		return GetResourceString("ClientProfileValueEncryptStregth" + optionEncryptionStrength);
	}

	public static string GetResourceString(OptionSkipGaps optionSkipGaps)
	{
		return GetResourceString("ClientProfileValue" + optionSkipGaps);
	}

	public static string GetResourceString(OptionPtzPointAndClickMode optionPtzPointAndClickMode)
	{
		return GetResourceString("ClientProfileValue" + optionPtzPointAndClickMode);
	}

	public static string GetResourceString(OptionExportVideoClipType optionExportVideoClipType)
	{
		string key = string.Empty;
		switch (optionExportVideoClipType)
		{
		case OptionExportVideoClipType.Audio:
			key = "ClientProfileValueAudio";
			break;
		case OptionExportVideoClipType.Video:
			key = "ClientProfileValueVideo";
			break;
		case OptionExportVideoClipType.Both:
			key = "ClientProfileValueAudioVideo";
			break;
		}
		return GetResourceString(key);
	}
}
