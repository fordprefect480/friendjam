using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FriendJam2.Startup))]
namespace FriendJam2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
