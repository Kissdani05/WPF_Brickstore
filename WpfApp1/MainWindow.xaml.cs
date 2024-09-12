using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace WPF_Brickstore
{
    public partial class MainWindow : Window
    {
        List<BrickItem> itemList = new List<BrickItem>();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "BSX fájlok (*.bsx)|*.bsx";
            if (dlg.ShowDialog() == true)
            {
                string fileName = dlg.FileName;
                XDocument doc = XDocument.Load(fileName);

                itemList = doc.Descendants("Item").Select(i => new BrickItem
                {
                    ItemID = i.Element("ItemID")?.Value,
                    ItemName = i.Element("ItemName")?.Value,
                    CategoryName = i.Element("CategoryName")?.Value,
                    ColorName = i.Element("ColorName")?.Value,
                    Qty = i.Element("OrigQty")?.Value
                }).ToList();

                DataGridItems.ItemsSource = itemList;
            }
            else
            {
                MessageBox.Show("Nem sikerült fájlt megnyitni");
            }
            
        }
        private void NameFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            FilterItems();
        }
        private void IDFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            FilterItems();
        }
        private void FilterItems()
        {
            var filteredItems = itemList.Where(i =>
                (string.IsNullOrEmpty(NameFilter.Text) ||
                 (i.ItemName != null && i.ItemName.IndexOf(NameFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)) &&
                 (string.IsNullOrEmpty(IDFilter.Text) || i.ItemID?.StartsWith(IDFilter.Text) == true) &&
                (string.IsNullOrEmpty(CatFilter.Text) ||
                 (i.CategoryName != null && i.CategoryName.IndexOf(CatFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0))
            ).ToList();

            DataGridItems.ItemsSource = filteredItems;
        }

        private void CatFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            FilterItems();
        }
    }
    public class BrickItem
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string ColorName { get; set; }
        public string Qty { get; set; }
    }
}