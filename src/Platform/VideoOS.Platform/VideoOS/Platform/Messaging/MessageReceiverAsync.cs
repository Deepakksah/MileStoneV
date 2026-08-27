using System.Threading.Tasks;

namespace VideoOS.Platform.Messaging;

public delegate Task MessageReceiverAsync(IMessageReceiverArgs message);
