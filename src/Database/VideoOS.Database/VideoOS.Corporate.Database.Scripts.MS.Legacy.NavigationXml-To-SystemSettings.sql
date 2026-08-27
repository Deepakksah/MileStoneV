-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * from SystemSettings where IDSystemSetting = '5B868950-9A55-4E0E-9F95-710CC42D184C')
BEGIN
	INSERT INTO SystemSettings ([LastModified], [IDSystemSetting], [Name], [Setting])
	VALUES(getdate(), '5B868950-9A55-4E0E-9F95-710CC42D184C', 'NavigationXML',
	'<main>
		<entry ID=''Root''>
			<text>{PRODUCT NAME}</text>
			<description>{PRODUCT NAME}</description>
			<iconcollapsed>0</iconcollapsed>
			<iconexpanded>0</iconexpanded>
			<data>VideoOS.html</data>
			<launch>
				<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
			</launch>
			<entry ID=''Basics''>
				<text>Basics</text>
				<description>Basics</description>
				<iconcollapsed>1</iconcollapsed>
				<iconexpanded>1</iconexpanded>
				<data>VideoOS.html</data>
				<launch>
					<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
					<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
					<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
				</launch>
				<entry ID=''GettingStarted''>
					<text>Getting Started</text>
					<description>How to get started</description>
					<iconcollapsed>2</iconcollapsed>
					<iconexpanded>2</iconexpanded>
					<data>GetStarted.html</data>
					<launch>
						<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
					</launch>
				</entry>
				<entry ID=''LicenseInformation''>
					<text>License Information</text>
					<description>License Information</description>
					<iconcollapsed>3</iconcollapsed>
					<iconexpanded>3</iconexpanded>
					<data/>
					<launch>
						<instance>41D2E5FD-76B3-4AD8-A0B4-7AF821F5E5A3</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlLicensingAddIn</classname>
					</launch>
				</entry>
			</entry>
			<entry ID=''Servers''>
				<text>Servers</text>
				<description>Servers</description>
				<iconcollapsed>72</iconcollapsed>
				<iconexpanded>72</iconexpanded>
				<data>VideoOS.html</data>
				<launch>
					<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
					<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
					<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
				</launch>
				<entry ID=''Recorders''>
					<text>Recording Servers</text>
					<description>Recording Servers</description>
					<iconcollapsed>4</iconcollapsed>
					<iconexpanded>4</iconexpanded>
					<data/>
					<launch>
						<instance>C14D292E-D76D-41a1-9828-E745E0756E2A</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlRecorder</classname>
					</launch>
				</entry>
				<entry ID=''FailoverServers''>
					<text>Failover Servers</text>
					<description>Failover Servers</description>
					<iconcollapsed>65</iconcollapsed>
					<iconexpanded>65</iconexpanded>
					<data>VideoOS.html</data>
					<launch>
						<instance>DA5BE5BB-D96A-4287-AB00-291261810407</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlFailover</classname>
					</launch>
				</entry>
			</entry>
			<entry ID=''Devices''>
				<text>Devices</text>
				<description>Devices</description>
				<iconcollapsed>6</iconcollapsed>
				<iconexpanded>6</iconexpanded>
				<data>VideoOS.html</data>
				<launch>
					<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
					<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
					<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
				</launch>
				<entry ID=''Cameras''>
					<text>Cameras</text>
					<description>Cameras</description>
					<iconcollapsed>7</iconcollapsed>
					<iconexpanded>7</iconexpanded>
					<data>Camera</data>
					<launch>
						<instance>32DEB0F4-DB44-47d4-932F-BFF9B20E6544</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlDevices</classname>
					</launch>
				</entry>
				<entry ID=''AudioInput''>
					<text>Microphones</text>
					<description>Microphones</description>
					<iconcollapsed>63</iconcollapsed>
					<iconexpanded>63</iconexpanded>
					<data>AudioInput</data>
					<launch>
						<instance>B755E9D2-2BC7-439c-A6C3-08A3C2B336CF</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlDevices</classname>
					</launch>
				</entry>
				<entry ID=''AudioOutput''>
					<text>Speakers</text>
					<description>Speakers</description>
					<iconcollapsed>64</iconcollapsed>
					<iconexpanded>64</iconexpanded>
					<data>AudioOutput</data>
					<launch>
						<instance>7892F7BD-9D96-4133-A38A-899AADCC5C96</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlDevices</classname>
					</launch>
				</entry>
				<entry ID=''Inputs''>
					<text>Inputs</text>
					<description>Inputs</description>
					<iconcollapsed>8</iconcollapsed>
					<iconexpanded>8</iconexpanded>
					<data>Input</data>
					<launch>
						<instance>CC30CCF0-883A-4432-B543-43E0EBEE1629</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlDevices</classname>
					</launch>
				</entry>
				<entry ID=''Outputs''>
					<text>Outputs</text>
					<description>Outputs</description>
					<iconcollapsed>9</iconcollapsed>
					<iconexpanded>9</iconexpanded>
					<data>Output</data>
					<launch>
						<instance>436D3371-3C64-4fa3-BB7B-D4DF4DAE3FAE</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlDevices</classname>
					</launch>
				</entry>
			</entry>
			<entry ID=''Client''>
				<text>Client</text>
				<description>Client</description>
				<iconcollapsed>75</iconcollapsed>
				<iconexpanded>75</iconexpanded>
				<data>VideoOS.html</data>
				<launch>
					<instance>0FC52433-0D61-4DED-A867-22EB989234F1</instance>
					<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
					<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
				</launch>
				<entry ID=''ViewGroups''>
					<text>View Groups</text>
					<description>View Groups</description>
					<iconcollapsed>73</iconcollapsed>
					<iconexpanded>73</iconexpanded>
					<data>VideoOS.html</data>
					<launch>
						<instance>C1EE1C00-8775-4B0B-8990-9D1E3755DA69</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlViewGroup</classname>
					</launch>
				</entry>
					  <entry ID=''Matrix''>
					<text>Matrix</text>
					<description>Matrix</description>
					<iconcollapsed>74</iconcollapsed>
					<iconexpanded>74</iconexpanded>
					<data>VideoOS.html</data>
					<launch>
						<instance>7C4EC322-555F-4558-8AC9-9482F2D06908</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlMatrix</classname>
					</launch>
				</entry>
			</entry>
			<entry ID=''RulesAlerts''>
				<text>Rules and Alerts</text>
				<description>Rules and Alerts</description>
				<iconcollapsed>10</iconcollapsed>
				<iconexpanded>10</iconexpanded>
				<data>VideoOS.html</data>
				<launch>
					<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
					<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
					<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
				</launch>
				<entry ID=''Rules''>
					<text>Rules</text>
					<description>Rules</description>
					<iconcollapsed>11</iconcollapsed>
					<iconexpanded>11</iconexpanded>
					<data/>
					<launch>
						<instance>F8904201-C7A5-41c5-8F57-BD0235FA90BD</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlRules</classname>
					</launch>
				</entry>
				<entry ID=''TimeProfiles''>
					<text>Time Profiles</text>
					<description>Time Profiles</description>
					<iconcollapsed>12</iconcollapsed>
					<iconexpanded>12</iconexpanded>
					<data/>
					<launch>
						<instance>E87D4A0A-D4C9-44d4-9DD8-5D732A39F2E5</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlTimeProfiles</classname>
					</launch>
				</entry>
				<entry ID=''NotificationProfiles''>
					<text>Notification Profiles</text>
					<description>Notification Profiles</description>
					<iconcollapsed>13</iconcollapsed>
					<iconexpanded>13</iconexpanded>
					<data/>
					<launch>
						<instance>31AEECB5-9C4E-40e8-95DA-8AF0BB0533DF</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlNotification</classname>
					</launch>
				</entry>
				<entry ID=''Alerts''>
					<text>Alerts</text>
					<description>Alerts</description>
					<iconcollapsed>14</iconcollapsed>
					<iconexpanded>14</iconexpanded>
					<data/>
					<launch>
						<instance>86EF6C49-DC11-404b-A67E-F6A2BF645353</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlAlarms</classname>
					</launch>
				</entry>
				<entry ID=''Events''>
					<text>Events</text>
					<description>Events</description>
					<iconcollapsed>56</iconcollapsed>
					<iconexpanded>56</iconexpanded>
					<data/>
					<launch>
						<instance>7D948DD0-56E9-4353-BD06-02CEAF11081D</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlUserDefinedEvents</classname>
					</launch>
				</entry>
			</entry>
			<entry ID=''Security''>
				<text>Security</text>
				<description>Security</description>
				<iconcollapsed>15</iconcollapsed>
				<iconexpanded>15</iconexpanded>
				<data>VideoOS.html</data>
				<launch>
					<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
					<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
					<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
				</launch>
				<!--
				<entry ID=''UsersGroups''>
					<text>User and Groups</text>
					<description>Users and Groups</description>
					<iconcollapsed>16</iconcollapsed>
					<iconexpanded>16</iconexpanded>
					<data>UsersGroups</data>
					<launch>
						<instance>84C2D590-4A3F-44d9-B307-B6BDBBBA2B72</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlUsersGroups</classname>
					</launch>
				</entry>
						-->
				<entry ID=''Roles''>
					<text>Roles</text>
					<description>Roles</description>
					<iconcollapsed>18</iconcollapsed>
					<iconexpanded>18</iconexpanded>
					<data>Roles</data>
					<launch>
						<instance>E9042A11-77B3-4bbf-9C6B-4DDFBD262E05</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlRoles</classname>
					</launch>
				</entry>
			</entry>
			<entry ID=''ServerLogs''>
				<text>Server Logs</text>
				<description>Server Logs</description>
				<iconcollapsed>19</iconcollapsed>
				<iconexpanded>19</iconexpanded>
				<data>VideoOS.html</data>
				<launch>
					<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
					<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
					<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
				</launch>
				<entry ID=''SystemLog''>
					<text>System Log</text>
					<description>System Log</description>
					<iconcollapsed>20</iconcollapsed>
					<iconexpanded>20</iconexpanded>
					<data>SYSTEM</data>
					<launch>
						<instance>32EB3730-ED99-4360-9F1D-FF19F50DEDCD</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlSystemLog</classname>
					</launch>
				</entry>
				<entry ID=''AuditLog''>
					<text>Audit Log</text>
					<description>Audit Log</description>
					<iconcollapsed>20</iconcollapsed>
					<iconexpanded>20</iconexpanded>
					<data>AUDIT</data>
					<launch>
						<instance>C142CCC6-666C-48d8-A8B5-02D48E5DFBA7</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlAuditLog</classname>
					</launch>
				</entry>
				<entry ID=''AlertLog''>
					<text>Alert Log</text>
					<description>Alert Log</description>
					<iconcollapsed>20</iconcollapsed>
					<iconexpanded>20</iconexpanded>
					<data>ALERT</data>
					<launch>
						<instance>5C4E39F2-4CD6-44fe-A051-D1BC27EFD798</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlAlertLog</classname>
					</launch>
				</entry>
				<entry ID=''EventLog''>
					<text>Event Log</text>
					<description>Event Log</description>
					<iconcollapsed>20</iconcollapsed>
					<iconexpanded>20</iconexpanded>
					<data>EVENT</data>
					<launch>
						<instance>20EE69E2-B565-4c20-9C0E-69C72611027C</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlEventLog</classname>
					</launch>
				</entry>
				<entry ID=''RuleLog''>
					<text>Rule Log</text>
					<description>Rule Log</description>
					<iconcollapsed>20</iconcollapsed>
					<iconexpanded>20</iconexpanded>
					<data>RULE</data>
					<launch>
						<instance>A0C81A3A-2895-4b0f-BF3F-5EC42C2C7B30</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlRuleLog</classname>
					</launch>
				</entry>
			</entry>
		</entry>
	</main>')
END
GO
