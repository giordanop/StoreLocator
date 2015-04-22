namespace StoreLocator.Domain.Setting
{
    public interface IDomainSettings
    {
        bool EnableCaching { get; }

        int DefaultCacheExpirationInMin { get; }
    }
}
