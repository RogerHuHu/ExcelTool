using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelTool.Controls
{
    /// <summary>
    /// CheckBoxGroup.xaml 的交互逻辑
    /// </summary>
    public partial class CheckBoxGroup : UserControl
    {
        public CheckBoxGroup()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<string>), typeof(CheckBoxGroup),
            new FrameworkPropertyMetadata(new ObservableCollection<string>(), OnCollectionChanged));

        public ObservableCollection<string> ItemsSource
        {
            get { return (ObservableCollection<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public ObservableCollection<CheckBox> CheckBoxes { get; set; } = new ObservableCollection<CheckBox>();

        //public static readonly DependencyProperty CheckBoxesProperty =
        //    DependencyProperty.Register("CheckBoxes", typeof(ObservableCollection<CheckBox>), typeof(CheckBoxGroup),
        //    new FrameworkPropertyMetadata(new ObservableCollection<CheckBox>(), null));

        //public ObservableCollection<CheckBox> CheckBoxes
        //{
        //    get { return (ObservableCollection<CheckBox>)GetValue(CheckBoxesProperty); }
        //    set { SetValue(CheckBoxesProperty, value); }
        //}

        //定义一个依赖属性，表示是否全选
        public static readonly DependencyProperty IsAllCheckedProperty =
            DependencyProperty.Register("IsAllChecked", typeof(bool), typeof(CheckBoxGroup),
                                        new FrameworkPropertyMetadata(false, OnIsAllCheckedChanged));

        //获取或设置是否全选的属性
        public bool IsAllChecked
        {
            get { return (bool)GetValue(IsAllCheckedProperty); }
            set { SetValue(IsAllCheckedProperty, value); }
        }

        //定义一个SelectionChanged事件，用于通知数据项的选中状态发生变化
        public event SelectionChangedEventHandler SelectionChanged;

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(List<object>), typeof(CheckBoxGroup),
                                        new FrameworkPropertyMetadata(new List<object>(), null));


        public List<object> SelectedItems
        {
            get { return (List<object>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        #region 选中事件处理
        //当是否全选的属性值发生变化时，触发该方法
        private static void OnIsAllCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //获取当前控件对象
            var checkBoxGroup = d as CheckBoxGroup;
            if (checkBoxGroup == null) return;

            //获取当前控件的子元素集合
            var items = checkBoxGroup.CheckBoxes;
            if (items == null || items.Count == 0) return;

            //遍历子元素集合，将每个CheckBox的IsChecked属性设置为与父控件一致
            foreach (var item in items)
            {
                var checkBox = item as CheckBox;
                if (checkBox != null && (checkBox.Content as string) != "全选")
                {
                    checkBox.IsChecked = checkBoxGroup.IsAllChecked ? true : false;
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if ((cb.Content as string) == "全选")
                return;

            SelectedItems.Add(cb.Content as string);
            RefreshSelectAllStatus();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if ((cb.Content as string) == "全选")
                return;

            SelectedItems.Remove(cb.Content as string);
            RefreshSelectAllStatus();
        }

        private void RefreshSelectAllStatus()
        {
            if (SelectedItems.Count == 0)
                CheckBoxes[0].IsChecked = false;
            else if (SelectedItems.Count == CheckBoxes.Count - 1)
                CheckBoxes[0].IsChecked = true;
            else
                CheckBoxes[0].IsChecked = null;

            if (SelectionChanged != null)
                SelectionChanged(this, null);
        }

        private void CheckBox_AllUnchecked(object sender, RoutedEventArgs e)
        {
            IsAllChecked = false;
        }

        private void CheckBox_AllChecked(object sender, RoutedEventArgs e)
        {
            IsAllChecked = true;
        }

        #endregion

        #region 集合变更事件处理

        private static void OnCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var checkBoxGroup = d as CheckBoxGroup;
            checkBoxGroup.ItemsSource.CollectionChanged += checkBoxGroup.ItemsSource_CollectionChanged;
            checkBoxGroup.CheckBoxes.Clear();
            CheckBox cb = new CheckBox() { Content = "全选", IsThreeState = true, Margin = new Thickness(5, 5, 0, 0) };
            cb.Checked += checkBoxGroup.CheckBox_AllChecked;
            cb.Unchecked += checkBoxGroup.CheckBox_AllUnchecked;
            foreach (var item in checkBoxGroup.ItemsSource)
                checkBoxGroup.CheckBoxes.Add(new CheckBox() { Content = item, IsThreeState = false, Margin = new Thickness(5, 5, 0, 0) });

            checkBoxGroup.cbgItemsControl.ItemsSource = checkBoxGroup.CheckBoxes;
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                CheckBoxes.Clear();
                CheckBox cb = new CheckBox() { Content = "全选", IsThreeState = true, Margin = new Thickness(5,5,0,0) };
                cb.Checked += CheckBox_AllChecked;
                cb.Unchecked += CheckBox_AllUnchecked;
                CheckBoxes.Add(cb);
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CheckBox cb = new CheckBox() { Content = e.NewItems[0], IsThreeState = false, Margin = new Thickness(5, 5, 0, 0) };
                cb.Checked += CheckBox_Checked;
                cb.Unchecked += CheckBox_Unchecked;
                CheckBoxes.Add(cb);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                CheckBox cb = CheckBoxes.Where(c => c.Content == e.NewItems[0]).FirstOrDefault();
                cb.Checked -= CheckBox_Checked;
                cb.Unchecked -= CheckBox_Unchecked;
                if (cb != null)
                    CheckBoxes.Remove(cb);
            }
        }

        //protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        //{
        //    CheckBoxes.Clear();
        //    CheckBoxes.Add(new CheckBox() { Content = "全选", IsThreeState = true, Margin = new Thickness(5, 5, 0, 0) });
        //    foreach (var item in newValue)
        //        CheckBoxes.Add(new CheckBox() { Content = item, IsThreeState = false, Margin = new Thickness(5, 5, 0, 0) });
        //}

        //protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Reset)
        //    {
        //        CheckBoxes.Clear();
        //        CheckBox cb = new CheckBox() { Content = "全选", IsThreeState = true, Margin = new Thickness(5, 5, 0, 0) };
        //        cb.Checked += CheckBox_AllChecked;
        //        cb.Unchecked += CheckBox_AllUnchecked;
        //        CheckBoxes.Add(cb);
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        CheckBox cb = new CheckBox() { Content = e.NewItems[0], IsThreeState = false, Margin = new Thickness(5, 5, 0, 0) };
        //        cb.Checked += CheckBox_Checked;
        //        cb.Unchecked += CheckBox_Unchecked;
        //        CheckBoxes.Add(cb);
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        CheckBox cb = CheckBoxes.Where(c => c.Content == e.NewItems[0]).FirstOrDefault();
        //        cb.Checked -= CheckBox_Checked;
        //        cb.Unchecked -= CheckBox_Unchecked;
        //        if (cb != null)
        //            CheckBoxes.Remove(cb);
        //    }
        //}
        #endregion
    }
}
