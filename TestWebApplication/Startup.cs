using Geta.SocialChannels.Instagram;
using Geta.SocialChannels.Twitter;
using Geta.SocialChannels.Twitter.DTO;
using Geta.SocialChannels.YouTube;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestWebApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var accountId = "17841404039638926";
                    var token = "EAAKZBEPN1yy8BAKVu2gLPsekK5QLI1p9fsr1EDIJdZC0K1zT3oicq7rfssMKK2hMLdq9N463r9dcG0nCoZBdciyaF4CGDzUqgCSOXyDpEQXgvx37Ly6Jc04ZB8ZApQ4dQ8f6OqFvNzUgl5AZCNma1u2V1Sj9SUZA0iTQKhvMvwps84rdxXTAh4I5WhCZC1wnZAlsZD";
                    var instaService = new InstagramService(token, accountId);
                    var data = instaService.GetMediaByHashTag("oslo");
                    var data2 = instaService.GetMedia();
                    // var ytKey = "AIzaSyBT84TeMZomv3O7zytjP5zm8LcaqZtE69g";
                    // var ytService = new YoutubeService(ytKey, null);
                    // var data = ytService.GetYoutubeFeed(new GetYoutubeFeedRequest
                    // {
                    //     ChannelId = "UCLm7DMKc2OAPTihoGQwCWmw"
                    // });
                    // var twitterService = new TwitterService("owpccRDErOU9xlbLIvpaSsDRo", "aS2sB31nMtNoqYIy0b0VZINRNrmqdeg2ZX5fUi1TQDOP6uuD8r");
                    // var a = twitterService.GetTweets(new GetTweetsRequest
                    // {
                    //     UserName = "Microsoft"
                    // });
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}