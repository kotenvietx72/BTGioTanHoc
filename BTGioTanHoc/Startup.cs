using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BTGioTanHoc.Startup))]
namespace BTGioTanHoc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
