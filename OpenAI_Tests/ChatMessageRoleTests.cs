using NUnit.Framework;
using OpenAI_API.Chat;

namespace OpenAI_Tests
{
    public class ChatMessageRoleTests
    {
        [Test]
        public void TestImplicitConversionNotThrowing()
        {
            // ReSharper disable once UnusedVariable
            string result = ChatMessageRole.System;
        }
        
        [Test]
        public void TestImplicitConversionValue()
        {
            string result = ChatMessageRole.System;
            Assert.AreEqual("system", result);
        }
    }
}
