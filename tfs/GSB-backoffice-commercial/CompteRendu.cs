using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSB_backoffice_commercial
{
    public class CompteRendu
    {
        private String codeProfessionnel;
        private String texte;
        private String visiteur;
        private String date;

        //Constructeur
        public CompteRendu(String unProfessionnel, String unTexte, String unVisiteur, String uneDate)
        {
            this.codeProfessionnel = unProfessionnel;
            this.texte = unTexte;
            this.visiteur = unVisiteur;
            this.date = uneDate;
        }

        public String getProfessionnel()
        {
            return this.codeProfessionnel;
        }
        public void setProfessionnel(String unProfessionnel)
        {
            this.codeProfessionnel = unProfessionnel;
        }

        public String getTexte()
        {
            return this.texte;
        }
        public void setTexte(String unTexte)
        {
            this.texte = unTexte;
        }

        public String getVisiteur()
        {
            return this.visiteur;
        }
        public void setVisiteur(String unVisiteur)
        {
            this.visiteur = unVisiteur;
        }

        public String getDate()
        {
            return this.date;
        }
        public void setDate(String uneDate)
        {
            this.date = uneDate;
        }
    }
}
