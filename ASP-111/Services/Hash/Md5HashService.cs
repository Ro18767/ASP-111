using System.Security.Cryptography;
using System.Text;

namespace ASP_111.Services.Hash
{
    public class Md5HashService : IHashService
    {
        public string GetHash(string text)
        {
            using var hasher = MD5.Create();
            return
                Convert.ToHexString(
                    hasher.ComputeHash(
                        Encoding.UTF8.GetBytes(text)));
        }
    }
}