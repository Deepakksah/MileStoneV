using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IBookmark
{
	Guid Id { get; set; }

	Guid DeviceId { get; set; }

	string Reference { get; set; }

	string Header { get; set; }

	string Description { get; set; }

	string User { get; set; }

	DateTime TimeBegin { get; set; }

	DateTime TimeTrigged { get; set; }

	DateTime TimeEnd { get; set; }
}
