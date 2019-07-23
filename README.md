# Content-aware automation for Episerver's EPiFocalPoint plugin using Azure Cognitive Services

## Basic installation:

1. Configure your Web.config file - add following keys:

```xml
  <appSettings>
    ...
    <add key="CognitiveServicesApiKey" value="0123456789abcdef"/>
    <add key="CognitiveServicesServer" value="https://westcentralus.api.cognitive.microsoft.com"/>
  </appSettings>
```

2.1. Make your ImageFile model inherit from FocalImageData

```csharp
public class ImageFile : FocalImageData
```

OR

2.2. Inherit from IFocalImageData

```csharp
public class ImageFile : IFocalImageData
```

In this case you need to decorate FocalPoint property with following attribute

```csharp
[BackingType(typeof(PropertyFocalPoint))]
public virtual FocalPoint FocalPoint { get; set; }
```

Also optionally decorate SmartCropEnabled flag to be more descriptive for editors

```csharp
[Display(Name = "Use FocalPoint for cropping")]
public virtual bool SmartFocalPointEnabled { get; set; }
```
<br/>

## Basic usage:

Use HtmlHelper extension FocusedImage

Parameters:

- image - ContentReference to ImageFile property
- width - Desired width of cropped image
- height - Desired height of cropped image
- forceSize - Always make output thumbnail to be in desired size. This flag  takes action only if original width/height is smaller than desired size
- noZoomout - Normally ImageResizer zooms out huge images to preserve as much content as possible. Use this flag to stop this behaviour

Example:

```html
@Html.FocusedImage(Model.Image, 300, 300, forceSize: true, noZoomout: true)
```