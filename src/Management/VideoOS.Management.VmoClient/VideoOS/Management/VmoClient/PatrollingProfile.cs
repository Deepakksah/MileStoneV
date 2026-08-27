using System;
using System.Collections.ObjectModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("PatrollingProfile")]
public sealed class PatrollingProfile : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	private PatrollingStepCollection _patrollingSteps;

	private PatrollingStep _endPosition;

	private double _initialPanSpeed = PatrollingStep.DefaultPanSpeed;

	private double _initialTiltSpeed = PatrollingStep.DefaultTiltSpeed;

	private double _initialZoomSpeed = PatrollingStep.DefaultZoomSpeed;

	private TimeSpan _initialExpectedTransitionTimeToPreset = PatrollingStep.DefaultExpectedTransitionTime;

	private bool _showCustomizations;

	public bool IsPanSpeedAvailable { get; private set; }

	public bool IsTiltSpeedAvailable { get; private set; }

	public bool IsZoomSpeedAvailable { get; private set; }

	public ReadOnlyCollection<PatrollingStep> PatrollingSteps => new ReadOnlyCollection<PatrollingStep>(_patrollingSteps);

	public PatrollingStep EndPosition
	{
		get
		{
			return _endPosition;
		}
		set
		{
			_endPosition = value;
			WriteDefinition();
		}
	}

	public string Name
	{
		get
		{
			return GetPropertyValue<string>("Name");
		}
		set
		{
			SetPropertyValue("Name", value);
		}
	}

	public string Description
	{
		get
		{
			return GetPropertyValue<string>("Description");
		}
		set
		{
			SetPropertyValue("Description", value);
		}
	}

	public double InitialPanSpeed
	{
		get
		{
			return _initialPanSpeed;
		}
		set
		{
			_initialPanSpeed = value;
			WriteDefinition();
		}
	}

	public double InitialTiltSpeed
	{
		get
		{
			return _initialTiltSpeed;
		}
		set
		{
			_initialTiltSpeed = value;
			WriteDefinition();
		}
	}

	public double InitialZoomSpeed
	{
		get
		{
			return _initialZoomSpeed;
		}
		set
		{
			_initialZoomSpeed = value;
			WriteDefinition();
		}
	}

	public TimeSpan InitialExpectedTransitionTimeToPreset
	{
		get
		{
			return _initialExpectedTransitionTimeToPreset;
		}
		set
		{
			_initialExpectedTransitionTimeToPreset = value;
			WriteDefinition();
		}
	}

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public string CameraId => GetPropertyValue<string>("CameraDeviceId");

	public CameraDevice CameraDevice => GetObjectById<CameraDevice>(CameraId);

	public string DefinitionXml
	{
		get
		{
			return GetPropertyValue<string>("DefinitionXml");
		}
		set
		{
			SetPropertyValue("DefinitionXml", value);
		}
	}

	public PatrollingProfile(CameraDevice cameraDevice)
		: this(cameraDevice.Repositories, cameraDevice.Id)
	{
	}

	public PatrollingProfile(IVmoRepositories repositories, Guid cameraDeviceId)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		SetPropertyValue("CameraDeviceId", cameraDeviceId.ToString());
	}

	public PatrollingProfile(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		_patrollingSteps = new PatrollingStepCollection();
		ReadDefinition();
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
		ReadDefinition();
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	public void InsertStepAt(int position, PatrollingStep step)
	{
		if (position < 0 || position > PatrollingSteps.Count)
		{
			throw new ArgumentOutOfRangeException("position");
		}
		if (step == null)
		{
			throw new ArgumentNullException("step");
		}
		_patrollingSteps.Insert(position, step);
		RepositionSteps();
		WriteDefinition();
	}

	public PatrollingStep InsertStepAt(int position, PatrollingPreset preset)
	{
		return InsertStepAt(position, preset, PatrollingStep.DefaultWaitTime);
	}

	public PatrollingStep InsertStepAt(int position, PatrollingPreset preset, TimeSpan waitTime)
	{
		return InsertStepAt(position, preset, waitTime, PatrollingStep.DefaultExpectedTransitionTime);
	}

	public PatrollingStep InsertStepAt(int position, PatrollingPreset preset, TimeSpan waitTime, TimeSpan expectedTransitionTimeToPreset)
	{
		if (position < 0 || position > PatrollingSteps.Count)
		{
			throw new ArgumentOutOfRangeException("position");
		}
		if (preset == null)
		{
			throw new ArgumentNullException("preset");
		}
		PatrollingStep patrollingStep = new PatrollingStep(this, preset, waitTime, expectedTransitionTimeToPreset);
		InsertStepAt(position, patrollingStep);
		return patrollingStep;
	}

	public void InsertStepFirst(PatrollingStep step)
	{
		if (step == null)
		{
			throw new ArgumentNullException("step");
		}
		_patrollingSteps.Insert(0, step);
		RepositionSteps();
		WriteDefinition();
	}

	public PatrollingStep InsertStepFirst(PatrollingPreset preset)
	{
		return InsertStepFirst(preset, PatrollingStep.DefaultWaitTime, PatrollingStep.DefaultExpectedTransitionTime);
	}

	public PatrollingStep InsertStepFirst(PatrollingPreset preset, TimeSpan waitTime)
	{
		return InsertStepFirst(preset, waitTime, PatrollingStep.DefaultExpectedTransitionTime);
	}

	public PatrollingStep InsertStepFirst(PatrollingPreset preset, TimeSpan waitTime, TimeSpan expectedTransitionTimeToPreset)
	{
		if (preset == null)
		{
			throw new ArgumentNullException("preset");
		}
		PatrollingStep patrollingStep = new PatrollingStep(this, preset, waitTime, expectedTransitionTimeToPreset);
		InsertStepFirst(patrollingStep);
		return patrollingStep;
	}

	public void InsertStepLast(PatrollingStep step)
	{
		if (step == null)
		{
			throw new ArgumentNullException("step");
		}
		step.SetPosition(_patrollingSteps.Count);
		_patrollingSteps.Add(step);
		WriteDefinition();
	}

	public PatrollingStep InsertStepLast(PatrollingPreset preset)
	{
		return InsertStepLast(preset, PatrollingStep.DefaultWaitTime, PatrollingStep.DefaultExpectedTransitionTime);
	}

	public PatrollingStep InsertStepLast(PatrollingPreset preset, TimeSpan waitTime)
	{
		return InsertStepFirst(preset, waitTime, PatrollingStep.DefaultExpectedTransitionTime);
	}

	public PatrollingStep InsertStepLast(PatrollingPreset preset, TimeSpan waitTime, TimeSpan expectedTransitionTimeToPreset)
	{
		if (preset == null)
		{
			throw new ArgumentNullException("preset");
		}
		PatrollingStep patrollingStep = new PatrollingStep(this, preset, waitTime, expectedTransitionTimeToPreset);
		InsertStepLast(patrollingStep);
		return patrollingStep;
	}

	public void RemoveStep(PatrollingStep step)
	{
		if (step == null)
		{
			throw new ArgumentNullException("step");
		}
		RemoveStepAt(step.Position);
	}

	public void RemoveStepAt(int position)
	{
		if (position < 0 || position > PatrollingSteps.Count - 1)
		{
			throw new ArgumentOutOfRangeException("position");
		}
		_patrollingSteps.RemoveAt(position);
		RepositionSteps();
		WriteDefinition();
	}

	public PatrollingStep FindFirstStep(PatrollingPreset preset)
	{
		if (preset == null)
		{
			throw new ArgumentNullException("preset");
		}
		throw new NotImplementedException("Probably not needed, use Linq for this");
	}

	public PatrollingStep FindLastStep(PatrollingPreset preset)
	{
		if (preset == null)
		{
			throw new ArgumentNullException("preset");
		}
		throw new NotImplementedException("Probably not needed, use Linq for this");
	}

	public bool HasPreset(PatrollingPreset preset)
	{
		if (preset == null)
		{
			throw new ArgumentNullException("preset");
		}
		throw new NotImplementedException("Probably not needed, use Linq for this");
	}

	public void MoveStepToPosition(int position, PatrollingStep step)
	{
		if (position < 0 || position > PatrollingSteps.Count)
		{
			throw new ArgumentOutOfRangeException("position");
		}
		if (step == null)
		{
			throw new ArgumentNullException("step");
		}
		PatrollingStep item = _patrollingSteps[step.Position];
		_patrollingSteps.RemoveAt(step.Position);
		_patrollingSteps.Insert(position, item);
		RepositionSteps();
		WriteDefinition();
	}

	public void MoveStepUp(PatrollingStep step)
	{
		if (step == null)
		{
			throw new ArgumentNullException("step");
		}
		MoveStepToPosition(step.Position - 1, step);
	}

	public void MoveStepDown(PatrollingStep step)
	{
		if (step == null)
		{
			throw new ArgumentNullException("step");
		}
		MoveStepToPosition(step.Position + 1, step);
	}

	public void MoveStepToFirst(PatrollingStep step)
	{
		if (step == null)
		{
			throw new ArgumentNullException("step");
		}
		MoveStepToPosition(0, step);
	}

	public void MoveStepToLast(PatrollingStep step)
	{
		if (step == null)
		{
			throw new ArgumentNullException("step");
		}
		MoveStepToPosition(_patrollingSteps.Count - 1, step);
	}

	private void RepositionSteps()
	{
		for (int i = 0; i < _patrollingSteps.Count; i++)
		{
			_patrollingSteps[i].SetPosition(i);
		}
	}

	private void ReadDefinition()
	{
		if (string.IsNullOrEmpty(DefinitionXml))
		{
			PatrollingSchema patrollingSchema = new PatrollingSchema(base.Id);
			patrollingSchema.InitialTransition.ExpectedTransitionTime = PatrollingStep.DefaultExpectedTransitionTime;
			patrollingSchema.InitialTransition.PanSpeed = PatrollingStep.DefaultPanSpeed;
			patrollingSchema.InitialTransition.TiltSpeed = PatrollingStep.DefaultTiltSpeed;
			patrollingSchema.InitialTransition.ZoomSpeed = PatrollingStep.DefaultZoomSpeed;
			DefinitionXml = patrollingSchema.ToXml();
		}
		PatrollingSchema patrollingSchema2 = new PatrollingSchema(base.Id);
		patrollingSchema2.LoadXml(DefinitionXml);
		_showCustomizations = patrollingSchema2.ShowCustomizeTransitions;
		IsPanSpeedAvailable = patrollingSchema2.InitialTransition.IsPanSpeedAvailable;
		_initialPanSpeed = patrollingSchema2.InitialTransition.PanSpeed;
		IsTiltSpeedAvailable = patrollingSchema2.InitialTransition.IsTiltSpeedAvailable;
		_initialTiltSpeed = patrollingSchema2.InitialTransition.TiltSpeed;
		IsZoomSpeedAvailable = patrollingSchema2.InitialTransition.IsZoomSpeedAvailable;
		_initialZoomSpeed = patrollingSchema2.InitialTransition.ZoomSpeed;
		_initialExpectedTransitionTimeToPreset = patrollingSchema2.InitialTransition.ExpectedTransitionTime;
		if (patrollingSchema2.EndPosition != null && patrollingSchema2.EndPosition.StartPosition != null && patrollingSchema2.EndPosition.StartPosition.Preset != null && !string.IsNullOrEmpty(patrollingSchema2.EndPosition.StartPosition.Preset.Key))
		{
			PatrollingPreset preset = new PatrollingPreset(patrollingSchema2.EndPosition.StartPosition.Preset.Key);
			PatrollingStep endPosition = new PatrollingStep(this, preset, patrollingSchema2.EndPosition.StartPosition.WaitTime, patrollingSchema2.EndPosition.Transition.ExpectedTransitionTime, patrollingSchema2.EndPosition.Transition.PanSpeed, patrollingSchema2.EndPosition.Transition.TiltSpeed, patrollingSchema2.EndPosition.Transition.ZoomSpeed);
			_endPosition = endPosition;
		}
		_patrollingSteps.Clear();
		if (patrollingSchema2.Edges != null && patrollingSchema2.Edges.Count > 0)
		{
			for (int i = 0; i < patrollingSchema2.Edges.Count; i++)
			{
				PatrollingEdge patrollingEdge = patrollingSchema2.Edges[i] as PatrollingEdge;
				PatrollingPreset preset2 = new PatrollingPreset(patrollingEdge.StartPosition.Preset.Key);
				PatrollingStep patrollingStep = new PatrollingStep(this, preset2, patrollingEdge.StartPosition.WaitTime, patrollingEdge.Transition.ExpectedTransitionTime, patrollingEdge.Transition.PanSpeed, patrollingEdge.Transition.TiltSpeed, patrollingEdge.Transition.ZoomSpeed);
				patrollingStep.SetPosition(i);
				_patrollingSteps.Add(patrollingStep);
			}
		}
	}

	public void WriteDefinition()
	{
		PatrollingSchema patrollingSchema = new PatrollingSchema(base.Id);
		patrollingSchema.DeviceId = new Guid(CameraId);
		patrollingSchema.InitialTransition = new PatrollingTransition();
		patrollingSchema.InitialTransition.IsPanSpeedAvailable = IsPanSpeedAvailable;
		patrollingSchema.InitialTransition.PanSpeed = InitialPanSpeed;
		patrollingSchema.InitialTransition.IsTiltSpeedAvailable = IsTiltSpeedAvailable;
		patrollingSchema.InitialTransition.TiltSpeed = InitialTiltSpeed;
		patrollingSchema.InitialTransition.IsZoomSpeedAvailable = IsZoomSpeedAvailable;
		patrollingSchema.InitialTransition.ZoomSpeed = InitialZoomSpeed;
		patrollingSchema.InitialTransition.ExpectedTransitionTime = InitialExpectedTransitionTimeToPreset;
		patrollingSchema.ShowCustomizeTransitions = _showCustomizations;
		if (_endPosition != null)
		{
			patrollingSchema.EndPosition = new PatrollingEdge();
			patrollingSchema.EndPosition.Order = -1;
			patrollingSchema.EndPosition.StartPosition.WaitTime = _endPosition.WaitTime;
			patrollingSchema.EndPosition.StartPosition.Preset.Key = _endPosition.Preset.Id.ToString();
			patrollingSchema.EndPosition.Transition.PanSpeed = _endPosition.PanSpeed;
			patrollingSchema.EndPosition.Transition.TiltSpeed = _endPosition.TiltSpeed;
			patrollingSchema.EndPosition.Transition.ZoomSpeed = _endPosition.ZoomSpeed;
			patrollingSchema.EndPosition.Transition.ExpectedTransitionTime = _endPosition.ExpectedTransitionTimeToPreset;
		}
		for (int i = 0; i < PatrollingSteps.Count; i++)
		{
			PatrollingEdge patrollingEdge = new PatrollingEdge();
			patrollingEdge.Order = i;
			patrollingEdge.StartPosition.WaitTime = PatrollingSteps[i].WaitTime;
			patrollingEdge.StartPosition.Preset.Key = PatrollingSteps[i].Preset.Id.ToString();
			patrollingEdge.Transition.PanSpeed = PatrollingSteps[i].PanSpeed;
			patrollingEdge.Transition.TiltSpeed = PatrollingSteps[i].TiltSpeed;
			patrollingEdge.Transition.ZoomSpeed = PatrollingSteps[i].ZoomSpeed;
			patrollingEdge.Transition.ExpectedTransitionTime = PatrollingSteps[i].ExpectedTransitionTimeToPreset;
			patrollingSchema.Edges.Add(patrollingEdge);
		}
		DefinitionXml = patrollingSchema.ToXml();
	}
}
