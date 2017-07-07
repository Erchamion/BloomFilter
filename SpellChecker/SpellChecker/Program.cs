using SpellChecker.Properties;
using System;
using System.Threading.Tasks;

namespace SpellChecker
{
    class Program
    {
        private static SpellChecker _spellChecker;

        static void Main(string[] args)
        {
            Console.WriteLine("Run spell checker with a high false positive rate?  Enter [y] for yes or [n] for no.");
            var option = Console.ReadLine();
            if (option.ToLower() == "n")
            {
                _spellChecker = new SpellChecker(Settings.Default.dictionaryFilePath, Settings.Default.bloomFilterSizeLowFalsePositiveRate, Settings.Default.hashCount);
            }
            else
            {
                _spellChecker = new SpellChecker(Settings.Default.dictionaryFilePath, Settings.Default.bloomFilterSizeHighFalsePositiveRate, Settings.Default.hashCount);

            }
            if (!Initialize())
            {
                return;
            }

            Console.WriteLine("Spell checker has been initialized.");
            Console.WriteLine("");
            Console.WriteLine("Geeky stats incomming...");
            Console.WriteLine($"{_spellChecker.GeekStats()}");

            StartSpellChecker();
        }

        private static bool Initialize()
        {
            Console.WriteLine("");
            Console.WriteLine("Initializing Spell Checker....");
            Task task = null;
            try
            {
                task = _spellChecker.InitializeAsync();
                task.Wait();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Something went wrong when trying to initialize the spell checker...");
                Console.WriteLine(ex.AggregateExceptions());
            }
            return false;
        }

        private static void StartSpellChecker()
        {
            Console.WriteLine("Type in a word and hit enter to check if the spelling is correct! Type q [enter] to quit.");
            do
            {
                string word = Console.ReadLine();
                if (word == "q") break;

                Console.WriteLine("checking...");
                try
                {
                    var probablyCorrect = _spellChecker.CheckSpelling(word);
                    Console.WriteLine(probablyCorrect ? "Spelling probably correct!" : "Spelling not correct.");
                }
                catch
                {
                    Console.WriteLine("An error occurred while checking the spelling.");
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Type in a word and hit enter to check if the spelling is correct! Type q [enter] to quit.");
                }

                Console.WriteLine(Environment.NewLine );
                Console.WriteLine("Type in a word and hit enter to check if the spelling is correct! Type q [enter] to quit.");

            } while (true);
        }
    }
}
