using System;

namespace spottr
{
    public class App
    {
        public static bool UseMockDataStore = false;
        public static string BackendUrl = "http://10.0.2.2:5000";

        public static void Initialize()
        {
            if (UseMockDataStore)
                ServiceLocator.Instance.Register<IDataStore<Item>, MockDataStore>();
            else
                ServiceLocator.Instance.Register<IDataStore<Item>, CloudDataStore>();
        }
    }
}
