using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SSearh.Helpers
{
    public static class FileSecure
    {
        public static Signature AuthenticodeSignature(string path)
        {
            string fullPath = Path.GetFullPath(path);

            if (!File.Exists(fullPath))
                return null;

            using (var ps = PowerShell.Create())
            {
                ps.AddCommand("Get-AuthenticodeSignature", true);
                ps.AddParameter("LiteralPath", fullPath);
                var results = ps.Invoke();

                return (Signature)results.Single().BaseObject;

            }
        }

        public static string GetSignatureOrganization(Signature signature)
        {

            string organization = string.Empty;
            try
            {
                var issuer = signature.SignerCertificate.IssuerName;
                var properties = issuer.Format(multiLine: true)
                                        .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(line => line.Split(new[] { '=' }, 2))
                                        .ToDictionary(parts => parts[0], parts => parts[1]);
                properties.TryGetValue("O", out organization);
            }
            catch (Exception ex)
            {

            }

            return organization;

        }


    }
}
