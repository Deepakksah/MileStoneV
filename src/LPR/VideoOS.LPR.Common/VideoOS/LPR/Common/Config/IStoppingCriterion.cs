namespace VideoOS.LPR.Common.Config;

public interface IStoppingCriterion
{
	float FrameRate { get; set; }

	bool Interlaced { get; set; }

	int MaxPlates { get; set; }

	float StopRecognitionAbove { get; set; }

	float DiscardRecognitionsBelow { get; set; }

	int MaxTimePerFrame { get; set; }

	float PartialRecognitionAvoidance { get; set; }

	void ApplyAll(IStoppingCriterion stoppingCriterion);
}
