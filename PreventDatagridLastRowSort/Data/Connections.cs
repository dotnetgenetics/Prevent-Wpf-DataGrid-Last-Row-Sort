using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PreventDatagridLastRowSort.Data
{
    public class Connections
    {
        public static DataTable GetProduct()
        {
            DataSet ds = new DataSet();
            string query = "Select ProductName,UnitPrice,QuantityPerUnit,Discontinue from Products;";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["products"].ConnectionString.ToString()))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds.Tables[0];
        }
    }
}
