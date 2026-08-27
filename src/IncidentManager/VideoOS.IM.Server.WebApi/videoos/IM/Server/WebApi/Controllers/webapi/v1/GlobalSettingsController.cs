using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.GlobalSettings;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.GlobalSettings;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Server.Logger.AuditLog;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.WebApi.Filters;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.WebApi.Controllers.webapi.v1;

public class GlobalSettingsController : BaseController
{
	public GlobalSettingsController()
	{
	}

	public GlobalSettingsController(IDtoDAL dataAccessLayer)
		: base(dataAccessLayer)
	{
	}

	public async Task<DtoGlobalSettingsListResponse> GetAsync(string inPagingJson, CancellationToken cancellationToken)
	{
		DtoPaging inPaging = null;
		if (!string.IsNullOrWhiteSpace(inPagingJson))
		{
			inPaging = JsonConvert.DeserializeObject<DtoPaging>(inPagingJson);
		}
		return await DataAccessLayer.GetGlobalSettingsDAL().GetListAsync(inPaging, cancellationToken);
	}

	public async Task<DtoGlobalSettingsResponse> GetAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetGlobalSettingsDAL().GetAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoGlobalSettingsResponse> PostAsync([FromBody] DtoGlobalSettings value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PostAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\GlobalSettings.cs", 55);
		return await DataAccessLayer.GetGlobalSettingsDAL().CreateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoGlobalSettingsResponse> PutAsync(long id, [FromBody] DtoGlobalSettings value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PutAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\GlobalSettings.cs", 65);
		if (value.Id != id)
		{
			value.Id = id;
		}
		DtoGlobalSettingsResponse response = await DataAccessLayer.GetGlobalSettingsDAL().UpdateAsync(value, cancellationToken);
		if (response != null && response.Success && response.Value != null && response.Value.GlobalSettingsIndentificator == "{incidentManager-incident-extendedInformationConfiguration}")
		{
			await WriteAuditLogEntryAsync(AuditLogMessageIds.IncidentCategoriesModified, "IncidentCategoriesModified");
		}
		return response;
	}

	[CheckPermission("GENERIC_WRITE", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoGlobalSettingsResponse> DeleteAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetGlobalSettingsDAL().DeleteAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "77FA1106-FA1D-4C52-A945-B6F87F89DEEC")]
	public async Task<DtoGlobalSettingsListResponse> DeleteRangeAsync([NotNull][FromUri] long[] ids, CancellationToken cancellationToken)
	{
		ids.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\GlobalSettings.cs", 94);
		return await DataAccessLayer.GetGlobalSettingsDAL().DeleteRangeAsync(ids.ToHashSet(), cancellationToken);
	}
}
