// Copyright (c) Christopher Currens.  Licensed under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0.html)

using System;
using System.Collections;
using System.Collections.Generic;
using Snappy.Net.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Snappy.Net.PInvoke.Test
{
    public class SnappyTests
    {
        [Fact]
        public void TestMaxCompressedLength_PositiveLength()
        {
            const long compSize = 10;

            long maxCompLen = Snappy.MaxCompressedLength(compSize);

            Assert.True(maxCompLen > 0, "Max compressed length should be greater than 0");
        }

        [Fact]
        public void TestMaxCompressedLength_ZeroLength()
        {
            long maxCompLen = Snappy.MaxCompressedLength(0);

            // MaxCompressedLength should always be greater than 0 (headers and things)
            Assert.NotEqual(maxCompLen, 0);
        }

        [Fact]
        public void TestMaxCompressedLengthLong_NegativeValueShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Snappy.MaxCompressedLength(-55));
            Assert.Throws<ArgumentOutOfRangeException>(() => Snappy.MaxCompressedLength((long)-55));
        }

        [Fact]
        public void TestMaxCompressedLengthInt_CompressedLengthGreaterThanIntMaxValueShouldThrow()
        {
            Assert.Throws<SnappyException>(() => Snappy.MaxCompressedLength(int.MaxValue));
        }

        [Fact]
        public void TestMaxCompressedLengthLong_CompressedLengthGreaterThanLongMaxValueShouldThrow()
        {
            Assert.Throws<SnappyException>(() => Snappy.MaxCompressedLength(long.MaxValue));
        }
        
        [Fact]
        public void TestGetUncompressedLength_ValidDataShouldSucceed()
        {
            var variableInt = new byte[] {0xFE, 0xFF, 0x7F};

            var result = Snappy.GetUncompressedLength(variableInt);

            Assert.Equal(2097150, result);
        }

        [Fact]
        public void TestGetUncompressedLength_InvalidData()
        {
            var variableInt = new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF};
            Assert.Throws<InvalidSnappyDataException>(() => Snappy.GetUncompressedLength(variableInt));
        }

        [Fact]
        public void TestGetUncompressedLength_NullArgumentShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Snappy.GetUncompressedLength(null));
        }

        [Fact]
        public void TestIsValidCompressedBuffer_InvalidBuffer()
        {
            var compressed = TestData.GetResourceBytes("baddata1.snappy");

            var result = Snappy.IsValidCompressedBuffer(compressed);

            Assert.False(result, "An valid compressed buffer was returned as invalid");
        }

        [Fact]
        public void TestIsValidCompressedBuffer_NullArgumentShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Snappy.IsValidCompressedBuffer(null));
        }
        
        [Fact]
        public void TestRawUncompress_NullArgumentShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Snappy.RawUncompress(null));
        }

        [Fact]
        public void TestRawUncompress_OneValidArgumentOneNullArgumentShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Snappy.RawUncompress(new byte[10], null));
            Assert.Throws<ArgumentNullException>(() => Snappy.RawUncompress(null, new byte[10]));
        }

        [Fact]
        public void TestRawUncompress_BadDataShouldReturnFalse()
        {
            var compressed = TestData.GetResourceBytes("baddata3.snappy");
            var uncompBuf = new byte[130378]; // Uncompressed size from snappy data header

            bool result = Snappy.RawUncompress(compressed, uncompBuf);

            Assert.False(result, "Invalid data was returned as successfully uncompressed");
        }

        [Fact]
        public void TestRawUncompress_BadDataShouldThrowCorruptDataException()
        {
            var compressed = TestData.GetResourceBytes("baddata2.snappy");

            Assert.Throws<InvalidSnappyDataException>(() => Snappy.RawUncompress(compressed));
        }

        [Theory, InlineData("aaaaaaaa"), InlineData("abbababbabbabbabababaab"), InlineData("abccdbsaaababsbsdjh")]
        public void TestRawCompress_SimpleTests(string data)
        {
            var originalBytes = System.Text.Encoding.Unicode.GetBytes(data);

            var compressedBytes = Snappy.RawCompress(originalBytes);
            var uncompressedBytes = Snappy.RawUncompress(compressedBytes);

            Assert.Equal(originalBytes, uncompressedBytes);
        }

        [Theory, ClassData(typeof(RandomTestData))]
        public void TestCompression_RandomData(byte[] data)
        {
            var compressedBytes = Snappy.RawCompress(data);
            var uncompressedBytes = Snappy.RawUncompress(compressedBytes);

            Assert.Equal(data, uncompressedBytes);
        }

        public class RandomTestData : IEnumerable<object[]>
        {
            private IEnumerable<object[]> Generate()
            {
                var rand = new Random((int)DateTimeOffset.UtcNow.Ticks);
                for (int i = 0; i < 35; i++)
                {
                    var b = new byte[rand.Next(0, 64*1024)];
                    rand.NextBytes(b);
                    yield return new object[] {b};
                }
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                return Generate().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
