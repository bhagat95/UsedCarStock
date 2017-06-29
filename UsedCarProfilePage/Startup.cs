using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UsedCarProfilePage.Startup))]
namespace UsedCarProfilePage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
