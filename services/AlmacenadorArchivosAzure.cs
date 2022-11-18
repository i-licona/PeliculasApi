using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace PeliculasApi.services
{
  public class AlmacenadorArchivosAzure : IAlmacenarArchivos
  {
    private readonly string connetionString;

    public AlmacenadorArchivosAzure(IConfiguration config)
    {
      this.connetionString = config.GetConnectionString("AzureStorage");
    }

    public async Task DeleteFile(string contenedor, string route)
    {
      
      if (string.IsNullOrEmpty(route))
      {
        return;
      }

      var client = new BlobContainerClient(connetionString, contenedor);
      await client.CreateIfNotExistsAsync();
      var file = Path.GetFileName(route);
      var blob = client.GetBlobClient(file);
      await blob.DeleteIfExistsAsync();
    }

    public async Task<string> EditFile(byte[] contenido, string extension, string contenedor, string route, string contentType)
    {
      await DeleteFile(contenedor,route);
      return await SaveFile(contenido, extension,contenedor, contentType);
    }

    public async Task<string> SaveFile(byte[] contenido, string extension, string contenedor, string contentType)
    {
      var client = new BlobContainerClient(connetionString, contenedor);
      await client.CreateIfNotExistsAsync();
      client.SetAccessPolicy(PublicAccessType.Blob);
      var fileName = $"{Guid.NewGuid()}{extension}";
      var blob =  client.GetBlobClient(fileName);
      var blobUploadOptions = new BlobUploadOptions();
      var blodHttpHeader = new BlobHttpHeaders();
      blodHttpHeader.ContentType = contentType;
      blobUploadOptions.HttpHeaders = blodHttpHeader;

      await blob.UploadAsync(new BinaryData(contenido), blobUploadOptions);
      return blob.Uri.ToString();
    
    }
  }
}