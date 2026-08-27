using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.DraftSequence;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.GlobalSettings;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentActivityLog;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentCall;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentComment;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentSequence;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.DraftSequence;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.GlobalSettings;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.Incident;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentActivityLog;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentCall;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentComment;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DraftSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.GlobalSettings;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentActivityLog;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentCall;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentComment;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentSequence;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;

namespace VideoOS.IM.Server.DataMapper.CustomMapperFramework;

public class MyMapper
{
	public static DtoDraftSequence Convert(RepoDraftSequence inObj)
	{
		DtoDraftSequence result = null;
		if (inObj != null)
		{
			result = new DtoDraftSequence
			{
				ModifiedUserId = inObj.ModifiedUserId,
				DraftSequenceIdentificationMsUSerId = inObj.DraftSequenceIdentificationMsUSerId,
				CreatedUserId = inObj.CreatedUserId,
				Description = inObj.Description,
				Id = inObj.Id,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				TimeStamp = inObj.TimeStamp,
				Title = inObj.Title,
				ModifiedUserName = inObj.ModifiedUserName,
				CreatedUserName = inObj.CreatedUserName
			};
		}
		return result;
	}

	public static List<DtoDraftSequence> Convert(List<RepoDraftSequence> inObj)
	{
		List<DtoDraftSequence> list = null;
		if (inObj != null)
		{
			list = new List<DtoDraftSequence>();
			if (inObj.Any())
			{
				foreach (RepoDraftSequence item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static RepoDraftSequence Convert(DtoDraftSequence inObj)
	{
		RepoDraftSequence result = null;
		if (inObj != null)
		{
			result = new RepoDraftSequence
			{
				ModifiedUserId = inObj.ModifiedUserId,
				DraftSequenceIdentificationMsUSerId = inObj.DraftSequenceIdentificationMsUSerId,
				CreatedUserId = inObj.CreatedUserId,
				Description = inObj.Description,
				Id = inObj.Id,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				TimeStamp = inObj.TimeStamp,
				Title = inObj.Title,
				CreatedUserName = inObj.CreatedUserName,
				ModifiedUserName = inObj.ModifiedUserName
			};
		}
		return result;
	}

	public static List<RepoDraftSequence> Convert(List<DtoDraftSequence> inObj)
	{
		List<RepoDraftSequence> list = null;
		if (inObj != null)
		{
			list = new List<RepoDraftSequence>();
			if (inObj.Any())
			{
				foreach (DtoDraftSequence item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static DtoDraftSequenceResponse Convert(RepositoryResponseBase<RepoDraftSequence> inObj)
	{
		DtoDraftSequenceResponse result = null;
		if (inObj != null)
		{
			result = new DtoDraftSequenceResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static DtoDraftSequenceListResponse Convert(RepositoryResponseBase<IEnumerable<RepoDraftSequence>> inObj)
	{
		DtoDraftSequenceListResponse result = null;
		if (inObj != null)
		{
			result = new DtoDraftSequenceListResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static RepositoryResponseBase<RepoDraftSequence> Convert(DtoDraftSequenceResponse inObj)
	{
		RepositoryResponseBase<RepoDraftSequence> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<RepoDraftSequence>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static RepositoryResponseBase<IEnumerable<RepoDraftSequence>> Convert(DtoDraftSequenceListResponse inObj)
	{
		RepositoryResponseBase<IEnumerable<RepoDraftSequence>> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<IEnumerable<RepoDraftSequence>>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static DtoDynamicalDataTypesAndOptions Convert(RepoDynamicalDataTypesAndOptions inObj)
	{
		DtoDynamicalDataTypesAndOptions result = null;
		if (inObj != null)
		{
			result = new DtoDynamicalDataTypesAndOptions
			{
				CreatedUserId = inObj.CreatedUserId,
				ClassOfData = inObj.ClassOfData,
				ClassOfDataint = inObj.ClassOfDataint,
				CustomAction = inObj.CustomAction,
				Description = inObj.Description,
				EntityScopeStr = inObj.EntityScopeStr,
				Id = inObj.Id,
				ModifiedUserId = inObj.ModifiedUserId,
				Name = inObj.Name,
				PropertyScopeStr = inObj.PropertyScopeStr,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				Title = inObj.Title,
				TypeOfItem = inObj.TypeOfItem,
				TypeOfItemStr = inObj.TypeOfItemStr,
				ValueBool = inObj.ValueBool,
				ValueDecimal = inObj.ValueDecimal,
				ValueInt = inObj.ValueInt,
				ValueStr = inObj.ValueStr,
				CreatedUserName = inObj.CreatedUserName,
				ModifiedUserName = inObj.ModifiedUserName,
				NumberOfForeignReferences = inObj.NumberOfForeignReferences
			};
		}
		return result;
	}

	public static List<DtoDynamicalDataTypesAndOptions> Convert(List<RepoDynamicalDataTypesAndOptions> inObj)
	{
		List<DtoDynamicalDataTypesAndOptions> list = null;
		if (inObj != null)
		{
			list = new List<DtoDynamicalDataTypesAndOptions>();
			if (inObj.Any())
			{
				foreach (RepoDynamicalDataTypesAndOptions item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static RepoDynamicalDataTypesAndOptions Convert(DtoDynamicalDataTypesAndOptions inObj)
	{
		RepoDynamicalDataTypesAndOptions result = null;
		if (inObj != null)
		{
			result = new RepoDynamicalDataTypesAndOptions
			{
				CreatedUserId = inObj.CreatedUserId,
				ClassOfData = inObj.ClassOfData,
				ClassOfDataint = inObj.ClassOfDataint,
				CustomAction = inObj.CustomAction,
				Description = inObj.Description,
				EntityScopeStr = inObj.EntityScopeStr,
				Id = inObj.Id,
				ModifiedUserId = inObj.ModifiedUserId,
				Name = inObj.Name,
				PropertyScopeStr = inObj.PropertyScopeStr,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				Title = inObj.Title,
				TypeOfItem = inObj.TypeOfItem,
				TypeOfItemStr = inObj.TypeOfItemStr,
				ValueBool = inObj.ValueBool,
				ValueDecimal = inObj.ValueDecimal,
				ValueInt = inObj.ValueInt,
				ValueStr = inObj.ValueStr,
				ModifiedUserName = inObj.ModifiedUserName,
				CreatedUserName = inObj.CreatedUserName,
				NumberOfForeignReferences = inObj.NumberOfForeignReferences
			};
		}
		return result;
	}

	public static List<RepoDynamicalDataTypesAndOptions> Convert(List<DtoDynamicalDataTypesAndOptions> inObj)
	{
		List<RepoDynamicalDataTypesAndOptions> list = null;
		if (inObj != null)
		{
			list = new List<RepoDynamicalDataTypesAndOptions>();
			if (inObj.Any())
			{
				foreach (DtoDynamicalDataTypesAndOptions item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static DtoDynamicalDataTypesAndOptionsResponse Convert(RepositoryResponseBase<RepoDynamicalDataTypesAndOptions> inObj)
	{
		DtoDynamicalDataTypesAndOptionsResponse result = null;
		if (inObj != null)
		{
			result = new DtoDynamicalDataTypesAndOptionsResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static DtoDynamicalDataTypesAndOptionsListResponse Convert(RepositoryResponseBase<IEnumerable<RepoDynamicalDataTypesAndOptions>> inObj)
	{
		DtoDynamicalDataTypesAndOptionsListResponse result = null;
		if (inObj != null)
		{
			result = new DtoDynamicalDataTypesAndOptionsListResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static RepositoryResponseBase<RepoDynamicalDataTypesAndOptions> Convert(DtoDynamicalDataTypesAndOptionsResponse inObj)
	{
		RepositoryResponseBase<RepoDynamicalDataTypesAndOptions> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<RepoDynamicalDataTypesAndOptions>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static RepositoryResponseBase<IEnumerable<RepoDynamicalDataTypesAndOptions>> Convert(DtoDynamicalDataTypesAndOptionsListResponse inObj)
	{
		RepositoryResponseBase<IEnumerable<RepoDynamicalDataTypesAndOptions>> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<IEnumerable<RepoDynamicalDataTypesAndOptions>>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static DtoGlobalSettings Convert(RepoGlobalSettings inObj)
	{
		DtoGlobalSettings result = null;
		if (inObj != null)
		{
			result = new DtoGlobalSettings
			{
				CreatedUserName = inObj.CreatedUserName,
				CreatedUserId = inObj.CreatedUserId,
				GlobalSettingsIndentificator = inObj.GlobalSettingsIndentificator,
				Id = inObj.Id,
				ModifiedUserId = inObj.ModifiedUserId,
				ModifiedUserName = inObj.ModifiedUserName,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				ValueJSon = inObj.ValueJSon
			};
		}
		return result;
	}

	public static List<DtoGlobalSettings> Convert(List<RepoGlobalSettings> inObj)
	{
		List<DtoGlobalSettings> list = null;
		if (inObj != null)
		{
			list = new List<DtoGlobalSettings>();
			if (inObj.Any())
			{
				foreach (RepoGlobalSettings item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static RepoGlobalSettings Convert(DtoGlobalSettings inObj)
	{
		RepoGlobalSettings result = null;
		if (inObj != null)
		{
			result = new RepoGlobalSettings
			{
				CreatedUserName = inObj.CreatedUserName,
				CreatedUserId = inObj.CreatedUserId,
				GlobalSettingsIndentificator = inObj.GlobalSettingsIndentificator,
				Id = inObj.Id,
				ModifiedUserId = inObj.ModifiedUserId,
				ModifiedUserName = inObj.ModifiedUserName,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				ValueJSon = inObj.ValueJSon
			};
		}
		return result;
	}

	public static List<RepoGlobalSettings> Convert(List<DtoGlobalSettings> inObj)
	{
		List<RepoGlobalSettings> list = null;
		if (inObj != null)
		{
			list = new List<RepoGlobalSettings>();
			if (inObj.Any())
			{
				foreach (DtoGlobalSettings item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static DtoGlobalSettingsResponse Convert(RepositoryResponseBase<RepoGlobalSettings> inObj)
	{
		DtoGlobalSettingsResponse result = null;
		if (inObj != null)
		{
			result = new DtoGlobalSettingsResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static DtoGlobalSettingsListResponse Convert(RepositoryResponseBase<IEnumerable<RepoGlobalSettings>> inObj)
	{
		DtoGlobalSettingsListResponse result = null;
		if (inObj != null)
		{
			result = new DtoGlobalSettingsListResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static RepositoryResponseBase<RepoGlobalSettings> Convert(DtoGlobalSettingsResponse inObj)
	{
		RepositoryResponseBase<RepoGlobalSettings> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<RepoGlobalSettings>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static RepositoryResponseBase<IEnumerable<RepoGlobalSettings>> Convert(DtoGlobalSettingsListResponse inObj)
	{
		RepositoryResponseBase<IEnumerable<RepoGlobalSettings>> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<IEnumerable<RepoGlobalSettings>>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static DtoIncidentActivityLog Convert(RepoIncidentActivityLog inObj)
	{
		DtoIncidentActivityLog result = null;
		if (inObj != null)
		{
			result = new DtoIncidentActivityLog
			{
				IncidentId = inObj.IncidentId,
				Id = inObj.Id,
				CreatedUserId = inObj.CreatedUserId,
				Description = inObj.Description,
				EntityId = inObj.EntityId,
				EntityType = inObj.EntityType,
				JsonNewValues = inObj.JsonNewValues,
				JsonOldValues = inObj.JsonOldValues,
				ModifiedUserId = inObj.ModifiedUserId,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				TimeOfActivityLog = inObj.TimeOfActivityLog,
				CreatedUserName = inObj.CreatedUserName,
				ModifiedUserName = inObj.ModifiedUserName
			};
		}
		return result;
	}

	public static List<DtoIncidentActivityLog> Convert(List<RepoIncidentActivityLog> inObj)
	{
		List<DtoIncidentActivityLog> list = null;
		if (inObj != null)
		{
			list = new List<DtoIncidentActivityLog>();
			if (inObj.Any())
			{
				foreach (RepoIncidentActivityLog item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static RepoIncidentActivityLog Convert(DtoIncidentActivityLog inObj)
	{
		RepoIncidentActivityLog result = null;
		if (inObj != null)
		{
			result = new RepoIncidentActivityLog
			{
				IncidentId = inObj.IncidentId,
				Id = inObj.Id,
				CreatedUserId = inObj.CreatedUserId,
				Description = inObj.Description,
				EntityId = inObj.EntityId,
				EntityType = inObj.EntityType,
				JsonNewValues = inObj.JsonNewValues,
				JsonOldValues = inObj.JsonOldValues,
				ModifiedUserId = inObj.ModifiedUserId,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				TimeOfActivityLog = inObj.TimeOfActivityLog,
				ModifiedUserName = inObj.ModifiedUserName,
				CreatedUserName = inObj.CreatedUserName
			};
		}
		return result;
	}

	public static List<RepoIncidentActivityLog> Convert(List<DtoIncidentActivityLog> inObj)
	{
		List<RepoIncidentActivityLog> list = null;
		if (inObj != null)
		{
			list = new List<RepoIncidentActivityLog>();
			if (inObj.Any())
			{
				foreach (DtoIncidentActivityLog item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static DtoIncidentActivityLogResponse Convert(RepositoryResponseBase<RepoIncidentActivityLog> inObj)
	{
		DtoIncidentActivityLogResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentActivityLogResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static DtoIncidentActivityLogListResponse Convert(RepositoryResponseBase<IEnumerable<RepoIncidentActivityLog>> inObj)
	{
		DtoIncidentActivityLogListResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentActivityLogListResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static RepositoryResponseBase<RepoIncidentActivityLog> Convert(DtoIncidentActivityLogResponse inObj)
	{
		RepositoryResponseBase<RepoIncidentActivityLog> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<RepoIncidentActivityLog>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static RepositoryResponseBase<IEnumerable<RepoIncidentActivityLog>> Convert(DtoIncidentActivityLogListResponse inObj)
	{
		RepositoryResponseBase<IEnumerable<RepoIncidentActivityLog>> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<IEnumerable<RepoIncidentActivityLog>>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static DtoIncidentCall Convert(RepoIncidentCall inObj)
	{
		DtoIncidentCall result = null;
		if (inObj != null)
		{
			result = new DtoIncidentCall
			{
				Arrived = inObj.Arrived,
				Called = inObj.Called,
				CreatedUserId = inObj.CreatedUserId,
				Departed = inObj.Departed,
				Id = inObj.Id,
				IncidentId = inObj.IncidentId,
				ModifiedUserId = inObj.ModifiedUserId,
				Notes = inObj.Notes,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				CreatedUserName = inObj.CreatedUserName,
				ModifiedUserName = inObj.ModifiedUserName
			};
		}
		return result;
	}

	public static List<DtoIncidentCall> Convert(List<RepoIncidentCall> inObj)
	{
		List<DtoIncidentCall> list = null;
		if (inObj != null)
		{
			list = new List<DtoIncidentCall>();
			if (inObj.Any())
			{
				foreach (RepoIncidentCall item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static RepoIncidentCall Convert(DtoIncidentCall inObj)
	{
		RepoIncidentCall result = null;
		if (inObj != null)
		{
			result = new RepoIncidentCall
			{
				Arrived = inObj.Arrived,
				Called = inObj.Called,
				CreatedUserId = inObj.CreatedUserId,
				Departed = inObj.Departed,
				Id = inObj.Id,
				IncidentId = inObj.IncidentId,
				ModifiedUserId = inObj.ModifiedUserId,
				Notes = inObj.Notes,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				ModifiedUserName = inObj.ModifiedUserName,
				CreatedUserName = inObj.CreatedUserName
			};
		}
		return result;
	}

	public static List<RepoIncidentCall> Convert(List<DtoIncidentCall> inObj)
	{
		List<RepoIncidentCall> list = null;
		if (inObj != null)
		{
			list = new List<RepoIncidentCall>();
			if (inObj.Any())
			{
				foreach (DtoIncidentCall item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static DtoIncidentCallResponse Convert(RepositoryResponseBase<RepoIncidentCall> inObj)
	{
		DtoIncidentCallResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentCallResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static DtoIncidentCallListResponse Convert(RepositoryResponseBase<IEnumerable<RepoIncidentCall>> inObj)
	{
		DtoIncidentCallListResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentCallListResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static RepositoryResponseBase<RepoIncidentCall> Convert(DtoIncidentCallResponse inObj)
	{
		RepositoryResponseBase<RepoIncidentCall> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<RepoIncidentCall>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static RepositoryResponseBase<IEnumerable<RepoIncidentCall>> Convert(DtoIncidentCallListResponse inObj)
	{
		RepositoryResponseBase<IEnumerable<RepoIncidentCall>> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<IEnumerable<RepoIncidentCall>>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static DtoIncidentComment Convert(RepoIncidentComment inObj)
	{
		DtoIncidentComment result = null;
		if (inObj != null)
		{
			result = new DtoIncidentComment
			{
				Id = inObj.Id,
				Comment = inObj.Comment,
				CreatedUserId = inObj.CreatedUserId,
				IncidentId = inObj.IncidentId,
				ModifiedUserId = inObj.ModifiedUserId,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				User = inObj.User,
				CreatedUserName = inObj.CreatedUserName,
				ModifiedUserName = inObj.ModifiedUserName
			};
		}
		return result;
	}

	public static List<DtoIncidentComment> Convert(List<RepoIncidentComment> inObj)
	{
		List<DtoIncidentComment> list = null;
		if (inObj != null)
		{
			list = new List<DtoIncidentComment>();
			if (inObj.Any())
			{
				foreach (RepoIncidentComment item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static RepoIncidentComment Convert(DtoIncidentComment inObj)
	{
		RepoIncidentComment result = null;
		if (inObj != null)
		{
			result = new RepoIncidentComment
			{
				Id = inObj.Id,
				Comment = inObj.Comment,
				CreatedUserId = inObj.CreatedUserId,
				IncidentId = inObj.IncidentId,
				ModifiedUserId = inObj.ModifiedUserId,
				SystemString = inObj.SystemString,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				User = inObj.User,
				ModifiedUserName = inObj.ModifiedUserName,
				CreatedUserName = inObj.CreatedUserName
			};
		}
		return result;
	}

	public static List<RepoIncidentComment> Convert(List<DtoIncidentComment> inObj)
	{
		List<RepoIncidentComment> list = null;
		if (inObj != null)
		{
			list = new List<RepoIncidentComment>();
			if (inObj.Any())
			{
				foreach (DtoIncidentComment item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static DtoIncidentCommentResponse Convert(RepositoryResponseBase<RepoIncidentComment> inObj)
	{
		DtoIncidentCommentResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentCommentResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static DtoIncidentEvidenceListResponse Convert(RepositoryResponseBase<IEnumerable<RepoIncidentComment>> inObj)
	{
		DtoIncidentEvidenceListResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentEvidenceListResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static RepositoryResponseBase<RepoIncidentComment> Convert(DtoIncidentCommentResponse inObj)
	{
		RepositoryResponseBase<RepoIncidentComment> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<RepoIncidentComment>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static RepositoryResponseBase<IEnumerable<RepoIncidentComment>> Convert(DtoIncidentEvidenceListResponse inObj)
	{
		RepositoryResponseBase<IEnumerable<RepoIncidentComment>> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<IEnumerable<RepoIncidentComment>>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static DtoIncident Convert(RepoIncident inObj)
	{
		DtoIncident dtoIncident = null;
		if (inObj != null)
		{
			dtoIncident = new DtoIncident
			{
				Category2Id = inObj.Category2Id,
				Category2 = Convert(inObj.Category2),
				Category4Id = inObj.Category4Id,
				Category4 = Convert(inObj.Category4),
				CreatedUserId = inObj.CreatedUserId,
				Date = inObj.Date,
				Description = inObj.Description,
				End = inObj.End,
				Id = inObj.Id,
				Category3Id = inObj.Category3Id,
				Category3 = Convert(inObj.Category3),
				Category5Id = inObj.Category5Id,
				Category5 = Convert(inObj.Category5),
				ModifiedUserId = inObj.ModifiedUserId,
				TypeId = inObj.TypeId,
				Type = Convert(inObj.Type),
				Reference = inObj.Reference,
				ReferenceStr = inObj.ReferenceStr,
				Category1Id = inObj.Category1Id,
				Category1 = Convert(inObj.Category1),
				Start = inObj.Start,
				StartStr = inObj.StartStr,
				StatusId = inObj.StatusId,
				Status = Convert(inObj.Status),
				StatusStr = inObj.StatusStr,
				SequencesCount = (inObj.SequencesList?.Count ?? 0),
				SystemString = inObj.SystemString,
				Time = inObj.Time,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				TypeStr = inObj.TypeStr,
				CreatedUserName = inObj.CreatedUserName,
				ModifiedUserName = inObj.ModifiedUserName,
				ExtendedPropsGroup1_JSonConfiguration = inObj.ExtendedPropsGroup1_JSonConfiguration,
				ExtendedPropsGroup2_JSonConfiguration = inObj.ExtendedPropsGroup2_JSonConfiguration,
				ExtPropBool1 = inObj.ExtPropBool1,
				ExtPropBool1_Desc = inObj.ExtPropBool1_Desc,
				ExtPropBool1_Title = inObj.ExtPropBool1_Title,
				ExtPropBool2 = inObj.ExtPropBool2,
				ExtPropBool2_Desc = inObj.ExtPropBool2_Desc,
				ExtPropBool2_Title = inObj.ExtPropBool2_Title,
				ExtPropBool3 = inObj.ExtPropBool3,
				ExtPropBool3_Desc = inObj.ExtPropBool3_Desc,
				ExtPropBool3_Title = inObj.ExtPropBool3_Title,
				ExtPropDateTime1 = inObj.ExtPropDateTime1,
				ExtPropDateTime1_Configuration = inObj.ExtPropDateTime1_Configuration,
				ExtPropDateTime1_Desc = inObj.ExtPropDateTime1_Desc,
				ExtPropDateTime1_Title = inObj.ExtPropDateTime1_Title,
				ExtPropDateTime2 = inObj.ExtPropDateTime2,
				ExtPropDateTime2_Configuration = inObj.ExtPropDateTime2_Configuration,
				ExtPropDateTime2_Desc = inObj.ExtPropDateTime2_Desc,
				ExtPropDateTime2_Title = inObj.ExtPropDateTime2_Title,
				ExtPropDateTime3 = inObj.ExtPropDateTime3,
				ExtPropDateTime3_Configuration = inObj.ExtPropDateTime3_Configuration,
				ExtPropDateTime3_Desc = inObj.ExtPropDateTime3_Desc,
				ExtPropDateTime3_Title = inObj.ExtPropDateTime3_Title,
				ExtPropDecimal1 = inObj.ExtPropDecimal1,
				ExtPropDecimal1_Desc = inObj.ExtPropDecimal1_Desc,
				ExtPropDecimal1_Title = inObj.ExtPropDecimal1_Title,
				ExtPropDecimal2 = inObj.ExtPropDecimal2,
				ExtPropDecimal2_Desc = inObj.ExtPropDecimal2_Desc,
				ExtPropDecimal2_Title = inObj.ExtPropDecimal2_Title,
				ExtPropDecimal3 = inObj.ExtPropDecimal3,
				ExtPropDecimal3_Desc = inObj.ExtPropDecimal3_Desc,
				ExtPropDecimal3_Title = inObj.ExtPropDecimal3_Title,
				ExtPropJSon1 = inObj.ExtPropJSon1,
				ExtPropJSon1_Configuration = inObj.ExtPropJSon1_Configuration,
				ExtPropJSon1_Desc = inObj.ExtPropJSon1_Desc,
				ExtPropJSon1_Title = inObj.ExtPropJSon1_Title,
				ExtPropJSon2 = inObj.ExtPropJSon2,
				ExtPropJSon2_Configuration = inObj.ExtPropJSon2_Configuration,
				ExtPropJSon2_Desc = inObj.ExtPropJSon2_Desc,
				ExtPropJSon2_Title = inObj.ExtPropJSon2_Title,
				ExtPropJSon3 = inObj.ExtPropJSon3,
				ExtPropJSon3_Configuration = inObj.ExtPropJSon3_Configuration,
				ExtPropJSon3_Desc = inObj.ExtPropJSon3_Desc,
				ExtPropJSon3_Title = inObj.ExtPropJSon3_Title,
				ExtPropLong1 = inObj.ExtPropLong1,
				ExtPropLong1_Desc = inObj.ExtPropLong1_Desc,
				ExtPropLong1_Title = inObj.ExtPropLong1_Title,
				ExtPropLong2 = inObj.ExtPropLong2,
				ExtPropLong2_Desc = inObj.ExtPropLong2_Desc,
				ExtPropLong2_Title = inObj.ExtPropLong2_Title,
				ExtPropLong3 = inObj.ExtPropLong3,
				ExtPropLong3_Desc = inObj.ExtPropLong3_Desc,
				ExtPropLong3_Title = inObj.ExtPropLong3_Title,
				ExtPropString1 = inObj.ExtPropString1,
				ExtPropString1_Desc = inObj.ExtPropString1_Desc,
				ExtPropString1_Title = inObj.ExtPropString1_Title,
				ExtPropString2 = inObj.ExtPropString2,
				ExtPropString2_Desc = inObj.ExtPropString2_Desc,
				ExtPropString2_Title = inObj.ExtPropString2_Title,
				ExtPropString3 = inObj.ExtPropString3,
				ExtPropString3_Desc = inObj.ExtPropString3_Desc,
				ExtPropString3_Title = inObj.ExtPropString3_Title,
				FlagNoDb_UserOpenedIncident = inObj.FlagNoDb_UserOpenedIncident,
				FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog = inObj.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog
			};
			int? num = inObj?.SequencesList?.Count;
			if (num.HasValue && num.Value > 0)
			{
				dtoIncident.DynamicLockerStatusStrDb1 = $"{num} Videos in Incident";
				dtoIncident.DynamicLockerStatusStrDb2 = $"{num} Videos in Incident";
			}
			else
			{
				dtoIncident.DynamicLockerStatusStrDb1 = "No Videos";
				dtoIncident.DynamicLockerStatusStrDb2 = "No Videos";
			}
		}
		return dtoIncident;
	}

	public static List<DtoIncident> Convert(List<RepoIncident> inObj)
	{
		List<DtoIncident> list = null;
		if (inObj != null)
		{
			list = new List<DtoIncident>();
			if (inObj.Any())
			{
				foreach (RepoIncident item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static RepoIncident Convert(DtoIncident inObj)
	{
		RepoIncident result = null;
		if (inObj != null)
		{
			result = new RepoIncident
			{
				Category2Id = inObj.Category2Id,
				Category4Id = inObj.Category4Id,
				CreatedUserId = inObj.CreatedUserId,
				Date = inObj.Date,
				Description = inObj.Description,
				End = inObj.End,
				Id = inObj.Id,
				Category3Id = inObj.Category3Id,
				Category5Id = inObj.Category5Id,
				ModifiedUserId = inObj.ModifiedUserId,
				TypeId = inObj.TypeId,
				Reference = inObj.Reference,
				ReferenceStr = inObj.ReferenceStr,
				Category1Id = inObj.Category1Id,
				Start = inObj.Start,
				StartStr = inObj.StartStr,
				StatusId = inObj.StatusId,
				StatusStr = inObj.StatusStr,
				SystemString = inObj.SystemString,
				Time = inObj.Time,
				TimeCreated = inObj.TimeCreated,
				TimeModified = inObj.TimeModified,
				TypeStr = inObj.TypeStr,
				ModifiedUserName = inObj.ModifiedUserName,
				CreatedUserName = inObj.CreatedUserName,
				ExtendedPropsGroup1_JSonConfiguration = inObj.ExtendedPropsGroup1_JSonConfiguration,
				ExtendedPropsGroup2_JSonConfiguration = inObj.ExtendedPropsGroup2_JSonConfiguration,
				ExtPropBool1 = inObj.ExtPropBool1,
				ExtPropBool1_Desc = inObj.ExtPropBool1_Desc,
				ExtPropBool1_Title = inObj.ExtPropBool1_Title,
				ExtPropBool2 = inObj.ExtPropBool2,
				ExtPropBool2_Desc = inObj.ExtPropBool2_Desc,
				ExtPropBool2_Title = inObj.ExtPropBool2_Title,
				ExtPropBool3 = inObj.ExtPropBool3,
				ExtPropBool3_Desc = inObj.ExtPropBool3_Desc,
				ExtPropBool3_Title = inObj.ExtPropBool3_Title,
				ExtPropDateTime1 = inObj.ExtPropDateTime1,
				ExtPropDateTime1_Configuration = inObj.ExtPropDateTime1_Configuration,
				ExtPropDateTime1_Desc = inObj.ExtPropDateTime1_Desc,
				ExtPropDateTime1_Title = inObj.ExtPropDateTime1_Title,
				ExtPropDateTime2 = inObj.ExtPropDateTime2,
				ExtPropDateTime2_Configuration = inObj.ExtPropDateTime2_Configuration,
				ExtPropDateTime2_Desc = inObj.ExtPropDateTime2_Desc,
				ExtPropDateTime2_Title = inObj.ExtPropDateTime2_Title,
				ExtPropDateTime3 = inObj.ExtPropDateTime3,
				ExtPropDateTime3_Configuration = inObj.ExtPropDateTime3_Configuration,
				ExtPropDateTime3_Desc = inObj.ExtPropDateTime3_Desc,
				ExtPropDateTime3_Title = inObj.ExtPropDateTime3_Title,
				ExtPropDecimal1 = inObj.ExtPropDecimal1,
				ExtPropDecimal1_Desc = inObj.ExtPropDecimal1_Desc,
				ExtPropDecimal1_Title = inObj.ExtPropDecimal1_Title,
				ExtPropDecimal2 = inObj.ExtPropDecimal2,
				ExtPropDecimal2_Desc = inObj.ExtPropDecimal2_Desc,
				ExtPropDecimal2_Title = inObj.ExtPropDecimal2_Title,
				ExtPropDecimal3 = inObj.ExtPropDecimal3,
				ExtPropDecimal3_Desc = inObj.ExtPropDecimal3_Desc,
				ExtPropDecimal3_Title = inObj.ExtPropDecimal3_Title,
				ExtPropJSon1 = inObj.ExtPropJSon1,
				ExtPropJSon1_Configuration = inObj.ExtPropJSon1_Configuration,
				ExtPropJSon1_Desc = inObj.ExtPropJSon1_Desc,
				ExtPropJSon1_Title = inObj.ExtPropJSon1_Title,
				ExtPropJSon2 = inObj.ExtPropJSon2,
				ExtPropJSon2_Configuration = inObj.ExtPropJSon2_Configuration,
				ExtPropJSon2_Desc = inObj.ExtPropJSon2_Desc,
				ExtPropJSon2_Title = inObj.ExtPropJSon2_Title,
				ExtPropJSon3 = inObj.ExtPropJSon3,
				ExtPropJSon3_Configuration = inObj.ExtPropJSon3_Configuration,
				ExtPropJSon3_Desc = inObj.ExtPropJSon3_Desc,
				ExtPropJSon3_Title = inObj.ExtPropJSon3_Title,
				ExtPropLong1 = inObj.ExtPropLong1,
				ExtPropLong1_Desc = inObj.ExtPropLong1_Desc,
				ExtPropLong1_Title = inObj.ExtPropLong1_Title,
				ExtPropLong2 = inObj.ExtPropLong2,
				ExtPropLong2_Desc = inObj.ExtPropLong2_Desc,
				ExtPropLong2_Title = inObj.ExtPropLong2_Title,
				ExtPropLong3 = inObj.ExtPropLong3,
				ExtPropLong3_Desc = inObj.ExtPropLong3_Desc,
				ExtPropLong3_Title = inObj.ExtPropLong3_Title,
				ExtPropString1 = inObj.ExtPropString1,
				ExtPropString1_Desc = inObj.ExtPropString1_Desc,
				ExtPropString1_Title = inObj.ExtPropString1_Title,
				ExtPropString2 = inObj.ExtPropString2,
				ExtPropString2_Desc = inObj.ExtPropString2_Desc,
				ExtPropString2_Title = inObj.ExtPropString2_Title,
				ExtPropString3 = inObj.ExtPropString3,
				ExtPropString3_Desc = inObj.ExtPropString3_Desc,
				ExtPropString3_Title = inObj.ExtPropString3_Title,
				FlagNoDb_UserOpenedIncident = inObj.FlagNoDb_UserOpenedIncident,
				FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog = inObj.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog
			};
		}
		return result;
	}

	public static List<RepoIncident> Convert(List<DtoIncident> inObj)
	{
		List<RepoIncident> list = null;
		if (inObj != null)
		{
			list = new List<RepoIncident>();
			if (inObj.Any())
			{
				foreach (DtoIncident item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static DtoIncidentResponse Convert(RepositoryResponseBase<RepoIncident> inObj)
	{
		DtoIncidentResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static DtoIncidentListResponse Convert(RepositoryResponseBase<IEnumerable<RepoIncident>> inObj)
	{
		DtoIncidentListResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentListResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList()),
				PagingObject = Convert(inObj.PagingObject)
			};
		}
		return result;
	}

	public static RepositoryResponseBase<RepoIncident> Convert(DtoIncidentResponse inObj)
	{
		RepositoryResponseBase<RepoIncident> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<RepoIncident>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static RepositoryResponseBase<IEnumerable<RepoIncident>> Convert(DtoIncidentListResponse inObj)
	{
		RepositoryResponseBase<IEnumerable<RepoIncident>> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<IEnumerable<RepoIncident>>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static DtoIncidentSequence Convert(RepoIncidentSequence inObj)
	{
		DtoIncidentSequence result = null;
		if (inObj != null)
		{
			result = new DtoIncidentSequence
			{
				TimeModified = inObj.TimeModified,
				TimeCreated = inObj.TimeCreated,
				SystemString = inObj.SystemString,
				ModifiedUserId = inObj.ModifiedUserId,
				IncidentId = inObj.IncidentId,
				CameraId = inObj.CameraId,
				CameraTitle = inObj.CameraTitle,
				DraftSequenceId = inObj.DraftSequenceId,
				CreatedUserId = inObj.CreatedUserId,
				StartTime = inObj.StartTime,
				Id = inObj.Id,
				OrderNumber = inObj.OrderNumber,
				EndTime = inObj.EndTime,
				CreatedUserName = inObj.CreatedUserName,
				ModifiedUserName = inObj.ModifiedUserName,
				FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog = inObj.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog
			};
		}
		return result;
	}

	public static List<DtoIncidentSequence> Convert(List<RepoIncidentSequence> inObj)
	{
		List<DtoIncidentSequence> list = null;
		if (inObj != null)
		{
			list = new List<DtoIncidentSequence>();
			if (inObj.Any())
			{
				foreach (RepoIncidentSequence item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static RepoIncidentSequence Convert(DtoIncidentSequence inObj)
	{
		RepoIncidentSequence result = null;
		if (inObj != null)
		{
			result = new RepoIncidentSequence
			{
				TimeModified = inObj.TimeModified,
				TimeCreated = inObj.TimeCreated,
				SystemString = inObj.SystemString,
				ModifiedUserId = inObj.ModifiedUserId,
				IncidentId = inObj.IncidentId,
				CameraId = inObj.CameraId,
				CameraTitle = inObj.CameraTitle,
				DraftSequenceId = inObj.DraftSequenceId,
				CreatedUserId = inObj.CreatedUserId,
				StartTime = inObj.StartTime,
				Id = inObj.Id,
				OrderNumber = inObj.OrderNumber,
				EndTime = inObj.EndTime,
				ModifiedUserName = inObj.ModifiedUserName,
				CreatedUserName = inObj.CreatedUserName,
				FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog = inObj.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog
			};
		}
		return result;
	}

	public static List<RepoIncidentSequence> Convert(List<DtoIncidentSequence> inObj)
	{
		List<RepoIncidentSequence> list = null;
		if (inObj != null)
		{
			list = new List<RepoIncidentSequence>();
			if (inObj.Any())
			{
				foreach (DtoIncidentSequence item in inObj)
				{
					if (item != null)
					{
						list.Add(Convert(item));
					}
				}
			}
		}
		return list;
	}

	public static DtoIncidentSequenceResponse Convert(RepositoryResponseBase<RepoIncidentSequence> inObj)
	{
		DtoIncidentSequenceResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentSequenceResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static DtoIncidentSequenceListResponse Convert(RepositoryResponseBase<IEnumerable<RepoIncidentSequence>> inObj)
	{
		DtoIncidentSequenceListResponse result = null;
		if (inObj != null)
		{
			result = new DtoIncidentSequenceListResponse
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static RepositoryResponseBase<RepoIncidentSequence> Convert(DtoIncidentSequenceResponse inObj)
	{
		RepositoryResponseBase<RepoIncidentSequence> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<RepoIncidentSequence>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value)
			};
		}
		return result;
	}

	public static RepositoryResponseBase<IEnumerable<RepoIncidentSequence>> Convert(DtoIncidentSequenceListResponse inObj)
	{
		RepositoryResponseBase<IEnumerable<RepoIncidentSequence>> result = null;
		if (inObj != null)
		{
			result = new RepositoryResponseBase<IEnumerable<RepoIncidentSequence>>
			{
				Message = inObj.Message,
				MessageDescription = inObj.MessageDescription,
				Success = inObj.Success,
				Value = Convert(inObj.Value?.ToList())
			};
		}
		return result;
	}

	public static DtoPaging Convert(RepositoryPaging inObj)
	{
		if (inObj == null)
		{
			return new DtoPaging();
		}
		return new DtoPaging
		{
			CountPerPage = inObj.CountPerPage,
			Page = inObj.Page,
			TotalCount = inObj.TotalCount,
			TotalPages = inObj.TotalPages,
			Filters = inObj.Filters,
			FiltersType = inObj.FiltersType,
			Orders = inObj.Orders
		};
	}

	public static RepositoryPaging Convert(DtoPaging inObj)
	{
		RepositoryPaging result = null;
		if (inObj != null)
		{
			result = new RepositoryPaging
			{
				CountPerPage = inObj.CountPerPage,
				Page = inObj.Page,
				TotalCount = inObj.TotalCount,
				TotalPages = inObj.TotalPages,
				Filters = inObj.Filters,
				FiltersType = inObj.FiltersType,
				Orders = inObj.Orders
			};
		}
		return result;
	}
}
