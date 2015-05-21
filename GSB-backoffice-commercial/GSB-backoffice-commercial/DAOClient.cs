using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GSB_backoffice_commercial
{
    class DAOClient
    {
        //retourne une liste de tous les clients présents dans la base de données
        public static List<Client> listeClient()
        {
            DAOFactory p = new DAOFactory();
            try
            {
                p.connexion();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
            string req = "SELECT * FROM Professionnel";
            List<Client> resultat = new List<Client>();
            SqlDataReader dr = p.execSql(req);
            while (dr.Read())
            {
                Client r = new Client(dr.GetString(0), dr.GetString(1),
                    dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5),
                    dr.GetString(6));
                resultat.Add(r);
            }
            p.deconnection();
            return resultat;
        }

        //Insère un nouveau client dans la Base de données.
        public static void creerClient(Client leClient)
        {
            DAOFactory p = new DAOFactory();
            Client client;
            client = leClient;
            string req = "insert into Professionnel (code, nom, rSociale, adresse, type, tel, mail) values('" + leClient.getCode() + "', '"
                + leClient.getNom() + "', '" + leClient.getRaison() + "', '" + leClient.getAdresse() + "', '"
                + leClient.getType() + "', '" + leClient.getTel() + "', '" + leClient.getMail()+"');";
            p.connexion();
            p.execSql(req);
            p.deconnection();
        }

        //Supprime un client de la base de données
        public static void supprimerClient(string code)
        {
            DAOFactory p = new DAOFactory();
            string requeteLigne = "DELETE FROM Professionnel WHERE code='" + code+"';";
            p.connexion();
            p.execSql(requeteLigne);
            p.deconnection();
        }

        //Modifie un client de la base de données
        public static void modifierClient(Client leClient)
        {
            DAOFactory p = new DAOFactory();
            p.connexion();
            string requeteLigne = "UPDATE Professionnel SET "
                + "nom = '" + leClient.getNom()
                + "', rSociale = '" + leClient.getRaison()
                + "', adresse = '" + leClient.getAdresse()
                + "', type = '" + leClient.getType()
                + "', tel = '" + leClient.getTel()
                + "', mail = '" + leClient.getMail()
                +"' WHERE code ='"+leClient.getCode()+"';";
            p.execSql(requeteLigne);
            p.deconnection();
        }
    }
}
