using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

namespace Wisielec
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Wisielec";

            string word = "";
            string status = "";
            int lives;

            //komunikaty końcowe
            string success ="Brawo! Odgadłeś słowo! - ";
            string fail = "Niestety! Przegrałeś :( Słowem było: ";

            Random rnd = new Random();           

            List<string> list_of_words = new();                             //lista słów
            List<string> hints = new();                                    //litery wygenerowane przez podpowiedź


            //pobranie listy słów z pliku dictionary
            using (StreamReader sr = new StreamReader("dictionary.txt"))
            {
                string w = sr.ReadLine();

                while (w != null)
                {
                    list_of_words.Add(w);
                    w = sr.ReadLine();
                }
            }

            // poniżej główna pętla programu
            while (true)
            {
                hints.Clear();
                lives = 10;
                status = "";
                word = list_of_words[rnd.Next(0, list_of_words.Count)];      //losowanie słowa z tablicy list_of_words              
                char[] ghost = Ghost(word);                                 //funkcja tworzy tablicę char wypełnioną myślnikami w ilości długości słowa word
                List<string> typed = new();                                //lista wpisywanych liter  
               
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Pozostałe szanse: " + lives);
                    Console.WriteLine("\n");

                    switch (lives)
                    {
                        case 10:
                            for (int i = 0; i < 7; i++)
                            {
                                Console.WriteLine();
                            }
                            break;
                        case 9:
                            Console.WriteLine();
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            break;
                        case 8:
                            Console.WriteLine("[][][][]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            break;
                        case 7:
                            Console.WriteLine("[][][][]");
                            Console.WriteLine("[]    #");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            break;
                        case 6:
                            Console.WriteLine("[][][][]");
                            Console.WriteLine("[]    #");
                            Console.WriteLine("[]    O");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            break;
                        case 5:
                            Console.WriteLine("[][][][]");
                            Console.WriteLine("[]    #");
                            Console.WriteLine("[]    O");
                            Console.WriteLine("[]    |");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            break;
                        case 4:
                            Console.WriteLine("[][][][]");
                            Console.WriteLine("[]    #");
                            Console.WriteLine("[]    O");
                            Console.WriteLine("[]   _|");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            break;
                        case 3:
                            Console.WriteLine("[][][][]");
                            Console.WriteLine("[]    #");
                            Console.WriteLine("[]    O");
                            Console.WriteLine("[]   _|_");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            break;
                        case 2:
                            Console.WriteLine("[][][][]");
                            Console.WriteLine("[]    #");
                            Console.WriteLine("[]    O");
                            Console.WriteLine("[]   _|_");
                            Console.WriteLine("[]    |");
                            Console.WriteLine("[]");
                            Console.WriteLine("[]");
                            break;
                        case 1:
                            Console.WriteLine("[][][][]");
                            Console.WriteLine("[]    #");
                            Console.WriteLine("[]    O");
                            Console.WriteLine("[]   _|_");
                            Console.WriteLine("[]    |");
                            Console.WriteLine("[]   /");
                            Console.WriteLine("[]");
                            break;
                    }

                    Console.WriteLine("\n");

                    Console.WriteLine("");                  //wypisananie stanu odgadniętego słowa na górze oknae;
                    for (int i = 0; i < ghost.Length; i++)
                    {
                        if (hints.Contains(Convert.ToString(ghost[i])))
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(ghost[i] + " ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                            Console.Write(ghost[i] + " ");
                    }
                    Console.WriteLine("\n");
                    Console.Write("\n\nPodane litery: ");     //ciąg wpisanych liter
                    foreach (string s in typed)
                    {
                        Console.Write(s + " ");
                    }
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n" + status);
                    status = "";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\nPodaj literę: ");
                    string litera = (Console.ReadLine());
                    if (litera == null)
                    {
                        Console.Clear();
                        continue;
                    }
                    try
                    {
                        LetterChecker(litera.ToUpper());
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        continue;
                    }
                    finally
                    {
                        litera = litera.ToUpper();
                    }

                    if (litera == "*")
                    {
                        do
                        {
                            litera = Convert.ToString(word[rnd.Next(0, word.Length)]);
                        }
                        while (typed.Contains(Convert.ToString(litera)));

                        hints.Add(litera);
                    }

                    int first_check = word.IndexOf(litera);         //próba wyszukania podanej litery w słowie word

                    if (first_check == -1)                      //co jeśli nie ma tej litery w słowie?
                    {

                        if (typed.Contains(litera))
                        {
                            status = "Już podałeś taką literę!";
                            Console.Clear();
                        }
                        else
                        {
                            typed.Add(litera);
                            status = "Brak litery!";
                            lives--;
                            Console.Clear();
                        }
                        if (lives == 0)
                        {
                            break;
                        }
                        continue;
                    }
                    else                                                                //a jeżeli jest?
                    {

                        for (int i = 0; i < ghost.Length; i++)
                        {
                            if (word[i] == Convert.ToChar(litera))
                            {
                                ghost[i] = Convert.ToChar(litera);
                            }

                        }
                        if (typed.Contains(litera))
                        {
                            status = "Już podałeś taką literę!";
                            Console.Clear();
                        }
                        else
                            typed.Add(litera);

                        if (Array.IndexOf(ghost, '-') == -1)
                        {
                            Console.Clear();
                            break;
                        }

                        Console.Clear();
                        continue;
                    }
                }

                if (lives < 1 && Array.IndexOf(ghost, '-') != -1)
                {
                    SaveToFile(fail + word);
                    Console.WriteLine(fail + word);
                }
                else
                {
                    SaveToFile(success +  word);
                    Console.WriteLine(success + word);                    
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nWciśnij dowolny klawisz oprócz Q żeby rozpocząć od nowa");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wciśnij Q, żeby zakończyć");

                if (Console.ReadKey().Key == ConsoleKey.Q) break;
 
                else
                {
                    Console.Clear();
                    continue;
                }
            }
        }

        /// <summary>
        /// This method check given string is a letter
        /// </summary>
        /// <param name="a">the value</param>
        /// <returns> true or false</returns>
        static bool LetterChecker(string a)
        {
            char[] letters = { 'A', 'Ą', 'B', 'C', 'Ć', 'D', 'E', 'Ę', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'Ł', 
                'M', 'N', 'Ń', 'O', 'Ó', 'P', 'Q', 'R', 'S', 'Ś', 'T', 'U', 'W', 'Y', 'X', 'Z', 'Ż', 'Ź', '*' };

            if (a.Length == 1)
            {
                int index = a.IndexOfAny(letters);
                if (index == -1)
                {
                    throw new Exception();
                }
                return true;
            }
            else
                throw new Exception();
        }

        /// <summary>
        /// This method create a "ghost" char array contains '-'
        /// </summary>
        /// <param name="a">the value</param>
        /// <returns> true or false</returns>
        static char[] Ghost(string word)
        {
            char[] ghost = new char[word.Length];

            for (int j = 0; j < word.Length; j++)
            {
                ghost[j] = '-';
            }
            return ghost;
        }

        static void SaveToFile(string result)
        {
            using (StreamWriter sw = new StreamWriter("results.txt", true))
            {
                DateTime time = DateTime.Now;
                sw.WriteLine($"{time} {result}");
            }
        }
    }
}