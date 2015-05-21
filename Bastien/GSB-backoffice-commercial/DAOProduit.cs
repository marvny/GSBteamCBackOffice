using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;

namespace GSB_backoffice_commercial
{
    class DAOProduit
    {
        public static List<Produit> listeProduit()
        {
            DAOFactory p = new DAOFactory();
            p.connexion();
            string req = "SELECT produit.* FROM produit;";
            List<Produit> resultat = new List<Produit>();
            // récupération des données de la requête
            try
            {
                SqlDataReader dr = p.execSql(req);

                // passage des données dans le vecteur

                while (dr.Read())
                {
                    Produit r = new Produit(dr.GetInt32(0), dr.GetString(1),
                        dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5),
                        dr.GetInt32(6), dr.GetDouble(7), dr.GetDouble(8), dr.GetString(9));
                    resultat.Add(r);
                }
                p.deconnexion();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return resultat;
        }

        public static ArrayList listeFamille()
        {
            DAOFactory p = new DAOFactory();
            p.connexion();
            string req = "SELECT * FROM famille" + ";";
            ArrayList lesFamilles = new ArrayList();
            // récupération des données de la requête
            try
            {
                SqlDataReader dr = p.execSql(req);

                // passage des données dans le vecteur

                while (dr.Read())
                {
                    lesFamilles.Add(dr.GetString(1));
                }
                p.deconnexion();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return lesFamilles;
        }

        public static string creerProduit(Produit leProduit)
        {
            DAOFactory p = new DAOFactory();
            Produit produit;
            //MySqlDataReader leSelect = select();
            produit = leProduit;
            string req = "set identity_insert produit ON; insert into produit(num, nom, effet, contre_indic, presentation, dosage, idFamille, prixHT, prixEchant, interactions) values(" + leProduit.getNum() + ", "
                +"'"+leProduit.getNom()+ "', '"+leProduit.getEffet()+"', '"+leProduit.getCI()+"', '"
                +leProduit.getPresentation()+"', '"+leProduit.getDosage()+"', "+getIDFamille(leProduit.getFamille())
                + ", " + leProduit.getPrixHT() + ", " + leProduit.getPrixEch() + ", '" + leProduit.getInteraction() + "');";
            try
            {
                p.connexion();
                p.execSql(req);
                p.deconnexion();
                return "Produit crée";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "Erreur dans la création du produit";
            }
        }

        public static string supprimerProduit(int num)
        {
            DAOFactory p = new DAOFactory();
            try
            {
                p.connexion();
                string requeteLigne = "DELETE FROM produit WHERE num=" + num + ";";
                p.execSql(requeteLigne);
                p.deconnexion();
                return "Produit Supprimé";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "Erreur dans la suppression du produit";
            }
        }

        public static string modifierProduit(Produit leProduit)
        {
            string prixHT = leProduit.getPrixHT().ToString();
            string prixEch = leProduit.getPrixEch().ToString();

            DAOFactory p = new DAOFactory();
            //MySqlDataReader leSelect = select();            
            string requeteLigne = "UPDATE produit "
                + "SET nom = '" + leProduit.getNom() + "'"
                + ", effet = '" + leProduit.getEffet().Replace("'", "`") + "'"
                + ", contre_indic = '" + leProduit.getCI().Replace("'", "`") + "'"
                + ", presentation = '" + leProduit.getPresentation().Replace("'", "`") + "'"
                + ", dosage = '" + leProduit.getDosage() + "'"
                + ", idFamille = " + (getIDFamille(leProduit.getFamille())-1)
                + ", prixHT = " + prixHT.Replace(',','.')
                + ", prixEchant = " + prixEch.Replace(',','.')
                + ", interactions = '" + leProduit.getInteraction().Replace("'", "`") + "'"
                + " WHERE num = " + leProduit.getNum()+";" ;
            try
            {
                p.connexion();
                p.execSql(requeteLigne);
                p.deconnexion();
                return "Produit modifié";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "Erreur dans la modification du produit";
            }
        }

        public static string getNomFamille(int idFamille)
        {
            string resultat = null;
            DAOFactory p = new DAOFactory();
            idFamille = idFamille + 1;
            p.connexion();
            string req = "SELECT nom FROM famille WHERE id = " + idFamille + ";";
            try
            {
                SqlDataReader dre = p.execSql(req);
                dre.Read(); //Soucis : Sans le Read, la requête ne passe pas !
                resultat = dre.GetString(0);
                p.deconnexion();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return resultat;
        }

        public static int getIDFamille(string nomFamille)
        {
            int resultat = 0;
            DAOFactory p = new DAOFactory();
            p.connexion();
            string req = "SELECT id FROM Famille WHERE nom = '" + nomFamille + "';";
            try
            {
                SqlDataReader dr = p.execSql(req);
                dr.Read();
                resultat = dr.GetInt32(0);
                p.deconnexion();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return resultat;
        }
    }
}
