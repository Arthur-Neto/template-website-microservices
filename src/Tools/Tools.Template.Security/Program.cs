using System;
using Template.Security;

namespace Tools.Template.Security
{
    public class Program
    {
        static void Main(string[] args)
        {
            var hashing = new Hashing();

            Console.WriteLine("Password: ");
            var password = Console.ReadLine();
            var salt = hashing.GenerateSalt();
            var hash = hashing.GenerateHash(password, salt);

            Console.WriteLine("Salt: " + salt);
            Console.WriteLine("Hash: " + hash);

            Console.ReadLine();
        }
    }
}
