using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSB_backoffice_commercial
{
    class Commande
    {
        int id;
        string idPro;
        DateTime dateCreation;
        List<Ligne> lesLignes;
        String etat;
        double montantTotal;

        public Commande(int unId, string unIdPro, DateTime uneDate, string unEtat, double unMontantTotal){
            id = unId;
            idPro = unIdPro;
            dateCreation = uneDate;
            etat = unEtat;
            montantTotal = unMontantTotal;
            lesLignes = DAOCommande.listeLignes(this);
        }

        public static int idNouvelleCommande(){
            return DAOCommande.listeCommandes().Last().getIdCommande()+1;
        }

        public static List<Commande> listeCommandes(){
            return DAOCommande.listeCommandes();
        }

        public void supprimerCommande(){
            DAOCommande.supprimerCommande(this);
        }

        public void ajouterLigne(Ligne uneLigne){
            lesLignes.Add(uneLigne);
            DAOCommande.creerLigne(uneLigne);
        }

        public void supprimerLigne(Ligne uneLigne){
            if (lesLignes.Contains(uneLigne)){
                lesLignes.Remove(uneLigne);
            }
        }

        public int nbLignes(){
            return lesLignes.Count();
        }

        #region get & setters
        public List<Ligne> getLignes(){
            return lesLignes;
        }

        public int getIdCommande(){
            return id;
        }

        public string getPro(){
            return idPro;
        }

        public void setPro(string unIdPro){
            idPro = unIdPro;
        }

        public DateTime getDate(){
            return dateCreation;
        }

        public void setDate(DateTime uneDate){
            dateCreation = uneDate;
        }

        public string getEtat(){
            return etat;
        }

        public void setEtat(string unEtat){
            etat = unEtat;
        }

        public double getMontantTotal(){
            return montantTotal;
        }

        public void setMontantTotal(double unMontantTotal){
            montantTotal = unMontantTotal;
        }
        #endregion
    }
}