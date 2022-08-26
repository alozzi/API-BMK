using System.Net.Http.Json;

namespace Benchmark.Services;

public class GoelandApiService
{
    #region Properties

    private HttpClient Client
    {
        get
        {
            if (_client != null) return _client;
            
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Resources.AccessToken);

            return _client;
        }
    }
    
    #endregion
    
    #region Backing Fields

    private HttpClient? _client;
    
    #endregion
    
    #region Public Methods

    public async Task<HttpResponseMessage> PostAsync(string uri, object obj)
    {
        return await Client.PostAsJsonAsync(uri, obj);
    }
    
    #endregion
}