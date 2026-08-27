using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi;

public class ExceptionResponseModel : ResponseModel
{
	[Required]
	public Exception Exception { get; set; }
}
