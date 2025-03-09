using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace FastighetsKompassen.API.Azure
{
    public  class AzureKeyVault
    {
        private bool _isInitialized = false;
        private readonly SecretClient _secretClient;
        private string _azureDbSecret;

        public AzureKeyVault(IConfiguration configuration)
        {
            var keyVaultUri = configuration["AzureKeyVault:VaultUri"];
            _azureDbSecret = configuration["AzureKeyVault:DbPassword"];
            _secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        }

        public async Task Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            var secretBundle = await _secretClient.GetSecretAsync(_azureDbSecret);
            _azureDbSecret = secretBundle.Value.Value;
            _isInitialized = true;
        }

        public async Task<string> GetDbSecret()
        {
            await Initialize();
            return _azureDbSecret;
        }
    }
}
