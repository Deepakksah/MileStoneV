using System;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.Config;

public class VideoSourceConfig : IVideoSourceConfig
{
	public bool Enabled { get; set; }

	public FQID FQID { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public FQID CameraFQID { get; set; }

	public Guid[] LicensePlateLists { get; set; }

	public string[] CountryModules { get; set; }

	public IRecognitionSettings RecognitionSettings { get; set; }

	public IStoppingCriterion StoppingCriterion { get; set; }
}
