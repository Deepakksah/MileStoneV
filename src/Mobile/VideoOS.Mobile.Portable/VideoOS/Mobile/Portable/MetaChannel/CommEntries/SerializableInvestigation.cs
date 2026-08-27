using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableInvestigation
{
	public const string Username = "Username";

	public const string FullUserName = "FullUserName";

	public const string UserSid = "UserSid";

	public const string ServerId = "ServerId";

	public static List<SerializableItem> GetItems(IEnumerable<IInvestigationEntryExt> investigations, InvestigationCameraParamsProvider provider, bool includePackages = false)
	{
		return investigations.Select((IInvestigationEntryExt entry) => GetItem(entry, provider, includePackages)).ToList();
	}

	public static List<SerializableItem> GetItems(IEnumerable<IInvestigationEntry> investigations, InvestigationCameraParamsProvider provider, bool includePackages = false)
	{
		return investigations.Select((IInvestigationEntry entry) => GetItem(entry, provider, includePackages)).ToList();
	}

	public static SerializableItem GetItem(IInvestigationEntryExt entry, InvestigationCameraParamsProvider provider, bool includePackages = false)
	{
		SerializableItem item = GetItem((IInvestigationEntry)entry, provider, false);
		item.Properties.Add("Username", entry.Username);
		item.Properties.Add("FullUserName", entry.FullUserName);
		item.Properties.Add("UserSid", entry.UserSid);
		item.Properties.Add("ServerId", entry.ServerId.ToString());
		return item;
	}

	public static SerializableItem GetItem(IInvestigationEntry entry, InvestigationCameraParamsProvider provider, bool includePackages = false)
	{
		SerializableItem serializableItem = new SerializableItem
		{
			Id = entry.Id,
			Name = entry.Name
		};
		serializableItem.SetItemType(SerializableItemTypes.Investigation);
		serializableItem.Properties.Add("StartTime", entry.StartTime.ToMilisecString());
		serializableItem.Properties.Add("EndTime", entry.EndTime.ToMilisecString());
		serializableItem.Properties.Add("State", EnumsHelper.GetDescription(entry.State));
		serializableItem.Properties.Add("Status", EnumsHelper.GetDescription(entry.Status));
		serializableItem.Properties.Add("StatusProgress", entry.StatusProgress.ToString());
		serializableItem.Properties.Add("CreatedAt", entry.CreatedAt.ToMilisecString());
		serializableItem.Properties.Add("ModifiedAt", entry.ModifiedAt.ToMilisecString());
		serializableItem.Properties.Add("CreatedBy", entry.CreatedBy);
		serializableItem.Properties.Add("ContainsAudio", ParamsHelper.BoolConvertionTo(entry.ContainsAudio));
		if (entry.ExpirationTime.HasValue)
		{
			serializableItem.Properties.Add("ExpirationTime", entry.ExpirationTime.Value.ToMilisecString());
		}
		if (entry.Cameras != null)
		{
			serializableItem.Children.AddRange(GetItems(entry.Cameras, provider));
		}
		if (entry.PreviousParams != null)
		{
			serializableItem.Children.Add(GetItem(entry.PreviousParams, provider));
		}
		FillExportsItem(serializableItem, entry.Exports);
		if (includePackages && entry.ExportEntries != null && entry.ExportEntries.Any((IExportEntry e) => e != null))
		{
			serializableItem.Children.AddRange(SerializableExport.GetItems(entry.ExportEntries.Where((IExportEntry e) => e != null)));
		}
		return serializableItem;
	}

	public static IEnumerable<SerializableItem> GetItems(IEnumerable<IInvestigationCamera> cameras, InvestigationCameraParamsProvider provider)
	{
		return cameras.Select((IInvestigationCamera camera) => GetItem(camera, provider)).ToList();
	}

	public static SerializableItem GetItem(IInvestigationCamera camera, InvestigationCameraParamsProvider provider)
	{
		SerializableItem serializableItem = new SerializableItem
		{
			Id = camera.CameraId,
			Name = camera.CameraName
		};
		if (camera.RelatedMicrophone != Guid.Empty)
		{
			serializableItem.Properties.Add("RelatedMicrophone", camera.RelatedMicrophone.ToString());
		}
		if (camera.RelatedSpeaker != Guid.Empty)
		{
			serializableItem.Properties.Add("RelatedSpeaker", camera.RelatedSpeaker.ToString());
		}
		serializableItem.SetItemType(SerializableItemTypes.Camera);
		FillExportsItem(serializableItem, camera);
		FillCameraItem(serializableItem, provider);
		return serializableItem;
	}

	public static ErrorCodes FillExportsItem(SerializableItem item, IInvestigationExports exports)
	{
		ErrorCodes result = ErrorCodes.WrongInputParameter;
		if (item != null && exports != null)
		{
			if (exports.OriginDbExportId != Guid.Empty)
			{
				item.Properties.Add("OriginDbExportId", exports.OriginDbExportId.ToString());
			}
			if (exports.DbExportId != Guid.Empty)
			{
				item.Properties.Add("DbExportId", exports.DbExportId.ToString());
			}
			if (exports.AviExportId != Guid.Empty)
			{
				item.Properties.Add("AviExportId", exports.AviExportId.ToString());
			}
			if (exports.MkvExportId != Guid.Empty)
			{
				item.Properties.Add("MkvExportId", exports.MkvExportId.ToString());
			}
			if (exports.BinaryExportId != Guid.Empty)
			{
				item.Properties.Add("BinaryExportId", exports.BinaryExportId.ToString());
			}
			result = ErrorCodes.Ok;
		}
		return result;
	}

	public static void FillCameraItem(SerializableItem cameraItem, InvestigationCameraParamsProvider provider)
	{
		IInvestigationCameraParams investigationCameraParams = null;
		if (provider != null)
		{
			investigationCameraParams = provider(cameraItem.Id);
		}
		if (investigationCameraParams != null)
		{
			if (!string.IsNullOrWhiteSpace(investigationCameraParams.Name))
			{
				cameraItem.Name = investigationCameraParams.Name;
			}
			cameraItem.Properties.Add("SrcWidth", investigationCameraParams.SrcWidth);
			cameraItem.Properties.Add("SrcHeight", investigationCameraParams.SrcHeight);
		}
	}

	public static InvestigationEntryImplementer GetImplementer(SerializableItem investigationItem)
	{
		InvestigationEntryImplementer investigationEntryImplementer = null;
		if (investigationItem != null && SerializableItemTypes.Investigation == investigationItem.GetItemType())
		{
			investigationEntryImplementer = new InvestigationEntryImplementer();
			investigationEntryImplementer.Id = investigationItem.Id;
			investigationEntryImplementer.Name = investigationItem.Name;
			SerializableProperties<string, string, StringStringFactory> properties = investigationItem.Properties;
			if (properties.ContainsKey("StartTime"))
			{
				investigationEntryImplementer.StartTime = TimeConverter.FromString(properties["StartTime"]);
			}
			if (properties.ContainsKey("EndTime"))
			{
				investigationEntryImplementer.EndTime = TimeConverter.FromString(properties["EndTime"]);
			}
			if (properties.ContainsKey("State"))
			{
				investigationEntryImplementer.State = EnumsHelper.EnumConverter<InvestigationState>(properties["State"]);
			}
			if (properties.ContainsKey("Status"))
			{
				investigationEntryImplementer.Status = EnumsHelper.EnumConverter<InvestigationSatus>(properties["Status"]);
			}
			if (properties.ContainsKey("StatusProgress"))
			{
				investigationEntryImplementer.StatusProgress = Convert.ToInt32(properties["StatusProgress"]);
			}
			if (properties.ContainsKey("CreatedAt"))
			{
				investigationEntryImplementer.CreatedAt = TimeConverter.FromString(properties["CreatedAt"]);
			}
			if (properties.ContainsKey("ModifiedAt"))
			{
				investigationEntryImplementer.ModifiedAt = TimeConverter.FromString(properties["ModifiedAt"]);
			}
			if (properties.ContainsKey("CreatedBy"))
			{
				investigationEntryImplementer.CreatedBy = properties["CreatedBy"];
			}
			if (properties.ContainsKey("ContainsAudio"))
			{
				investigationEntryImplementer.ContainsAudio = ParamsHelper.BoolConvertion(properties["ContainsAudio"]);
			}
			if (properties.ContainsKey("ExpirationTime"))
			{
				investigationEntryImplementer.ExpirationTime = TimeConverter.FromString(properties["ExpirationTime"]);
			}
			List<InvestigationCameraImplementer> cameras = (from serializableItem in investigationItem.Children
				where serializableItem.GetItemType() == SerializableItemTypes.Camera
				select GetCameraImplementer(serializableItem)).ToList();
			investigationEntryImplementer.Cameras = cameras;
			List<InvestigationEntryImplementer> list = (from serializableItem in investigationItem.Children
				where serializableItem.GetItemType() == SerializableItemTypes.Investigation
				select GetImplementer(serializableItem)).ToList();
			if (list.Count > 0)
			{
				investigationEntryImplementer.PreviousParams = list.First();
			}
			InvestigationExportsImplementer exports = new InvestigationExportsImplementer();
			FillInvestigationExports(exports, investigationItem);
			investigationEntryImplementer.Exports = exports;
			List<ExportEntryImplementer> exportEntries = (from serializableItem in investigationItem.Children
				where serializableItem.GetItemType() == SerializableItemTypes.Export
				select SerializableExport.GetImplementer(serializableItem)).ToList();
			investigationEntryImplementer.ExportEntries = exportEntries;
		}
		return investigationEntryImplementer;
	}

	public static InvestigationCameraImplementer GetCameraImplementer(SerializableItem cameraItem)
	{
		InvestigationCameraImplementer investigationCameraImplementer = null;
		if (cameraItem != null && SerializableItemTypes.Camera == cameraItem.GetItemType())
		{
			investigationCameraImplementer = new InvestigationCameraImplementer();
			investigationCameraImplementer.CameraId = cameraItem.Id;
			investigationCameraImplementer.CameraName = cameraItem.Name;
			try
			{
				if (cameraItem.Properties.ContainsKey("RelatedMicrophone") && cameraItem.Properties.ContainsKey("RelatedSpeaker"))
				{
					investigationCameraImplementer.RelatedMicrophone = Guid.Parse(cameraItem.Properties["RelatedMicrophone"]);
					investigationCameraImplementer.RelatedSpeaker = Guid.Parse(cameraItem.Properties["RelatedSpeaker"]);
				}
			}
			catch (Exception)
			{
			}
			FillInvestigationExports(investigationCameraImplementer, cameraItem);
		}
		return investigationCameraImplementer;
	}

	public static ErrorCodes FillInvestigationExports(InvestigationExportsImplementer exports, SerializableItem item)
	{
		ErrorCodes result = ErrorCodes.WrongInputParameter;
		if (item != null && exports != null)
		{
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("OriginDbExportId"))
			{
				exports.OriginDbExportId = new Guid(properties["OriginDbExportId"]);
			}
			if (properties.ContainsKey("DbExportId"))
			{
				exports.DbExportId = new Guid(properties["DbExportId"]);
			}
			if (properties.ContainsKey("AviExportId"))
			{
				exports.AviExportId = new Guid(properties["AviExportId"]);
			}
			if (properties.ContainsKey("MkvExportId"))
			{
				exports.MkvExportId = new Guid(properties["MkvExportId"]);
			}
			if (properties.ContainsKey("BinaryExportId"))
			{
				exports.BinaryExportId = new Guid(properties["BinaryExportId"]);
			}
			result = ErrorCodes.Ok;
		}
		return result;
	}

	public static InvestigationEntryImplementerExt GetImplementerExt(SerializableItem investigationItem)
	{
		InvestigationEntryImplementerExt investigationEntryImplementerExt = null;
		InvestigationEntryImplementer implementer = GetImplementer(investigationItem);
		if (implementer != null)
		{
			investigationEntryImplementerExt = new InvestigationEntryImplementerExt();
			investigationEntryImplementerExt.InitFromEntry(implementer);
			SerializableProperties<string, string, StringStringFactory> properties = investigationItem.Properties;
			if (properties.ContainsKey("Username"))
			{
				investigationEntryImplementerExt.Username = properties["Username"];
			}
			if (properties.ContainsKey("FullUserName"))
			{
				investigationEntryImplementerExt.FullUserName = properties["FullUserName"];
			}
			if (properties.ContainsKey("UserSid"))
			{
				investigationEntryImplementerExt.UserSid = properties["UserSid"];
			}
			if (properties.ContainsKey("ServerId"))
			{
				investigationEntryImplementerExt.ServerId = new Guid(properties["ServerId"]);
			}
		}
		return investigationEntryImplementerExt;
	}

	public static IEnumerable<InvestigationEntryImplementer> GetImplementersList(IEnumerable<SerializableItem> investigationItems)
	{
		if (investigationItems != null)
		{
			return investigationItems.Select((SerializableItem serializableItem) => GetImplementer(serializableItem)).ToList();
		}
		return new List<InvestigationEntryImplementer>();
	}

	public static IEnumerable<InvestigationEntryImplementerExt> GetImplementersExtList(IEnumerable<SerializableItem> investigationItems)
	{
		if (investigationItems != null)
		{
			return investigationItems.Select((SerializableItem serializableItem) => GetImplementerExt(serializableItem)).ToList();
		}
		return new List<InvestigationEntryImplementerExt>();
	}
}
