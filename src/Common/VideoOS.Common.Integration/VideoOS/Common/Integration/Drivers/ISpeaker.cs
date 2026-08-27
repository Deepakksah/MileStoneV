using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Drivers;

public interface ISpeaker
{
	bool PutFrame(IReadOnlyBuffer frame);

	bool EnsureConnected();
}
