using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace spottr.Droid
{
    [Activity(Label = "Details", ParentActivity = typeof(MainActivity))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = ".MainActivity")]
    public class BrowseItemDetailActivity : BaseActivity
    {
        /// <summary>
        /// Specify the layout to inflace
        /// </summary>
        protected override int LayoutResource => Resource.Layout.activity_item_details;

        LoctionDetailViewModel viewModel;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var data = Intent.GetStringExtra("data");

            var location = Newtonsoft.Json.JsonConvert.DeserializeObject<Location>(data);
            viewModel = new LoctionDetailViewModel(location);

            FindViewById<TextView>(Resource.Id.description).Text = location.Description;
            FindViewById<TextView>(Resource.Id.lat_long).Text = location.LocationPoint;
            FindViewById<TextView>(Resource.Id.date_updated).Text = location.LastUpdated;

            SupportActionBar.Title = location.Name;
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
