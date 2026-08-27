using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileExportChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> PanesPlaybackExportValues => base.Wrapper.GetEnumValues("PanesPlaybackExport");

	public string PanesPlaybackExport
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesPlaybackExport");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackExport", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportDestinationTypeValues => base.Wrapper.GetEnumValues("ExportDestinationType");

	public string ExportDestinationType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportDestinationType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportDestinationType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportDestinationTypeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportDestinationTypeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportDestinationTypeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsPlaybackExportPathValues => base.Wrapper.GetEnumValues("FunctionsPlaybackExportPath");

	public string FunctionsPlaybackExportPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsPlaybackExportPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackExportPath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string FunctionsPlaybackExportPathCustom
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsPlaybackExportPathCustom");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackExportPathCustom", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsPlaybackExportPathCustomLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsPlaybackExportPathCustomLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackExportPathCustomLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportPrivacyMaskValues => base.Wrapper.GetEnumValues("ExportPrivacyMask");

	public string ExportPrivacyMask
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportPrivacyMask");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportPrivacyMask", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportPrivacyMaskLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportPrivacyMaskLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportPrivacyMaskLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportSmartClientPlayerFormatValues => base.Wrapper.GetEnumValues("ExportSmartClientPlayerFormat");

	public string ExportSmartClientPlayerFormat
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerFormat");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerFormat", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerFormatLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerFormatLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerFormatLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportSmartClientPlayerIncludePlayerValues => base.Wrapper.GetEnumValues("ExportSmartClientPlayerIncludePlayer");

	public string ExportSmartClientPlayerIncludePlayer
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerIncludePlayer");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerIncludePlayer", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerIncludePlayerLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerIncludePlayerLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerIncludePlayerLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportSmartClientPlayerLockForReExportValues => base.Wrapper.GetEnumValues("ExportSmartClientPlayerLockForReExport");

	public string ExportSmartClientPlayerLockForReExport
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerLockForReExport");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerLockForReExport", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerLockForReExportLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerLockForReExportLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerLockForReExportLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportSmartClientPlayerEncryptDatabasesValues => base.Wrapper.GetEnumValues("ExportSmartClientPlayerEncryptDatabases");

	public string ExportSmartClientPlayerEncryptDatabases
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerEncryptDatabases");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerEncryptDatabases", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerEncryptDatabasesLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerEncryptDatabasesLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerEncryptDatabasesLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string ExportSmartClientPlayerEncryptPassword
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerEncryptPassword");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerEncryptPassword", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerEncryptPasswordLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerEncryptPasswordLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerEncryptPasswordLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportSmartClientPlayerEncryptionStrengthValues => base.Wrapper.GetEnumValues("ExportSmartClientPlayerEncryptionStrength");

	public string ExportSmartClientPlayerEncryptionStrength
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerEncryptionStrength");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerEncryptionStrength", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerEncryptionStrengthLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerEncryptionStrengthLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerEncryptionStrengthLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportSmartClientPlayerGlobalCommentsModeValues => base.Wrapper.GetEnumValues("ExportSmartClientPlayerGlobalCommentsMode");

	public string ExportSmartClientPlayerGlobalCommentsMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerGlobalCommentsMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerGlobalCommentsMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerGlobalCommentsModeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerGlobalCommentsModeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerGlobalCommentsModeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string ExportSmartClientPlayerGlobalCommentsContent
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerGlobalCommentsContent");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerGlobalCommentsContent", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerGlobalCommentsContentLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerGlobalCommentsContentLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerGlobalCommentsContentLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportSmartClientPlayerIndividualCommentsModeValues => base.Wrapper.GetEnumValues("ExportSmartClientPlayerIndividualCommentsMode");

	public string ExportSmartClientPlayerIndividualCommentsMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerIndividualCommentsMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerIndividualCommentsMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerIndividualCommentsModeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerIndividualCommentsModeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerIndividualCommentsModeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportSmartClientPlayerSignDataValues => base.Wrapper.GetEnumValues("ExportSmartClientPlayerSignData");

	public string ExportSmartClientPlayerSignData
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportSmartClientPlayerSignData");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerSignData", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportSmartClientPlayerSignDataLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportSmartClientPlayerSignDataLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportSmartClientPlayerSignDataLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportVideoClipFormatValues => base.Wrapper.GetEnumValues("ExportVideoClipFormat");

	public string ExportVideoClipFormat
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportVideoClipFormat");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipFormat", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportVideoClipFormatLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportVideoClipFormatLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipFormatLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportVideoClipTypeValues => base.Wrapper.GetEnumValues("ExportVideoClipType");

	public string ExportVideoClipType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportVideoClipType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportVideoClipTypeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportVideoClipTypeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipTypeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportVideoClipOutputTypeValues => base.Wrapper.GetEnumValues("ExportVideoClipOutputType");

	public string ExportVideoClipOutputType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportVideoClipOutputType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipOutputType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportVideoClipOutputTypeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportVideoClipOutputTypeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipOutputTypeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportVideoClipTimestampValues => base.Wrapper.GetEnumValues("ExportVideoClipTimestamp");

	public string ExportVideoClipTimestamp
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportVideoClipTimestamp");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipTimestamp", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportVideoClipTimestampLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportVideoClipTimestampLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipTimestampLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportVideoClipFrameRateValues => base.Wrapper.GetEnumValues("ExportVideoClipFrameRate");

	public string ExportVideoClipFrameRate
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportVideoClipFrameRate");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipFrameRate", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportVideoClipFrameRateLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportVideoClipFrameRateLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipFrameRateLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportVideoClipTextsModeValues => base.Wrapper.GetEnumValues("ExportVideoClipTextsMode");

	public string ExportVideoClipTextsMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportVideoClipTextsMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipTextsMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ExportVideoClipTextsContent
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportVideoClipTextsContent");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipTextsContent", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportVideoClipTextsContentLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportVideoClipTextsContentLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipTextsContentLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportVideoClipCodecPropertiesValues => base.Wrapper.GetEnumValues("ExportVideoClipCodecProperties");

	public string ExportVideoClipCodecProperties
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportVideoClipCodecProperties");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipCodecProperties", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportVideoClipCodecPropertiesLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportVideoClipCodecPropertiesLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportVideoClipCodecPropertiesLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeMediaPlayerFormatValues => base.Wrapper.GetEnumValues("ExportMergeMediaPlayerFormat");

	public string ExportMergeMediaPlayerFormat
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeMediaPlayerFormat");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeMediaPlayerFormat", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeMediaPlayerFormatLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeMediaPlayerFormatLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeMediaPlayerFormatLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeVideoClipTypeValues => base.Wrapper.GetEnumValues("ExportMergeVideoClipType");

	public string ExportMergeVideoClipType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeVideoClipType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeVideoClipType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeVideoClipTypeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeVideoClipTypeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeVideoClipTypeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeQualityValues => base.Wrapper.GetEnumValues("ExportMergeQuality");

	public string ExportMergeQuality
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeQuality");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeQuality", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeQualityLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeQualityLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeQualityLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeFramesPerSecondValues => base.Wrapper.GetEnumValues("ExportMergeFramesPerSecond");

	public string ExportMergeFramesPerSecond
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeFramesPerSecond");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeFramesPerSecond", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeFramesPerSecondLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeFramesPerSecondLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeFramesPerSecondLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeResolutionValues => base.Wrapper.GetEnumValues("ExportMergeResolution");

	public string ExportMergeResolution
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeResolution");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeResolution", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeResolutionLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeResolutionLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeResolutionLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeFormatValues => base.Wrapper.GetEnumValues("ExportMergeFormat");

	public string ExportMergeFormat
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeFormat");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeFormat", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeFormatLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeFormatLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeFormatLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeSequenceTypeValues => base.Wrapper.GetEnumValues("ExportMergeSequenceType");

	public string ExportMergeSequenceType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeSequenceType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeSequenceType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeSequenceTypeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeSequenceTypeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeSequenceTypeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergePreventUpscalingValues => base.Wrapper.GetEnumValues("ExportMergePreventUpscaling");

	public string ExportMergePreventUpscaling
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergePreventUpscaling");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergePreventUpscaling", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergePreventUpscalingLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergePreventUpscalingLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergePreventUpscalingLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeAddPaddingValues => base.Wrapper.GetEnumValues("ExportMergeAddPadding");

	public string ExportMergeAddPadding
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeAddPadding");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeAddPadding", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeAddPaddingLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeAddPaddingLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeAddPaddingLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeIncludeTimestampsValues => base.Wrapper.GetEnumValues("ExportMergeIncludeTimestamps");

	public string ExportMergeIncludeTimestamps
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeIncludeTimestamps");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeIncludeTimestamps", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeIncludeTimestampsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeIncludeTimestampsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeIncludeTimestampsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportMergeIncludeCameraNamesValues => base.Wrapper.GetEnumValues("ExportMergeIncludeCameraNames");

	public string ExportMergeIncludeCameraNames
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportMergeIncludeCameraNames");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeIncludeCameraNames", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportMergeIncludeCameraNamesLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportMergeIncludeCameraNamesLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportMergeIncludeCameraNamesLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportStillImageFormatValues => base.Wrapper.GetEnumValues("ExportStillImageFormat");

	public string ExportStillImageFormat
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportStillImageFormat");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportStillImageFormat", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportStillImageFormatLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportStillImageFormatLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportStillImageFormatLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExportStillImageTimestampValues => base.Wrapper.GetEnumValues("ExportStillImageTimestamp");

	public string ExportStillImageTimestamp
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExportStillImageTimestamp");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportStillImageTimestamp", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExportStillImageTimestampLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExportStillImageTimestampLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExportStillImageTimestampLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileExportChildItem(ServerId serverId, ConfigurationItem clientProfileExportChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileExportChildItem, parentWrapper)
	{
	}

	public ClientProfileExportChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileExportChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
