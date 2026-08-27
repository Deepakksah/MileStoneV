using VideoOS.Common.Integration;

namespace VideoOS.Server.DataComponents;

public interface ISqlLogFactory
{
	ISqlLogProcessor CreateSqlLogProcessor();
}
