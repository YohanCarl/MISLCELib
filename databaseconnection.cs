using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace MISLCELib
{
    public class databaseconnection
    {
        private static string ConString = Properties.Settings.Default.ConnectionString;
        /// <summary>
        /// Get Table Result from Select Query
        /// </summary>
        /// <param name="query">A Select Query</param>
        /// <returns></returns>
        public DataSet GetDataset(string query)
        {
            DataSet dset = new DataSet();
            try
            {
                MySqlConnection con = new MySqlConnection(ConString);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(query, con);
                mySqlDataAdapter.Fill(dset);
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
               
            }
            return dset;
        }


        /// <summary>
        /// Execute Query (Insert, Update, Delete)
        /// </summary>
        /// <param name="query">An Insert, Update or Delete statement</param>
        /// <returns></returns>
        public bool Exec(string query)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(ConString);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
