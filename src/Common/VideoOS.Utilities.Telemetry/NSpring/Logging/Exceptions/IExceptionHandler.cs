namespace NSpring.Logging.Exceptions;

public interface IExceptionHandler
{
	void Handle(LoggerException exception);

	void Open();

	void Close();
}
