namespace GadgetIPhoneStore.Configuration
{
    public interface IController<T> where T : class
    {
        public Task<List<T>> Add(T t);
        public Task<List<T>> Update(T t);
        public Task<List<T>> Delete(T t);
        public Task<List<T>> Select();
    }
}
