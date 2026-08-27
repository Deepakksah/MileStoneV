-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM [DevicePrivacyMask] WHERE [PrivacyMask].exist('/PrivacyMask') = 1)
BEGIN
	UPDATE [DevicePrivacyMask]
	SET [PrivacyMask] = [PrivacyMask].query('	element mask
												{
													attribute enabled { data((/PrivacyMask/@enabled)[1]) },

													element methods
													{
														element solid
														{
															attribute id { "1" },
															attribute motionDetection { "false" }
														}
													},

													element grid
													{
														attribute size
														{
															let $n := xs:integer((/PrivacyMask/Cells/@noofXCells)[1])
															let $s := xs:string(max(($n, 8)))
															return concat($s, "x", $s)
														},

														element cells
														{
															for $cell in /PrivacyMask/Cells/Cell
																return element cell
																{
																	attribute x { data($cell/@x) },
																	attribute y { data($cell/@y) },
																	attribute methodId { "1" }
																}
														}
													}
												}
											')
		WHERE [PrivacyMask].exist('/PrivacyMask') = 1
END
GO
