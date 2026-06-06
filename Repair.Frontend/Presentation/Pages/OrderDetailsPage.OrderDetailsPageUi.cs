using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;
using Repair.Frontend.Presentation.Pieces;
using Repair.Frontend.Services;

namespace Repair.Frontend.Presentation.Pages;

internal sealed partial class OrderDetailsPage
{
    private sealed class OrderDetailsPageUi(OrderDetailsPageLogic logic, OrderDetailsPageViewModel viewModel)
        : BaseUi<OrderDetailsPageLogic, OrderDetailsPageViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Margin = new Thickness(0);
            grid.Padding = new Thickness(10);
            grid.DefineRows(GridLength.Auto, new GridLength(1, GridUnitType.Star));
            grid.RowSpacing = 10;
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHeader().SetRow(0));
            grid.Children.Add(CreateButtonsGrid().SetRow(0));
            grid.Children.Add(CreateOrderEditor().SetRow(1));
        }

        private UIElement CreateHeader()
        {
            return TextBlockFactory.CreateHeader("Order Details");
        }

        private Grid CreateButtonsGrid()
        {
            Grid grid = GridFactory.CreateDefaultGrid().DefineColumns(GridLength.Auto,
                new GridLength(1, GridUnitType.Star), GridLength.Auto, GridLength.Auto, GridLength.Auto);

            grid.ColumnSpacing = 8;

            grid.Children.Add(CreatePrintButton().SetColumn(0));
            grid.Children.Add(CreateEditCheckBoxGrid().SetColumn(2));
            grid.Children.Add(CreateSaveButton().SetColumn(3));
            grid.Children.Add(CreateCancelButton().SetColumn(4));

            return grid;
        }

        private Button CreatePrintButton()
        {
            ViewModel.PrintButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 120,
                Padding = new Thickness(20, 8, 20, 8),
            };

            ViewModel.PrintButton.SetBinding(ContentControl.ContentProperty, new Binding
            {
                Path = new PropertyPath(nameof(OrderDetailsPageViewModel.PrintButtonText)),
                Mode = BindingMode.OneWay,
            });

            ViewModel.PrintButton.Click += Logic.PrintClicked;

            return ViewModel.PrintButton;
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
            CheckBox checkBox = CheckBoxFactory.CreateLightCheckBox(nameof(OrderDetailsPageViewModel.IsEditing));

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
                Path = new PropertyPath(nameof(OrderDetailsPageViewModel.SaveButtonText)),
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
                Path = new PropertyPath(nameof(OrderDetailsPageViewModel.CancelButtonText)),
                Mode = BindingMode.OneWay,
            });

            ViewModel.CancelButton.Click += Logic.CancelClicked;

            return ViewModel.CancelButton;
        }

        private OrderEditor CreateOrderEditor()
        {
            ArgumentsFactory argumentsFactory = Logic.GetArgumentsFactory();

            ViewModel.OrderEditor = new OrderEditor(argumentsFactory.CreateOrderEditorArguments());

            Logic.RegisterOrderEditorEvents();

            return ViewModel.OrderEditor;
        }
    }
}
