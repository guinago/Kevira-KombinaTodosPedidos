using Microsoft.Owin;
using Owin;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

[assembly: OwinStartupAttribute(typeof(KeViraKombinaTodos.Web.Startup))]
namespace KeViraKombinaTodos.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            IServiceCollection services = new ServiceCollection();

            //Configure other services up here
           // var multiplexer = ConnectionMultiplexer.Connect("localhost");
            //services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        }       
    }
}
