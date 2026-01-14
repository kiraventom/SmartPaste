using System.Security.Cryptography;
using System.Text;

namespace SmartPaste;

public static class Utils
{
    public static string Md5Hex(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hash = MD5.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
