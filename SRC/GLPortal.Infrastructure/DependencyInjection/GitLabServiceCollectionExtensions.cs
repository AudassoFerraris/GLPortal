namespace GLPortal.Infrastructure.DependencyInjection;

using GLPortal.Core.Settings;
using GLPortal.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

public static class GitLabServiceCollectionExtensions
{
    public static void AddGitLabServices(this IHostApplicationBuilder builder)
    {
        // Configura i settings di GitLab
        builder.Services.Configure<GitLabSettings>(builder.Configuration.GetSection(GitLabSettings.DefaultSectionName));

        // Configura HttpClient per GitLabService con base URL e token
        builder.Services.AddHttpClient<IGitLabService, GitLabService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<GitLabSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
            client.DefaultRequestHeaders.Add("Private-Token", settings.Token);
        });
    }
}
