using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class BulkEventModel : CommandModelBase
{
	[Required]
	public IEnumerable<TriggerEventCommandPackageDto> EventCommands { get; set; }
}
