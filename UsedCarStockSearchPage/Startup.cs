using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UsedCarStockSearchPage.Startup))]
namespace UsedCarStockSearchPage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
