using System;
using spottr.Services;

namespace spottr
{
    public class App
    {
        public static string BackendUrl = "http://35.185.99.130:90";

        public static void Initialize()
        {
            ServiceLocator.Instance.Register<IDataStore<LocationPin>, CloudDataStore>();
        }
    }
}
