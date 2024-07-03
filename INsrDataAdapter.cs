using NsrModels;

namespace NsrTagPlanner
{
    public interface INsrDataAdapter<T>
    {
        T GetData();
    }
}