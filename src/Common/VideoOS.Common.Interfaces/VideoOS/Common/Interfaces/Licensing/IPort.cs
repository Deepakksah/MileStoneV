namespace VideoOS.Common.Interfaces.Licensing;

public interface IPort
{
	string Name { get; }

	int Count { get; set; }
}
