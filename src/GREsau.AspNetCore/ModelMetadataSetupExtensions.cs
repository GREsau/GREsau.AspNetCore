using GREsau.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for configuring MVC model binding metadata.
    /// </summary>
    public static class ModelMetadataSetupExtensions
    {
        /// <summary>
        /// Configures a <see cref="IMetadataDetailsProvider" /> that sets <see cref="BindingMetadata.BinderModelName" />
        /// and <see cref="DisplayMetadata.DisplayName" /> of all properties to their corresponding JSON name.
        /// </summary>
        public static IMvcBuilder WithJsonPropertyModelMetadataNames(this IMvcBuilder builder)
        {
            AddMetadataProviderSetup(builder.Services);
            return builder;
        }

        /// <summary>
        /// Configures a <see cref="IMetadataDetailsProvider" /> that sets <see cref="BindingMetadata.BinderModelName" />
        /// and <see cref="DisplayMetadata.DisplayName" /> of all properties to their corresponding JSON name.
        /// </summary>
        public static IMvcCoreBuilder WithJsonPropertyModelMetadataNames(this IMvcCoreBuilder builder)
        {
            AddMetadataProviderSetup(builder.Services);
            return builder;
        }

        private static void AddMetadataProviderSetup(IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, JsonPropertyModelMetadataProviderSetup>());
        }
    }
}
