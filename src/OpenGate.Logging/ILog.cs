namespace OpenGate.Logging
{
    public interface ILog
    {
        public void Information(string message);
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);
        void Debug(string message);
    }
}