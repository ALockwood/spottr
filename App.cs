using System;

namespace spottr
{
    public class App
    {
        public static string BackendUrl = "http://10.0.2.2:5000";

        public static void Initialize()
        {
            ServiceLocator.Instance.Register<IDataStore<Location>, CloudDataStore>();
        }
    }
}
