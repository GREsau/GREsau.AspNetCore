using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GREsau.AspNetCore
{
    internal class JsonPropertyModelMetadataProvider : IBindingMetadataProvider, IDisplayMetadataProvider
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public JsonPropertyModelMetadataProvider(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
            var jsonPropertyName = JsonPropertyName(context.Attributes, context.Key);
            if (jsonPropertyName != null)
            {
                context.BindingMetadata.BinderModelName ??= jsonPropertyName;
            }
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            var jsonPropertyName = JsonPropertyName(context.Attributes, context.Key);
            if (jsonPropertyName != null)
            {
                context.DisplayMetadata.DisplayName ??= () => jsonPropertyName;
            }
        }

        private string JsonPropertyName(IReadOnlyList<object> attributes, ModelMetadataIdentity key)
        {
            if (key.MetadataKind != ModelMetadataKind.Property
                || typeof(ControllerBase).IsAssignableFrom(key.ContainerType)
                || typeof(PageModel).IsAssignableFrom(key.ContainerType))
            {
                return null;
            }

            return attributes.OfType<JsonPropertyNameAttribute>().FirstOrDefault()?.Name
                ?? _jsonSerializerOptions.PropertyNamingPolicy?.ConvertName(key.Name);

        }
    }
}
