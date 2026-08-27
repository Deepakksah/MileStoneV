using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public delegate void DatabaseExceptionHandler(DatabaseException ex, QueryExecutionState state);
