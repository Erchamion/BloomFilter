using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpellChecker;

namespace Tests
{
    [TestClass]
    public class BloomFilterTests
    {
        private string[] dictionary = new string []
        {
            "dog",
            "cat",
            "bear",
            "bird",
            "shark",
            "coral",
            "pine",
            "rose"
        };

        [TestMethod]
        public void InitBloomFilter_WithOneHundredPercentFalsePositive_ReturnFalsePositive()
        {
            BloomFilter bloomFilter = new BloomFilter(dictionary, (uint)dictionary.Length, 2);
            var exists = bloomFilter.Lookup("foo");
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void InitBloomFilter_WithOneHundredPercentFalsePositive_ReturnOneHundredPercentFalsePositiveRate()
        {
            BloomFilter bloomFilter = new BloomFilter(dictionary, (uint)dictionary.Length, (uint)dictionary.Length);
            double fp = bloomFilter.FalsePositiveRate * 100;
            Assert.IsTrue(fp == 100);
        }

        [TestMethod]
        public void InitBloomFilter_WithOnePercentFalsePositive_ReturnNoFalsePositive()
        {
            BloomFilter bloomFilter = new BloomFilter(dictionary, 150, 2);
            var exists = bloomFilter.Lookup("foo");
            Assert.IsTrue(!exists);
        }

        [TestMethod]
        public void InitBloomFilter_WithOnePercentFalsePositive_ReturnTrueValueInDictionary()
        {
            BloomFilter bloomFilter = new BloomFilter(dictionary, 150, 2);
            var exists = bloomFilter.Lookup("cat");
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void InitBloomFilter_WithOnePercentFalsePositive_ReturnOnePercentFalsePositiveRate()
        {
            BloomFilter bloomFilter = new BloomFilter(dictionary, 150, 2);
            double fp = bloomFilter.FalsePositiveRate * 100;
            Assert.IsTrue(fp == 1);
        }

        [TestMethod]
        public void InitBloomFilter_GetDictionaryListSize_ReturnDictionaryListSize()
        {
            BloomFilter bloomFilter = new BloomFilter(dictionary, 150, 2);
            Assert.IsTrue(bloomFilter.ItemCount == dictionary.Length);
        }

        [TestMethod]
        public void InitBloomFilter_GetBloomFilterSize_ReturnBloomFilterSize()
        {
            BloomFilter bloomFilter = new BloomFilter(dictionary, 150, 2);
            Assert.IsTrue(bloomFilter.BloomFilterSize == 150);
        }

        [TestMethod]
        public void InitBloomFilter_GetHashCount_ReturnHashCount()
        {
            BloomFilter bloomFilter = new BloomFilter(dictionary, 150, 2);
            Assert.IsTrue(bloomFilter.HashCount == 2);
        }

        [TestMethod]
        public void InitBloomFilter_InvalidBloomFilterSize_AssertExceptionThrown()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () =>
                {
                    var foo = new BloomFilter(dictionary, 0, 0);
                });
        }

        [TestMethod]
        public void InitBloomFilter_InvalidHashSize_AssertExceptionThrown()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () =>
                {
                    var foo = new BloomFilter(dictionary, 15, 0);
                });
        }

        [TestMethod]
        public void InitBloomFilter_InvalidDictionaryEmptyArray_AssertExceptionThrown()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var foo = new BloomFilter(new string[0], 15, 0);
                });
        }

        [TestMethod]
        public void InitBloomFilter_InvalidDictionaryNullArray_AssertExceptionThrown()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var foo = new BloomFilter(null, 15, 0);
                });
        }

        [TestMethod]
        public void InitBloomFilter_InvalidHashSizeWithValidBloomFilterSize_AssertExceptionThrown()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    var foo = new BloomFilter(dictionary, 15, 20);
                });
        }
    }
}
