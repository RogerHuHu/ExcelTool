using System.Windows.Controls;

namespace ExcelTool.Views
{
    /// <summary>
    /// ExcelRemoveDuplicateView.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelRemoveInvalidItemView : UserControl
    {
        public ExcelRemoveInvalidItemView()
        {
            InitializeComponent();
            DataContext = new ViewModels.ExcelRemoveInvalidItemViewModel();
        }
    }
}