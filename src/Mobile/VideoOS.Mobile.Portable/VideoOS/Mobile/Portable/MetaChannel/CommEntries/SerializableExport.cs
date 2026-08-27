using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableExport
{
	public static List<SerializableItem> GetItems(IEnumerable<IExportEntry> exports)
	{
		return exports.Select(GetItem).ToList();
	}

	public static SerializableItem GetItem(IExportEntry exportEntry)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.Id = exportEntry.Params.ExportId;
		serializableItem.Name = exportEntry.Params.ExportName;
		serializableItem.SetItemType(SerializableItemTypes.Export);
		serializableItem.Properties.Add("StartTime", exportEntry.Params.StartTime.ToMilisecString());
		serializableItem.Properties.Add("EndTime", exportEntry.Params.EndTime.ToMilisecString());
		serializableItem.Properties.Add("QueuedTime", exportEntry.QueuedOnUTC.ToMilisecString());
		serializableItem.Properties.Add("CompletedTime", exportEntry.CompletedOnUTC.ToMilisecString());
		serializableItem.Properties.Add("FileName", exportEntry.FileName);
		serializableItem.Properties.Add("CameraId", exportEntry.Params.CameraId.ToString());
		serializableItem.Properties.Add("CameraName", exportEntry.CameraName);
		serializableItem.Properties.Add("Type", EnumsHelper.GetDescription(exportEntry.Params.Type));
		serializableItem.Properties.Add("CodecName", exportEntry.Params.CodecName);
		serializableItem.Properties.Add("State", exportEntry.State.ToString());
		serializableItem.Properties.Add("User", exportEntry.Username);
		serializableItem.Properties.Add("FullUserName", exportEntry.FullUserName);
		serializableItem.Properties.Add("InitType", EnumsHelper.GetDescription(exportEntry.ExportInitType));
		serializableItem.Properties.Add("Size", exportEntry.SizeBytes.ToString());
		serializableItem.Properties.Add("KeepOriginals", exportEntry.Params.KeepOriginals.ToString());
		serializableItem.Properties.Add("IncludeAudio", exportEntry.Params.IncludeAudio.ToString());
		serializableItem.Properties.Add("InvestigationId", exportEntry.Params.InvestigationId.ToString());
		serializableItem.Properties.Add("ReExportFromId", exportEntry.Params.ReExportFromId.ToString());
		serializableItem.Properties.Add("ServerId", exportEntry.ProcessingServerId.ToString());
		return serializableItem;
	}

	public static ExportEntryImplementer GetImplementer(SerializableItem item)
	{
		ExportEntryImplementer exportEntryImplementer = null;
		if (item != null && SerializableItemTypes.Export == item.GetItemType())
		{
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			ExportParams exportParams = new ExportParams();
			exportParams.ExportId = item.Id;
			exportParams.ExportName = item.Name;
			if (properties.ContainsKey("StartTime"))
			{
				exportParams.StartTime = TimeConverter.FromString(properties["StartTime"]);
			}
			if (properties.ContainsKey("EndTime"))
			{
				exportParams.EndTime = TimeConverter.FromString(properties["EndTime"]);
			}
			if (properties.ContainsKey("CameraId"))
			{
				exportParams.CameraId = new Guid(properties["CameraId"]);
			}
			if (properties.ContainsKey("Type"))
			{
				exportParams.Type = EnumsHelper.EnumConverter<ExportType>(properties["Type"]);
			}
			if (properties.ContainsKey("CodecName"))
			{
				exportParams.CodecName = properties["CodecName"];
			}
			if (properties.ContainsKey("KeepOriginals"))
			{
				exportParams.KeepOriginals = string.Compare(properties["KeepOriginals"], true.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
			}
			if (properties.ContainsKey("InvestigationId"))
			{
				exportParams.InvestigationId = new Guid(properties["InvestigationId"]);
			}
			if (properties.ContainsKey("ReExportFromId"))
			{
				exportParams.ReExportFromId = new Guid(properties["ReExportFromId"]);
			}
			if (properties.ContainsKey("IncludeAudio"))
			{
				exportParams.IncludeAudio = ParamsHelper.BoolConvertion(properties["IncludeAudio"]);
			}
			exportEntryImplementer = new ExportEntryImplementer();
			exportEntryImplementer.Params = exportParams;
			if (properties.ContainsKey("QueuedTime"))
			{
				exportEntryImplementer.QueuedOnUTC = TimeConverter.FromString(properties["QueuedTime"]);
			}
			if (properties.ContainsKey("CompletedTime"))
			{
				exportEntryImplementer.CompletedOnUTC = TimeConverter.FromString(properties["CompletedTime"]);
			}
			if (properties.ContainsKey("FileName"))
			{
				exportEntryImplementer.FileName = properties["FileName"];
			}
			if (properties.ContainsKey("State"))
			{
				exportEntryImplementer.State = Convert.ToInt32(properties["State"]);
			}
			if (properties.ContainsKey("Size"))
			{
				exportEntryImplementer.SizeBytes = Convert.ToInt64(properties["Size"]);
			}
			if (properties.ContainsKey("User"))
			{
				exportEntryImplementer.Username = properties["User"];
			}
			if (properties.ContainsKey("FullUserName"))
			{
				exportEntryImplementer.FullUserName = properties["FullUserName"];
			}
			if (properties.ContainsKey("InitType"))
			{
				exportEntryImplementer.ExportInitType = EnumsHelper.EnumConverter<ExportInitType>(properties["InitType"]);
			}
			if (properties.ContainsKey("ServerId"))
			{
				exportEntryImplementer.ProcessingServerId = Guid.Parse(properties["ServerId"]);
			}
		}
		return exportEntryImplementer;
	}
}
