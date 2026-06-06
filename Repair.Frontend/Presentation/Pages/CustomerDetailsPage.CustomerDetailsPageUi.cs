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
            ConfigureDefaultPageGrid(grid);

            grid.DefineRows(GridLength.Auto, GridLength.Auto, new GridLength(1, GridUnitType.Star));
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateButtonsGrid().SetRow(0));
            grid.Children.Add(CreateCustomerEditor().SetRow(1));
            grid.Children.Add(CreateOrdersGrid().SetRow(2));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Customer Details");
        }

        private Grid CreateButtonsGrid()
        {
            Grid grid = GridFactory.CreateDefaultGrid().DefineColumns(GridLength.Auto,
                new GridLength(1, GridUnitType.Star), GridLength.Auto, GridLength.Auto, GridLength.Auto,
                GridLength.Auto);

            grid.ColumnSpacing = 8;

            grid.Children.Add(CreateCreateOrderButton().SetColumn(0));
            grid.Children.Add(CreateDeleteButton().SetColumn(2));
            grid.Children.Add(CreateEditCheckBoxGrid().SetColumn(3));
            grid.Children.Add(CreateSaveButton().SetColumn(4));
            grid.Children.Add(CreateCancelButton().SetColumn(5));

            return grid;
        }

        private Button CreateDeleteButton()
        {
            ViewModel.DeleteButton = new Button
            {
                Content = "Delete",
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(20, 8, 20, 8),
            };

            ViewModel.DeleteButton.SetBinding(Control.IsEnabledProperty, new Binding
            {
                Path = new PropertyPath(nameof(CustomerDetailsPageViewModel.CanDelete)),
                Mode = BindingMode.OneWay,
            });

            ViewModel.DeleteButton.Click += async (sender, args) => await Logic.DeleteClicked(sender, args);

            return ViewModel.DeleteButton;
        }

        private Button CreateCreateOrderButton()
        {
            Button button = new()
            {
                Content = "Create Order",
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(20, 8, 20, 8),
            };

            button.Click += Logic.CreateOrderClicked;

            return button;
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
            ArgumentsFactory argumentsFactory = Logic.GetArgumentsFactory();

            ViewModel.CustomerEditor = new CustomerEditor(argumentsFactory.CreateCustomerEditorArguments());

            Logic.RegisterCustomerEditorEvents();

            return ViewModel.CustomerEditor;
        }

        private OrdersGrid CreateOrdersGrid()
        {
            ArgumentsFactory argumentsFactory = Logic.GetArgumentsFactory();

            ViewModel.OrdersGrid = new OrdersGrid(
                argumentsFactory.CreateOrderGridArguments(ViewModel.Arguments.CustomerId));

            ViewModel.OrdersGrid.ViewModel.DataGrid.SelectionChanged += Logic.OrderClicked;

            return ViewModel.OrdersGrid;
        }
    }
}
