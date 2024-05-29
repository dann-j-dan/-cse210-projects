using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScriptureMemorizer
{
    // Class to represent a single verse or verse range
    public class ScriptureReference
    {
        public string Book { get; }
        public int Chapter { get; }
        public int VerseStart { get; }
        public int VerseEnd { get; }

        // Constructor for single verse
        public ScriptureReference(string book, int chapter, int verse)
        {
            Book = book;
            Chapter = chapter;
            VerseStart = verse;
            VerseEnd = verse;
        }

        // Constructor for verse range
        public ScriptureReference(string book, int chapter, int verseStart, int verseEnd)
        {
            Book = book;
            Chapter = chapter;
            VerseStart = verseStart;
            VerseEnd = verseEnd;
        }

        public override string ToString()
        {
            if (VerseStart == VerseEnd)
                return $"{Book} {Chapter}:{VerseStart}";
            else
                return $"{Book} {Chapter}:{VerseStart}-{VerseEnd}";
        }
    }

    // Class to represent a word in the scripture
    public class ScriptureWord
    {
        public string Word { get; }
        public bool Hidden { get; set; }

        public ScriptureWord(string word)
        {
            Word = word;
            Hidden = false;
        }

        public override string ToString()
        {
            return Hidden ? "_".PadRight(Word.Length) : Word;
        }
    }

    // Class to represent a scripture
    public class Scripture
    {
        public ScriptureReference Reference { get; }
        private List<ScriptureWord> Words { get; }

        public Scripture(ScriptureReference reference, string text)
        {
            Reference = reference;
            Words = text.Split(' ').Select(word => new ScriptureWord(word)).ToList();
        }

        // Hide a few random words
        public void HideRandomWords(int count)
        {
            Random random = new Random();
            List<int> hiddenIndices = new List<int>();

            while (hiddenIndices.Count < count)
            {
                int index = random.Next(0, Words.Count);
                if (!Words[index].Hidden)
                {
                    Words[index].Hidden = true;
                    hiddenIndices.Add(index);
                }
            }
        }

        // Check if all words are hidden
        public bool AllWordsHidden()
        {
            return Words.All(word => word.Hidden);
        }

        public override string ToString()
        {
            return $"{Reference}\n\n{string.Join(" ", Words)}";
        }
    }

    // Main program class
    class Program
    {
        static void Main(string[] args)
        {
            // Load scriptures from file or hardcode them
            List<Scripture> scriptures = LoadScripturesFromFile("scriptures.txt");

            foreach (var scripture in scriptures)
            {
                DisplayScripture(scripture);

                while (!scripture.AllWordsHidden())
                {
                    Console.WriteLine("\nPress Enter to reveal more words or type 'quit' to exit:");
                    string input = Console.ReadLine();

                    if (input.ToLower() == "quit")
                        return;

                    Console.Clear();
                    scripture.HideRandomWords(3); // Hide 3 random words
                    DisplayScripture(scripture);
                }
            }

            Console.WriteLine("All scriptures are memorized!");
        }

        // Display the scripture
        static void DisplayScripture(Scripture scripture)
        {
            Console.WriteLine(scripture);
        }

        // Load scriptures from file
        static List<Scripture> LoadScripturesFromFile(string filePath)
        {
            List<Scripture> scriptures = new List<Scripture>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string[] parts = line.Split('|');
                    string[] referenceParts = parts[0].Split(' ');
                    string book = referenceParts[0];
                    int chapter = int.Parse(referenceParts[1].Split(':')[0]);
                    string[] verseParts = referenceParts[1].Split(':')[1].Split('-');
                    int verseStart = int.Parse(verseParts[0]);
                    int verseEnd = verseParts.Length > 1 ? int.Parse(verseParts[1]) : verseStart;
                    var reference = new ScriptureReference(book, chapter, verseStart, verseEnd);
                    var scripture = new Scripture(reference, parts[1]);
                    scriptures.Add(scripture);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Scriptures file not found!");
            }

            return scriptures;
        }
    }
}
