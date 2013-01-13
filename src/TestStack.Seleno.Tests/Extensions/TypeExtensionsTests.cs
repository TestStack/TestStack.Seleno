using System;
using NUnit.Framework;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.Extensions
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void TryConvertTo()
        {
            Assert.AreEqual(null, "21/03/18 rubbish date".TryConvertTo<string>(typeof(DateTime)));
            Assert.AreEqual(0d, "£8.99".TryConvertTo(typeof(decimal), 0d));
            Assert.AreEqual(9, (9.0f).TryConvertTo(0));
            Assert.AreEqual("47623", (47623).TryConvertTo(""));
            Assert.AreEqual(AnEnum.Value1, "Value1".TryConvertTo(typeof(AnEnum?), null));
            Assert.AreEqual(false, "false".TryConvertTo(typeof(bool?), null));
        }
    }

    internal enum AnEnum
    {
        Value1
    }
}
