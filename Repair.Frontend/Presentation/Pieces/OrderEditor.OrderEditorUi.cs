using Microsoft.UI.Text;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation.Pieces;

internal sealed partial class OrderEditor
{
    internal sealed class OrderEditorUi(OrderEditorLogic logic, OrderEditorViewModel viewModel)
        : BaseUi<OrderEditorLogic, OrderEditorViewModel>(logic, viewModel)
    {
        protected override void ConfigureGrid(Grid grid)
        {
            grid.RowSpacing = 8;
            grid.ColumnSpacing = 8;

            grid.DefineRows(GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto,
                new GridLength(1, GridUnitType.Star));
            grid.DefineColumns(GridUnitType.Star, [1, 1, 1,]);
        }

        protected override void AddControlsToGrid(Grid grid)
        {
            grid.Children.Add(CreateHandInWhatTextBox().SetRow(0).SetColumn(0));
            grid.Children.Add(CreateHandInWhenPicker().SetRow(0).SetColumn(1));
            grid.Children.Add(CreateReturnedWhenPicker().SetRow(0).SetColumn(2));
            grid.Children.Add(CreateBorrowedPhoneTextBox().SetRow(1).SetColumn(0));
            grid.Children.Add(CreateIsOrderCompleteCheckBox().SetRow(2).SetColumn(0));

            grid.Children.Add(CreateRepairWhatTextBox().SetRow(1, 2).SetColumn(1, 2));
            grid.Children.Add(CreateSelectedCustomerTextBlock().SetRow(3).SetColumn(0, 3));
            grid.Children.Add(CreateCustomersGrid().SetRow(4).SetColumn(0, 3));
        }

        private TextBlock CreateSelectedCustomerTextBlock()
        {
            ViewModel.SelectedCustomerTextBlock = new TextBlock
            {
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Colors.Black),
            };

            ViewModel.SelectedCustomerTextBlock.SetBinding(TextBlock.TextProperty, new Binding
            {
                Path = new PropertyPath(nameof(OrderEditorViewModel.SelectedCustomerText)),
                Mode = BindingMode.OneWay,
            });

            return ViewModel.SelectedCustomerTextBlock;
        }

        private DateTimePicker CreateHandInWhenPicker()
        {
            DateTimePicker.DateTimePickerArguments arguments = Logic.GetArgumentsFactory()
                .CreateDateTimePickerArguments("Handed in at?", ViewModel.Arguments.Order?.HandInWhen ?? DateTime.Now);

            ViewModel.HandInWhenPicker = new DateTimePicker(arguments);

            return ViewModel.HandInWhenPicker;
        }

        private DateTimePicker CreateReturnedWhenPicker()
        {
            DateTimePicker.DateTimePickerArguments arguments = Logic.GetArgumentsFactory()
                .CreateDateTimePickerArguments("Returned at?", ViewModel.Arguments.Order?.ReturnedWhen);

            ViewModel.ReturnedWhenPicker = new DateTimePicker(arguments);

            return ViewModel.ReturnedWhenPicker;
        }

        private Grid CreateIsOrderCompleteCheckBox()
        {
            Grid grid = CheckBoxFactory.CreateLightCheckBoxWithLabel("Is order complete?",
                nameof(OrderEditorViewModel.IsOrderComplete), out CheckBox checkBox);

            ViewModel.IsOrderCompleteCheckBox = checkBox;

            return grid;
        }

        private TextBox CreateBorrowedPhoneTextBox()
        {
            ViewModel.BorrowedPhoneTextBox = TextBoxFactory.CreateSearchBox("Borrowed phone?", "Borrowed phone...",
                nameof(OrderEditorViewModel.BorrowedPhone));

            return ViewModel.BorrowedPhoneTextBox;
        }

        private TextBox CreateHandInWhatTextBox()
        {
            ViewModel.HandInWhatTextBox = TextBoxFactory.CreateSearchBox("What was handed in?", "What was handed in...",
                nameof(OrderEditorViewModel.HandInWhat));

            return ViewModel.HandInWhatTextBox;
        }

        private TextBox CreateRepairWhatTextBox()
        {
            ViewModel.RepairWhatTextBox = TextBoxFactory.CreateMultilineTextBox("What is to be repaired?",
                "Describe the repair...", nameof(OrderEditorViewModel.RepairWhat));

            return ViewModel.RepairWhatTextBox;
        }

        private CustomersGrid CreateCustomersGrid()
        {
            CustomersGrid.CustomersGridArguments arguments = Logic.GetArgumentsFactory()
                .CreateCustomersGridArguments(ViewModel.Arguments.Order?.CustomerId ??
                                              ViewModel.Arguments.SelectedCustomerId);

            ViewModel.CustomersGrid = new CustomersGrid(arguments);
            Logic.RegisterCustomerSelectionIndicator();

            return ViewModel.CustomersGrid;
        }
    }
}
