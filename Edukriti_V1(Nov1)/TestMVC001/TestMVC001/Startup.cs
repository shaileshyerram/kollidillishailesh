using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestMVC001.Startup))]
namespace TestMVC001
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
