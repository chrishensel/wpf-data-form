using System.Windows;
using Wpf.DataForm.Sample.ViewModels;

namespace Wpf.DataForm.Sample
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;

            dfc.XmlLinkRegistry.RegisterXmlLinkFile("my", "Files\\MyLink.xml");
            dfc.XmlLinkRegistry.RegisterXmlLinkFile("myerroneous", "Files\\MyErroneousLink.xml");
            dfc.XmlLinkRegistry.RegisterXmlLinkFile("myrec", "Files\\MyRecursiveLink.xml");
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            dfc.Validate();

            if (dfc.DataFormObject.IsModified)
            {
                if (MessageBox.Show("The data form has modified content. Do you want to quit?", "Data modified", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
