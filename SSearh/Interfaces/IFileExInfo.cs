using System.IO;



namespace SSearh.Interfaces
{   

    /// <summary>
    /// interface for base information of file
    /// </summary>
    public interface IFileExInfo
    {
        string Name { get; set; } // Name of file or service
        FileInfo PathF { get; set; } // path to exe file
        string CommandToRun { get; set; } // string command with args to run in cmd
    }
}
