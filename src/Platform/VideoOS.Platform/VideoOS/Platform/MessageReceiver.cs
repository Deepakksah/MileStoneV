using VideoOS.Platform.Messaging;

namespace VideoOS.Platform;

public delegate object MessageReceiver(Message message, FQID destination, FQID sender);
