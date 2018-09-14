using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DistinctionTask.Startup))]
namespace DistinctionTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
