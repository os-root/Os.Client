namespace OrlemSoftware.Client.Abstractions;

public interface IClientLogger
{
    void Log(ClientLogLevel level, string message, params object[] args);
    void Log(ClientLogLevel level, Exception ex, string message, params object[] args);
}