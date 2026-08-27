namespace VideoOS.Installer.UI.Framework.IISSetup;

public class SiteInfo
{
	public class SiteState
	{
		public enum STATETYPE
		{
			starting = 1,
			started,
			stopping,
			stopped,
			pausing,
			paused,
			continuing
		}

		public STATETYPE State { get; private set; }

		public SiteState(STATETYPE state)
		{
			State = state;
		}

		public override string ToString()
		{
			return State switch
			{
				STATETYPE.starting => "Starting", 
				STATETYPE.started => "Started", 
				STATETYPE.stopping => "Stopping", 
				STATETYPE.stopped => "Stopped", 
				STATETYPE.pausing => "Pausing", 
				STATETYPE.paused => "Paused", 
				STATETYPE.continuing => "Continuing", 
				_ => "Unknown state", 
			};
		}
	}

	public string Id { get; private set; }

	public string Name { get; private set; }

	public string Address { get; private set; }

	public int HttpPort { get; private set; }

	public int HttpsPort { get; private set; }

	public SiteState State { get; private set; }

	public SiteInfo(string id, string name, string address, int httpPort, int httpsPort, int state)
	{
		Id = id;
		Name = name;
		Address = address;
		HttpPort = httpPort;
		HttpsPort = httpsPort;
		State = new SiteState((SiteState.STATETYPE)state);
	}

	public override string ToString()
	{
		return Name + ", http binding=" + Address + ":" + HttpPort + ", https binding=" + Address + ":" + HttpsPort + ", state=" + State.ToString();
	}
}
