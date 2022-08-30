using Azure.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace OrderService.Security
{
    public class AuthenticateViaAzureManagedIdentity : DelegatingHandler
    {
        private readonly string CACHE_KEY = nameof(AuthenticateViaAzureManagedIdentity);
        private readonly IConfiguration configuration;
        private readonly IMemoryCache cache;
        private readonly ILogger<AuthenticateViaAzureManagedIdentity> logger;

        public AuthenticateViaAzureManagedIdentity(IConfiguration configuration, IMemoryCache cache, ILogger<AuthenticateViaAzureManagedIdentity> logger)
        {
            this.configuration = configuration;
            this.cache = cache;
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var audience = configuration["Audience"];
            logger.LogTrace("Configured autidence is {audience}", audience);
            var token = await cache.GetOrCreateAsync(CACHE_KEY, async ce =>
            {
                var cred = new DefaultAzureCredential(new DefaultAzureCredentialOptions
                { 
                    ExcludeAzureCliCredential = true,
                    ExcludeEnvironmentCredential = true,
                    ExcludeAzurePowerShellCredential = true,
                    ExcludeInteractiveBrowserCredential = true,
                    ExcludeSharedTokenCacheCredential = true,
#if DEBUG
                    ExcludeManagedIdentityCredential = true,
                    ExcludeVisualStudioCodeCredential = false,
                    ExcludeVisualStudioCredential = false
#else
                    ExcludeManagedIdentityCredential = true,
                    ExcludeVisualStudioCodeCredential = true,
                    ExcludeVisualStudioCredential = false
#endif
                });
                var tokenResult = await cred.GetTokenAsync(new Azure.Core.TokenRequestContext(new string[] { $"{audience}/.default" }, claims: "roles"));
                logger.LogTrace("new access token for api to api communication has been fetched expires in {expiration}", tokenResult.ExpiresOn);
                ce.AbsoluteExpiration = tokenResult.ExpiresOn.AddMinutes(-5);
                return tokenResult.Token;
            });

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
