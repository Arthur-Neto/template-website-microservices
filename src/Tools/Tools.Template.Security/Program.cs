using System;
using Template.Security;

namespace Tools.Template.Security
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Password: ");
            var password = Console.ReadLine();
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.GenerateHash(password, salt);

            Console.WriteLine("Salt: " + salt);
            Console.WriteLine("Hash: " + hash);

            Console.ReadLine();
        }
    }
}
