# GREsau.AspNetCore
Utilities for ASP.NET Core web applications.

## Installing
Install via NuGet:
```sh
> dotnet add package GREsau.AspNetCore
```

## JSON Property Model Metadata Names

Basic usage:
```csharp
services.AddControllers().WithJsonPropertyModelMetadataNames();
```

By default, when model validation fails, MVC will use the C# property names in the resulting [`ValidationProblemDetails`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails?view=aspnetcore-5.0). This will typically returned to the client as a JSON body of a 400 Bad Request response.

For example, given the model class:
```csharp
public class MyModel
{
    [Range(1, 100)]
    [JsonPropertyName("foo")]
    public int Number { get; set; }

    [Required]
    public string Text { get; set; }
}
```

If the client POSTs the JSON `{ "foo": 0 }` to a controller that takes `MyModel`, then they will receive a 400 with a body similar to:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "|01234567-0123456789abcdef.",
  "errors": {
    "Number": [
      "The field Number must be between 1 and 100."
    ],
    "Text": [
      "The field Text is required."
    ]
  }
}
```

...even though the client would have no prior knowledge of the identifier `Number`. To make MVC use the identifier `foo` instead, you can use this library's `WithJsonPropertyModelMetadataNames()` extension method on `IMvcBuilder` (or `IMvcCoreBuilder`), e.g. in your Startup class:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers().WithJsonPropertyModelMetadataNames();
}
```

Then, the response body would be:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "|01234567-0123456789abcdef.",
  "errors": {
    "foo": [
      "The field foo must be between 1 and 100."
    ],
    "text": [
      "The field text is required."
    ]
  }
}
```

Be aware this will alter the name used for [model binding](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-5.0) of any properties, unless they are explicitly overridden (e.g. using a `[BindProperty(Name = "...")]` attribute). However, it will not alter the model binding name for any method parameters, or properties of a controller (any subclass of [`ControllerBase`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase?view=aspnetcore-5.0)).