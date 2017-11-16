using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Support.Design.Widget;

namespace spottr.Droid
{
    [Activity(Label = "AddItemActivity")]
    public class AddItemActivity : Activity
    {
        FloatingActionButton saveButton;
        EditText title, description;

        public LocationsViewModel ViewModel { get; set; }

        Services.GpsApiClient gps;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = BrowseFragment.ViewModel;

            // Create your application here
            SetContentView(Resource.Layout.activity_add_item);
            saveButton = FindViewById<FloatingActionButton>(Resource.Id.save_button);
            title = FindViewById<EditText>(Resource.Id.txtTitle);
            description = FindViewById<EditText>(Resource.Id.txtDesc);

            saveButton.Click += SaveButton_Click;

            gps = new Services.GpsApiClient(this, true);
            gps.LocationUpdated += HandleCustomEvent;
        }

        void SaveButton_Click(object sender, EventArgs e)
        {
            var location = new LocationPin
            {
                Name = title.Text,
                Description = description.Text,
                Latitude = gps.Latitude,
                Longitude = gps.Longitude
            };
            ViewModel.AddLocationsCommand.Execute(location);

            Finish();
        }

        void HandleCustomEvent(object sender, EventArgs a)
        {
            FindViewById<TextView>(Resource.Id.add_lat).Text = $"{gps.Latitude}";
            FindViewById<TextView>(Resource.Id.add_long).Text = $"{gps.Longitude}";
        }
    }
}
