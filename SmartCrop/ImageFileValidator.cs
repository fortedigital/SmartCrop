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
            ErrorMessage = "There was a problem connecting to Cognitive Services. " +
                           "Check your API key and server availability.";
        }

        public IEnumerable<ValidationError> Validate(FocalImageData image)
        {
            if (image.SmartCropEnabled
                && image.FocalPointX == 0
                && image.FocalPointY == 0)
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
