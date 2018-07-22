

using SSearh.Types;

namespace SSearh.Interfaces
{

    /// <summary>
    /// realize factory searching
    /// </summary>
    public interface ISearchFactory
    {
        event SearchEventDelegate SearchMessageEvent;
        void Search();
    }
}
