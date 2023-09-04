using CostumerBase.Presentation.Mvc.Interfaces;
using CostumerBase.Presentation.Mvc.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddHttpClient<ICustomerBaseService, CustomerBaseService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7108/"); // Substitua pela URL real da sua API.
//});

builder.Services.AddHttpClient<ICustomerBaseService, CustomerBaseService>();


builder.Services.AddScoped<ICustomerBaseService, CustomerBaseService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=Index}/{id?}");
    


app.Run();
