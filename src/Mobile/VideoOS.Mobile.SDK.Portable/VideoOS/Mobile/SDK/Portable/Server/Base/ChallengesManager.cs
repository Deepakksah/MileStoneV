using System;
using System.Collections.Generic;
using System.Text;
using VideoOS.Mobile.Portable.Encryption;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class ChallengesManager : IChallengesManager
{
	private enum State
	{
		Normal,
		MinQueueSizeOneReached,
		MinQueueSizeTwoReached
	}

	private bool _initialized;

	private bool _activated;

	private byte[] _privateKey;

	private string _privateKeyHexEncoded;

	private readonly object _queueLocker = new object();

	private readonly Queue<ChallengeData> _challenges = new Queue<ChallengeData>();

	private readonly ISha512Hasher _sha512Hasher;

	private State _state;

	private readonly object _stateLocker = new object();

	public const uint MaxSingleGeneratedChallenges = 100u;

	public const uint MaxQueueSize = 500u;

	public const uint MinQueueSizeOne = 50u;

	public const uint MinQueueSizeTwo = 25u;

	public const uint QueueSizeOneRequestCount = 50u;

	public const uint QueueSizeTwoRequestCount = 100u;

	public static readonly TimeSpan ExpirationServer = new TimeSpan(1, 0, 0);

	public static readonly TimeSpan ExpirationThreshold = new TimeSpan(0, 5, 0);

	public static readonly TimeSpan ExpirationInterval = ExpirationServer - ExpirationThreshold;

	public bool IsActive => _activated;

	public GetChallenges GetChallenges { get; set; }

	public ChallengesManager()
	{
		_sha512Hasher = new Sha512HasherImplementer();
	}

	public bool Initialize(byte[] privateKey)
	{
		_privateKey = new byte[privateKey.Length];
		Array.Copy(privateKey, _privateKey, privateKey.Length);
		return Initialize(EncodeToHexString(_privateKey));
	}

	public bool Initialize(string privateKeyHexString)
	{
		_privateKeyHexEncoded = privateKeyHexString;
		_initialized = true;
		return _initialized;
	}

	public bool Activate()
	{
		_activated = _initialized;
		return _activated;
	}

	public string GenerateHashForChallenge(string challenge)
	{
		string s = challenge.ToUpper() + _privateKeyHexEncoded;
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		return Convert.ToBase64String(_sha512Hasher.ComputeHash(bytes));
	}

	public static string EncodeToHexString(byte[] bytes)
	{
		return BitConverter.ToString(bytes).Replace("-", "");
	}

	public ChallengePair GetNextChallenge()
	{
		if (!_activated)
		{
			return null;
		}
		ChallengePair challengePair = null;
		int count = 0;
		DateTime utcNow = DateTime.UtcNow;
		lock (_queueLocker)
		{
			while (_challenges.Count > 0)
			{
				ChallengeData challengeData = _challenges.Dequeue();
				if (utcNow - challengeData.CreatedUtc < ExpirationInterval)
				{
					challengePair = new ChallengePair
					{
						Challenge = challengeData.Challenge
					};
					break;
				}
			}
			count = _challenges.Count;
		}
		if (challengePair != null)
		{
			challengePair.Hash = GenerateHashForChallenge(challengePair.Challenge);
			CheckRestCount(count);
		}
		return challengePair;
	}

	public void AddChallenges(IEnumerable<string> challenges)
	{
		AddChallengesInternal(challenges);
	}

	public int AddChallengesInternal(IEnumerable<string> challenges)
	{
		if (!_activated)
		{
			return 0;
		}
		DateTime utcNow = DateTime.UtcNow;
		int num = 0;
		lock (_queueLocker)
		{
			foreach (string challenge in challenges)
			{
				_challenges.Enqueue(new ChallengeData
				{
					Challenge = challenge,
					CreatedUtc = utcNow
				});
			}
			return _challenges.Count;
		}
	}

	private void CheckRestCount(int count)
	{
		if ((long)count >= 50L)
		{
			return;
		}
		lock (_stateLocker)
		{
			if (_state == State.Normal)
			{
				_state = State.MinQueueSizeOneReached;
				RequestChallenges(50u);
			}
			if ((long)count < 25L && _state != State.MinQueueSizeTwoReached)
			{
				_state = State.MinQueueSizeTwoReached;
				RequestChallenges(100u);
			}
		}
	}

	private void RequestChallenges(uint count)
	{
		GetChallenges(count, AddChallengesExternal);
	}

	private void AddChallengesExternal(IEnumerable<string> challenges)
	{
		int count = AddChallengesInternal(challenges);
		lock (_stateLocker)
		{
			_state = State.Normal;
		}
		CheckRestCount(count);
	}
}
