using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms; 

namespace GSB_backoffice_commercial
{
    class DAOFactory
    {
        public SqlConnection cnx = new SqlConnection("Data Source=172.17.21.10;Initial Catalog=TeamC;User ID=mangel;Password=Abc123");

        public void connexion()
        {
            try
            {
                cnx.Open();
                Console.WriteLine("\n !!! success, connected successfully !!!\n");
            }
            catch (SqlException SqlException)
            {
                for (int i = 0; i < SqlException.Errors.Count; i++)
                {
                MessageBox.Show("ERROR #" + i + "\n" +
                                  "Message: " +
                                  SqlException.Errors[i].Message + "\n" +
                                  "Ligne: " +
                                  SqlException.Errors[i].LineNumber.ToString() + "\n" +
                                  "Source: " +
                                  SqlException.Errors[i].Source + "\n" +
                                  "SQL: " +
                                  SqlException.Errors[i].State + "\n");
                }
            }
        }

        public void deconnexion(){
            cnx.Close();
        }

        public SqlDataReader execSql(string requete)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            try
            {
                cmd.CommandText = requete;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cnx;

                dr = cmd.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
