-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Reorder 'Microphone' action ordering
UPDATE [SecurityAction]
SET [order_no] = [order_no] + 1
WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	AND [order_no] >= 4

-- Add new 'Retrieve remote recordings' to 'Microphone' security namespace
INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
VALUES ('15F48F88-CA89-4926-9A84-2B02864EC77A', 'READ_REMOTE_RECORDINGS', 'Retrieve remote recordings', 4, 1)

-- Reorder 'Speaker' action ordering
UPDATE [SecurityAction]
SET [order_no] = [order_no] + 1
WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	AND [order_no] >= 5

-- Add new 'Retrieve remote recordings' to 'Speaker' security namespace
INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
VALUES ('48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'READ_REMOTE_RECORDINGS', 'Retrieve remote recordings', 5, 1)

-- Reorder 'Metadata' playback action ordering
UPDATE [SecurityAction]
SET [order_no] = 3
WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [name] = 'PLAYBACK'

-- Add new 'Retrieve remote recordings' to 'Metadata' security namespace
INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
VALUES ('EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'READ_REMOTE_RECORDINGS', 'Retrieve remote recordings', 4, 1)

-- Reorder all numbering on 'Metadata' actions
;WITH new_ordering
AS
(
	SELECT [namespace_id], [name], [display_name], ROW_NUMBER() OVER(ORDER BY [order_no]) - 1 as row_no
	FROM [SecurityAction]
	WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
)
UPDATE sa
SET sa.[order_no] = row_no
FROM [SecurityAction] sa
INNER JOIN [new_ordering] new ON sa.[namespace_id] = new.namespace_id AND sa.[name] = new.[name]
GO