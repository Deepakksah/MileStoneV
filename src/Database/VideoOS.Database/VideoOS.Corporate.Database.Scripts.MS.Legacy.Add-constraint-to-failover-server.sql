-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @invalidFailoverGroup [GuidType]

WHILE EXISTS(
	SELECT DISTINCT IDFailoverGroup
	FROM FailoverServers
	GROUP BY GroupSequence, IDFailoverGroup
	HAVING COUNT(GroupSequence) > 1
)
BEGIN
	SELECT DISTINCT @invalidFailoverGroup = IDFailoverGroup
	FROM FailoverServers
	GROUP BY GroupSequence, IDFailoverGroup
	HAVING COUNT(GroupSequence) > 1

	DECLARE @tmp TABLE (IDFailoverServer [GuidType], GroupSequence SMALLINT)

	INSERT @tmp
	SELECT IDFailoverServer, ROW_NUMBER() OVER(ORDER BY GroupSequence)
	FROM FailoverServers
	WHERE IDFailoverGroup = @invalidFailoverGroup

	UPDATE fs
	SET fs.[GroupSequence] = tmp.[GroupSequence] - 1
	FROM FailoverServers fs
	INNER JOIN @tmp tmp ON tmp.[IDFailoverServer] = fs.[IDFailoverServer]
END
GO

-- Add a constraint to ensure that two groups doesn't have the same parent and same name
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'IX_UniqueSequenceInGroup')
ALTER TABLE [FailoverServers]
ADD CONSTRAINT [IX_UniqueSequenceInGroup] UNIQUE NONCLUSTERED
(
	[IDFailoverGroup] ASC,
	[GroupSequence] ASC
) ON [PRIMARY]
GO
