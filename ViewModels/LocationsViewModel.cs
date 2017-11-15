using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace spottr
{
    public class LocationsViewModel : BaseViewModel
    {
        public ObservableCollection<Location> Locations { get; set; }
        public Command LoadLocationsCommand { get; set; }
        public Command AddLocationsCommand { get; set; }

        public LocationsViewModel()
        {
            Title = "Browse";
            Locations = new ObservableCollection<Location>();
            LoadLocationsCommand = new Command(async () => await ExecuteLoadLocationsCommand());
            AddLocationsCommand = new Command<Location>(async (Location location) => await AddLocation(location));
        }

        async Task ExecuteLoadLocationsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Locations.Clear();
                var locations = await DataStore.GetLocationsAsync(true);
                foreach (var location in locations)
                {
                    Locations.Add(location);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task AddLocation(Location location)
        {
            Locations.Add(location);
            await DataStore.AddLocationAsync(location);
        }
    }
}
