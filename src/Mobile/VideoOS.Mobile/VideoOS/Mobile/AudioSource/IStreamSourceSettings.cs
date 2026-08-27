namespace VideoOS.Mobile.AudioSource;

public interface IStreamSourceSettings
{
	AudioCorrection AudioCorrection { get; }

	int AudioCompressionLevel { get; }
}
