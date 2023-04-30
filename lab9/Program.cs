using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace lab9
{
    public class Program
    {
        public static string Sometext { get; set;}
        static void Main(string[] args)
        {
            using (FileStream streamtoread = File.OpenRead(ConfigurationManager.AppSettings.Get("FilePath")))
            {
                byte[] buffer = new byte[streamtoread.Length];

                streamtoread.Read(buffer, 0, buffer.Length);

                string textfromfile = Encoding.Default.GetString(buffer);

                //a 
                FileStream SecondfileStream = new FileStream(ConfigurationManager.AppSettings.Get("SecondFilePath"), FileMode.OpenOrCreate);

                SecondfileStream.Write(buffer, 0, buffer.Length);

                SecondfileStream.Close();

                //b

                FileStream ThirdfileStream = new FileStream(ConfigurationManager.AppSettings.Get("ThirdFilePath"), FileMode.OpenOrCreate);

                ThirdfileStream.Write(buffer.Reverse().ToArray(), 0, buffer.Length);

                ThirdfileStream.Close();
            }


            List<string> ListToCreate = new List<string>();
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder();

            while (true)
            {
                Console.WriteLine("chose your action:");
                Console.WriteLine("C for create and fill file \n\n R for read file \n\n CO for complete the file \n\n S for search \n\n M for Sort by marks");
                string action = Console.ReadLine().ToLower();

                switch (action)
                {
                    case "c":
                        Console.Clear();
                        Console.WriteLine("count of students:");
                        int StudCounter = int.Parse(Console.ReadLine());
                        for (int i = 0; i < StudCounter; i++)
                        {
                            Console.WriteLine("write the info about student");
                            Console.Write("student Name:");
                            string StudName = Console.ReadLine();
                            double mark = random.NextDouble() + random.Next(9);

                            Console.WriteLine($"student mark: {mark}");
                            ListToCreate.Add($"{StudName}:{mark} \n");
                            Sometext = stringBuilder.Append(ListToCreate[i] + "\n").ToString();
                        }
                        byte[] bytes = Encoding.Default.GetBytes(Sometext);

                        using (FileStream stream = new FileStream(ConfigurationManager.AppSettings.Get("FilePath"), FileMode.OpenOrCreate))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                            if (stream != null) { stream.Close(); }
                        }
                        break;

                    case "r":
                        Console.Clear();
                        using (FileStream streamtoread = File.OpenRead(ConfigurationManager.AppSettings.Get("FilePath")))
                        {
                            byte[] buffer = new byte[streamtoread.Length];

                            streamtoread.Read(buffer, 0, buffer.Length);

                            string textfromfile = Encoding.Default.GetString(buffer);

                            Console.WriteLine($"text fron file: \r{textfromfile}");
                            if (streamtoread != null) { streamtoread.Close(); } 
                        }
                        break;

                    case "co":
                        Console.Clear();
                        List<string> ListToAppend = new List<string>();
                        Console.WriteLine("count of students for append:");
                        int StudCounterForAppend = int.Parse(Console.ReadLine());
                        Console.WriteLine("write the info about student");
                        for (int i = 0; i < StudCounterForAppend; i++)
                        {
                            Console.Write("student Name:");
                            string StudName = Console.ReadLine();
                            double mark = random.NextDouble() + random.Next(9);

                            Console.WriteLine($"student mark: {mark}");
                            ListToAppend.Add($"{StudName}:{mark} \n");
                        }
                        File.AppendAllLines(ConfigurationManager.AppSettings.Get("FilePath"), ListToAppend);
                        
                        break;
                    case "s":
                        Console.Clear();
                        using (FileStream streamtoread = File.OpenRead(ConfigurationManager.AppSettings.Get("FilePath")))
                        {
                            byte[] buffer = new byte[streamtoread.Length];

                            streamtoread.Read(buffer, 0, buffer.Length);

                            string textfromfile = Encoding.Default.GetString(buffer);

                            Console.Write("note to find");
                            string toFind = Console.ReadLine();

                            string[] strings = textfromfile.Split(' ');

                            foreach (string s in strings)
                            {
                                if (s.Contains(toFind))
                                {
                                    Console.WriteLine(s);
                                    break;
                                }
                            }
                            
                        }
                        break;
                    case "m":
                        Console.Clear();
                        using (FileStream streamtoread = File.OpenRead(ConfigurationManager.AppSettings.Get("FilePath")))
                        {
                            {
                                byte[] buffer = new byte[streamtoread.Length];

                                streamtoread.Read(buffer, 0, buffer.Length);

                                string textfromfile = Encoding.Default.GetString(buffer);

                                string[] strings = textfromfile.Split(' ');

                                Student[] students = new Student[strings.Length - 1];

                                for (int i = 0; i < strings.Length - 1; i++)
                                {
                                    students[i] = new Student();
                                    students[i].Name = strings[i].Split(':')[0];
                                    students[i].Mark = double.Parse(strings[i].Split(':')[1]);
                                }
                                foreach (var item in students.OrderBy(x => x.Mark))
                                {
                                    Console.WriteLine($"Name: {item.Name}, Mark: {item.Mark}");
                                }
                            }
                            break;
                        }
                }
            }
        }
    }
}
