-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_site_transaction]') AND type = 'P')
DROP PROCEDURE [delete_site_transaction]
GO

CREATE PROCEDURE [delete_site_transaction]
	@IDSite GuidType
AS
BEGIN
	SET NOCOUNT ON
	
	 -- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END
	
	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()
	
	DECLARE @SiteType INT
	SET @SiteType = (SELECT [IDSiteType]
					 FROM Sites 
					 WHERE [IDSite] = @IDSite)
	
	-- Trying to delete homesite?
	IF(@SiteType = 1)
	BEGIN
	    -- Raise error - home site can't be deleted!
		;THROW 60405, @procedureName, 1;
	END
	ELSE IF(@SiteType = 2) -- Deleting master site?
	BEGIN
	    -- Delete relations to master site
		DELETE FROM SitesRelation
		WHERE IDSiteMaster = @IDSite
		
		-- Delete master site
		DELETE FROM Sites
		WHERE IDSite = @IDSite

		-- Unregister the security object
		EXEC [security_unregister_object_transaction]
			@namespace_name = 'Site',
			@object_id = @IDSite,
			@CurrentUtcTime = @CurrentUtcTime
	END
	ELSE IF(@SiteType = 3) -- Deleting slave site?
	BEGIN
	     -- Find the site and its slaves and slaves slaves etc.
		 -- in recursive function
		 DECLARE @SitesToDelete TABLE([IDSite] GuidType)
		;WITH SitesCTE(IDSite) AS
		(
			SELECT @IDSite
		
			UNION ALL
		
			SELECT	sr.[IDSiteSlave]
			FROM SitesCTE AS P
			INNER JOIN SitesRelation sr ON sr.IDSiteMaster = P.IDSite
		)
		INSERT INTO @SitesToDelete
		SELECT DISTINCT [IDSite] FROM SitesCTE
		
		-- Delete all relation where these sites are slaves
		DELETE FROM SitesRelation
		WHERE IDSiteSlave IN ( SELECT [IDSite] FROM @SitesToDelete )
		
		-- Delete all slaves site that doesn't have a slave relation
		DELETE FROM Sites
		WHERE IDSite IN ( SELECT [IDSite] FROM @SitesToDelete )

		DECLARE @DeletedSiteId GuidType
		
		DECLARE security_cursor CURSOR
		FOR SELECT [IDSite] FROM @SitesToDelete

		OPEN security_cursor
		BEGIN TRY
			FETCH NEXT FROM security_cursor INTO @DeletedSiteId
			WHILE @@FETCH_STATUS = 0
			BEGIN
				BEGIN TRY
					-- Unregister the security object
					EXEC [security_unregister_object_transaction]
						@namespace_name = 'Site',
						@object_id = @DeletedSiteId, 
						@CurrentUtcTime = @CurrentUtcTime
				END TRY
				BEGIN CATCH
					CLOSE security_cursor
					DEALLOCATE security_cursor
					;THROW;
				END CATCH
		
				FETCH NEXT FROM security_cursor INTO @DeletedSiteId
			END
		END TRY
		BEGIN CATCH
			CLOSE security_cursor
			DEALLOCATE security_cursor
			;THROW
		END CATCH

		CLOSE security_cursor
		DEALLOCATE security_cursor
	END
END
GO