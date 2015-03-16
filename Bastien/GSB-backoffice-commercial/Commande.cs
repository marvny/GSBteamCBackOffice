using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSB_backoffice_commercial
{
    class Commande
    {
        int id;
        int idPro;
        string dateCreation;
        List<Ligne> lesLignes;
        String etat;
        double montantTotal;

        public Commande(int unId, int unIdPro, String uneDate, String unEtat, double unMontantTotal){
            id = unId;
            idPro = unIdPro;
            dateCreation = uneDate;
            etat = unEtat;
            montantTotal = unMontantTotal;
            lesLignes = new List<Ligne>();
        }
        

        public static List<Commande> listeCommandes(){
            return DAOCommande.listeCommandes();
        }

        public void ajouterLigne(Ligne uneLigne){
            lesLignes.Add(uneLigne);
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

        public int getPro(){
            return idPro;
        }

        public void setPro(int unIdPro){
            idPro = unIdPro;
        }

        public string getDate(){
            return dateCreation;
        }

        public void setDate(string uneDate){
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