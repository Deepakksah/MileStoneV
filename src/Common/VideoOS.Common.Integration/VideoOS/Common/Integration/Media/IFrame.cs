using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Media;

public interface IFrame : IFrameSimple
{
	IReadOnlyBuffer FrameDataView { get; }

	ReadOnlySharedBuffer FrameBuffer { get; }

	void SetMetadata(IFrameMetadata metadata);

	IFrameMetadata[] GetMetadataCollection();
}
