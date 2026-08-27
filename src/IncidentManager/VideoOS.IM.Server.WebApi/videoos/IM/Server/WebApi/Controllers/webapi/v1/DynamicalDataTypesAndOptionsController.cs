using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.GlobalDefinitions.UserAccessManagement;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Server.Logger.AuditLog;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.WebApi.Filters;
using VideoOS.IM.Server.WebApi.Utils;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.WebApi.Controllers.webapi.v1;

public class DynamicalDataTypesAndOptionsController : BaseController
{
	private static readonly IDictionary<(string, string), Guid> MessageIds = new Dictionary<(string, string), Guid>
	{
		{
			("PostAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Type),
			AuditLogMessageIds.TypeCreated
		},
		{
			("PostAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Status),
			AuditLogMessageIds.StatusCreated
		},
		{
			("PostAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category1),
			AuditLogMessageIds.CategoryCreated
		},
		{
			("PostAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category2),
			AuditLogMessageIds.CategoryCreated
		},
		{
			("PostAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category3),
			AuditLogMessageIds.CategoryCreated
		},
		{
			("PostAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category4),
			AuditLogMessageIds.CategoryCreated
		},
		{
			("PostAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category5),
			AuditLogMessageIds.CategoryCreated
		},
		{
			("PutAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Type),
			AuditLogMessageIds.TypeModified
		},
		{
			("PutAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Status),
			AuditLogMessageIds.StatusModified
		},
		{
			("PutAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category1),
			AuditLogMessageIds.CategoryModified
		},
		{
			("PutAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category2),
			AuditLogMessageIds.CategoryModified
		},
		{
			("PutAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category3),
			AuditLogMessageIds.CategoryModified
		},
		{
			("PutAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category4),
			AuditLogMessageIds.CategoryModified
		},
		{
			("PutAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category5),
			AuditLogMessageIds.CategoryModified
		},
		{
			("DeleteAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Type),
			AuditLogMessageIds.TypeDeleted
		},
		{
			("DeleteAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Status),
			AuditLogMessageIds.StatusDeleted
		},
		{
			("DeleteAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category1),
			AuditLogMessageIds.CategoryDeleted
		},
		{
			("DeleteAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category2),
			AuditLogMessageIds.CategoryDeleted
		},
		{
			("DeleteAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category3),
			AuditLogMessageIds.CategoryDeleted
		},
		{
			("DeleteAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category4),
			AuditLogMessageIds.CategoryDeleted
		},
		{
			("DeleteAsync", DynamicalTypesAndOptionsScopeStrings.IncidentDetails_Category5),
			AuditLogMessageIds.CategoryDeleted
		}
	};

	public DynamicalDataTypesAndOptionsController()
	{
	}

	public DynamicalDataTypesAndOptionsController(IDtoDAL dataAccessLayer)
		: base(dataAccessLayer)
	{
	}

	[CheckPermission("GENERIC_READ", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoDynamicalDataTypesAndOptionsListResponse> GetAsync(string inPagingJson, CancellationToken cancellationToken)
	{
		DtoPaging inPaging = null;
		if (!string.IsNullOrWhiteSpace(inPagingJson))
		{
			inPaging = JsonConvert.DeserializeObject<DtoPaging>(inPagingJson);
		}
		return await DataAccessLayer.GetDynamicalDataTypesAndOptionsDAL().GetListAsync(inPaging, cancellationToken);
	}

	[CheckPermission("GENERIC_READ", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoDynamicalDataTypesAndOptionsResponse> GetAsync(long id)
	{
		return await DataAccessLayer.GetDynamicalDataTypesAndOptionsDAL().GetAsync(id);
	}

	[CheckPermission("GENERIC_WRITE", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoDynamicalDataTypesAndOptionsResponse> PostAsync([FromBody] DtoDynamicalDataTypesAndOptions value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PostAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\DynamicalDataTypesAndOptions.cs", 85);
		DtoDynamicalDataTypesAndOptionsResponse response = await DataAccessLayer.GetDynamicalDataTypesAndOptionsDAL().CreateAsync(value, cancellationToken);
		if (response != null && response.Success && response.Value != null)
		{
			Guid guid = MessageIds[("PostAsync", response.Value.PropertyScopeStr)];
			await WriteAuditLogEntryAsync(guid, AuditLogHelper.GetAuditMessageId(guid), response.Value.Id.ToString(), response.Value.Title);
		}
		return response;
	}

	[CheckPermission("GENERIC_WRITE", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoDynamicalDataTypesAndOptionsResponse> PutAsync(long id, [FromBody] DtoDynamicalDataTypesAndOptions value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PutAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\DynamicalDataTypesAndOptions.cs", 103);
		if (value.Id != id)
		{
			value.Id = id;
		}
		DtoDynamicalDataTypesAndOptionsResponse response = await DataAccessLayer.GetDynamicalDataTypesAndOptionsDAL().UpdateAsync(value, cancellationToken);
		if (response != null && response.Success && response.Value != null)
		{
			Guid guid = MessageIds[("PutAsync", response.Value.PropertyScopeStr)];
			await WriteAuditLogEntryAsync(guid, AuditLogHelper.GetAuditMessageId(guid), response.Value.Id.ToString(), response.Value.Title);
		}
		return response;
	}

	[CheckPermission("GENERIC_WRITE", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoDynamicalDataTypesAndOptionsResponse> DeleteAsync(long id, CancellationToken cancellationToken)
	{
		DtoDynamicalDataTypesAndOptionsResponse response = await DataAccessLayer.GetDynamicalDataTypesAndOptionsDAL().DeleteAsync(id, cancellationToken);
		if (response != null && response.Success && response.Value != null)
		{
			Guid guid = MessageIds[("DeleteAsync", response.Value.PropertyScopeStr)];
			await WriteAuditLogEntryAsync(guid, AuditLogHelper.GetAuditMessageId(guid), response.Value.Id.ToString(), response.Value.Title);
		}
		return response;
	}

	[CheckPermission("GENERIC_WRITE", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoDynamicalDataTypesAndOptionsListResponse> DeleteRangeAsync([NotNull][FromUri] long[] ids, CancellationToken cancellationToken)
	{
		ids.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\DynamicalDataTypesAndOptions.cs", 141);
		DtoDynamicalDataTypesAndOptionsListResponse response = await DataAccessLayer.GetDynamicalDataTypesAndOptionsDAL().DeleteRangeAsync(ids.ToHashSet(), cancellationToken);
		if (response != null && response.Success && response.Value != null)
		{
			foreach (DtoDynamicalDataTypesAndOptions item in response.Value)
			{
				Guid guid = MessageIds[("DeleteAsync", item.PropertyScopeStr)];
				await WriteAuditLogEntryAsync(guid, AuditLogHelper.GetAuditMessageId(guid), item.Id.ToString(), item.Title);
			}
		}
		return response;
	}
}
