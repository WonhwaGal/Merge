
public sealed class ServiceLocator
{
    private static ServiceLocator _instance;

    public static ServiceLocator Container => _instance ?? (_instance = new ServiceLocator());

    public void Register<TService>(TService someService) where TService : IService
    {
        HolderFor<TService>.ServiceInstance = someService;
    }

    public TService RequestFor<TService>() where TService : IService
    {
        return HolderFor<TService>.ServiceInstance;
    }

    public TService RegisterAndAssign<TService>(TService someService) where TService : IService
    {
        return HolderFor<TService>.ServiceInstance = someService;
    }

    private static class HolderFor<TService> where TService : IService
    {
        public static TService ServiceInstance;
    }
}