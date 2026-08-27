-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID ( '[UpdateEventTypeWithStateGroup]', 'P') IS NOT NULL
	DROP PROCEDURE [UpdateEventTypeWithStateGroup]
GO

CREATE PROCEDURE [UpdateEventTypeWithStateGroup]
	@EventId NVARCHAR(36),
	@StateGroupId NVARCHAR(36),
	@State NVARCHAR(64)
AS
BEGIN

	DECLARE @StateInfo xml = '<StateGroupId>'+@StateGroupId+'</StateGroupId><State>'+@State+'</State>'; 

	UPDATE [EventTypes]
	SET [Definition].modify('insert sql:variable("@StateInfo") as last into (/EventType[1]) ') WHERE IDEventType = @EventId

END
GO

BEGIN TRAN

-- Modify existing EventTypes and insert StateGroupId and State

	--                                   IDEventType                            StateGroupId                            State
	EXEC [UpdateEventTypeWithStateGroup] '247fef11-15db-4c99-99db-de5475eae443','C0279551-7CD7-40C5-AA62-C6350A9C4430','No Overflow'
	EXEC [UpdateEventTypeWithStateGroup] '536ad730-05ae-42cd-b14c-07b07c293e79','C0279551-7CD7-40C5-AA62-C6350A9C4430','Overflow'

	EXEC [UpdateEventTypeWithStateGroup] '2eeb582a-1506-4d73-96c9-ad356acf85fb','5EEDDBBA-7066-4AAA-92E5-6CB9FAE1FC85','Started'
	EXEC [UpdateEventTypeWithStateGroup] 'eed47283-694f-4683-9e4d-18e7d93cf796','5EEDDBBA-7066-4AAA-92E5-6CB9FAE1FC85','Idle'

	EXEC [UpdateEventTypeWithStateGroup] '35742498-bcc5-4f0a-9800-827c9388d1cd','2F965115-6F57-4004-AC0B-4246AEE5200A','Not Active'
	EXEC [UpdateEventTypeWithStateGroup] '7a78f5bb-d8c3-4997-89b7-cae72713b7db','2F965115-6F57-4004-AC0B-4246AEE5200A','Active'

	EXEC [UpdateEventTypeWithStateGroup] '4577f552-765a-438c-bc7d-e5ff1f754bc3','15F76BAD-F660-448F-9E1C-0539D3F54F71','Recording'
	EXEC [UpdateEventTypeWithStateGroup] '79a94f89-92de-4fca-8a43-5561d407423d','15F76BAD-F660-448F-9E1C-0539D3F54F71','Not Recording'

	EXEC [UpdateEventTypeWithStateGroup] '66bdd008-b856-44b6-b385-b1f6c3f76f1b','137E6804-F29D-418F-B9BF-1B7733B2DB95','No'
	EXEC [UpdateEventTypeWithStateGroup] 'eeda47ff-4f3d-459e-8143-69896c7c74ad','137E6804-F29D-418F-B9BF-1B7733B2DB95','Yes'

	EXEC [UpdateEventTypeWithStateGroup] 'a9c1f6fa-add6-4bf4-ba2d-a0ceddb73c51','2A91B8A3-0BEE-4C4E-BC6D-49EAB6776738','Recording'
	EXEC [UpdateEventTypeWithStateGroup] 'd740951a-984d-4049-99b5-25f5cbabd21f','2A91B8A3-0BEE-4C4E-BC6D-49EAB6776738','Not Recording'

	EXEC [UpdateEventTypeWithStateGroup] '836ca458-a833-4742-8ee0-64b2380984bd','FA20664C-C35D-4346-A87D-87E9D7511194','Active'
	EXEC [UpdateEventTypeWithStateGroup] '8666e3dc-57a7-4f38-9611-51d774ee7358','FA20664C-C35D-4346-A87D-87E9D7511194','Not Active'

	EXEC [UpdateEventTypeWithStateGroup] '0553c396-5e16-4c22-b3d1-f548e42dfbb4','435FAFBE-3A5B-4A90-A02D-69008740B0C7','Started'
	EXEC [UpdateEventTypeWithStateGroup] '63ea1f06-5a83-4f39-9fab-49959fde7b66','435FAFBE-3A5B-4A90-A02D-69008740B0C7','Stopped'
	EXEC [UpdateEventTypeWithStateGroup] '6BAAD64B-C395-4F52-B6A3-DD1B64AA2F0F','435FAFBE-3A5B-4A90-A02D-69008740B0C7','Error'

	EXEC [UpdateEventTypeWithStateGroup] 'c7b9227a-8e11-451b-bc9c-8ea659731012','54FD5E27-FA54-4D5D-89D3-E4C288B0C7F2','No'
	EXEC [UpdateEventTypeWithStateGroup] 'f951b1f0-2fed-48f7-88d3-49eb5999c923','54FD5E27-FA54-4D5D-89D3-E4C288B0C7F2','Yes'

	EXEC [UpdateEventTypeWithStateGroup] '0EE90664-2924-42A0-A816-4129D0ECABDC','53B40C77-CF48-42A2-A432-DB65AE3AFC7A','Stopped'
	EXEC [UpdateEventTypeWithStateGroup] 'A334AF1C-4B4B-4957-9E5F-AB8CA07FEAB6','53B40C77-CF48-42A2-A432-DB65AE3AFC7A','Error'
	EXEC [UpdateEventTypeWithStateGroup] 'DD3E6464-7DC0-405a-A92F-6150587563E8','53B40C77-CF48-42A2-A432-DB65AE3AFC7A','Started'

	EXEC [UpdateEventTypeWithStateGroup] '6EF169EC-4B56-48E4-A8AA-65A07D0C35DC','C1EB0D2B-1903-45F6-B0C8-3F4CD1879B2A','Normal'
	EXEC [UpdateEventTypeWithStateGroup] '2FE3A39D-7ACF-436C-9552-67FDF9978A69','C1EB0D2B-1903-45F6-B0C8-3F4CD1879B2A','Critical'

	EXEC [UpdateEventTypeWithStateGroup] '0C5B3A2D-F60F-4CF0-A99C-1D62DCAEE5B4','FEB52F64-AD52-41F4-AC13-16576460DB7D','Warning'
	EXEC [UpdateEventTypeWithStateGroup] '2457EF7A-5742-4550-9948-AD5CC51D0645','FEB52F64-AD52-41F4-AC13-16576460DB7D','Normal'
	EXEC [UpdateEventTypeWithStateGroup] '363741CF-4D7A-426D-9FE2-A201C4DB51C0','FEB52F64-AD52-41F4-AC13-16576460DB7D','Critical'

	EXEC [UpdateEventTypeWithStateGroup] '48248FDA-AE5B-40BA-99EA-A02B7F39235B','8710ED5D-1FB8-4551-81FB-49B269267D5D','Warning'
	EXEC [UpdateEventTypeWithStateGroup] 'CB218E23-934F-4E65-A0A6-11F1822C0F99','8710ED5D-1FB8-4551-81FB-49B269267D5D','Normal'
	EXEC [UpdateEventTypeWithStateGroup] 'CBE4E664-8AD6-404F-9EA3-63633EBB1273','8710ED5D-1FB8-4551-81FB-49B269267D5D','Critical'

	EXEC [UpdateEventTypeWithStateGroup] '7A60A969-E2B9-4F47-84E7-A2F6D07C56FD','60217956-BEA0-4C7D-ADD5-6485C2176FFD','Critical'
	EXEC [UpdateEventTypeWithStateGroup] '839754E6-82AF-44FC-9E2F-437413D602D6','60217956-BEA0-4C7D-ADD5-6485C2176FFD','Normal'
	EXEC [UpdateEventTypeWithStateGroup] 'C93E7128-BD54-43AA-9FB2-971E7926D14B','60217956-BEA0-4C7D-ADD5-6485C2176FFD','Warning'

	EXEC [UpdateEventTypeWithStateGroup] '1B36A57E-B656-402E-81E2-E4B873201F06','8C958201-AA02-40CF-A643-FA7F91D0BED2','Critical'
	EXEC [UpdateEventTypeWithStateGroup] '727A8D92-75FD-424F-A2D7-EDDBBBC84305','8C958201-AA02-40CF-A643-FA7F91D0BED2','Normal'
	EXEC [UpdateEventTypeWithStateGroup] '8E98B3E2-D0EA-4608-AD4D-66FCBB422A62','8C958201-AA02-40CF-A643-FA7F91D0BED2','Warning'

	EXEC [UpdateEventTypeWithStateGroup] '6EB95DD6-7CCC-4BCE-99F8-AF0D0B582D77','720EF62A-78BE-418A-8E4C-EE04FFCA6429','Motion'
	EXEC [UpdateEventTypeWithStateGroup] '6F55A7A7-D21C-4629-AC18-AF1975E395A2','720EF62A-78BE-418A-8E4C-EE04FFCA6429','No Motion'

COMMIT TRAN
GO