using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation.Core.Details;

internal abstract class BaseDetailsPageUi<TLogic, TViewModel>(TLogic logic, TViewModel viewModel)
    : BaseUi<TLogic, TViewModel>(logic, viewModel) where TLogic : BaseDetailsPageLogic<TViewModel>
    where TViewModel : BaseDetailsPageViewModel
{
    protected Grid CreateDetailsButtonsGrid(UIElement leftButton)
    {
        Grid grid = GridFactory.CreateDefaultGrid().DefineColumns(GridLength.Auto, new GridLength(1, GridUnitType.Star),
            GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto);

        grid.ColumnSpacing = 8;

        grid.Children.Add(leftButton.SetColumn(0));
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
            Path = new PropertyPath(nameof(Core.BaseDetailsPageViewModel.CanDelete)),
            Mode = BindingMode.OneWay,
        });

        ViewModel.DeleteButton.Click += async (sender, args) => await Logic.DeleteClicked(sender, args);

        return ViewModel.DeleteButton;
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
        CheckBox checkBox = CheckBoxFactory.CreateLightCheckBox(nameof(Core.BaseDetailsPageViewModel.IsEditing));

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
            Path = new PropertyPath(nameof(Core.BaseDetailsPageViewModel.SaveButtonText)),
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
            Path = new PropertyPath(nameof(Core.BaseDetailsPageViewModel.CancelButtonText)),
            Mode = BindingMode.OneWay,
        });

        ViewModel.CancelButton.Click += Logic.CancelClicked;

        return ViewModel.CancelButton;
    }
}
