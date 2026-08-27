using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPICompressor")]
[EntityName("ConfigAPICompressorFolder")]
public class CompressorConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.IsFolder)
		{
			Compressors[] array = Compressors.QueryCompressors(requestContext);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			Compressors[] array2 = array;
			foreach (Compressors compressors in array2)
			{
				string systemSetting = SystemSetting.GetSystemSetting(requestContext, "AVICodec");
				bool isSelected = compressors.Handler.ToString() == systemSetting;
				list.Add(ConvertToConfigurationItem(compressors, isSelected));
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("CompressorFolder", ConfigurationItemPath.CreateSystemPath());
		}
		Guid idCodec = new Guid(path.Id);
		Compressors obj = Compressors.QueryCompressors(requestContext).FirstOrDefault((Compressors c) => c.IdCodec == idCodec) ?? throw new PathNotFoundException(path.Text);
		string systemSetting = SystemSetting.GetSystemSetting(requestContext, "AVICodec");
		bool isSelected = obj.Handler.ToString() == systemSetting;
		return ConvertToConfigurationItem(obj, isSelected);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new NotSupportedException();
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ValidateInternalCompressor(requestContext, item, out var quality, out var keyFrames, out var dataRate, out var isSelected);
		Guid idCodec = new Guid(path.Id);
		Compressors compressors = Compressors.QueryCompressors(requestContext).FirstOrDefault((Compressors c) => c.IdCodec == idCodec);
		if (compressors == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		compressors.Quality = quality;
		compressors.KeyFrames = keyFrames;
		compressors.DataRate = dataRate;
		Compressors.UpdateCompressor(requestContext, compressors.Handler, compressors.IdCodec, compressors.Flags, compressors.Quality, compressors.KeyFrames, compressors.DataRate, compressors.ConfigData);
		if (isSelected)
		{
			SystemSetting.SetSystemSetting(requestContext, "AVICodec", compressors.Handler.ToString());
		}
		return new ValidateResult(ConvertToConfigurationItem(compressors, isSelected));
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ValidateInternalCompressor(requestContext, item, out var _, out var _, out var _, out var _);
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = null
		};
	}

	private static ValidateResult ValidateInternalCompressor(IGeminiRequestContext requestContext, ConfigurationItem item, out int quality, out int keyFrames, out int dataRate, out bool isSelected)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AviGenerationQuality", out string value) || !int.TryParse(value, out var result))
		{
			throw new PropertyInvalidException("", "AviGenerationQuality");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AviGenerationKeyFrames", out string value2) || !int.TryParse(value2, out var result2))
		{
			throw new PropertyInvalidException("", "AviGenerationKeyFrames");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AviGenerationDataRate", out string value3) || !int.TryParse(value3, out var result3))
		{
			throw new PropertyInvalidException("", "AviGenerationDataRate");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AviGenerationSelected", out string value4) || !bool.TryParse(value4, out var result4))
		{
			throw new PropertyInvalidException("", "AviGenerationSelected");
		}
		quality = result;
		keyFrames = result2;
		dataRate = result3;
		isSelected = result4;
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConvertToConfigurationItem(Compressors compressor, bool isSelected)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "CompressorFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("Compressor", compressor.IdCodec.ToString());
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "Compressor",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = compressor.Description,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text
		};
		List<Property> list = new List<Property>();
		obj.MethodIds = new string[0];
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(compressor.LastModified));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("AviGenerationHandler", compressor.Handler.ToString(CultureInfo.InvariantCulture), "AviGenerationHandlerTooltip", settable: false));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("AviGenerationFlags", compressor.Flags.ToString(CultureInfo.InvariantCulture), "AviGenerationFlagsTooltip", settable: false));
		list.Add(ToolOptionPropertyDefinitions.GetRangeIntProperty("AviGenerationQuality", compressor.Quality.ToString(CultureInfo.InvariantCulture), "AviGenerationQualityTooltip", 0, 100, compressor.QualityEnabled));
		list.Add(ToolOptionPropertyDefinitions.GetRangeIntProperty("AviGenerationKeyFrames", compressor.KeyFrames.ToString(CultureInfo.InvariantCulture), "AviGenerationKeyFrameTooltip", 0, 100, compressor.KeyFramesEnabled));
		list.Add(ToolOptionPropertyDefinitions.GetRangeIntProperty("AviGenerationDataRate", compressor.DataRate.ToString(CultureInfo.InvariantCulture), "AviGenerationDataRateTooltip", 0, 99999, compressor.DataRateEnabled));
		list.Add(ToolOptionPropertyDefinitions.GetStringProperty("AviGenerationConfigData", Convert.ToBase64String(compressor.ConfigData, Base64FormattingOptions.None), "", settable: false));
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("AviGenerationSelected", isSelected.ToString(), "AviGenerationSelectedTooltip", settable: true));
		obj.Properties = list.ToArray();
		return obj;
	}
}
