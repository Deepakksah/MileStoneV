IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'LogServer')
	EXECUTE('CREATE SCHEMA[LogServer]')
GO