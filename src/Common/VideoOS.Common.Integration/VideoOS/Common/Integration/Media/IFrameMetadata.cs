using System;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Media;

public interface IFrameMetadata : IDisposable
{
	Guid Id { get; }

	ReadOnlySharedBuffer UnmanagedData { get; }

	bool IsInSequence { get; }
}
