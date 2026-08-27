using System;

namespace VideoOS.Mobile.Portable.Interfaces;

public interface IThumbnailManager
{
	IThumbnail this[Guid key] { get; }
}
