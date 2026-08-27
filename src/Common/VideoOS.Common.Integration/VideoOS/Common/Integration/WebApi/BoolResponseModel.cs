using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi;

public class BoolResponseModel : ResponseModel
{
	[Required]
	public bool Value { get; set; }
}
