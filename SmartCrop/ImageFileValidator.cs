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
    public class ImageFileValidator : IValidate<ImageFile>
    {
        public string ErrorMessage { get; set; }

        public ImageFileValidator()
        {
            ErrorMessage = "There was a problem connecting to Cognitive Services. " +
                           "Check your API key and server availability.";
        }

        public IEnumerable<ValidationError> Validate(ImageFile image)
        {
            if (image.SmartCropEnabled 
                && image.AreaOfInterestX == 0 
                && image.AreaOfInterestY == 0
                && image.AreaOfInterestHeight == 0 
                && image.AreaOfInterestWidth == 0)
            {
                return new ValidationError[]
                {
                    new ValidationError()
                    {
                        ErrorMessage = ErrorMessage,
                        PropertyName = image.GetPropertyName<ImageFile>(p => p.Name),
                        Severity = ValidationErrorSeverity.Warning,
                        ValidationType = ValidationErrorType.Unspecified
                    }
                };
            }

            return Enumerable.Empty<ValidationError>();
        }
    }
}
