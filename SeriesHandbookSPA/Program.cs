using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SeriesHandbookSPA.Authentication;
using SeriesHandbookSPA.Network;
using SeriesHandbookSPA.Services;
using SeriesHandbookSPA.StateContainer;
using System;
using System.Threading.Tasks;

namespace SeriesHandbookSPA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);


            var server = new Uri(builder.Configuration["ServerIp"]);

            builder.RootComponents.Add<App>("#app");
            
            builder.Services.AddAntDesign();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddTransient<AuthHeaderHandler>();
            


            builder.Services.AddRefitClient<IWeatherApiService>(new RefitSettings(new NewtonsoftJsonContentSerializer()))
                .ConfigureHttpClient(c => c.BaseAddress = server)
                .AddHttpMessageHandler<AuthHeaderHandler>();


            builder.Services.AddRefitClient<SeriesHandbookApi>(new RefitSettings(new NewtonsoftJsonContentSerializer()))
                .ConfigureHttpClient(c => c.BaseAddress = server)
                .AddHttpMessageHandler<AuthHeaderHandler>();


            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<IWeatherService, WeatherService>();

            builder.Services.AddScoped<ISeriesHandbookService, SeriesHandbookService>();
            builder.Services.AddTransient<SeriesHandbookHandler>();

            builder.Services.AddSingleton<MainStateContainer>();

            

            await builder.Build().RunAsync();
        }
    }
}
