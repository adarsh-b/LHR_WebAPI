using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using WebAPI.Web.Security;

[assembly: OwinStartup(typeof(WebAPI.Web.Startup))]

namespace WebAPI.Web
{
    public partial class Startup
    {
        //public void Configuration(IAppBuilder app)
        //{
        //    ConfigureAuth(app);
        //}

        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var myProvider = new AuthorizationServiceProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Account/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                Provider = myProvider,
                RefreshTokenProvider = new RefreshTokenProvider()
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}
