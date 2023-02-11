using System.Windows.Controls;

namespace ExcelTool.Views
{
    /// <summary>
    /// ExcelMergeView.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelMergeView : UserControl
    {
        public ExcelMergeView()
        {
            InitializeComponent();
            DataContext = new ViewModels.ExcelMergeViewModel();
        }
    }
}