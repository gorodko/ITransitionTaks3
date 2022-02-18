using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ItransitionTask3.Startup))]
namespace ItransitionTask3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
