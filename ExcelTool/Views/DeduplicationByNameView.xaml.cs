using System.Windows.Controls;

namespace ExcelTool.Views
{
    /// <summary>
    /// DeduplicationByNameView.xaml 的交互逻辑
    /// </summary>
    public partial class DeduplicationByNameView : UserControl
    {
        public DeduplicationByNameView()
        {
            InitializeComponent();
            DataContext = new ViewModels.DeduplicationByNameViewModel();
        }
    }
}