using System;

namespace spottr
{
    public class LoctionDetailViewModel : BaseViewModel
    {
        public LocationPin Location { get; set; }
        public LoctionDetailViewModel(LocationPin location = null)
        {
            if (location != null)
            {
                Title = location.Name;
                Location = location;
            }
        }
    }
}
