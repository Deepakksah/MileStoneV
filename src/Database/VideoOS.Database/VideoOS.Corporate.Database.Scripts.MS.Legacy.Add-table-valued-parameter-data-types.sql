-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.[name] = N'tvp_StringTableVarchar' AND ss.name = N'dbo')
CREATE TYPE tvp_StringTableVarchar AS TABLE ( ID VARCHAR(MAX) )
GO

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.[name] = N'tvp_StringTableNvarchar' AND ss.name = N'dbo')
CREATE TYPE tvp_StringTableNvarchar AS TABLE ( ID NVARCHAR(MAX) )
GO
