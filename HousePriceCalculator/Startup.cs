using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HousePriceCalculator.Startup))]
namespace HousePriceCalculator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
