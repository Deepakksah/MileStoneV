using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class SetMatrixViewModel : CommandModelBase
{
	public Guid MatrixId { get; set; }

	public IEnumerable<Guid> DeviceIds { get; set; }
}
