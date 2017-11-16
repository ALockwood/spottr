using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace spottr
{
    public class LocationsViewModel : BaseViewModel
    {
        public ObservableCollection<LocationPin> Locations { get; set; }
        public Command LoadLocationsCommand { get; set; }
        public Command AddLocationsCommand { get; set; }

        public LocationsViewModel()
        {
            Title = "Browse";
            Locations = new ObservableCollection<LocationPin>();
            LoadLocationsCommand = new Command(async () => await ExecuteLoadLocationsCommand());
            AddLocationsCommand = new Command<LocationPin>(async (LocationPin location) => await AddLocation(location));
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

        async Task AddLocation(LocationPin location)
        {
            Locations.Add(location);
            await DataStore.AddLocationAsync(location);
        }
    }
}
