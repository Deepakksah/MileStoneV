-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Migrate contents in the 'Addresses' column from semicolon separated URLs
-- into an XML fragment with 'url' and 'type' properties.

DECLARE @old_urls NVARCHAR(MAX)
DECLARE @new_urls NVARCHAR(MAX)
DECLARE @current_url NVARCHAR(MAX)
DECLARE @current_urls NVARCHAR(MAX)
DECLARE @semicolon_pos INT
DECLARE registered_servers_cursor CURSOR FOR 
  SELECT [Addresses] FROM [dbo].[RegisteredServers] FOR UPDATE OF [Addresses]

OPEN registered_servers_cursor

FETCH NEXT FROM registered_servers_cursor
INTO @old_urls

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @current_urls = @old_urls
	SET @new_urls = ''

	--get first old url
	SET @semicolon_pos = PATINDEX('%;%', @current_urls)
	IF @semicolon_pos > 0
	BEGIN
		SET @current_url = SUBSTRING(@current_urls, 1, @semicolon_pos - 1)
		SET @current_urls = SUBSTRING(@current_urls, @semicolon_pos + 1, LEN(@current_urls))
	END
	ELSE
	BEGIN
		SET @current_url = @current_urls
		SET @current_urls = ''
	END

	WHILE LEN(@current_url) > 0
	BEGIN
		SET @new_urls = @new_urls + '<Address url="' + @current_url + '" type="Internal" />'
		
		--get next old url
		SET @semicolon_pos = PATINDEX('%;%', @current_urls)
		IF @semicolon_pos > 0
		BEGIN
			SET @current_url = SUBSTRING(@current_urls, 1, @semicolon_pos - 1)
			SET @current_urls = SUBSTRING(@current_urls, @semicolon_pos + 1, LEN(@current_urls))
		END
		ELSE
		BEGIN
			SET @current_url = @current_urls
			SET @current_urls = ''
		END
	END

	SET @new_urls = '<Addresses>' + @new_urls + '</Addresses>'
	
	UPDATE [dbo].[RegisteredServers] SET [Addresses] = @new_urls
	WHERE CURRENT OF registered_servers_cursor
	
	FETCH NEXT FROM registered_servers_cursor
	INTO @old_urls
END

CLOSE registered_servers_cursor
DEALLOCATE registered_servers_cursor
GO
