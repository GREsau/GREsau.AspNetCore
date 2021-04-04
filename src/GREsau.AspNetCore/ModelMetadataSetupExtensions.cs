using GREsau.AspNetCore;
using Microsoft.AspNetCore.Mvc;
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
        /// Configures an <c>IMetadataDetailsProvider</c> that sets the <c>BindingMetadata.BinderModelName</c>
        /// and <c>DisplayMetadata.DisplayName</c> of all properties to their corresponding JSON names.
        /// <para>A property's JSON name is determined from a <c>[JsonPropertyName]</c> attribute if present,
        /// otherwise falling back to the property naming policy configured on the MVC JSON options.</para>
        /// </summary>
        /// <remarks>
        /// This will alter the result of failed model validation to use the JSON property names rather than
        /// C# property names.
        /// </remarks>
        public static IMvcBuilder AddJsonPropertyModelMetadataNames(this IMvcBuilder builder)
        {
            AddMetadataProviderSetup(builder.Services);
            return builder;
        }

        ///<inheritdoc cref="AddJsonPropertyModelMetadataNames(IMvcBuilder)"/>
        public static IMvcCoreBuilder AddJsonPropertyModelMetadataNames(this IMvcCoreBuilder builder)
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
