using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UserTest
    {
        [TestClass]
        public class Password
        {
            private const string TooShortPasswordErrorMessage = "Wachtzin moet minimaal 15 tekens lang zijn";

            private const string TooLongPasswordErrorMessage = "Wachtzin mag maximaal 255 tekens lang zijn";

            private const string RegexFailedErrorMessage =
                "Je wachtzin moet bestaan uit hoofdletters, kleine letters, cijfers, en speciale tekens en mag geen herhalende reeksen bevatten";
            private List<ValidationResult> GetInvalidFields(Player user)
            {
                var validationResults = new List<ValidationResult>();
                var ctx = new ValidationContext(user, null, null);
                Validator.TryValidateObject(user, ctx, validationResults, true);
                return validationResults;
            }

            [TestMethod]
            public void ShouldBeInvalid_WhenPasswordIsShorterThan15Characters()
            {
                // Arrange
                var user = new Player
                {
                    Avatar = "ik",
                    EmailAddress = "x@y.com",
                    PassPhrase = "Test2@"
                };

                // Act
                var invalidFields = GetInvalidFields(user);

                // Assert
                Assert.IsTrue(invalidFields.Exists(v => v.ErrorMessage.Equals(TooShortPasswordErrorMessage)));
            }

            [TestMethod]
            public void ShouldBeInvalid_WhenPasswordIsLongerThan255Characters()
            {
                // Arrange
                var user = new Player
                {
                    Avatar = "ik",
                    EmailAddress = "x@y.com",
                    PassPhrase =
                        "Testabcdefghijlkmnopqrstuvwxyz1234567890@Testabcdefghijlkmnopqrstuvwxyz1234567890@Testabcdefghijlkmnopqrstuvwxyz1234567890@Testabcdefghijlkmnopqrstuvwxyz1234567890@Testabcdefghijlkmnopqrstuvwxyz1234567890@Testabcdefghijlkmnopqrstuvwxyz1234567890@Testabcdefghijlkmnopqrstuvwxyz1234567890@"
                };

                // Act
                var invalidFields = GetInvalidFields(user);

                // Assert
                Assert.IsTrue(invalidFields.Exists(v => v.ErrorMessage.Equals(TooLongPasswordErrorMessage)));
            }

            [TestMethod]
            public void ShouldBeInvalid_WhenPasswordContainsNoCapitals()
            {
                // Arrange
                var user = new Player
                {
                    Avatar = "ik",
                    EmailAddress = "x@y.com",
                    PassPhrase = "testabcdefghijklmnopqrstuvwxyz2@"
                };

                // Act
                var invalidFields = GetInvalidFields(user);

                // Assert
                Assert.IsTrue(invalidFields.Exists(v => v.ErrorMessage.Equals(RegexFailedErrorMessage)));
            }

            [TestMethod]
            public void ShouldBeInvalid_WhenPasswordContainsNoSmallLetters()
            {
                // Arrange
                var user = new Player
                {
                    Avatar = "ik",
                    EmailAddress = "x@y.com",
                    PassPhrase = "TESTABCDEFGHIJKLMNOPQRSTUVWXYZ2@"
                };

                // Act
                var invalidFields = GetInvalidFields(user);

                // Assert
                Assert.IsTrue(invalidFields.Exists(v => v.ErrorMessage.Equals(RegexFailedErrorMessage)));
            }

            [TestMethod]
            public void ShouldBeInvalid_WhenPasswordContainsNoNumbers()
            {
                // Arrange
                var user = new Player
                {
                    Avatar = "ik",
                    EmailAddress = "x@y.com",
                    PassPhrase = "Testabcdefghijklmnopqrstuvwxyz@"
                };

                // Act
                var invalidFields = GetInvalidFields(user);

                // Assert
                Assert.IsTrue(invalidFields.Exists(v => v.ErrorMessage.Equals(RegexFailedErrorMessage)));
            }

            [TestMethod]
            public void ShouldBeInvalid_WhenPasswordContainsNoSpecialCharacters()
            {
                // Arrange
                var user = new Player
                {
                    Avatar = "ik",
                    EmailAddress = "x@y.com",
                    PassPhrase = "Testabcdefghijklmnopqrstuvwxyz2"
                };

                // Act
                var invalidFields = GetInvalidFields(user);

                // Assert
                Assert.IsTrue(invalidFields.Exists(v => v.ErrorMessage.Equals(RegexFailedErrorMessage)));
            }

            [TestMethod]
            public void ShouldBeInvalid_WhenPasswordContainsThreeSameCharactersNearEachOther()
            {
                // Arrange
                var user = new Player
                {
                    Avatar = "ik",
                    EmailAddress = "x@y.com",
                    PassPhrase = "TTTestabcdefghijklmnopqrstuvwxyz2@"
                };

                // Act
                var invalidFields = GetInvalidFields(user);

                // Assert
                Assert.IsTrue(invalidFields.Exists(v => v.ErrorMessage.Equals(RegexFailedErrorMessage)));
            }

            [TestMethod]
            public void ShouldBeValid_WhenAllConditionsAreMet()
            {
                // Arrange
                var user = new Player
                {
                    Avatar = "ik",
                    EmailAddress = "x@y.com",
                    PassPhrase = "Testabcdefghijklmnopqrstuvwxyz2@"
                };

                // Act
                var invalidFields = GetInvalidFields(user);

                // Assert
                Assert.IsTrue(invalidFields.Count == 0);
            }
        }
    }
}
