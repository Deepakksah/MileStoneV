using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class PerformFileSystemOperationModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid ServerId { get; set; }

	[Required]
	public ServerFileSystemServerTypeDto ServerType { get; set; }

	[Required]
	public ServerFileSystemOperationDto Operation { get; set; }

	public string Path { get; set; }
}
