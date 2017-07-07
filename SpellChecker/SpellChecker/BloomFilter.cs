using System;
using System.Data.HashFunction;
using System.Text;

namespace SpellChecker
{
    public class BloomFilter
    {
        // CTOR
        public BloomFilter(string[] values, uint bloomFilterSize, uint hashCount)
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException("The list of values cannot be null or have a length of zero.");
            }
            if (hashCount < 1 || hashCount > 20)
            {
                throw new ArgumentOutOfRangeException("hashCount", "Hash count valid range is between 1 and 20");
            }
            if (bloomFilterSize == 0)
            {
                throw new ArgumentOutOfRangeException("bloomFilterSize", "Bloom filter size cannot be zero.");
            }
            if (bloomFilterSize < hashCount)
            {
                throw new ArgumentException("Bloom filter size cannot be less than the hash count.");
            }

            _itemCount = (uint)values.Length;
            _bloomSize = bloomFilterSize;        
            _hashCount = hashCount;
            _vector = new bool[_bloomSize];
            _seedValues = new uint[_hashCount];

            InitSeedValues();
            InitVector(values);

            double m = _bloomSize;
            double n = _itemCount;
            double k = _hashCount;
            _falsePositiveRate = Math.Round(Math.Pow(1 - Math.Exp(-k * n / m), k), 2);
        }

        // PRIVATE INSTANCE FIELDS
        private bool[] _vector;
        private uint[] _seedValues;
        private uint _bloomEntryCount;
        private readonly uint _bloomSize;
        private readonly uint _itemCount;
        private readonly uint _hashCount;
        private double _falsePositiveRate;

        // PUBLIC INSTANCE PROPERTIES
        public double FalsePositiveRate
        {
            get
            {
                return _falsePositiveRate;
            }
        }

        public uint BloomFilterSize
        {
            get
            {
                return _bloomSize;
            }
        }

        public uint HashCount
        {
            get
            {
                return _hashCount;
            }
        }

        public uint ItemCount
        {
            get
            {
                return _itemCount;
            }
        }

        public uint BloomEntryCount
        {
            get
            {
                return _bloomEntryCount;
            }
        }

        // PRIVATE INSTANCE METHODS
        private void InitVector(string[] values)
        {
            for (int i = 0; i < _vector.Length; i++)
            {
                _vector[i] = false;
            }

            uint index = 0;
            for (int i = 0; i < values.Length; i++)
            {
                for (int j = 0; j < _seedValues.Length; j++)
                {
                    index = Hash(values[i], _seedValues[j]);
                    _vector[index] = true;
                    _bloomEntryCount++;
                }
            }
        }

        private void InitSeedValues()
        {
            uint seed = 40;
            for (int i = 0; i < _seedValues.Length; i++)
            {
                _seedValues[i] = seed++;
            }
        }

        private uint Hash(string value, uint seed)
        {
            value = value.ToUpper();
            var hash = new MurmurHash3(seed).ComputeHash(value);
            uint index = (uint)
                           (hash[0]
                            | hash[1] << 8
                            | hash[2] << 16
                            | hash[3] << 24);
            index = index % _bloomSize;
            return index;
        }

        // PUBLIC INSTANCE METHODS
        public bool Lookup(string value)
        {
            uint index = 0;
            for (int i = 0; i < _seedValues.Length; i++)
            {
                index = Hash(value, _seedValues[i]);
                if (!_vector[index]) return false;
            }
            return true;
        }

        public string GeekStats()
        {
            var sb = new StringBuilder();
            sb.AppendLine(" ================================================");
            sb.AppendLine(" | m is the size of the bloom filter            |");
            sb.AppendLine(" | k is the number of hash functions used       |");
            sb.AppendLine(" | n is the number of items                     |");
            sb.AppendLine(" | q is the number of items in the bloom filter |");
            sb.AppendLine(" ================================================");
            sb.AppendLine($" m = {BloomFilterSize}");
            sb.AppendLine($" k = {HashCount}");
            sb.AppendLine($" n = {ItemCount}");
            sb.AppendLine($" q = {BloomEntryCount}");
            sb.Append($" Probability of a false positive is: {_falsePositiveRate * 100.0}%");
            return sb.ToString();
        }
    }
}
