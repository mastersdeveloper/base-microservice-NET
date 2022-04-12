namespace BASE.MICRONET.Cross.Cache.Dir
{
    public interface IExtensionCache
    {
        void SetData<T>(T TEntity, string key, int lifeTimeInMinutes);
        T GetData<T>(string key);
    }
}
