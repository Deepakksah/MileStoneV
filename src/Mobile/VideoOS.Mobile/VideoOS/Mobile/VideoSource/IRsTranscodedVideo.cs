namespace VideoOS.Mobile.VideoSource;

public interface IRsTranscodedVideo
{
	void BeginParamsChange();

	void SetWidthHeight(int width, int height);

	void SetCropping(double left, double top, double width, double height);

	void SetQuality(int quality);

	void SetFPS(int fps);

	void SetCustomData(object data);

	void EndParamsChange();
}
