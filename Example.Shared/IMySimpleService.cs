using MagicOnion;

namespace Example.Shared;

public interface IMySimpleService : IService<IMySimpleService>
{
    UnaryResult<string> PrintMessage(string message);
}