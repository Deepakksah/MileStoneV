namespace VideoOS.Mobile.VideoSource;

public interface IRsStream<TData>
{
	OnStreamData<TData> OnData { get; set; }

	OnEventData OnEvent { get; set; }

	OnFailCallback OnFail { get; set; }

	bool IsPaused { get; set; }
}
