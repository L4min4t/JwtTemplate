using App.Options;
using Microsoft.Extensions.Options;

namespace App.OptionsSetup;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private readonly IConfiguration _configuration;
    private readonly string SectionName = "Jwt";
    
    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(SectionName)
            .Bind(options);
    }
}
