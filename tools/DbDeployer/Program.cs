using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VideoOS.Corporate.Database;
using VideoOS.SqlUpdate;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Configuration;
using VideoOS.SqlUpdate.Core.Operations;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.SqlUpdate.Core.Scripts;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;

namespace MilestoneDbDeployer
{
    public class SimpleConsoleLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string msg = formatter(state, exception);
            ConsoleColor prevColor = Console.ForegroundColor;
            switch (logLevel)
            {
                case LogLevel.Error:
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.WriteLine(string.Format("[{0:HH:mm:ss}] [{1}] {2}", DateTime.Now, logLevel, msg));
            if (exception != null)
            {
                Console.WriteLine(exception.ToString());
            }
            Console.ForegroundColor = prevColor;
        }
    }

    public class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("==========================================================");
            Console.WriteLine("   Milestone XProtect / LycheeVMS 2025 R3 Database Deployer");
            Console.WriteLine("==========================================================");

            SimpleConsoleLogger logger = new SimpleConsoleLogger();

            string server = "localhost";
            string msDbName = "Surveillance";
            string logDbName = "SurveillanceLogServer";

            string masterConnStr = string.Format("Data Source={0};Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True", server);
            string msConnStr = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;TrustServerCertificate=True", server, msDbName);
            string logConnStr = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;TrustServerCertificate=True", server, logDbName);

            try
            {
                Console.WriteLine("\n[1/4] Testing Local SQL Server Connection...");
                using (SqlConnection conn = new SqlConnection(masterConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT @@VERSION";
                        object val = cmd.ExecuteScalar();
                        string ver = (val != null) ? val.ToString() : "";
                        string[] lines = ver.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine("Connected to SQL Server: " + (lines.Length > 0 ? lines[0] : ver));
                    }
                }

                Console.WriteLine("\n[2/4] Initializing Key Management...");
                DatabaseConnectionInfo dbConnInfo = new DatabaseConnectionInfo(msConnStr);
                SqlConnectionManager sqlConnMgr = new SqlConnectionManager(dbConnInfo, logger);
                
                IEncryptionKeyCreator encryptionKeyCreator = null;
                try
                {
                    encryptionKeyCreator = KeyManagerFactory.GetEncryptionKeyCreator("Milestone", sqlConnMgr, logger);
                }
                catch
                {
                    encryptionKeyCreator = KeyManagerFactory.GetEncryptionKeyCreator("VideoOS", sqlConnMgr, logger);
                }

                Console.WriteLine(string.Format("\n[3/4] Creating & Deploying Management Server Database: [{0}]...", msDbName));
                Console.WriteLine("Applying 1,393 Core Baseline + Upgrade Schemas, Functions, Triggers, Views & IDP/IM Tables...");
                
                bool msResult = Database.CreateOrUpgradeDatabase(
                    msConnStr,
                    msConnStr,
                    msConnStr,
                    logger,
                    "XPCO",
                    encryptionKeyCreator,
                    SqlUpdateAction.CreateOrUpgrade,
                    null,
                    null
                );

                if (!msResult)
                {
                    Console.WriteLine("ERROR: Management Server database deployment failed!");
                    return 1;
                }
                Console.WriteLine(string.Format("SUCCESS: [{0}] database created and upgraded to 2025 R3!", msDbName));

                Console.WriteLine(string.Format("\n[4/4] Creating & Deploying Log Server Database: [{0}]...", logDbName));
                DatabaseConnectionInfo logDbConnInfo = new DatabaseConnectionInfo(logConnStr);
                SqlConnectionManager logSqlConnMgr = new SqlConnectionManager(logDbConnInfo, logger);
                IEncryptionKeyCreator logKeyCreator = KeyManagerFactory.GetRestrictedEncryptionKeyCreator("Milestone", logSqlConnMgr, logger);

                AppConfig logAppConfig = new AppConfig(SqlUpdateAction.CreateOrUpgrade, false, "XPCO", null, null, null, "1.0.0", logConnStr);
                AssemblyWrapper logAssemblyWrapper = new AssemblyWrapper(logger, "VideoOS.Installer.LogServerPlugins");
                
                Assembly logAsm = Assembly.LoadFrom("VideoOS.Installer.LogServerPlugins.dll");
                Type lsScriptsType = logAsm.GetType("VideoOS.Installer.LogServerPlugins.LsScripts");
                object lsScriptsObj = Activator.CreateInstance(lsScriptsType, new object[] { logger });
                
                string embeddedPath = (string)lsScriptsType.GetProperty("EmbeddedPath").GetValue(lsScriptsObj, null);
                IReadOnlyList<ScriptBase> upgradeScripts = (IReadOnlyList<ScriptBase>)lsScriptsType.GetMethod("GetUpgradeScripts").Invoke(lsScriptsObj, null);
                IReadOnlyList<ScriptBase> baselineScripts = (IReadOnlyList<ScriptBase>)lsScriptsType.GetMethod("GetBaselineScripts").Invoke(lsScriptsObj, null);

                Type logVersionProviderType = logAsm.GetType("VideoOS.Installer.LogServerPlugins.LogServerDatabaseVersionProvider");
                IDatabaseVersionProvider logVersionProvider = (IDatabaseVersionProvider)Activator.CreateInstance(logVersionProviderType, new object[] { logger });
                DatabaseSkuProvider logSkuProvider = new DatabaseSkuProvider();

                bool logResult = UpdateRunner.Run(
                    logAppConfig,
                    logKeyCreator,
                    logger,
                    embeddedPath,
                    upgradeScripts,
                    baselineScripts,
                    logAssemblyWrapper,
                    logVersionProvider,
                    logSkuProvider
                );

                if (!logResult)
                {
                    Console.WriteLine("ERROR: Log Server database deployment failed!");
                    return 1;
                }
                Console.WriteLine(string.Format("SUCCESS: [{0}] database created and deployed successfully!", logDbName));

                Console.WriteLine("\n==========================================================");
                Console.WriteLine("   ALL DATABASES CREATED AND CONFIGURED SUCCESSFULLY!");
                Console.WriteLine("==========================================================");

                // Run a quick verification query
                using (SqlConnection conn = new SqlConnection(msConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM sys.tables";
                        int tableCount = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine(string.Format("[Surveillance] Total Tables: {0}", tableCount));

                        cmd.CommandText = "SELECT COUNT(*) FROM sys.views";
                        int viewCount = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine(string.Format("[Surveillance] Total Views: {0}", viewCount));

                        cmd.CommandText = "SELECT COUNT(*) FROM sys.procedures";
                        int procCount = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine(string.Format("[Surveillance] Total Stored Procedures: {0}", procCount));

                        cmd.CommandText = "SELECT TOP 1 VersionNumber, Sku FROM dbo.SystemSettings";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Console.WriteLine(string.Format("[Surveillance] System Version: {0}, SKU: {1}", reader[0], reader[1]));
                            }
                        }
                    }
                }

                using (SqlConnection conn = new SqlConnection(logConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM sys.tables";
                        int tableCount = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine(string.Format("[SurveillanceLogServer] Total Tables: {0}", tableCount));
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("FATAL EXCEPTION: {0}", ex.Message));
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(string.Format("INNER EXCEPTION: {0}", ex.InnerException.Message));
                    Console.WriteLine(ex.InnerException.StackTrace);
                }
                Console.ResetColor();
                return 1;
            }
        }
    }
}
