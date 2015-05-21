using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSB_backoffice_commercial
{
	class Ligne
	{
		Produit leProduit;
		int qte;
		int total;
		int idCommande;

		public Ligne(int unIdProduit, int uneQte, int unTotal, int unIdCommande){
            List<Produit>lesProduits = new List<Produit>();
            for (int i = 0; i < lesProduits.Count(); i++){
                if (lesProduits[i].getNum()== unIdProduit){
		             leProduit = lesProduits[i];
	            }
            }
			idCommande = unIdCommande;
			qte = uneQte;
			total = unTotal;
		}

        #region get & setters
        public int getIdProduit(){
			return leProduit.getNum();
		}
        public double getPrixProduit(){
            return leProduit.getPrixHT();
        }
        public string getNomProduit(){
            return leProduit.getNom();
        }
        public int getQte(){
        	return qte;
        }
        public void setQte(int uneQte){
        	qte = uneQte;
        }
        public int getTotal(){
        	return total;
        }
        public void setTotal(int unTotal){
        	total = unTotal;
        }
        public int getIdCommande(){
        	return idCommande;
        }
        public void setIdCommande(int unIdCommande){
        	idCommande = unIdCommande;
        }
        #endregion
    }
}
