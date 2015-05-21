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
        //Recuperer
        //Ecrire
        //modifier
        //Supprimer
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
            // récupération des données de la requête

            SqlDataReader dr = p.execSql(req);

            // passage des données dans le vecteur

            while (dr.Read())
            {
                Client r = new Client(dr.GetString(0), dr.GetString(1),
                    dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5),
                    dr.GetString(6));
                resultat.Add(r);
                //recup des cr
            }
            p.deconnexion();
            return resultat;
        }

        public static void creerClient(Client leClient)
        {

            DAOFactory p = new DAOFactory();
            Client client;
            //MySqlDataReader leSelect = select();
            client = leClient;
            string req = "insert into Professionnel (code, nom, rSociale, adresse, type, tel, mail) values('" + leClient.getCode() + "', '"
                + leClient.getNom() + "', '" + leClient.getRaison() + "', '" + leClient.getAdresse() + "', '"
                + leClient.getType() + "', '" + leClient.getTel() + "', '" + leClient.getMail()+"');";
            p.connexion();
            p.execSql(req);
            p.deconnexion();
        }

        public static void supprimerClient(string code)
        {

            DAOFactory p = new DAOFactory();
            //MySqlDataReader leSelect = select();
            string requeteLigne = "DELETE FROM Professionnel WHERE code='" + code+"';";
            p.connexion();
            p.execSql(requeteLigne);
            p.deconnexion();
        }

        public static void modifierClient(Client leClient)
        {
            DAOFactory p = new DAOFactory();
            //MySqlDataReader leSelect = select();
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
            p.deconnexion();
        }
    }
}
