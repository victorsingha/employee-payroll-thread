using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollThread
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Employee Payroll using Threads");
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");

            #region ParallelTasks
            Parallel.Invoke(() =>
            {
                Console.WriteLine("Begin first task...");
                GetLongestWord(words);
            },// close 1st action
            () =>
            {
                Console.WriteLine("Begin second task...");
                GetMostCommonWords(words, "second");
            },
            () =>
            {
                Console.WriteLine("Begin third task...");
                GetMostCommonWords(words, "third");
            }// close third action
            );// close parallel.invoke
            #endregion
            Console.ReadKey();
        }

        private static void GetMostCommonWords(string[] words, string task)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into q
                                 orderby q.Count() descending
                                 select q.Key;
            var commonWord = frequencyOrder.Take(10);
            Console.WriteLine("---------------------" + task + "------------------------------");
            foreach (var item in commonWord)
            {
                Console.WriteLine(task + ":" + item);
            }
        }

        public static string[] CreateWordArray(string uri)
        {
            Console.WriteLine($"Retrieving from {uri}");
            //Download a web page the easy way
            string blog = new WebClient().DownloadString(uri);
            return blog.Split(
                new char[] { ' ', '\u000A', '.', ',', '-', '_', '/' },
                StringSplitOptions.RemoveEmptyEntries);
        }
        private static string GetLongestWord(string[] words)
        {
            string longestWord = (from w in words
                                  orderby w.Length descending
                                  select w).First();

            Console.WriteLine($"Task 1 -- The longest word is {longestWord}.");
            return longestWord;
        }


    }

}
