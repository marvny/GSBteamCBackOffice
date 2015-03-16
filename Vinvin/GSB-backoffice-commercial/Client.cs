using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSB_backoffice_commercial
{

    public class Client
    {
        private String code;
        private String nom;
        private String raisonSociale;
        private String adresse;
        private String type;
        private String telephone;
        private String mail;

        List<CompteRendu> lesComptesRendus = new List<CompteRendu>();

        public Client(String unCode, String unNom, String uneRaisonSociale, String uneAdresse, 
            String unType, String unTelephone, String unMail)
        {
            this.code = unCode;
            this.nom = unNom;
            this.raisonSociale = uneRaisonSociale;
            this.adresse = uneAdresse;
            this.type = unType;
            this.telephone = unTelephone;
            this.mail = unMail;
        }
        public void initLesComptesRendus()
        {
            setLesComptesRendus(DAOCompteRendu.listeCR(this.nom));
        }

        public List<CompteRendu> getLesComptesRendus()
        {
            return this.lesComptesRendus;
        }

        public void setLesComptesRendus(List<CompteRendu> CR)
        {
            this.lesComptesRendus = CR;

        }

        public string getCode()
        {
            return this.code;
        }
        public void setCode(String unCode)
        {
            this.code = unCode;
        }

        public string getNom()
        {
            return this.nom;
        }
        public void setNom(String unNom)
        {
            this.nom = unNom;
        }

        public string getRaison()
        {
            return this.raisonSociale;
        }
        public void setRaison(String uneRaison)
        {
            this.raisonSociale = uneRaison;
        }

        public string getAdresse()
        {
            return this.adresse;
        }
        public void setAdresse(String uneAdresse)
        {
            this.adresse = uneAdresse;
        }

        public string getType()
        {
            return this.type;
        }
        public void setType(String unType)
        {
            this.type = unType;
        }

        public string getTel()
        {
            return this.telephone;
        }
        public void setTel(String unTel)
        {
            this.telephone = unTel;
        }

        public string getMail()
        {
            return this.mail;
        }
        public void setMail(String unMail)
        {
            this.mail = unMail;
        }
    }
}
