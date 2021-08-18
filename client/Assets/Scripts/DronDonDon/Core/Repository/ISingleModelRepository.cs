using JetBrains.Annotations;

namespace DronDonDon.Core.Repository
{
    public interface ISingleModelRepository<T>

    {
        [CanBeNull]
        [PublicAPI]
        T Get();

        [PublicAPI]
        T Require();

        [PublicAPI]
        bool Exists();

        [PublicAPI]
        void Set(T model);

        [PublicAPI]
        void Delete();
    }
}