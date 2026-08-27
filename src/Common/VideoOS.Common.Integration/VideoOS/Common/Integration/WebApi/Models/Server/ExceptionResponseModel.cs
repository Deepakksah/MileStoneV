using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ExceptionResponseModel : ResponseModel
{
	[Required]
	public Exception Exception { get; set; }
}
