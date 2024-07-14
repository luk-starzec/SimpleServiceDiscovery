namespace Discovery.Service.Interfaces;

public interface IWatchDogService
{
    Task StatusCheckAsync();
}
