using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileGeneralChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> GeneralTimeInTitlebarValues => base.Wrapper.GetEnumValues("GeneralTimeInTitlebar");

	public string GeneralTimeInTitlebar
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralTimeInTitlebar");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralTimeInTitlebar", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralTimeInTitlebarLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralTimeInTitlebarLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralTimeInTitlebarLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralTitleBarValues => base.Wrapper.GetEnumValues("GeneralTitleBar");

	public string GeneralTitleBar
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralTitleBar");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralTitleBar", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralTitleBarLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralTitleBarLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralTitleBarLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralHtmlViewItemScriptingValues => base.Wrapper.GetEnumValues("GeneralHtmlViewItemScripting");

	public string GeneralHtmlViewItemScripting
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralHtmlViewItemScripting");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralHtmlViewItemScripting", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralEmptySpacesValues => base.Wrapper.GetEnumValues("GeneralEmptySpaces");

	public string GeneralEmptySpaces
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralEmptySpaces");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralEmptySpaces", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralEmptySpacesLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralEmptySpacesLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralEmptySpacesLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string ViewsCustomLogoInEmptySpaces
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ViewsCustomLogoInEmptySpaces");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ViewsCustomLogoInEmptySpaces", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralCameraErrorsValues => base.Wrapper.GetEnumValues("GeneralCameraErrors");

	public string GeneralCameraErrors
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralCameraErrors");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralCameraErrors", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralCameraErrorsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralCameraErrorsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralCameraErrorsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralServerErrorsValues => base.Wrapper.GetEnumValues("GeneralServerErrors");

	public string GeneralServerErrors
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralServerErrors");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralServerErrors", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralServerErrorsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralServerErrorsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralServerErrorsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralCameraStoppedMessageValues => base.Wrapper.GetEnumValues("GeneralCameraStoppedMessage");

	public string GeneralCameraStoppedMessage
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralCameraStoppedMessage");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralCameraStoppedMessage", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralCameraStoppedMessageLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralCameraStoppedMessageLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralCameraStoppedMessageLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralViewGridSpacerValues => base.Wrapper.GetEnumValues("GeneralViewGridSpacer");

	public string GeneralViewGridSpacer
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralViewGridSpacer");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralViewGridSpacer", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralViewGridSpacerLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralViewGridSpacerLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralViewGridSpacerLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralApplicationMaximizationValues => base.Wrapper.GetEnumValues("GeneralApplicationMaximization");

	public string GeneralApplicationMaximization
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralApplicationMaximization");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralApplicationMaximization", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralApplicationMaximizationLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralApplicationMaximizationLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralApplicationMaximizationLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int ApplicationInactivityTimeout
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationInactivityTimeout");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationInactivityTimeout", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralDefaultVideoBufferValues => base.Wrapper.GetEnumValues("GeneralDefaultVideoBuffer");

	public string GeneralDefaultVideoBuffer
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralDefaultVideoBuffer");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralDefaultVideoBuffer", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralDefaultVideoBufferLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralDefaultVideoBufferLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralDefaultVideoBufferLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationMinimizeButtonValues => base.Wrapper.GetEnumValues("ApplicationMinimizeButton");

	public string ApplicationMinimizeButton
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationMinimizeButton");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationMinimizeButton", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationMaximizeButtonValues => base.Wrapper.GetEnumValues("ApplicationMaximizeButton");

	public string ApplicationMaximizeButton
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationMaximizeButton");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationMaximizeButton", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationLogoutButtonValues => base.Wrapper.GetEnumValues("ApplicationLogoutButton");

	public string ApplicationLogoutButton
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationLogoutButton");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationLogoutButton", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationExitButtonValues => base.Wrapper.GetEnumValues("ApplicationExitButton");

	public string ApplicationExitButton
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationExitButton");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationExitButton", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationOptionsDialogButtonValues => base.Wrapper.GetEnumValues("ApplicationOptionsDialogButton");

	public string ApplicationOptionsDialogButton
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationOptionsDialogButton");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationOptionsDialogButton", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationKeyboardSetupValues => base.Wrapper.GetEnumValues("ApplicationKeyboardSetup");

	public string ApplicationKeyboardSetup
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationKeyboardSetup");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationKeyboardSetup", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationJoystickSetupValues => base.Wrapper.GetEnumValues("ApplicationJoystickSetup");

	public string ApplicationJoystickSetup
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationJoystickSetup");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationJoystickSetup", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationRememberPasswordValues => base.Wrapper.GetEnumValues("ApplicationRememberPassword");

	public string ApplicationRememberPassword
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationRememberPassword");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationRememberPassword", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationAutoLoginValues => base.Wrapper.GetEnumValues("ApplicationAutoLogin");

	public string ApplicationAutoLogin
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationAutoLogin");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationAutoLogin", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationStartModeValues => base.Wrapper.GetEnumValues("ApplicationStartMode");

	public string ApplicationStartMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationStartMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationStartMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ApplicationStartModeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationStartModeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationStartModeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationStartViewValues => base.Wrapper.GetEnumValues("ApplicationStartView");

	public string ApplicationStartView
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationStartView");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationStartView", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ApplicationStartViewLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationStartViewLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationStartViewLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationNewSCVersionWindowValues => base.Wrapper.GetEnumValues("ApplicationNewSCVersionWindow");

	public string ApplicationNewSCVersionWindow
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationNewSCVersionWindow");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationNewSCVersionWindow", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ApplicationNewSCVersionText
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationNewSCVersionText");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationNewSCVersionText", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GeneralDefaultPtzPointAndClickModeValues => base.Wrapper.GetEnumValues("GeneralDefaultPtzPointAndClickMode");

	public string GeneralDefaultPtzPointAndClickMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralDefaultPtzPointAndClickMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralDefaultPtzPointAndClickMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GeneralDefaultPtzPointAndClickModeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralDefaultPtzPointAndClickModeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralDefaultPtzPointAndClickModeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> SystemMonitorTabValues => base.Wrapper.GetEnumValues("SystemMonitorTab");

	public string SystemMonitorTab
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SystemMonitorTab");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SystemMonitorTab", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> CentralizedSearchTabValues => base.Wrapper.GetEnumValues("CentralizedSearchTab");

	public string CentralizedSearchTab
	{
		get
		{
			return base.Wrapper.GetPropertyValue("CentralizedSearchTab");
		}
		set
		{
			base.Wrapper.SetPropertyValue("CentralizedSearchTab", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> CentralizedSearchMaxDeviceCountValues => base.Wrapper.GetEnumValues("CentralizedSearchMaxDeviceCount");

	public string CentralizedSearchMaxDeviceCount
	{
		get
		{
			return base.Wrapper.GetPropertyValue("CentralizedSearchMaxDeviceCount");
		}
		set
		{
			base.Wrapper.SetPropertyValue("CentralizedSearchMaxDeviceCount", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationHideMousePointerTimeoutValues => base.Wrapper.GetEnumValues("ApplicationHideMousePointerTimeout");

	public string ApplicationHideMousePointerTimeout
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationHideMousePointerTimeout");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationHideMousePointerTimeout", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ApplicationHideMousePointerTimeoutLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationHideMousePointerTimeoutLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationHideMousePointerTimeoutLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> AlarmManagerTabValues => base.Wrapper.GetEnumValues("AlarmManagerTab");

	public string AlarmManagerTab
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AlarmManagerTab");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AlarmManagerTab", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationSnapshotAvailabilityValues => base.Wrapper.GetEnumValues("ApplicationSnapshotAvailability");

	public string ApplicationSnapshotAvailability
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationSnapshotAvailability");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSnapshotAvailability", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ApplicationSnapshotAvailabilityLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationSnapshotAvailabilityLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSnapshotAvailabilityLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string ApplicationSnapshotPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationSnapshotPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSnapshotPath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ApplicationSnapshotPathLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationSnapshotPathLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSnapshotPathLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationSnapshotTimeStampOverlayValues => base.Wrapper.GetEnumValues("ApplicationSnapshotTimeStampOverlay");

	public string ApplicationSnapshotTimeStampOverlay
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationSnapshotTimeStampOverlay");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSnapshotTimeStampOverlay", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ApplicationSnapshotTimeStampOverlayLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationSnapshotTimeStampOverlayLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSnapshotTimeStampOverlayLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationSnapshotCameraNameOverlayValues => base.Wrapper.GetEnumValues("ApplicationSnapshotCameraNameOverlay");

	public string ApplicationSnapshotCameraNameOverlay
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationSnapshotCameraNameOverlay");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSnapshotCameraNameOverlay", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ApplicationSnapshotCameraNameOverlayLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationSnapshotCameraNameOverlayLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSnapshotCameraNameOverlayLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationEvidenceLockValues => base.Wrapper.GetEnumValues("ApplicationEvidenceLock");

	public string ApplicationEvidenceLock
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationEvidenceLock");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationEvidenceLock", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationLiftPrivacyMaskTimeoutValues => base.Wrapper.GetEnumValues("ApplicationLiftPrivacyMaskTimeout");

	public string ApplicationLiftPrivacyMaskTimeout
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationLiftPrivacyMaskTimeout");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationLiftPrivacyMaskTimeout", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationOnlineHelpValues => base.Wrapper.GetEnumValues("ApplicationOnlineHelp");

	public string ApplicationOnlineHelp
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationOnlineHelp");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationOnlineHelp", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ApplicationOnlineHelpLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationOnlineHelpLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationOnlineHelpLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationVideoTutorialsValues => base.Wrapper.GetEnumValues("ApplicationVideoTutorials");

	public string ApplicationVideoTutorials
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationVideoTutorials");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationVideoTutorials", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ApplicationVideoTutorialsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationVideoTutorialsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationVideoTutorialsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ApplicationTransactTabValues => base.Wrapper.GetEnumValues("ApplicationTransactTab");

	public string ApplicationTransactTab
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationTransactTab");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationTransactTab", value);
			base.Wrapper.IsDirty = true;
		}
	}

	[Obsolete("Property no longer supported", false)]
	public Dictionary<string, string> ApplicationSimpleModeValues => base.Wrapper.GetEnumValues("ApplicationSimpleMode");

	[Obsolete("Property no longer supported", false)]
	public string ApplicationSimpleMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ApplicationSimpleMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSimpleMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	[Obsolete("Property no longer supported", false)]
	public bool ApplicationSimpleModeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ApplicationSimpleModeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ApplicationSimpleModeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	[Obsolete("Property no longer supported", false)]
	public Dictionary<string, string> GeneralDefaultImageQualityValues => base.Wrapper.GetEnumValues("GeneralDefaultImageQuality");

	[Obsolete("Property no longer supported", false)]
	public string GeneralDefaultImageQuality
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralDefaultImageQuality");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralDefaultImageQuality", value);
			base.Wrapper.IsDirty = true;
		}
	}

	[Obsolete("Property no longer supported", false)]
	public bool GeneralDefaultImageQualityLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralDefaultImageQualityLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralDefaultImageQualityLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	[Obsolete("Property no longer supported", false)]
	public Dictionary<string, string> GeneralDefaultFrameRateValues => base.Wrapper.GetEnumValues("GeneralDefaultFrameRate");

	[Obsolete("Property no longer supported", false)]
	public string GeneralDefaultFrameRate
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GeneralDefaultFrameRate");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralDefaultFrameRate", value);
			base.Wrapper.IsDirty = true;
		}
	}

	[Obsolete("Property no longer supported", false)]
	public bool GeneralDefaultFrameRateLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GeneralDefaultFrameRateLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GeneralDefaultFrameRateLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileGeneralChildItem(ServerId serverId, ConfigurationItem clientProfileGeneralChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileGeneralChildItem, parentWrapper)
	{
	}

	public ClientProfileGeneralChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileGeneralChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
