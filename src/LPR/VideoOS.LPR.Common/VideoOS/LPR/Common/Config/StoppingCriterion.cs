using System;

namespace VideoOS.LPR.Common.Config;

public class StoppingCriterion : IStoppingCriterion
{
	public float FrameRate { get; set; }

	public bool Interlaced { get; set; }

	public int MaxPlates { get; set; }

	public float StopRecognitionAbove { get; set; }

	public float DiscardRecognitionsBelow { get; set; }

	public int MaxTimePerFrame { get; set; }

	public float PartialRecognitionAvoidance { get; set; }

	public StoppingCriterion()
	{
	}

	public StoppingCriterion(IStoppingCriterion stoppingCriterion)
	{
		ApplyAll(stoppingCriterion);
	}

	public void ApplyAll(IStoppingCriterion stoppingCriterion)
	{
		if (stoppingCriterion == null)
		{
			throw new ArgumentException("The parameter stoppingCriterion should not be null");
		}
		MaxTimePerFrame = stoppingCriterion.MaxTimePerFrame;
		StopRecognitionAbove = stoppingCriterion.StopRecognitionAbove;
		MaxPlates = stoppingCriterion.MaxPlates;
		DiscardRecognitionsBelow = stoppingCriterion.DiscardRecognitionsBelow;
		FrameRate = stoppingCriterion.FrameRate;
		Interlaced = stoppingCriterion.Interlaced;
		PartialRecognitionAvoidance = stoppingCriterion.PartialRecognitionAvoidance;
	}
}
