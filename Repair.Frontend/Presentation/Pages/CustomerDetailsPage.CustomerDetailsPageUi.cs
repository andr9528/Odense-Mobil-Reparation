using Microsoft.Extensions.DependencyInjection;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class CustomerDetailsPage
{
    private sealed class CustomerDetailsPageUi(CustomerDetailsPageLogic logic, CustomerDetailsPageViewModel viewModel)
        : BaseUi<CustomerDetailsPageLogic, CustomerDetailsPageViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(0);
            grid.Padding = new Thickness(10);
            grid.DefineRows(GridLength.Auto, GridLength.Auto, new GridLength(1, GridUnitType.Star));
            grid.RowSpacing = 10;
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateCustomerEditor().SetRow(1));
            grid.Children.Add(CreateOrderGrid().SetRow(2));
        }

        private UIElement CreateHeader()
        {
            Grid header = GridFactory.CreateDefaultGrid().DefineColumns(new GridLength(1, GridUnitType.Star),
                GridLength.Auto, GridLength.Auto, GridLength.Auto);

            header.ColumnSpacing = 8;

            header.Children.Add(CreateHeaderTextBlock().SetColumn(0, 4));
            header.Children.Add(CreateEditCheckBoxGrid().SetColumn(1));
            header.Children.Add(CreateSaveButton().SetColumn(2));
            header.Children.Add(CreateCancelButton().SetColumn(3));

            return header;
        }

        private TextBlock CreateHeaderTextBlock()
        {
            return TextBlockFactory.CreateHeader("Customer details");
        }

        private Grid CreateEditCheckBoxGrid()
        {
            Grid grid = GridFactory.CreateDefaultGrid().DefineColumns(GridLength.Auto, GridLength.Auto);

            TextBlock label = TextBlockFactory.CreateBlackText("Edit");
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Margin = new Thickness(0, 0, 8, 0);

            CheckBox checkBox = CreateEditCheckBox();

            grid.Children.Add(label.SetColumn(0));
            grid.Children.Add(checkBox.SetColumn(1));

            return grid;
        }

        private CheckBox CreateEditCheckBox()
        {
            CheckBox checkBox = CheckBoxFactory.CreateLightCheckBox(nameof(CustomerDetailsPageViewModel.IsEditing));

            checkBox.VerticalAlignment = VerticalAlignment.Center;
            checkBox.HorizontalAlignment = HorizontalAlignment.Left;

            checkBox.Checked += Logic.EditCheckBoxChanged;
            checkBox.Unchecked += Logic.EditCheckBoxChanged;

            ViewModel.EditCheckBox = checkBox;

            return checkBox;
        }

        private Button CreateSaveButton()
        {
            ViewModel.SaveButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(20, 8, 20, 8),
            };

            ViewModel.SaveButton.SetBinding(ContentControl.ContentProperty, new Binding
            {
                Path = new PropertyPath(nameof(CustomerDetailsPageViewModel.SaveButtonText)),
                Mode = BindingMode.OneWay,
            });

            ViewModel.SaveButton.Click += async (sender, args) => await Logic.SaveClicked(sender, args);

            return ViewModel.SaveButton;
        }

        private Button CreateCancelButton()
        {
            ViewModel.CancelButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(20, 8, 20, 8),
            };

            ViewModel.CancelButton.SetBinding(ContentControl.ContentProperty, new Binding
            {
                Path = new PropertyPath(nameof(CustomerDetailsPageViewModel.CancelButtonText)),
                Mode = BindingMode.OneWay,
            });

            ViewModel.CancelButton.Click += Logic.CancelClicked;

            return ViewModel.CancelButton;
        }

        private CustomerEditor CreateCustomerEditor()
        {
            ArgumentsFactory argumentsFactory = GetArgumentsFactory();

            ViewModel.CustomerEditor = new CustomerEditor(argumentsFactory.CreateCustomerEditorArguments());

            Logic.RegisterCustomerEditorEvents();

            return ViewModel.CustomerEditor;
        }

        private OrderGrid CreateOrderGrid()
        {
            ArgumentsFactory argumentsFactory = GetArgumentsFactory();

            ViewModel.OrderGrid = new OrderGrid(
                argumentsFactory.CreateOrderGridArguments(ViewModel.Arguments.CustomerId));

            ViewModel.OrderGrid.ViewModel.DataGrid.SelectionChanged += Logic.OrderClicked;

            return ViewModel.OrderGrid;
        }

        private ArgumentsFactory GetArgumentsFactory()
        {
            return App.Startup.ServiceProvider.GetRequiredService<ArgumentsFactory>();
        }
    }
}
