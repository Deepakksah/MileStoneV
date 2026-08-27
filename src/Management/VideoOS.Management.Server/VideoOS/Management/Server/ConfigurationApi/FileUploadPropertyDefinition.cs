using System;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class FileUploadPropertyDefinition
{
	public static Property TransferIdProperty(Guid id)
	{
		return new Property("TransferId", PropertyTypesHelper.GetValue(id))
		{
			ValueType = "String",
			DisplayName = ConfigurationApiResources.PropertyUploadFileTransferId,
			TranslationId = "PropertyUploadFileTransferId",
			ToolTipTranslationId = "PropertyUploadFileTransferIdToolTip",
			IsSettable = true,
			UIImportance = UIImportance.Summary
		};
	}

	public static Property ChunkDataProperty(string data)
	{
		return new Property("ChunkData", data)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyUploadFileChunkData,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUploadFileChunkData",
			ToolTipTranslationId = "PropertyUploadFileChunkDataTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ChunkOffsetProperty(int value)
	{
		return new Property("Offset", value.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyUploadFileChunkOffset,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUploadFileChunkOffset",
			ToolTipTranslationId = "PropertyUploadFileChunkOffsetToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FileSizeProperty(long value)
	{
		return new Property("Size", value.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyUploadFileSize,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUploadFileSize",
			ToolTipTranslationId = "PropertyUploadFileSizeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FileChecksumProperty(string data)
	{
		return new Property("Checksum", data)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyUploadFileChecksum,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUploadFileChecksum",
			ToolTipTranslationId = "PropertyUploadFileChecksumToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FileStorageIdProperty(string data)
	{
		return new Property("StorageId", data)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyUploadFileStorageId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUploadFileStorageId",
			ToolTipTranslationId = "PropertyUploadFileStorageIdToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
