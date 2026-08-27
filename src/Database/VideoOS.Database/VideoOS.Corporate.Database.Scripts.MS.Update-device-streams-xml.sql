-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE  d
SET		d.[Streams] = d.[Streams].query('
			element streams
			{
				let $primaryTrackId := "16CE3AA1-5F93-458A-ABE5-5C95D9ED1372"
				for $stream in streams/*
				return element stream
				{
					for $property in $stream/*
					return
						if (local-name($property)!="plugins" and local-name($property)!="record" ) then
							$property
						else
							(),

						element trackdefinition
						{
							if ($stream/record="True") then
								$primaryTrackId
							else
								""
						}
				}
			} ')
FROM	[Devices] d
WHERE   [Streams].exist('streams/stream/record') = 1
GO
