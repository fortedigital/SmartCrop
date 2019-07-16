using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Core;
using EPiServer.Validation;
using Forte.SmartCrop.Models.Media;

namespace Forte.SmartCrop
{
    public class ImageFileValidator : IValidate<FocalImageData>
    {
        public string ErrorMessage { get; set; }

        public ImageFileValidator()
        {
            ErrorMessage = "Focal Point properties not set." + 
                           "Run scheduled job to update all images' properties or" +
                           "check your API key and server availability to Cognitive Services.";
        }

        public IEnumerable<ValidationError> Validate(FocalImageData image)
        {
            if (image.SmartCropEnabled
                && image.FocalPointX == 0
                && image.FocalPointY == 0
                && image.FocalPoint == null)
            {
                return new ValidationError[]
                {
                    new ValidationError()
                    {
                        ErrorMessage = ErrorMessage,
                        PropertyName = image.GetPropertyName<FocalImageData>(p => p.Name),
                        Severity = ValidationErrorSeverity.Warning,
                        ValidationType = ValidationErrorType.Unspecified
                    }
                };
            }

            return Enumerable.Empty<ValidationError>();
        }
    }
}
