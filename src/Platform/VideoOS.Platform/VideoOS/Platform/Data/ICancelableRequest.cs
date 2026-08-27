namespace VideoOS.Platform.Data;

public interface ICancelableRequest
{
	bool Canceled { get; set; }
}
