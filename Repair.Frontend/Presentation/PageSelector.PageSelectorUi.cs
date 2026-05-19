using Repair.Frontend.Abstraction;
using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Core;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Presentation
{
    internal sealed partial class PageSelector
    {
        private sealed class PageSelectorUi : BaseUi<PageSelectorLogic, PageSelectorViewModel>
        {
            private const double PANE_COLUMN_WEIGHT = 12d;

            private readonly IEnumerable<IPageRegion> regionDefinitions;
            private readonly INavigationService navigationService;

            public PageSelectorUi(
                PageSelectorLogic logic, PageSelectorViewModel viewModel, IEnumerable<IPageRegion> regionDefinitions,
                INavigationService navigationService) : base(logic, viewModel)
            {
                this.regionDefinitions = regionDefinitions;
                this.navigationService = navigationService;
            }

            protected override void ConfigureGrid(Grid grid)
            {
                CreateFrames();

                grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                grid.VerticalAlignment = VerticalAlignment.Stretch;
                grid.Margin = new Thickness(0);
                grid.Padding = new Thickness(0);

                const double contentColumnWeight = 100 - PANE_COLUMN_WEIGHT;

                grid.ColumnDefinitions.Add(new ColumnDefinition
                    {Width = new GridLength(PANE_COLUMN_WEIGHT, GridUnitType.Star),});
                grid.ColumnDefinitions.Add(new ColumnDefinition
                    {Width = new GridLength(contentColumnWeight, GridUnitType.Star),});
            }

            protected override void AddControlsToGrid(Grid grid)
            {
                grid.Children.Add(ViewModel.PaneFrame.SetColumn(0));
                grid.Children.Add(ViewModel.ContentFrame.SetColumn(1));
            }

            private void CreateFrames()
            {
                ViewModel.Regions = regionDefinitions.ToList();
                ViewModel.MenuList = CreateMenuList(ViewModel.Regions);

                ViewModel.ContentFrame = new Frame();
                navigationService.RegisterContentFrame(ViewModel.ContentFrame);

                ViewModel.PaneFrame = new Frame
                {
                    Content = CreateNavigationPaneContentGrid(),
                };
            }

            private Grid CreateNavigationPaneContentGrid()
            {
                Grid paneRoot = GridFactory.CreateDefaultGrid()
                    .DefineRows(GridLength.Auto, new GridLength(1, GridUnitType.Star));

                paneRoot.Background = new SolidColorBrush(Color.FromArgb(255, 32, 32, 32));
                paneRoot.HorizontalAlignment = HorizontalAlignment.Stretch;
                paneRoot.VerticalAlignment = VerticalAlignment.Stretch;
                paneRoot.Margin = new Thickness(0);
                paneRoot.Padding = new Thickness(0);

                Button backButton = CreateBackButton();

                paneRoot.Children.Add(backButton.SetRow(0));
                paneRoot.Children.Add(ViewModel.MenuList.SetRow(1));

                return paneRoot;
            }

            private Button CreateBackButton()
            {
                var button = new Button
                {
                    // TODO: Find / Make better content.
                    Content = "←",
                    Background = new SolidColorBrush(Colors.Transparent),
                    Foreground = new SolidColorBrush(Colors.White),
                    BorderThickness = new Thickness(0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(5),
                    Padding = new Thickness(10, 5, 10, 5),
                };

                button.Click += Logic.BackButtonClicked;

                return button;
            }

            private ListView CreateMenuList(IEnumerable<IPageRegion> regions)
            {
                var menuList = new ListView
                {
                    Background = new SolidColorBrush(Colors.Transparent),
                    SelectionMode = ListViewSelectionMode.Single,
                    ItemsSource = regions,
                    ItemTemplate = CreateMenuItemTemplate(),
                    ItemContainerStyle = CreateMenuItemStyle(),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(5),
                };

                menuList.SelectionChanged += Logic.MenuListSelectionChanged;

                return menuList;
            }

            private DataTemplate CreateMenuItemTemplate()
            {
                return new DataTemplate(() =>
                {
                    Grid templateGrid = GridFactory.CreateDefaultGrid();
                    templateGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    templateGrid.Margin = new Thickness(0);

                    templateGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Auto,
                    });
                    templateGrid.ColumnDefinitions.Add(new ColumnDefinition
                        {Width = new GridLength(1, GridUnitType.Star),});

                    var iconPresenter = new ContentPresenter
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    iconPresenter.SetBinding(ContentPresenter.ContentProperty,
                        new Binding {Path = new PropertyPath(nameof(IPageRegion.Icon)),});

                    var text = new TextBlock
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Margin = new Thickness(5, 0, 0, 0),
                        TextWrapping = TextWrapping.NoWrap,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                    };
                    text.SetBinding(TextBlock.TextProperty,
                        new Binding {Path = new PropertyPath(nameof(IPageRegion.DisplayName)),});

                    templateGrid.Children.Add(iconPresenter.SetColumn(0));
                    templateGrid.Children.Add(text.SetColumn(1));

                    return templateGrid;
                });
            }

            private Style CreateMenuItemStyle()
            {
                var style = new Style(typeof(ListViewItem));
                style.Setters.Add(new Setter(BackgroundProperty, new SolidColorBrush(Colors.Transparent)));
                style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));
                style.Setters.Add(new Setter(PaddingProperty, new Thickness(4, 2, 4, 2)));
                style.Setters.Add(new Setter(ForegroundProperty, new SolidColorBrush(Colors.White)));
                style.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));
                return style;
            }
        }
    }
}
