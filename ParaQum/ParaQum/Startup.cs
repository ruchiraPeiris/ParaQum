using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ParaQum.Startup))]
namespace ParaQum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
