using System.Collections.ObjectModel;
using Repair.Models.Entity.Model;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderGrid
{
    internal sealed partial class OrderGridViewModel : ObservableObject
    {
        public OrderGridViewModel(int customerId = 0)
        {
            CustomerId = customerId;
        }

        public int CustomerId { get; }

        public ObservableCollection<Order> Orders { get; } = [];

        [ObservableProperty] private string handInWhatSearchText = string.Empty;

        [ObservableProperty] private string repairWhatSearchText = string.Empty;

        [ObservableProperty] private bool useFuzzySearch;
    }
}
