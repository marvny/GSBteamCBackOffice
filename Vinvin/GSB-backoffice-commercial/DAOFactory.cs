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
        public static SqlConnection cnx = new SqlConnection("Data Source=172.17.21.10;Initial Catalog=TeamC;User ID=Mangel;Password=Abc123");
        

        public void connexion()
        {
            try
            {
                cnx.Open();
                //MessageBox.Show("\n !!! success, connected successfully !!!\n");
            }
            catch (SqlException mySqlException)
            {
                for (int i = 0; i < mySqlException.Errors.Count; i++)
                {
                    MessageBox.Show("ERROR #" + i + "\n" +
                                      "Message: " +
                                      mySqlException.Errors[i].Message + "\n" +
                                      "Ligne: " +
                                      mySqlException.Errors[i].LineNumber.ToString() + "\n" +
                                      "Source: " +
                                      mySqlException.Errors[i].Source + "\n" +
                                      "SQL: " +
                                      mySqlException.Errors[i].State + "\n");
                }
            }
        }

        public void deconnexion()
        {
            cnx.Close();
        }

        public SqlDataReader execSql(string requete)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            cmd.CommandText = requete;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnx;

            dr = cmd.ExecuteReader();
            return dr;
        }
    }
}
