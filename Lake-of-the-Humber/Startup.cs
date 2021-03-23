using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lake_of_the_Humber.Startup))]
namespace Lake_of_the_Humber
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
