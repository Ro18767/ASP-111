using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace ASP_111.Services
{
    public class ValidatorService
    {

        public bool ValidateLogin(string str)
        {
            string pattern = @"\W";
            return !Regex.Match(str, pattern, RegexOptions.None).Success;
        }
    }
}
