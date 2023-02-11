using System.Windows.Controls;

namespace ExcelTool.Views
{
    /// <summary>
    /// ExcelScreeningView.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelScreeningView : UserControl
    {
        public ExcelScreeningView()
        {
            InitializeComponent();
            DataContext = new ViewModels.ExcelScreeningViewModel();
        }
    }
}