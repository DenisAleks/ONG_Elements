namespace _Common.Pool
{
    public interface IPool<T>
    {
        T Get();
        void Release(T balloon);
    }
}