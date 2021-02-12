namespace Networking
{
    public enum MessageType
    {
        ClientLoginTry,
        ServerLoginVerify,
        ClientRequestGroups,
        ServerSendGroups,
        ClientRequestTest,
        ServerSendTest,
        ClientSendResult
    }
}
