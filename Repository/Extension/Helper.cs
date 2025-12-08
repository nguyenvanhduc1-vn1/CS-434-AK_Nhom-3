using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WebDatMonAn.Repository.Extension
{
    public static class Helper
    {
        public static bool IsValidEmail (string email){
            if (email.Trim().EndsWith("_"))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
            }
        public static int PAGE_SIZE = 20;

        public static void CreateMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string GetRandomKey(int length = 5)
        {
            string pattern = @"0123456789zxcvbnmasdfghjklqwertyuiop[]{}:~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }

        public static string SEOUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            // Convert to lowercase
            url = url.ToLower();

            
            // Replace unwanted characters with hyphens
            url = Regex.Replace(url, @"[^a-z0-9\s-]", "");

            // Replace multiple spaces or hyphens with a single hyphen
            url = Regex.Replace(url, @"[\s-]+", " ").Trim();
            url = Regex.Replace(url, @"\s", "-");

            return url;
        }

    }
}
