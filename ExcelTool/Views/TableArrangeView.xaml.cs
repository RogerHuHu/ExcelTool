using System.Windows.Controls;

namespace ExcelTool.Views
{
    /// <summary>
    /// TableArrangeView.xaml 的交互逻辑
    /// </summary>
    public partial class TableArrangeView : UserControl
    {
        public TableArrangeView()
        {
            InitializeComponent();
            DataContext = new ViewModels.TableArrangeViewModel();
        }
    }
}