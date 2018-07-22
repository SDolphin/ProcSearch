using SSearh.Interfaces;
using System.ComponentModel;

namespace SSearh.Types
{

    public delegate void SearchEventDelegate(object sender, IFileExInfo message);
    public delegate void SecureEventDelegate(object sender, ISecureInfo message);

    public enum StartType
    {
        [Description("Registry")]
        Registry,
        [Description("Start Menu")]
        StartMenu,
        [Description("Sheduler")]
        Sheduler,
    }
}
