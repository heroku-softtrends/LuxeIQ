using LuxeIQ.Common;
using LuxeIQ.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using LuxeIQ.Repositories;
using NuGet.Protocol.Core.Types;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<LuxeIQContext>(options => options.UseNpgsql(Constants.connectionString, b => b.MigrationsAssembly("LuxeIQ")));


//set form options
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue; //default 1024
    options.ValueLengthLimit = int.MaxValue; //not recommended value
    options.MultipartBodyLengthLimit = long.MaxValue; //not recommended value
    options.KeyLengthLimit = int.MaxValue;
});
//set session default timeout
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//set max file handle size
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = null;
    serverOptions.Limits.MaxRequestBufferSize = null;
    serverOptions.Limits.MaxResponseBufferSize = null;
});

//Add services to the container.
builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    })
    .AddSessionStateTempDataProvider()
    .AddRazorRuntimeCompilation();
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    //consent required
    options.CheckConsentNeeded = context => true;

    //prevent access from javascript 
    options.HttpOnly = HttpOnlyPolicy.Always;

    //If the URI that provides the cookie is HTTPS, 
    //cookie will be sent ONLY for HTTPS requests 
    options.Secure = CookieSecurePolicy.SameAsRequest;

    //refer "SameSite cookies" on website
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

//Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = Constants.DEFAULT_AUTH_COOKIE_SCHEME;
    options.DefaultSignInScheme = Constants.DEFAULT_AUTH_COOKIE_SCHEME;
    options.DefaultSignOutScheme = Constants.DEFAULT_AUTH_COOKIE_SCHEME;
}).AddCookie(Constants.DEFAULT_AUTH_COOKIE_SCHEME, options =>
{
    //add an instance of the patched manager to the options:
    options.CookieManager = new ChunkingCookieManager();

    options.Cookie.Name = Constants.DEFAULT_AUTH_COOKIE_NAME;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.LoginPath = new PathString("/login/herokuauth/");
    options.AccessDeniedPath = new PathString("/home/unauthorized/");
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);

});

//Add framework services.
builder.Services.AddCors();

builder.Services.AddDistributedMemoryCache();
//Add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(5);
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Name = Constants.DEFAULT_AUTH_SESSION_NAME;

});

//Register dependency injections
builder.Services.AddTransient<IManufacturersRepository, ManufacturersRepository>();
builder.Services.AddTransient<IManufacturersTerritoryRepository, ManufacturersTerritoryRepository>();
builder.Services.AddTransient<IWholesalerRepository, WholesalerRepository>();
builder.Services.AddTransient<IWholesalerShowroomRepository, WholesalerShowroomRepository>();
builder.Services.AddTransient<ISalesRepAgencyRepository, SalesRepAgencyRepository>();
builder.Services.AddTransient<IUsersRepository, UserRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IWholesalerHQRepository, WholesalerHQRepository>();
builder.Services.AddTransient<ServiceRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
//app.UseMvc();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
