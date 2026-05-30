using CommunityToolkit.Mvvm.ComponentModel;
using Repair.Models.Entity.Model;
using System.Collections.ObjectModel;
using Repair.Frontend.Presentation.Pieces;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomersPage
{
    internal sealed partial class CustomersPageViewModel(CustomersPageArguments arguments) : ObservableObject
    {
        public CustomersPageArguments Arguments { get; } = arguments;

        public CustomerGrid CustomersGrid { get; set; } = null!;
    }
}
