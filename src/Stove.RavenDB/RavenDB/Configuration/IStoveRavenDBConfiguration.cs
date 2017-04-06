namespace Stove.RavenDB.Configuration
{
    public interface IStoveRavenDBConfiguration
    {
        string Url { get; set; }

        string DefaultDatabase { get; set; }
    }
}
