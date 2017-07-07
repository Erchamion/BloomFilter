using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker
{
    public class SpellChecker
    {
        // CTOR
        public SpellChecker(string dictionaryFilePath, uint bloomFilterSize, uint hashCount)
        {
            _dictinaryFilePath = dictionaryFilePath;
            _bloomFilterSize = bloomFilterSize;
            _hashCount = hashCount;
        }

        // PRIVATE INSTANCE FIELDS
        private BloomFilter _bloomFilter;
        private readonly string _dictinaryFilePath;
        private readonly uint _bloomFilterSize;
        private readonly uint _hashCount;
        private const int _largeListCapacity = 350000;

        // PRIVATE INSTANCE METHODS
        private async Task<string[]> LoadDictionaryAsync()
        {
            List<string> buffer = new List<string>(_largeListCapacity);
            using (var sr = new StreamReader(_dictinaryFilePath))
            {
                string word = string.Empty;
                do
                {
                    word = await sr.ReadLineAsync() ?? string.Empty;
                    buffer.Add(word.ToUpper());
                } while (!string.IsNullOrWhiteSpace(word));
            }
            return buffer.ToArray();
        }

        // PUBLIC INSTANCE METHODS
        public async Task InitializeAsync()
        {
            var words = await LoadDictionaryAsync();
            _bloomFilter = new BloomFilter(words, _bloomFilterSize, _hashCount);
        }

        public bool CheckSpelling(string word)
        {
            return _bloomFilter.Lookup(word);
        }

        public string GeekStats()
        {
            var sb = new StringBuilder();
            sb.AppendLine("This spell checker is using bloom filters.");
            sb.AppendLine("Bloom filters use a probablistic approach when determining if an item exists in the filter.");
            sb.AppendLine("Here are some stats from the bloom filter:");
            sb.AppendLine("");
            sb.AppendLine(_bloomFilter.GeekStats());
            return sb.ToString();
        }
    }
}
