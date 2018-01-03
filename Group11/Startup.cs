using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Group11.Startup))]
namespace Group11
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
