namespace Stove.Runtime.Session
{
    public class SessionOverride
    {
        public SessionOverride(long? userId)
        {
            UserId = userId;
        }

        public long? UserId { get; }
    }
}
