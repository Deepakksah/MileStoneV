namespace VideoOS.Platform.SDK.Interface;

public interface IAlertsGetAroundWithSpanCompletedEventArgs : IAsyncCompletedEventArgs
{
	IAlertOccurrence[] Result { get; }
}
