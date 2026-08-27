using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IAlertTypeInfo
{
	Guid AlertTypeId { get; set; }

	string Name { get; set; }

	string Description { get; set; }
}
