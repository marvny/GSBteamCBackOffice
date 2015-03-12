using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace GSB_backoffice_commercial
{
    class Produit
    {

        #region Attributs

        int num;
        String nom;
        String effet;
        String CI;
        String presentation;
        String dosage;
        String famille;
        String interaction;
        double prixHT;
        double prixEch;

        #endregion

        #region Constructeur

        // Constructeur de Produit.
        public Produit(int unNum, string unNom, string unEffet, string desCI, string unePresentation, string unDosage, int unIDFamille, double unPrixHT, double unPrixEch, string uneInteraction)
        {
            num = unNum;
            nom = unNom;
            effet = unEffet;
            CI = desCI;
            presentation = unePresentation;
            dosage = unDosage;
            famille = DAOProduit.getNomFamille(unIDFamille);
            prixHT = unPrixHT;
            prixEch = unPrixEch;
            interaction=uneInteraction;
        }
        
        #endregion

        #region getter/setter
        public int getNum()
        {
            return num;
        }

        public void setNom(string value)
        {
            this.nom = value;
        }

        public string getNom()
        {
            return nom;
        }

        public void setEffet(string value)
        {
            this.effet = value;
        }

        public string getEffet()
        {
            return effet;
        }

        public void setCI(string value)
        {
            this.CI = value;
        }

        public string getCI()
        {
            return CI;
        }

        public void setPresentation(string value)
        {
            this.presentation = value;
        }

        public string getPresentation()
        {
            return presentation;
        }

        public void setDosage(string value)
        {
            this.dosage = value;
        }

        public string getDosage()
        {
            return dosage;
        }

        public void setFamille(string value)
        {
            this.famille = value;
        }

        public string getFamille()
        {
            return famille;
        }

        public void setPrixHT(double value)
        {
            this.prixHT = value;
        }

        public double getPrixHT()
        {
            return this.prixHT;
        }

        public void setPrixEch(double value)
        {
            this.prixEch = value;
        }

        public double getPrixEch()
        {
            return prixEch;
        }

        public string getInteraction()
        {
            return interaction;
        }

        #endregion

        #region Procédures/Fonctions

        // Valide lors de la création / Modification si le Numéro et le nom de produit ne sont pas déjà utilisés dans un produit de la Liste lesProduits.
        // Return False si le Nom ou le NUM sont déjà utilisés, sinon return True
        public static Boolean validerNumNom(int num, string nom)
        {
            int ok=0;
            List<Produit> lesProduits = listeProduit();
            for (int i = 0; i < lesProduits.Count(); i++)
            {
                if (lesProduits.ElementAt(i).getNum().Equals(num) || lesProduits.ElementAt(i).getNom().Equals(nom))
                {
                }
                else
                {
                    ok=ok+1;
                }
            }

            if (ok == lesProduits.Count())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Après avoir vérifié que le Nom et le Num n'étaient pas déjà utilisés (return true à vérification), appel à la méthode DAOProduit pour en crée un nouveau.
        //Return le message de validation ou d'erreur.
        public static String creerProduit(Produit unProduit)
        {
            string message;
            Boolean verification = Produit.validerNumNom(unProduit.getNum(), unProduit.getNom());
            if (verification)
            {
                message = DAOProduit.creerProduit(unProduit);
                return message;
            }
            else
            {
                return "Le numero de produit ou le nom utilisé existe déjà";
            }
        }

        //Suppression d'un produit et return le message de validation ou d'erreur.
        public static string supprimerProduit(Produit unProduit)
        {
            string message;
            message = DAOProduit.supprimerProduit(unProduit.getNum());
            return message;
        }

        // Création de la list Produit en provenance du DAOProduit où ils ont été récupérés.
        public static List<Produit> listeProduit()
        {
            List<Produit> laListe = DAOProduit.listeProduit();
            return laListe;
        }

        // Modification d'un produit et return le message de validation ou d'erreur.
        public static string modifierProduit(Produit unProduit)
        {
            string message;
            message = DAOProduit.modifierProduit(unProduit);
            return message;
        }

        // Création de la list Famille en provenance du DAOProduit où ils ont été récupérés.
        public static ArrayList listeFamille()
        {
            ArrayList lesFamilles = DAOProduit.listeFamille();
            return lesFamilles;
        }

        #endregion
    }
}
