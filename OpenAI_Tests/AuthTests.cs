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
			File.WriteAllText(".openai", "OPENAI_KEY=pk-test12");
			Environment.SetEnvironmentVariable("OPENAI_KEY", "pk-test-env");
			//Environment.SetEnvironmentVariable("OPENAI_SECRET_KEY", "sk-test-env");
		}

		[Test]
		public void GetAuthFromEnv()
		{
			var auth = OpenAI_API.APIAuthentication.LoadFromEnv();
			Assert.IsNotNull(auth);
			Assert.IsNotNull(auth.ApiKey);
			Assert.IsNotEmpty(auth.ApiKey);
			Assert.AreEqual("pk-test-env", auth.ApiKey);
		}

		[Test]
		public void GetAuthFromFile()
		{
			var auth = OpenAI_API.APIAuthentication.LoadFromPath();
			Assert.IsNotNull(auth);
			Assert.IsNotNull(auth.ApiKey);
			Assert.AreEqual("pk-test12", auth.ApiKey);
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
			Assert.IsNotNull(auth.ApiKey);
			Assert.AreEqual(envAuth.ApiKey, auth.ApiKey);
		}



		[Test]
		public void testHelper()
		{
			OpenAI_API.APIAuthentication defaultAuth = OpenAI_API.APIAuthentication.Default;
			OpenAI_API.APIAuthentication manualAuth = new OpenAI_API.APIAuthentication("pk-testAA");
			OpenAI_API.OpenAIAPI api = new OpenAI_API.OpenAIAPI();
			OpenAI_API.APIAuthentication shouldBeDefaultAuth = api.Auth;
			Assert.IsNotNull(shouldBeDefaultAuth);
			Assert.IsNotNull(shouldBeDefaultAuth.ApiKey);
			Assert.AreEqual(defaultAuth.ApiKey, shouldBeDefaultAuth.ApiKey);

			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication("pk-testAA");
			api = new OpenAI_API.OpenAIAPI();
			OpenAI_API.APIAuthentication shouldBeManualAuth = api.Auth;
			Assert.IsNotNull(shouldBeManualAuth);
			Assert.IsNotNull(shouldBeManualAuth.ApiKey);
			Assert.AreEqual(manualAuth.ApiKey, shouldBeManualAuth.ApiKey);
		}

		[Test]
		public void GetKey()
		{
			var auth = new OpenAI_API.APIAuthentication("pk-testAA");
			Assert.IsNotNull(auth.ApiKey);
			Assert.AreEqual("pk-testAA", auth.ApiKey);
		}

		[Test]
		public void ParseKey()
		{
			var auth = new OpenAI_API.APIAuthentication("pk-testAA");
			Assert.IsNotNull(auth.ApiKey);
			Assert.AreEqual("pk-testAA", auth.ApiKey);
			auth = "pk-testCC";
			Assert.IsNotNull(auth.ApiKey);
			Assert.AreEqual("pk-testCC", auth.ApiKey);

			auth = new OpenAI_API.APIAuthentication("sk-testBB");
			Assert.IsNotNull(auth.ApiKey);
			Assert.AreEqual("sk-testBB", auth.ApiKey);
		}

	}
}