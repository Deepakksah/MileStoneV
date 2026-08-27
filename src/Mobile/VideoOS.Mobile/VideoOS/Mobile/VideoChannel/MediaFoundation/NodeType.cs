namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

public enum NodeType : uint
{
	Output = 0u,
	SourceStream = 1u,
	Transform = 2u,
	Tee = 3u,
	Max = uint.MaxValue
}
