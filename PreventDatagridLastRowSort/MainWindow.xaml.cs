using PreventDatagridLastRowSort.Data;
using PreventDatagridLastRowSort.Helpers;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreventDatagridLastRowSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static double Total = 0;
        DataRowView rvCopy = null;
        DataView dvCopy = null;
        bool sorted_aborted = true;
        private DataGridHelpers gridHelpers;

        public MainWindow()
        {
            InitializeComponent();
            ShowProducts();
            gridHelpers = new DataGridHelpers();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisableLastRow();
            this.Title = "Add Totals Row in Datagrid";
        }

        #region datagrid events

        private void dgProducts_Sorting(object sender, DataGridSortingEventArgs e)
        {
            DataRowView rv = (DataRowView)dgProducts.Items[dgProducts.Items.Count - 1];
            if (rv[0].ToString().Contains("Total:"))
            {
                dvCopy = dgProducts.Items.SourceCollection as DataView;
                rv.Delete();
            }

            sorted_aborted = e.Handled;
        }

        private void dgProducts_LayoutUpdated(object sender, EventArgs e)
        {
            if (!sorted_aborted)
            {
                ShowProductSorted();
                sorted_aborted = true;
            }
            else
            {
                DisableLastRow();
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// show sorted products
        /// </summary>
        private void ShowProductSorted()
        {
            Total = 0;
            DataTable dt = new DataTable();
            dt = dvCopy.ToTable();
            dvCopy = null;
            dgProducts.ItemsSource = null;
            foreach (DataRow row in dt.Rows)
            {
                Total = Total + Convert.ToDouble(row[1].ToString());
            }
            DataRow dr1 = dt.NewRow();
            dr1[0] = "Total:";
            dr1[1] = Total;
            dr1[2] = String.Empty;

            dr1[3] = false;
            dt.Rows.Add(dr1);
            dgProducts.ItemsSource = dt.AsDataView();
        }

        /// <summary>
        /// first load of products
        /// </summary>
        private void ShowProducts()
        {
            DataTable dt = new DataTable();
            dt = Connections.GetProduct();
            foreach (DataRow row in dt.Rows)
            {
                Total = Total + Convert.ToDouble(row[1].ToString());
            }
            DataRow dr1 = dt.NewRow();
            dr1[0] = "Total:";
            dr1[1] = Total;
            dr1[2] = String.Empty;

            dr1[3] = false;
            dt.Rows.Add(dr1);
            dgProducts.ItemsSource = dt.AsDataView();
        }

        /// <summary>
        /// prevent row from being edited and hide checkbox
        /// </summary>
        private void DisableLastRow()
        {
            var row = gridHelpers.GetDataGridRows(dgProducts);

            foreach (DataGridRow r in row)
            {
                if (((DataRowView)r.Item)[0].Equals("Total:"))
                {
                    r.FontWeight = FontWeights.Bold;
                    r.IsEnabled = false;

                    DataGridCell cell = gridHelpers.GetCell(dgProducts, r, 3);
                    CheckBox chk = gridHelpers.GetVisualChild<CheckBox>(cell);
                    if (chk != null)
                    {
                        chk.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        #endregion
    }
}