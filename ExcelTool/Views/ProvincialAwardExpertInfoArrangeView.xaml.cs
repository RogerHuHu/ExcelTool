using System.Windows.Controls;

namespace ExcelTool.Views
{
    /// <summary>
    /// ProvincialAwardExpertInfoArrangeView.xaml 的交互逻辑
    /// </summary>
    public partial class ProvincialAwardExpertInfoArrangeView : UserControl
    {
        public ProvincialAwardExpertInfoArrangeView()
        {
            InitializeComponent();
            DataContext = new ViewModels.ProvincialAwardExpertInfoArrangeViewModel();
        }
    }
}