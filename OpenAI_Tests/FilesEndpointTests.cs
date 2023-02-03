using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI_Tests
{
	public class FilesEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[Test]
		[Order(1)]
		public async Task UploadFile()
		{
			var api = new OpenAI_API.OpenAIAPI();
			var response = await api.Files.UploadFileAsync("fine-tuning-data.jsonl");
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Id.Length > 0);
			Assert.IsTrue(response.Object == "file");
			Assert.IsTrue(response.Bytes > 0);
			Assert.IsTrue(response.CreatedAt > 0);
			Assert.IsTrue(response.Status == "uploaded");
			// The file must be processed before it can be used in other operations, so for testing purposes we just sleep awhile.
			Thread.Sleep(10000);
		}

		[Test]
		[Order(2)]
		public async Task ListFiles()
		{
				var api = new OpenAI_API.OpenAIAPI();
				var response = await api.Files.GetFilesAsync();
				
				foreach (var file in response)
				{
					Assert.IsNotNull(file);
					Assert.IsTrue(file.Id.Length > 0);
				}
		}


		[Test]
		[Order(3)]
		public async Task GetFile()
		{
			var api = new OpenAI_API.OpenAIAPI();
			var response = await api.Files.GetFilesAsync();
			foreach (var file in response)
			{
				Assert.IsNotNull(file);
				Assert.IsTrue(file.Id.Length > 0);
				string id = file.Id;
				if (file.Name == "fine-tuning-data.jsonl")
				{
					var fileResponse = await api.Files.GetFileAsync(file.Id);
					Assert.IsNotNull(fileResponse);
					Assert.IsTrue(fileResponse.Id == id);
				}
			}
		}

		[Test]
		[Order(4)]
		public async Task DeleteFiles()
		{
			var api = new OpenAI_API.OpenAIAPI();
			var response = await api.Files.GetFilesAsync();
			foreach (var file in response)
			{
				Assert.IsNotNull(file);
				Assert.IsTrue(file.Id.Length > 0);
				if (file.Name == "fine-tuning-data.jsonl")
				{
					var deleteResponse = await api.Files.DeleteFileAsync(file.Id);
					Assert.IsNotNull(deleteResponse);
					Assert.IsTrue(deleteResponse.Deleted);
				}
			}
		}

	}
}
