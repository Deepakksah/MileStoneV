
-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @clientVersionXml XML

SELECT @clientVersionXml = CAST([Setting] AS XML)
FROM   [SystemSettings]
WHERE  [Name] = 'ClientVersion'

SET @clientVersionXml = @clientVersionXml.query('
    element versions
    {
        element smartclient
        {
            /versions/smartclient/@version,

            element version
            {
                attribute major { 25 },
                attribute minor { 2 },
                attribute revision { "a" },
                /versions/smartclient/version/@beta,
                /versions/smartclient/version/@forceupgrade,
                attribute displayname { "25.2a" }
            },

            element upgradeinfo
            {
                element url
                {
                    element hostname { "[SubstUrlHostname]" },
                    element port { "[SubstUrlPort]" },
                    element path { "[SubstUrlPath]" }
                },

                element message { }
            }
        },

        /versions/upgrade
    }')

UPDATE [SystemSettings]
SET    [Setting] = CAST(@clientVersionXml AS NVARCHAR(MAX))
WHERE  [Name] = 'ClientVersion'

GO
