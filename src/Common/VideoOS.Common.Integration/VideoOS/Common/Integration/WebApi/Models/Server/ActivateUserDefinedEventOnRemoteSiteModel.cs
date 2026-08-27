using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ActivateUserDefinedEventOnRemoteSiteModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid SiteId { get; set; }

	[Required]
	public string UserDefinedEventNameOrId { get; set; }
}
