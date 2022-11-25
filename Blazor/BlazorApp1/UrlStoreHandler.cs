using Fluxor.Persist.Middleware;
using Fluxor;
using Fluxor.Persist.Storage;

//public class UrlStoreHandler : IStoreHandler
//{
//    private readonly IObjectStateStorage _objectStateStorage;
//    private readonly ILogger<PersistMiddleware> _logger;
//    public UrlStoreHandler(IObjectStateStorage objectStateStorage, 
//        ILogger<PersistMiddleware> logger)
//    {
//        _objectStateStorage = objectStateStorage;
//        _logger = logger;
//    }

//    public async Task<object> GetState(IFeature feature)
//    {
//        return feature.GetState();
//    }

//    public async Task SetState(IFeature feature)
//    {
//        object state = feature.GetState();
//        //await _objectStateStorage.StoreStateAsync( feature.GetName(),feature);
//    }
//}

//public class UrlStateStorage : IObjectStateStorage
//{
//    public ValueTask<object> GetStateAsync(string statename)
//    {
//        throw new NotImplementedException();
//    }

//    public ValueTask StoreStateAsync(string statename, object state)
//    {
//        throw new NotImplementedException();
//    }
//}
