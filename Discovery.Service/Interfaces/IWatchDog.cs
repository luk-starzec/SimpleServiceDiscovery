namespace Discovery.Service.Interfaces;

public interface IWatchDog
{
    Task StatusCheckAsync();
}
