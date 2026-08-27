namespace VideoOS.Platform.Util.AdaptiveStreaming;

public interface IStreamSelectionParams
{
	StreamSelectionType StreamSelectionType { get; set; }

	void SetStreamAdaptiveResolution(int width, int height);

	void GetStreamAdaptiveResolution(out int width, out int height);
}
