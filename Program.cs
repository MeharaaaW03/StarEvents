using Microsoft.Extensions.Options;
using StarEvents.Models;
using StarEvents.Services; // ADD THIS LINE

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ADD THESE LINES FOR MONGODB ↓
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSingleton<MongoDBContext>();
// ADD THESE LINES FOR MONGODB ↑

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // This line should be here instead of MapStaticAssets()
app.UseRouting();

app.UseAuthorization();

// app.MapStaticAssets(); // REMOVE THIS LINE - UseStaticFiles() is above

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// .WithStaticAssets(); // REMOVE THIS LINE

app.Run();