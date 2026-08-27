-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SecurityPermissionsDevice]') AND name = N'IX_SecurityPermissionsDevice_IDRole')
DROP INDEX [IX_SecurityPermissionsDevice_IDRole] ON [dbo].[SecurityPermissionsDevice] WITH ( ONLINE = OFF )
GO

CREATE NONCLUSTERED INDEX [IX_SecurityPermissionsDevice_IDRole] ON [SecurityPermissionsDevice] 
(
	[IDRole] ASC
)
INCLUDE ( [SecurityAttributeID], [IDDevice]) ON [PRIMARY]
GO

