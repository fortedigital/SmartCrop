# Content-aware image cropping for EPiServer using Azure Cognitive Services

Basic installation:

Configure your Web.config file - add following keys:

Example:

```xml
  <appSettings>
    ...
    <add key="CognitiveServicesApiKey" value="0123456789abcdef"/>
    <add key="CognitiveServicesServer" value="https://westcentralus.api.cognitive.microsoft.com"/>
  </appSettings>
```