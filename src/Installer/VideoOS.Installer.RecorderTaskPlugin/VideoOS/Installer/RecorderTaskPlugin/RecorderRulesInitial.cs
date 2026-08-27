using System;
using VideoOS.Installer.Common.Utilities;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class RecorderRulesInitial : Rules
{
	public override Version ConfigurationFileVersion => new Version("0.0");

	public RecorderRulesInitial()
	{
		RegisterNodeToOverwrite("/recorderconfig/webserver/allowConnectIfPrivacyMaskNotSupported");
		RegisterNodeToOverwrite("/recorderconfig/pipeline/gopframecountlimit");
		RegisterNodeToOverwrite("/recorderconfig/pipeline/gopbytecountlimit");
		RegisterNodeToOverwrite("/recorderconfig/pipeline/goptimespanlimit");
	}
}
