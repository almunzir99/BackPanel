using BackPanel.Application.Interfaces;

namespace BackPanel.WebApplication.implementation;

public class WebConfiguration : IWebConfiguration
{
    private readonly IConfiguration _configuration;

    public WebConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetSecretKey()
    {
        return _configuration.GetValue<string>("SecretKey:key");
    }
}