using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentComment;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentComment;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.WebApi.Filters;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.WebApi.Controllers.webapi.v1;

public class IncidentCommentController : BaseController
{
	public IncidentCommentController()
	{
	}

	public IncidentCommentController(IDtoDAL dataAccessLayer)
		: base(dataAccessLayer)
	{
	}

	[CheckPermission("GENERIC_READ", "567A4819-A38C-4E17-A982-45C780485C13")]
	public async Task<DtoIncidentEvidenceListResponse> GetAsync(string inPagingJson, CancellationToken cancellationToken)
	{
		DtoPaging inPaging = null;
		if (!string.IsNullOrWhiteSpace(inPagingJson))
		{
			inPaging = JsonConvert.DeserializeObject<DtoPaging>(inPagingJson);
		}
		return await DataAccessLayer.GetIncidentCommentDAL().GetListAsync(inPaging, cancellationToken);
	}

	[CheckPermission("GENERIC_READ", "567A4819-A38C-4E17-A982-45C780485C13")]
	public async Task<DtoIncidentCommentResponse> GetAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetIncidentCommentDAL().GetAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "567A4819-A38C-4E17-A982-45C780485C13")]
	public async Task<DtoIncidentCommentResponse> PostAsync([FromBody] DtoIncidentComment value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PostAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentComment.cs", 55);
		return await DataAccessLayer.GetIncidentCommentDAL().CreateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "567A4819-A38C-4E17-A982-45C780485C13")]
	public async Task<DtoIncidentCommentResponse> PutAsync(long id, [FromBody] DtoIncidentComment value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PutAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentComment.cs", 65);
		if (value.Id != id)
		{
			value.Id = id;
		}
		return await DataAccessLayer.GetIncidentCommentDAL().UpdateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "567A4819-A38C-4E17-A982-45C780485C13")]
	public async Task<DtoIncidentCommentResponse> DeleteAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetIncidentCommentDAL().DeleteAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "567A4819-A38C-4E17-A982-45C780485C13")]
	public async Task<DtoIncidentEvidenceListResponse> DeleteRangeAsync([NotNull][FromUri] long[] ids, CancellationToken cancellationToken)
	{
		ids.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentComment.cs", 86);
		return await DataAccessLayer.GetIncidentCommentDAL().DeleteRangeAsync(ids.ToHashSet(), cancellationToken);
	}
}
