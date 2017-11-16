using System;
using Android.Gms.Common;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Support.V7.App;

namespace spottr.Services
{
    public class GpsApiClient : AppCompatActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    {
        const string CLIENT_LOG_NAME = "GpsApiClient";

        public bool IsGooglePlayServicesInstalled { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public event EventHandler LocationUpdated;

        GoogleApiClient _apiClient;
        LocationRequest _locRequest;
        bool _useInterval;
        bool _intervalConfigured;

        public GpsApiClient(Android.Content.Context context, bool useInterval) 
        {
            _useInterval = useInterval;
            CheckSetGooglePlayServicesInstalled(context);

            if (IsGooglePlayServicesInstalled)
            {
                // pass in the Context, ConnectionListener and ConnectionFailedListener
                _apiClient = new GoogleApiClient.Builder(context, this, this).AddApi(LocationServices.API).Build();
                _locRequest = new LocationRequest();

                if (useInterval)
                {
                    _locRequest.SetPriority(100);
                    _locRequest.SetFastestInterval(500);
                    _locRequest.SetInterval(1000);

                    Log.Debug("GpsApiClient", "Request priority set to status code {0}, interval set to {1} ms", _locRequest.Priority.ToString(), _locRequest.Interval.ToString());
                }
                _apiClient.Connect();
            }
            else
            {
                Log.Error(CLIENT_LOG_NAME, "Google Play Services is not installed");
            }
        }

        void CheckSetGooglePlayServicesInstalled(Android.Content.Context context)
        {
            IsGooglePlayServicesInstalled = false;

            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
            if (queryResult == ConnectionResult.Success)
            {
                IsGooglePlayServicesInstalled = true;
                Log.Info(CLIENT_LOG_NAME, "Google Play Services is installed on this device.");
            }
            else
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
                {
                    string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                    Log.Error(CLIENT_LOG_NAME, $"There is a problem with Google Play Services on this device: {queryResult} - {errorString}");
                }
            }
        }

        public void OnConnected(Bundle connectionHint)
        {
            // This method is called when we connect to the LocationClient. We can start location updated directly form
            // here if desired, or we can do it in a lifecycle method, as shown above 

            // You must implement this to implement the IGooglePlayServicesClientConnectionCallbacks Interface
            Log.Info(CLIENT_LOG_NAME, "Now connected to client");

            if (_apiClient.IsConnected)
            {
                if (_useInterval && !_intervalConfigured)
                {
                    _intervalConfigured = true;
                    Log.Info(CLIENT_LOG_NAME, "Setting up interval");
                    LocationServices.FusedLocationApi.RequestLocationUpdates(_apiClient, _locRequest, this);
                }

                Log.Info(CLIENT_LOG_NAME, "Getting Current Location");
                Location location = LocationServices.FusedLocationApi.GetLastLocation(_apiClient);
                LogLocation(location);
            }
            else
            {
                Log.Info(CLIENT_LOG_NAME, "Please wait for client to connect");
            }
        }

        public void OnDisconnected()
        {
            // This method is called when we disconnect from the LocationClient.

            // You must implement this to implement the IGooglePlayServicesClientConnectionCallbacks Interface
            Log.Info(CLIENT_LOG_NAME, "Now disconnected from client");
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            // This method is used to handle connection issues with the Google Play Services Client (LocationClient). 
            // You can check if the connection has a resolution (bundle.HasResolution) and attempt to resolve it

            // You must implement this to implement the IGooglePlayServicesClientOnConnectionFailedListener Interface
            Log.Info(CLIENT_LOG_NAME, "Connection failed, attempting to reach google play services");
        }

        public void OnConnectionSuspended(int cause)
        {
            Log.Info(CLIENT_LOG_NAME, "Connection suspended.");
        }

        public void OnLocationChanged(Location location)
        {
            // This method returns changes in the user's location if they've been requested

            // You must implement this to implement the Android.Gms.Locations.ILocationListener Interface
            Log.Debug(CLIENT_LOG_NAME, "Location updated");
            LogLocation(location);
        }

        void LogLocation(Location location)
        {
            if (location != null)
            {
                Log.Info(CLIENT_LOG_NAME, $"Latitude: {location.Latitude}");
                Log.Info(CLIENT_LOG_NAME, $"Longitude: {location.Longitude}");
                Log.Info(CLIENT_LOG_NAME, $"Provider: {location.Provider}");

                Latitude = location.Latitude;
                Longitude = location.Longitude;

                LocationUpdated?.Invoke(this, null);
            }
        }
    }
}
