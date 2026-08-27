namespace VideoOS.Mobile.VideoSource;

public interface IRsStreamSource<TData>
{
	void SetSource(IRsStream<TData> source);
}
