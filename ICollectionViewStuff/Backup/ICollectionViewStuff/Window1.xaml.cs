using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace ICollectionViewStuff
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        ViewModel viewModel;

        public Window1()
        {
            viewModel = new ViewModel(new SampleModel());
            DataContext = viewModel;
            InitializeComponent();

            //Command bindings for the navigation ( all handlers are using lambdas to simplify the syntax of hooking the eventhandler and delegate the work to the ViewModel
            CommandBindings.Add(new CommandBinding(
                Commands.SelectFirst,
                (sender, e) => viewModel.SelectFirstItem(),
                (sender, e) => e.CanExecute = viewModel.CanSelectFirstItem()
                ));

            CommandBindings.Add(new CommandBinding(
                Commands.SelectLast,
                (sender, e) => viewModel.SelectLastItem(),
                (sender, e) => e.CanExecute = viewModel.CanSelectLastItem()
                ));

            CommandBindings.Add(new CommandBinding(
                Commands.SelectNext,
                (sender, e) => viewModel.SelectNextItem(),
                (sender, e) => e.CanExecute = viewModel.CanSelectNextItem()
                ));

            CommandBindings.Add(new CommandBinding(
                Commands.SelectPrevious,
                (sender, e) => viewModel.SelectPreviousItem(),
                (sender, e) => e.CanExecute = viewModel.CanSelectPreviousItem()
                ));
        }
    }
}
