using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace GSB_backoffice_commercial
{
	class DAOCommande
	{
		public static List<Commande> listeCommandes(){
			DAOFactory p = new DAOFactory();

			p.connexion();
			string reqC = "SELECT * FROM commande;";
			List<Commande> resultat = new List<Commande>();
			// récupération des données de la requête

			SqlDataReader drC= p.execSql(reqC);
			// passage des données dans le vecteur

			while (drC.Read())
			{
				Commande c = new Commande(drC.GetInt32(0), drC.GetString(1),
					Convert.ToDateTime(drC.GetString(2)), drC.GetString(3), drC.GetDouble(4));
                listeLignes(c); 
				resultat.Add(c);
			}
			p.deconnection();
			return resultat;
		}

        public static List<Ligne> listeLignes(Commande laCommande)
		{
			DAOFactory p = new DAOFactory();

			p.connexion();
			string reqL = "SELECT * FROM ligne WHERE idCommande = " + laCommande.getIdCommande() + ";";

			// récupération des données de la requête

			SqlDataReader drL= p.execSql(reqL);
			// passage des données dans le vecteur

            List<Ligne> lesLignes = new List<Ligne>();
			while (drL.Read()){
                Ligne uneLigne = new Ligne(drL.GetInt32(0), drL.GetInt32(1), drL.GetDouble(2), drL.GetInt32(3));
				lesLignes.Add(uneLigne);
			}
			p.deconnection();
            return lesLignes;
		}

		public static void creerCommande(Commande laCommande)
		{

			DAOFactory p = new DAOFactory();
			Commande commande = laCommande;

            List<Ligne> lignes = laCommande.getLignes();

			p.connexion();
			for (int i=0; i<laCommande.nbLignes(); i++){
                string reqL = "INSERT INTO lignes VALUES(" + lignes[i].getIdProduit() + ", "
                    + lignes[i].getQte() + ", " + lignes[i].getTotal() + ", "
                    + lignes[i].getIdCommande() + ");";
				p.execSql(reqL);
			}
			string reqC = "INSERT INTO commande VALUES(" + laCommande.getIdCommande().ToString() + ", "
				+ laCommande.getPro().ToString() + ", " + laCommande.getDate().ToString() + ", "
				+ laCommande.getEtat() + "," + laCommande.getMontantTotal().ToString() + ");";
			p.execSql(reqC);
			p.deconnection();
		}

        public static void creerLigne(Ligne laLigne)
        {
            DAOFactory p = new DAOFactory();

            p.connexion();
            string reqL = "INSERT INTO lignes VALUES(" + laLigne.getIdProduit() + ", "
                + laLigne.getQte() + ", " + laLigne.getTotal() + ", "
                + laLigne.getIdCommande() + ");";
            p.execSql(reqL);
            p.deconnection();
        }

		public static void supprimerCommande(Commande laCommande)
		{
			DAOFactory p = new DAOFactory();

			p.connexion();
			string requeteLigne = "DELETE FROM commande WHERE idCommande=" + laCommande.getIdCommande().ToString() + ";"
				+ "DELETE FROM lignes WHERE idCommande=" + laCommande.getIdCommande().ToString() + ";";
			p.execSql(requeteLigne);
			p.deconnection();
		}

        public static void supprimerLigne(Ligne laLigne)
        {
            DAOFactory p = new DAOFactory();

            p.connexion();
            string requeteLigne = "DELETE FROM ligne WHERE idProduit=" + laLigne.getIdProduit().ToString() + " AND idCommande;" + laLigne.getIdCommande().ToString() + ";";
            p.execSql(requeteLigne);
            p.deconnection();
        }

		public static void modifierCommande(Commande laCommande)
		{
			DAOFactory p = new DAOFactory();

			p.connexion();
			string requeteLigne = "UPDATE commande "
				+ "SET idCommande = " + laCommande.getIdCommande().ToString()
				+ ", idProfessionnel = " + laCommande.getPro().ToString()
				+ ", Date = " + laCommande.getDate().ToString()
				+ ", Etat = " + laCommande.getEtat().ToString()
				+ ", montantTotal = " + laCommande.getMontantTotal().ToString() + ";";
			p.execSql(requeteLigne);
			p.deconnection();
		}

		public static void modifierLigne(Ligne laLigne)
		{

			DAOFactory p = new DAOFactory();

			p.connexion();
			string requeteLigne = "UPDATE ligne "
				+ "SET idProduit = " + laLigne.getIdProduit().ToString()
				+ ", qte = " + laLigne.getQte().ToString()
				+ ", Total = " + laLigne.getTotal().ToString()
				+ ", idCommande = " + laLigne.getIdCommande().ToString() + ";";
			p.execSql(requeteLigne);
			p.deconnection();
		}

		public static string getNomProduit(Ligne laLigne)
		{
			DAOFactory p = new DAOFactory();

			p.connexion();
            string req = "SELECT nom FROM produit WHERE produit.num = " + laLigne.getIdProduit().ToString() + ";";
			SqlDataReader dr = p.execSql(req);
			string resultat = dr.GetString(0);
			p.deconnection();
			return resultat;
		}

		public static int getIDProduit(string unNomProduit)
		{
			DAOFactory p = new DAOFactory();

			p.connexion();
			string req = "SELECT num FROM produit WHERE produit.nom = " + unNomProduit + ";";
			SqlDataReader dr = p.execSql(req);
			int resultat = dr.GetInt16(0);
			p.deconnection();
			return resultat;
		}
	}
}
