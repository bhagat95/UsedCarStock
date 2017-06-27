using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UsedCarStock.Startup))]
namespace UsedCarStock
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
