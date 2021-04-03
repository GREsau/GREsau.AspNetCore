using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GREsau.AspNetCore
{
    internal class JsonPropertyModelMetadataProviderSetup : IConfigureOptions<MvcOptions>
    {
        private readonly IOptions<JsonOptions> _jsonOptions;

        public JsonPropertyModelMetadataProviderSetup(IOptions<JsonOptions> jsonOptions)
        {
            _jsonOptions = jsonOptions;
        }

        public void Configure(MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(new JsonPropertyModelMetadataProvider(_jsonOptions.Value.JsonSerializerOptions));
        }
    }
}
