using MagicOnion;

namespace Example.Shared;

public interface IMyFirstService : IService<IMyFirstService>
{
    UnaryResult<int> SumAsync(int x, int y);
}