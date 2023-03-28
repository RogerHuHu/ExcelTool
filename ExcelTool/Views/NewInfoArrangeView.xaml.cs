using System.Windows.Controls;

namespace ExcelTool.Views
{
    /// <summary>
    /// NewInfoArrangeView.xaml 的交互逻辑
    /// </summary>
    public partial class NewInfoArrangeView : UserControl
    {
        public NewInfoArrangeView()
        {
            InitializeComponent();
            DataContext = new ViewModels.NewInfoArrangeViewModel();
        }
    }
}