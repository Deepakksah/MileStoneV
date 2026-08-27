using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public sealed class SecurityUserTokensModel : CommandModelBase
{
	[Required]
	public SecurityTokenCollectionDto2 SecurityTokensDto { get; set; }
}
