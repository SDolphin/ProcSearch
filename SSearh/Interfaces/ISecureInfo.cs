

using SSearh.Types;

namespace SSearh.Interfaces
{

    /// <summary>
    /// extra data for file
    /// </summary>
    public interface ISecureInfo : IFileExInfo
    {
        StartType Type { get; set; } //registr | start menu | services

        bool IsSignatureContains { get; set; } //is signed
        bool IsCorrect { get; set; } // is sign valid
        string SignCompany { get; set; } //name of company
    }
}
