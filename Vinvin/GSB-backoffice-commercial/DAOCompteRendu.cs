using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace GSB_backoffice_commercial
{
    class DAOCompteRendu
    {
        public static List<CompteRendu> listeCR(String unCode)
        {
            DAOFactory p = new DAOFactory();
            try
            {
                p.connexion();
            }
            catch (Exception)
            {

            }
            string req = "SELECT * FROM Compte_Rendu where codeProfessionnel='"+unCode+"';";
            List<CompteRendu> resultat = new List<CompteRendu>();
            // récupération des données de la requête

            SqlDataReader dr = p.execSql(req);

            // passage des données dans le vecteur
                while (dr.Read())
                {
                    CompteRendu r = new CompteRendu(dr.GetString(4), dr.GetString(1),
                        dr.GetString(2), dr.GetString(3));
                    resultat.Add(r);
                }
            dr.Close();
            p.deconnexion();
            return resultat;
        }

        public static void creerCR(CompteRendu leCR)
        {
            DAOFactory p = new DAOFactory();
            CompteRendu CR;
            //MySqlDataReader leSelect = select();
            p.connexion();
            CR = leCR;
            string req = "insert into Compte_Rendu (texte, visiteur, date, codeProfessionnel) values('" + leCR.getTexte() + "', '"
                + leCR.getVisiteur() + "', '" + leCR.getDate() + "', '" + leCR.getProfessionnel() +"');";
            p.execSql(req);
            p.deconnexion();
        }

        public static void supprimerCR(CompteRendu CR)
        {

            DAOFactory p = new DAOFactory();
            //MySqlDataReader leSelect = select();
            p.connexion();
            string requeteLigne = "DELETE FROM Compte_Rendu WHERE id=(SELECT id FROM Compte_rendu WHERE texte='" + CR.getTexte() + "'AND visiteur='"+CR.getVisiteur()+"' AND date='"+CR.getDate()+"' AND codeProfessionnel='"+CR.getProfessionnel()+"');";
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
                + "' WHERE code ='" + leClient.getCode() + "';";
            p.execSql(requeteLigne);
            p.deconnexion();
        }
    }
}

