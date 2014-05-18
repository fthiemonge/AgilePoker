using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AgilePoker.Tests
{
    [TestClass]
    public class MiscelaneousUnitTests
    {
        [TestMethod]
        public void DeserializationWorks()
        {
            var before = new TestClass(aString: "15", aNumber: 15);
            var serialized = JsonConvert.SerializeObject(before);
            var after = JsonConvert.DeserializeObject<TestClass>(serialized);
            Assert.AreEqual(before, after, String.Format("{0} is not the same as {1}", before, after));
        }

        public class TestClass
        {
            public string AString { get; set; }
            public decimal ANumber { get; set; }

            public TestClass(string aString, decimal aNumber)
            {
                this.AString = aString;
                this.ANumber = aNumber;
            }

            public bool Equals(TestClass other)
            {
                var ret = false;
                if (AString.Equals(other.AString) && ANumber.Equals(other.ANumber))
                {
                    ret = true;
                }
                return ret;
            }

            public override string ToString()
            {
                return String.Format("( AString: '{0}', ANumber: {1}", AString, ANumber);
            }
        }
    }
}
