using BackPanel.Application.DI;
using BackPanel.Application.Interfaces;
using BackPanel.FilesManager.DI;
using BackPanel.Persistence.Database;
using BackPanel.Persistence.DI;
using BackPanel.SMTP.DI;
using BackPanel.TranslationEditor.DI;
using BackPanel.WebApplication.Extensions;
using BackPanel.WebApplication.implementation;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllersWithViews().AddNewtonsoftJson(opts =>
{
    opts.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
    opts.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'  'HH':'mm':'ss";
});
builder.Services.RegisterDbContext<AppDbContext>(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.RegisterRepositories();
builder.Services.RegisterRequiredApplicationService();
builder.Services.RegisterRequiredSmtpServices();
builder.Services.RegisterRequiredFilesManagerServices();
builder.Services.ImplementPathProvider<PathProvider>();
builder.Services.AddScoped<IWebConfiguration, WebConfiguration>();
builder.Services.RegisterJwtConfiguration(builder.Configuration.GetValue<string>("SecretKey:key"));
builder.Services.ImplementPathProviderToTranslationService<PathProvider>();
builder.Services.RegisterRequiredTranslationEditorServices();
builder.Services.ImplementUriService(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext?.Request;
    var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());
    return new UriService(uri);
});
builder.Services.ConfigureSwagger();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        o => o.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
var app = builder.Build();
// load company info 
await app.LoadCompanyInfo();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts. app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.Run();