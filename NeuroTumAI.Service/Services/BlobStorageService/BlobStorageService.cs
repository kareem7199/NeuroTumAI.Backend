using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.Service.Services.BlobStorageService
{
	public class BlobStorageService : IBlobStorageService
	{
		private readonly BlobServiceClient _blobServiceClient;

		public BlobStorageService(IConfiguration configuration)
		{
			var connectionString = configuration["AzureStorage:ConnectionString"];
			_blobServiceClient = new BlobServiceClient(connectionString);
		}

		public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string containerName)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
			
			string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

			var blobClient = containerClient.GetBlobClient(uniqueFileName);
			await blobClient.UploadAsync(fileStream, true);

			return blobClient.Uri.ToString();
		}
	}
}
