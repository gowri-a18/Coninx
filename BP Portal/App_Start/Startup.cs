using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using Microsoft.Owin.Security;
using Microsoft.Owin.Infrastructure;
//using IdentityModel;

[assembly: OwinStartup(typeof(DemoApi.App_Start.Startup))]
namespace DemoApi.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                //The Path For generating the Toekn  
                TokenEndpointPath = new PathString("/token"),
                //Setting the Token Expired Time (24 hours)  
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
                //AuthorizationServerProvider class will validate the user credentials  
                Provider = new AuthorizationServerProvider()
            };           
            //Token Generations  
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}