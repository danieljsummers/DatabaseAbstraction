namespace DatabaseAbstraction.Tests.Models
{
    using DatabaseAbstraction.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit Tests for the ParameterDictionary class
    /// </summary>
    [TestClass]
    public class ParameterDictionaryTest
    {
        /// <summary>
        /// Test the index accessor with a non-existent key
        /// </summary>
        [TestMethod]
        public void ParameterDictionary_Index_NonExistentKey_Success()
        {
            var dictionary = new ParameterDictionary();

            Assert.IsNotNull(dictionary, "The parameter dictionary should not be null");
            Assert.IsNull(dictionary["non_existent_key"], "A non-existent key should have returned null");
        }

        /// <summary>
        /// Test the index accessor with an existing value
        /// </summary>
        [TestMethod]
        public void ParameterDictionary_Index_ExistingValue_Success()
        {
            var dictionary = new ParameterDictionary();
            dictionary["test"] = 1;

            Assert.AreEqual(1, dictionary["test"], "An existing value was not returned correctly");
        }

        /// <summary>
        /// Test the index accessor with a null Nullable object
        /// </summary>
        [TestMethod]
        public void ParameterDictionary_Index_NullNullable_Success()
        {
            int? nullableInt = null;

            var dictionary = new ParameterDictionary();
            dictionary["nullable"] = nullableInt;

            Assert.IsNull(dictionary["nullable"], "The null Nullable<?> item should have returned null");
        }

        /// <summary>
        /// Test the index accessor with a non-null Nullable object
        /// </summary>
        [TestMethod]
        public void ParameterDictionary_Index_NotNullNullable_Success()
        {
            var dictionary = new ParameterDictionary();
            dictionary["nullable"] = new int?(3);

            Assert.AreEqual(3, dictionary["nullable"], "The non-null Nullable<?> item should have returned its value");
        }
    }
}
