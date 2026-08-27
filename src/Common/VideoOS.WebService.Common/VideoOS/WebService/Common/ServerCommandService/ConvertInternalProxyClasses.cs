using System.Xml;

namespace VideoOS.WebService.Common.ServerCommandService;

public static class ConvertInternalProxyClasses
{
	public static ViewGroup ConvertToViewGroup(ViewGroupInternal viewGroupInternal)
	{
		if (viewGroupInternal == null)
		{
			return null;
		}
		return new ViewGroup
		{
			ViewGroupInfo = viewGroupInternal.ViewGroupInfo,
			ViewGroupData = ConvertToViewGroupData(viewGroupInternal.ViewGroupData)
		};
	}

	public static ViewGroupData ConvertToViewGroupData(ViewGroupInternalData viewGroupInternalData)
	{
		if (viewGroupInternalData == null)
		{
			return null;
		}
		ViewGroupData viewGroupData = new ViewGroupData();
		viewGroupData.DataVersion = viewGroupInternalData.DataVersion;
		viewGroupData.XmlVersion = viewGroupInternalData.XmlVersion;
		if (!string.IsNullOrEmpty(viewGroupInternalData.ViewGroupDataXml))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(viewGroupInternalData.ViewGroupDataXml);
			viewGroupData.ViewGroupDataXml = xmlDocument;
		}
		return viewGroupData;
	}

	public static ViewGroupInternal ConvertToViewGroupInternal(ViewGroup viewGroup)
	{
		if (viewGroup == null)
		{
			return null;
		}
		return new ViewGroupInternal
		{
			ViewGroupInfo = viewGroup.ViewGroupInfo,
			ViewGroupData = ConvertToViewGroupInternalData(viewGroup.ViewGroupData)
		};
	}

	public static ViewGroupInternalData ConvertToViewGroupInternalData(ViewGroupData viewGroupData)
	{
		if (viewGroupData == null)
		{
			return null;
		}
		ViewGroupInternalData viewGroupInternalData = new ViewGroupInternalData();
		viewGroupInternalData.DataVersion = viewGroupData.DataVersion;
		viewGroupInternalData.XmlVersion = viewGroupData.XmlVersion;
		if (viewGroupData.ViewGroupDataXml != null)
		{
			viewGroupInternalData.ViewGroupDataXml = viewGroupData.ViewGroupDataXml.OuterXml;
		}
		return viewGroupInternalData;
	}

	public static CustomSetting ConvertToCustomSetting(CustomSettingInternal customSettingInternal)
	{
		if (customSettingInternal == null)
		{
			return null;
		}
		return new CustomSetting
		{
			CustomSettingInfo = customSettingInternal.CustomSettingInfo,
			CustomSettingData = ConvertToCustomSettingData(customSettingInternal.CustomSettingData)
		};
	}

	public static CustomSettingData ConvertToCustomSettingData(CustomSettingInternalData customSettingInternalData)
	{
		if (customSettingInternalData == null)
		{
			return null;
		}
		CustomSettingData customSettingData = new CustomSettingData();
		customSettingData.DataVersion = customSettingInternalData.DataVersion;
		if (!string.IsNullOrEmpty(customSettingInternalData.CustomSettingDataXml))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(customSettingInternalData.CustomSettingDataXml);
			customSettingData.CustomSettingDataXml = xmlDocument;
		}
		return customSettingData;
	}

	public static CustomSettingInternal ConvertToCustomSettingInternal(CustomSetting customSetting)
	{
		if (customSetting == null)
		{
			return null;
		}
		return new CustomSettingInternal
		{
			CustomSettingInfo = customSetting.CustomSettingInfo,
			CustomSettingData = ConvertToCustomSettingInternalData(customSetting.CustomSettingData)
		};
	}

	public static CustomSettingInternalData ConvertToCustomSettingInternalData(CustomSettingData customSettingData)
	{
		if (customSettingData == null)
		{
			return null;
		}
		CustomSettingInternalData customSettingInternalData = new CustomSettingInternalData();
		customSettingInternalData.DataVersion = customSettingData.DataVersion;
		if (customSettingData.CustomSettingDataXml != null)
		{
			customSettingInternalData.CustomSettingDataXml = customSettingData.CustomSettingDataXml.OuterXml;
		}
		return customSettingInternalData;
	}
}
