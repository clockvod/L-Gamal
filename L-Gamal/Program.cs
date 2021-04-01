using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static int PowerNmod(int number, int power, int delimeter)
        {
            int result = 1;
            while (power > 0)
            {
                if (power % 2 == 1)
                {
                    result = result * number % delimeter;
                    power--;
                }
                else
                {
                    number = number * number % delimeter;
                    power /= 2;
                }
            }
            return result;
        }
        private static int MulNmod(int number, int multiplexor, int delimeter)
        {
            int sum = 0;
            for (int i = 0; i < multiplexor; i++)
            {
                sum += number;
                if (sum >= delimeter) sum -= delimeter;
            }
            return sum;
        }

        private static int GetPrimitiveRoot(int number)
        {
            List<int> factorizaion = new List<int>();
            int phi = number - 1, n = phi;
            for (int i = 2; i < n; i++)
            {
                if (n % i == 0)
                {
                    factorizaion.Add(i);
                    while (n % i == 0)
                        n /= i;
                }
            }
            if (n > 1) factorizaion.Add(n);

            for (int result = 2; result < number; result++)
            {
                bool found = true;
                for (int i = 0; i < factorizaion.Count && found; i++)
                {
                    found &= PowerNmod(result, phi / factorizaion[i], number) != 1;
                }
                if (found) return result;
            }
            return -1;
        }

        private static bool IsCoprime(int a, int b)
        {
            return a == b ? a == 1 : a > b ? IsCoprime(a - b, b) : IsCoprime(b - a, a);
        }

        private static bool IsPrime(int a)
        {
            bool result = true;
            for (int i = 2; i < a; i++)
            {
                if (a % i == 0)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private static int GetRandomCoprime(int src)
        {
            List<int> coprimes = new List<int>();
            for (int i = 2; i < src; i++)
                if (IsCoprime(i, src)) coprimes.Add(i);
            Random rnd = new Random();
            return rnd.Next(coprimes.Count - 1);
        }
        public static void Main(string[] args)
        {
            //encrypt

            Console.WriteLine("encrypt");
            Random rnd = new Random();
            int p = 0;
            bool primeFlag = false;
            while (!primeFlag)
            {
                p = rnd.Next(256, 2000);
                primeFlag = IsPrime(p);
            }


            int x = GetRandomCoprime(p);
            int g = GetPrimitiveRoot(p);
            int y = PowerNmod(g, x, p);

            Console.WriteLine("Open key (p,g,y) = {0} {1} {2}", p, g, y);
            Console.WriteLine("Secret key (x) = " + x.ToString());
            Console.WriteLine("Enter text:");
            string input = Console.ReadLine();

            string output = "";
            for (int i = 0; i < input.Length; i++)
            {
                char m = input[i];
                int k = GetRandomCoprime(p); //session key
                int a = PowerNmod(g, k, p);
                int b = MulNmod(PowerNmod(y, k, p), m, p);
                output += a + " " + b + " ";
            }
            Console.WriteLine(output);

            //decrypt 
            
            Console.WriteLine("dacrypt");
            Console.WriteLine("Enter p:");
            int p1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter x:");
            int x1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter line:");
            string input1 = Console.ReadLine();
            string[] inputArr = input1.Split(' ');
            string output1 = "";
            for (int i = 0; i < inputArr.Length; i += 2)
            {
                int a = int.Parse(inputArr[i]);
                int b = int.Parse(inputArr[i + 1]);
                output1 += (char)MulNmod(b, PowerNmod(a, p - 1 - x, p), p);
            }
            Console.WriteLine(output1);
            Console.ReadLine();
        }
    }
}