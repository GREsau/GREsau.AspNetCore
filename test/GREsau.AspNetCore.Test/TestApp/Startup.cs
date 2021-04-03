using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GREsau.AspNetCore.Test.TestApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().WithJsonPropertyModelMetadataNames();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting().UseEndpoints(c => c.MapControllers());
        }
    }
}
