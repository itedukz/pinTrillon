using System.Security.Cryptography;
using System.Text;

namespace ms.MainApi.Entity.Models.Services;

public class HashMD5
{
    public static string HashMD5String(string source)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            string hash = GetMd5Hash(md5Hash, source);
            return hash;
        }
    }

    static string GetMd5Hash(MD5 md5Hash, string input)
    {
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }
}
