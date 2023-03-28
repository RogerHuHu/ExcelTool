using ExcelTool.Views;
using HandyControl.Controls;
using HIIUtils.Commands;
using HIIUtils.MVVM;

namespace ExcelTool
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region 变量

        private int buttonWidth = 100;

        #endregion 变量

        #region 构造函数

        public MainWindowViewModel()
        {
            Init();
        }

        #endregion 构造函数

        #region 属性

        public int ButtonWidth
        {
            get => buttonWidth;
        }

        #endregion 属性

        #region 命令

        public DelegateCommand DocumentMergeCmd { get; private set; }
        public DelegateCommand ScreeningCmd { get; private set; }
        public DelegateCommand RemoveInvalidItemCmd { get; private set; }
        public DelegateCommand DeduplicationByNameCmd { get; private set; }
        public DelegateCommand NewInfoArrangeCmd { get; private set; }

        #endregion 命令

        #region 方法

        private void Init()
        {
            DocumentMergeCmd = new DelegateCommand(() => DocumentMerge());
            ScreeningCmd = new DelegateCommand(() => Screening());
            RemoveInvalidItemCmd = new DelegateCommand(() => RemoveInvalidItem());
            DeduplicationByNameCmd = new DelegateCommand(() => DeduplicationByName());
            NewInfoArrangeCmd = new DelegateCommand(() => NewInfoArrange());
        }

        private void DocumentMerge()
        {
            Dialog.Show(new ExcelMergeView());
        }

        private void Screening()
        {
            Dialog.Show(new ExcelScreeningView());
        }

        private void RemoveInvalidItem()
        {
            Dialog.Show(new ExcelRemoveInvalidItemView());
        }

        private void DeduplicationByName()
        {
            Dialog.Show(new DeduplicationByNameView());
        }

        private void NewInfoArrange()
        {
            Dialog.Show(new NewInfoArrangeView());
        }
        #endregion 方法
    }
}