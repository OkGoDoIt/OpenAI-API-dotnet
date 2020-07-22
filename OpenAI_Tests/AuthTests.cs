using NUnit.Framework;
using System;
using System.IO;

namespace OpenAI_Tests
{
	public class AuthTests
	{
		[SetUp]
		public void Setup()
		{
			File.WriteAllText(".openai", "OPENAI_KEY=pk-test12" + Environment.NewLine + "OPENAI_SECRET_KEY: sk-test34");
			Environment.SetEnvironmentVariable("OPENAI_KEY", "pk-test-env");
			Environment.SetEnvironmentVariable("OPENAI_SECRET_KEY", "sk-test-env");
		}

		[Test]
		public void GetAuthFromEnv()
		{
			var auth = OpenAI_API.APIAuthentication.LoadFromEnv();
			Assert.IsNotNull(auth);
			Assert.IsNotNull(auth.APIKey);
			Assert.IsNotEmpty(auth.APIKey);
			Assert.AreEqual("pk-test-env", auth.APIKey);

			Assert.IsNotNull(auth.Secretkey);
			Assert.IsNotEmpty(auth.Secretkey);
			Assert.AreEqual("sk-test-env", auth.Secretkey);

		}

		[Test]
		public void GetAuthFromFile()
		{
			var auth = OpenAI_API.APIAuthentication.LoadFromPath();
			Assert.IsNotNull(auth);
			Assert.IsNotNull(auth.APIKey);
			Assert.AreEqual("pk-test12", auth.APIKey);
			Assert.IsNotNull(auth.Secretkey);
			Assert.AreEqual("sk-test34", auth.Secretkey);
		}


		[Test]
		public void GetAuthFromNonExistantFile()
		{
			var auth = OpenAI_API.APIAuthentication.LoadFromPath(filename: "bad.config");
			Assert.IsNull(auth);
		}


		[Test]
		public void GetDefault()
		{
			var auth = OpenAI_API.APIAuthentication.Default;
			var envAuth = OpenAI_API.APIAuthentication.LoadFromEnv();
			Assert.IsNotNull(auth);
			Assert.IsNotNull(auth.APIKey);
			Assert.AreEqual(envAuth.APIKey, auth.APIKey);
			Assert.IsNotNull(auth.Secretkey);
			Assert.AreEqual(envAuth.Secretkey, auth.Secretkey);
		}



		[Test]
		public void testHelper()
		{
			OpenAI_API.APIAuthentication defaultAuth = OpenAI_API.APIAuthentication.Default;
			OpenAI_API.APIAuthentication manualAuth = new OpenAI_API.APIAuthentication("pk-testAA", "sk-testBB");
			OpenAI_API.OpenAIAPI api = new OpenAI_API.OpenAIAPI();
			OpenAI_API.APIAuthentication shouldBeDefaultAuth = api.Auth;
			Assert.IsNotNull(shouldBeDefaultAuth);
			Assert.IsNotNull(shouldBeDefaultAuth.APIKey);
			Assert.AreEqual(defaultAuth.APIKey, shouldBeDefaultAuth.APIKey);
			Assert.IsNotNull(shouldBeDefaultAuth.Secretkey);
			Assert.AreEqual(defaultAuth.Secretkey, shouldBeDefaultAuth.Secretkey);

			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication("pk-testAA", "sk-testBB");
			api = new OpenAI_API.OpenAIAPI();
			OpenAI_API.APIAuthentication shouldBeManualAuth = api.Auth;
			Assert.IsNotNull(shouldBeManualAuth);
			Assert.IsNotNull(shouldBeManualAuth.APIKey);
			Assert.AreEqual(manualAuth.APIKey, shouldBeManualAuth.APIKey);
			Assert.IsNotNull(shouldBeManualAuth.Secretkey);
			Assert.AreEqual(manualAuth.Secretkey, shouldBeManualAuth.Secretkey);
		}

		[Test]
		public void GetKey()
		{
			var auth = new OpenAI_API.APIAuthentication("pk-testAA", "sk-testBB");
			Assert.IsNotNull(auth.GetKey());
			Assert.AreEqual("sk-testBB", auth.GetKey());

			auth = new OpenAI_API.APIAuthentication("pk-testAA", null);
			Assert.IsNotNull(auth.GetKey());
			Assert.AreEqual("pk-testAA", auth.GetKey());
		}

		[Test]
		public void ParseKey()
		{
			var auth = new OpenAI_API.APIAuthentication("pk-testAA");
			Assert.IsNotNull(auth.APIKey);
			Assert.IsNull(auth.Secretkey);
			Assert.AreEqual("pk-testAA", auth.APIKey);
			auth = "pk-testCC";
			Assert.IsNotNull(auth.APIKey);
			Assert.IsNull(auth.Secretkey);
			Assert.AreEqual("pk-testCC", auth.APIKey);

			auth = new OpenAI_API.APIAuthentication("sk-testBB");
			Assert.IsNotNull(auth.Secretkey);
			Assert.IsNull(auth.APIKey);
			Assert.AreEqual("sk-testBB", auth.Secretkey);
			auth = "sk-testDD";
			Assert.IsNotNull(auth.Secretkey);
			Assert.IsNull(auth.APIKey);
			Assert.AreEqual("sk-testDD", auth.Secretkey);
		}

	}
}