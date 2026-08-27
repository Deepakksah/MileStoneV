namespace VideoOS.Management.Server;

public class ObjectToken
{
	public string ObjectId { get; set; }

	public string Token { get; set; }

	public ObjectToken()
	{
	}

	public ObjectToken(string objectId, string token)
	{
		ObjectId = objectId;
		Token = token;
	}
}
