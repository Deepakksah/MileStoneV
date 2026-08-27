namespace VideoOS.Platform.SDK.Interface;

public interface IGetPresetsCompletedEventArgs : IAsyncCompletedEventArgs
{
	IPresetInfo[] Result { get; }
}
