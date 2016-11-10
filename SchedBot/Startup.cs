using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SchedBot.Startup))]
namespace SchedBot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
