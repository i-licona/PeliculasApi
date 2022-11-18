using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Validaciones
{
  public class SizeFileValidation : ValidationAttribute
  {
    private readonly int maxSizeMB;
    public SizeFileValidation(int maxSizeMB)
    {
      this.maxSizeMB = maxSizeMB;
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value == null)
      {
        return ValidationResult.Success;
      }

      IFormFile formFile = value as IFormFile;
      
      if (formFile == null)
      {
        return ValidationResult.Success;
      }

      if (formFile.Length > maxSizeMB  * 1024 * 1024)
      {
        return new ValidationResult($"El peso del archivo no debe ser mayor a {maxSizeMB}mb");
      }

      return ValidationResult.Success;
    }
  }
}