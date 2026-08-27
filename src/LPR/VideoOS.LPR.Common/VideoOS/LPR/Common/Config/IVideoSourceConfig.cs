using System;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.Config;

public interface IVideoSourceConfig
{
	bool Enabled { get; set; }

	FQID FQID { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	FQID CameraFQID { get; set; }

	Guid[] LicensePlateLists { get; set; }

	string[] CountryModules { get; set; }

	IRecognitionSettings RecognitionSettings { get; }

	IStoppingCriterion StoppingCriterion { get; }
}
