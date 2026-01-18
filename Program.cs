using Mobilis_Real_Time_Assistant.Hubs;
using Mobilis_Real_Time_Assistant.Service;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSignalR();
builder.Services.AddHttpClient<ApiIntegration>(); // registering http service for api fetching
builder.Services.AddSingleton<ApiIntegration>();
builder.Services.AddSingleton<IContextCacheService, ContextCacheService>();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}"); // setting up port for render

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapHub<ChatPipe>("/chatHub");

app.Run();
