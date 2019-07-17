# Content-aware image cropping for EPiServer using Azure Cognitive Services

## Basic installation:

1. Configure your Web.config file - add following keys:

Example:

```xml
  <appSettings>
    ...
    <add key="CognitiveServicesApiKey" value="0123456789abcdef"/>
    <add key="CognitiveServicesServer" value="https://westcentralus.api.cognitive.microsoft.com"/>
  </appSettings>
```

2. Make your ImageFile model inherit from FocalImageData

Example:

```csharp
public class ImageFile : FocalImageData
```

## Basic usage:

Use ImageResize HtmlHelper extension ResizeImage with Crop mode

Example:

```html
<img src="@Html.ResizeImage(Model.CurrentPage.Image, 300, 600).FitMode(FitMode.Crop)" /> <br /><br />
```