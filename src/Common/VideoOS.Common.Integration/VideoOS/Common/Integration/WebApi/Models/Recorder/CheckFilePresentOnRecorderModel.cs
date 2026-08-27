using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class CheckFilePresentOnRecorderModel : CommandModelBase
{
	[Required]
	public string FileId { get; set; }
}
