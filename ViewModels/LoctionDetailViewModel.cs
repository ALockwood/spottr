using System;

namespace spottr
{
    public class LoctionDetailViewModel : BaseViewModel
    {
        public Location Location { get; set; }
        public LoctionDetailViewModel(Location location = null)
        {
            if (location != null)
            {
                Title = location.Name;
                Location = location;
            }
        }
    }
}
