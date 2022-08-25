namespace Benchmark.Services;

public class GoelandApiService
{
    // region Properties

    private HttpClient Client
    {
        get
        {
            if (_client == null)
            {
                _client = new HttpClient();
            }

            return _client;
        }
    }
    
    // endregion
    
    // region Backing Fields

    private HttpClient _client;
    
    // endregion
}