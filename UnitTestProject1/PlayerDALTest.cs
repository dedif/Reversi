using System.Collections.Generic;
using System.Security.Cryptography;
using API.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class PlayerDalTest
    {
        [TestClass]
        public class ByteArrayToAndFromStringConversion
        {
            [TestMethod]
            public void ByteArraysShouldBeCorrectlyConvertedForthAndBack_When100RandomByteArraysAreGenerated()
            {
                // Arrange
                var playerDal = new PlayerDal();
                var byteArrays = new List<byte[]>();
                using (var rng = RandomNumberGenerator.Create())
                {
                    for (var i = 0; i < 100; i++)
                    {
                        var byteArray = new byte[128 / 8];
                        rng.GetBytes(byteArray);
                        byteArrays.Add(byteArray);
                    }
                }
                var newByteArrays = new List<byte[]>();

                // Act
                byteArrays.ForEach(byteArray =>
                    newByteArrays.Add(playerDal.StringToByteArray(playerDal.ByteArrayToString(byteArray))));

                // Assert
                for (var i = 0; i < 100; i++)
                {
                    for (var j = 0; j < byteArrays[i].Length; j++)
                    {
                        if (!newByteArrays[i][j].Equals(byteArrays[i][j]))
                        {
                            Assert.Fail();
                        }
                    }
                }
            }

            //[TestMethod]
            //public void StringsShouldBeCorrectlyConvertedForthAndBack_When100RandomStringsAreGenerated()
            //{
            //    // Arrange
            //    var playerDal = new PlayerDal();
            //    var strings = new List<string>();
            //        for (var i = 0; i < 100; i++)
            //        {
            //            strings.Add(byteArray);
            //        }
            //    var newByteArrays = new List<byte[]>();
            //}
        }

        [TestClass]
        public class HahsPassPhrase
        {
            [TestMethod]
            public void ScryptShouldNotCauseANullReferenceException_WhenTheMethodIsCalled()
            {
                // Arrange
                var playerDal = new PlayerDal();
                var salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // Act
                playerDal.HashPassPhrase("test", salt);

                // Assert
                // There is no assertion for this test,
                // because it just needs to finish the test method to succeed.
            }
        }
    }
}