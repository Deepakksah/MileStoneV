namespace VideoOS.Common.Integration.Command;

public interface ICanBulk<T> where T : ICommandBulk
{
	T BulkCommand { get; }
}
