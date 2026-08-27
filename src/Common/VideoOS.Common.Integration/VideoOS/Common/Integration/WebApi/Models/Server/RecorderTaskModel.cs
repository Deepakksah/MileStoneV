using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class RecorderTaskModel
{
	[Required]
	public IEnumerable<TaskModel> Tasks { get; set; }
}
