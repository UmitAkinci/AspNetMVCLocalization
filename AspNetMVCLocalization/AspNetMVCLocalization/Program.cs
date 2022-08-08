using AspNetMVCLocalization.Resources;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });

builder.Services.AddSingleton<LocalizationService>();
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).
    AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(ApplicationResource).GetTypeInfo().Assembly.FullName);
            return factory.Create("ApplicationResource", assemblyName.Name);
        };
    });
builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var cultures = new List<CultureInfo>()
    {
        new CultureInfo("tr-TR"),
        new CultureInfo("en-US"),
        new CultureInfo("ru-RU"),
        new CultureInfo("de-DE")
    };
    opt.DefaultRequestCulture = new RequestCulture(new CultureInfo("tr-TR"));
    opt.SupportedCultures = cultures;
    opt.SupportedUICultures = cultures;

    opt.RequestCultureProviders = new List<IRequestCultureProvider>()
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
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

var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if (options != null)
{
    app.UseRequestLocalization(options.Value);
}

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
