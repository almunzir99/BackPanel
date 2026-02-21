using BackPanel.Application.DI;
using BackPanel.Application.Interfaces;
using BackPanel.FilesManager.DI;
using BackPanel.Persistence.Database;
using BackPanel.Persistence.DI;
using BackPanel.Persistence.Identity;
using BackPanel.SMTP.DI;
using BackPanel.SMTP.Models;
using BackPanel.TranslationEditor.DI;
using BackPanel.WebApplication.Extensions;
using BackPanel.WebApplication.implementation;
using BackPanel.WebApplication.Middlewares;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllersWithViews().AddNewtonsoftJson(opts =>
{
    opts.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
    opts.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'  'HH':'mm";
});
 builder.Services.RegisterDbContext<AppDbContext>(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.RegisterRepositories();
builder.Services.RegisterUnitOfWork();
builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.RegisterRequiredFilesManagerServices();
builder.Services.ImplementPathProvider<PathProvider>();
builder.Services.AddScoped<IWebConfiguration, WebConfiguration>();
builder.Services.AddScoped<IIdentityRoleService, IdentityRoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.RegisterJwtConfiguration(builder.Configuration.GetValue<string>("SecretKey:key")!);
builder.Services.ImplementPathProviderToTranslationService<PathProvider>();
builder.Services.RegisterRequiredTranslationEditorServices();
builder.Services.RegisterApplicationCQRS();
builder.Services.RegisterMiddlewares();
builder.Services.RegisterResolvers(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext?.Request;
    var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());
    return new UriResolver(uri);
});
builder.Services.ConfigureSwagger();
builder.Services.RegisterRequiredSmtpServices(
    new SmtpConfigurationModel()
    {
        Port = builder.Configuration.GetValue<int>("Smtp:port"),
        Host = builder.Configuration.GetValue<string>("Smtp:host"),
        Username = builder.Configuration.GetValue<string>("Smtp:username"),
        Password = builder.Configuration.GetValue<string>("Smtp:password"),
    }

);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        o => o.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
// Configure Serilog 
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
var app = builder.Build();
// load company info 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts. app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");


app.MapFallbackToFile("index.html");
app.Run();
