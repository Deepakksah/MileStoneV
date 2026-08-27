using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IBookmarkReference
{
	string Reference { get; set; }

	DateTime Time { get; set; }
}
