namespace Networking
{
    public enum MessageType
    {
        ClientLoginTry,
        ServerLoginVerify,
        ClientRequestGroups,
        ServerSendGroups,
        ClientRequestTests,
        ServerSendTest,
        ClientSendResult
    }
}
