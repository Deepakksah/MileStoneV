using System;

namespace VideoOS.Platform.Messaging;

public class IsDirectInputHIDHandledByPlugInRequestData
{
	public Guid InstanceId { get; set; }

	public string InstanceName { get; set; }

	public Guid ProductGuid { get; set; }

	public int VendorId { get; set; }

	public int ProductId { get; set; }
}
