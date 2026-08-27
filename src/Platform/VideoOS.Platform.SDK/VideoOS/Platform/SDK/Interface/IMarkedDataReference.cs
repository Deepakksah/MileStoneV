using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IMarkedDataReference
{
	string Reference { get; set; }

	DateTime Time { get; set; }
}
