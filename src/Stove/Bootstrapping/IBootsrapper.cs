namespace Stove.Bootstrapping
{
    public interface IBootsrapper
    {
        void PreStart();

        void Start();

        void PostStart();

        void Shutdown();
    }
}
