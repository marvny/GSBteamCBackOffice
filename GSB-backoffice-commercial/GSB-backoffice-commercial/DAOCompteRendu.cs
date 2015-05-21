using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace GSB_backoffice_commercial
{
    class DAOCompteRendu
    {
        //Retourne une liste de tous les comptes rendus pour un client
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
            SqlDataReader dr = p.execSql(req);
            while (dr.Read())
            {
                CompteRendu r = new CompteRendu(dr.GetString(4), dr.GetString(1),
                    dr.GetString(2), dr.GetString(3));
                resultat.Add(r);
            }
            dr.Close();
            p.deconnection();
            return resultat;
        }

        //insère un nouveau compte rendu dans la base de données
        public static void creerCR(CompteRendu leCR)
        {
            DAOFactory p = new DAOFactory();
            CompteRendu CR;
            p.connexion();
            CR = leCR;
            string req = "insert into Compte_Rendu (texte, visiteur, date, codeProfessionnel) values('" + leCR.getTexte() + "', '"
                + leCR.getVisiteur() + "', '" + leCR.getDate() + "', '" + leCR.getProfessionnel() +"');";
            p.execSql(req);
            p.deconnection();
        }

        //supprime un compte rendu dans la base de données
        public static void supprimerCR(CompteRendu CR)
        {
            DAOFactory p = new DAOFactory();
            p.connexion();
            string requeteLigne = "DELETE FROM Compte_Rendu WHERE id=(SELECT id FROM Compte_rendu WHERE texte='" + CR.getTexte() + "'AND visiteur='"+CR.getVisiteur()+"' AND date='"+CR.getDate()+"' AND codeProfessionnel='"+CR.getProfessionnel()+"');";
            p.execSql(requeteLigne);
            p.deconnection();
        }

        //modifie un compte rendu dans la base de données
        public static void modifierCR(CompteRendu leCR, CompteRendu oldCR)
        {
            DAOFactory p = new DAOFactory();
            p.connexion();
            string requeteLigne = "UPDATE Compte_Rendu SET "
                + "texte = '" + leCR.getTexte()
                + "', visiteur = '" + leCR.getVisiteur()
                + "', date = '" + leCR.getDate()
                + "', codeProfessionnel = '" + leCR.getProfessionnel()
                + "' WHERE id=(SELECT id FROM Compte_rendu WHERE texte='" + oldCR.getTexte() 
                + "' AND visiteur='" + oldCR.getVisiteur() 
                + "' AND date='" + oldCR.getDate() 
                + "' AND codeProfessionnel='" + oldCR.getProfessionnel() + "');";
            p.execSql(requeteLigne);
            p.deconnection();
        }
    }
}

