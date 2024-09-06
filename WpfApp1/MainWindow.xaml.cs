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
                    CategoryName = i.Element("Category")?.Value,
                    ColorName = i.Element("Color")?.Value,
                    Qty = i.Element("Qty")?.Value
                }).ToList();

                DataGridItems.ItemsSource = itemList; 
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
                (string.IsNullOrEmpty(NameFilter.Text) || i.ItemName?.StartsWith(NameFilter.Text) == true) &&
                (string.IsNullOrEmpty(IDFilter.Text) || i.ItemID?.StartsWith(IDFilter.Text) == true)
            ).ToList();

            DataGridItems.ItemsSource = filteredItems; 
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
