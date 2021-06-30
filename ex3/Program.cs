using System;
using System.Text;
using System.Security.Cryptography;
namespace ex3
{   
    class Program
    {   
        static void Main(string[] input)
        {       
            int n = input.Length;
            string error = " Error: неверный ввод аргументов.\n";
            string example = " Количество вводимых аргументов должно быть нечётным и >=3 \n" +
            " Пример: 1 2 3 (или) a b c d e (или) камень ножницы бумага \n";
            if (n==0) 
            {
                Console.WriteLine(error + " КОЛИЧЕСТВО ВВЕДЁННЫХ АРГУМЕНТОВ = 0\n" + example); 
                return; 
            }
            
            if (n==1) 
            { 
                Console.WriteLine(error + " КОЛИЧЕСТВО ВВЕДЁННЫХ АРГУМЕНТОВ = 1\n" + example); 
                return; 
            }
                
            if (n%2==0) 
            { 
                Console.WriteLine(error + " ЧЁТНОЕ КОЛИЧЕСТВО ВВЕДЁННЫХ АРГУМЕНТОВ \n" + example); 
                return; 
            }

            for (int i = 0; i < n; i++)
            {   
                for (int j = i + 1; j < n; j++)
                {   
                    if (input[i] == input[j])
                    { 
                        Console.WriteLine(error + " ВВЕДЕНЫ ПОВТОРЯЮЩИЕСЯ АРГУМЕНТЫ \n" + example); 
                        return; 
                    }
                }
            }

            while (true)
            {   
                RandomNumberGenerator rng = new RNGCryptoServiceProvider();
                byte[] key = new byte[24];  
                rng.GetBytes(key);
                string hmac_key = Convert.ToBase64String(key);

                Random rnd = new Random(); int number_move = rnd.Next(1, n+1);
                string move = input[number_move-1];

                byte[] t = Encoding.ASCII.GetBytes(hmac_key); 
                byte[] s = Encoding.ASCII.GetBytes(move);
                var hash = new HMACSHA256(t);
                string hmac = BitConverter.ToString(hash.ComputeHash(s)).Replace("-", "");

                Console.WriteLine("\n   HMAC: " + hmac);
                Console.WriteLine("   Available moves:"); int temp = 1;
                foreach (string i in input)  
                    Console.WriteLine((temp++).ToString() + " - " + i);
                Console.WriteLine("0 - exit");

                Console.Write(" Enter your move:   "); 
                int my_move = Convert.ToInt32(Console.ReadLine());
                if (my_move == 0) 
                { 
                    Console.WriteLine("Your choise: Exit "); 
                    return; 
                }

                Console.WriteLine(" Your move:    " + input[my_move - 1]);
                Console.WriteLine(" Computer move:   " + move);

                string result="";
                // number_move - ход компьютера    
                // my_move - наш ход (в числах от 1 до кол-во исходных атрибутов)
                // оценка относительно нашего хода:
                if (number_move == my_move) 
                { 
                    result = " Draw !!! "; 
                }
                else
                {   
                    my_move--; 
                    number_move--;
                    int N = (n - 1) / 2;
                    if (my_move + N < n)
                    {   
                        if (my_move < number_move && number_move <= my_move + N) 
                            result = " Computer win ! ";
                        else 
                            result = " You win ! ";
                    }
                    else
                    {   if (number_move > my_move || number_move < N - (n-my_move-1) ) 
                            result = " Computer win ! ";
                        else 
                            result = " You win ! ";
                    }
                }

                Console.WriteLine(result);  
                Console.WriteLine("   HMAC key: " + hmac_key);
                Console.WriteLine("   Please press any key ..."); 
                Console.ReadKey();
            }

        }       
    }
}