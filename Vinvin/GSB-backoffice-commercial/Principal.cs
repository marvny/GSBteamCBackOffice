using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GSB_backoffice_commercial
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
            desactiverClient();
            remplirComboBoxFilterPro();
            foreach (Client client in lesPros)
            {
                client.initLesComptesRendus();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        #region Onglets Clients / Prospects

        #region variables

        public List<Client> lesPros = DAOClient.listeClient();
        
        public CompteRendu old = new CompteRendu("", "", "", "");

        #endregion

        #region evenements professionnels

        private void buttonCreerPro_Click(object sender, EventArgs e)
        {
            activerClient(1);
        }

        private void buttonModifierPro_Click(object sender, EventArgs e)
        {
            try
            {
                modifierProfessionnel(textBoxCodePro.Text, textBoxNomPro.Text, textBoxRaisonSocialePro.Text, textBoxAdressePro.Text,
                    textBoxTypePro.Text, textBoxTelPro.Text, textBoxAdresseMailPro.Text);
                viderTextBoxPro();
                desactiverClient();
                remplirComboBoxFilterPro();
                MessageBox.Show("Professionel modifié avec succès");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSupprimerPro_Click(object sender, EventArgs e)
        {
            supprimerProfessionnel(textBoxCodePro.Text);
            viderTextBoxPro();
            desactiverClient();
            remplirComboBoxFilterPro();
            comboBoxFiltrerPro.Text = "";
            MessageBox.Show("Professionel supprimé avec succès");
        }

        private void buttonValiderPro_Click(object sender, EventArgs e)
        {
            try{
                creerClient(textBoxCodePro.Text, textBoxNomPro.Text, textBoxRaisonSocialePro.Text, textBoxAdressePro.Text,
                    textBoxTypePro.Text, textBoxTelPro.Text, textBoxAdresseMailPro.Text);
                viderTextBoxPro();
                desactiverClient();
                MessageBox.Show("Professionel créé avec succès");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonAnnulerPro_Click(object sender, EventArgs e)
        {
            desactiverClient();
            viderTextBoxPro();
            viderGroupBoxCR();
        }

        private void textBoxRaisonSocialePro_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxFiltrerPro_SelectedIndexChanged(object sender, EventArgs e)
        {
            activerClient(2);
            viderGroupBoxCR();
            activerCR(1);
            remplirTextBoxSelectionClients(comboBoxFiltrerPro.SelectedItem.ToString());
            remplirDatagridViewComptesRendus(comboBoxFiltrerPro.SelectedItem.ToString());
        }

        #endregion 

        #region fonctions professionels

        private void remplirComboBoxFilterPro()
        {
            comboBoxFiltrerPro.Items.Clear();
            for (int i = 0; i < lesPros.Count; i++)
            {
                comboBoxFiltrerPro.Items.Add(lesPros[i].getNom());
            }
        }

        private void remplirTextBoxSelectionClients(String unNom)
        {
            for (int i = 0; i < lesPros.Count; i++)
            {
                if (lesPros[i].getNom() == unNom)
                {
                    textBoxCodePro.Text = lesPros[i].getCode();
                    textBoxNomPro.Text = lesPros[i].getNom();
                    textBoxRaisonSocialePro.Text = lesPros[i].getRaison();
                    textBoxAdressePro.Text = lesPros[i].getAdresse();
                    textBoxTypePro.Text = lesPros[i].getType();
                    textBoxTelPro.Text = lesPros[i].getTel();
                    textBoxAdresseMailPro.Text = lesPros[i].getMail();

                    //lesPros[i].setLesComptesRendus(DAOCompteRendu.listeCR(lesPros[i].getCode()));
                    remplirDatagridViewComptesRendus(lesPros[i].getNom());
                }
            }
        }

        private void activerClient(int choix)
        {
            if (choix == 1)
            {
                buttonValiderPro.Enabled = true;
                groupBoxPro.Enabled = true;
                textBoxCodePro.Enabled = true;
                buttonModifierPro.Enabled = false;
                buttonSupprimerPro.Enabled = false;
                comboBoxFiltrerPro.Enabled = false;
                buttonModifierPro.Enabled = false;

            }
            else if (choix == 2)
            {
                groupBoxPro.Enabled = true;
                groupBoxCompteRenduPro.Enabled = true;
                buttonCreerPro.Enabled = false;
                buttonValiderPro.Enabled = false;
                textBoxCodePro.Enabled = false;
                buttonModifierPro.Enabled = true;
                buttonSupprimerPro.Enabled = true;
            }
        }

        private void desactiverClient()
        {
            groupBoxPro.Enabled = false;
            groupBoxCompteRenduPro.Enabled = false;
            comboBoxFiltrerPro.Enabled = true;
            buttonCreerPro.Enabled = true;
        }

        private void creerClient(String unCode, String unNom, String uneRaisonSociale, String uneAdresse,
            String unType, String unTelephone, String unMail)
        {
            Client unClient = new Client(unCode, unNom, uneRaisonSociale, uneAdresse, unType, unTelephone, unMail);
            if (lesPros.Contains(unClient) == true)
            {
                throw new Exception("Un client similaire existe déjà");
            }
            else
            {
                DAOClient.creerClient(unClient);
                lesPros.Add(unClient);
                remplirComboBoxFilterPro();
            }
        }

        private void viderTextBoxPro()
        {
            textBoxNomPro.Clear();
            textBoxCodePro.Clear();
            textBoxRaisonSocialePro.Clear();
            textBoxAdressePro.Clear();
            textBoxAdresseMailPro.Clear();
            textBoxTypePro.Clear();
            textBoxTelPro.Clear();
        }

        private void modifierProfessionnel(String unCode, String unNom, String uneRaisonSociale, String uneAdresse,
            String unType, String unTelephone, String unMail)
        {
            Client unClient = new Client(unCode, unNom, uneRaisonSociale, uneAdresse, unType, unTelephone, unMail);
            if (lesPros.Contains(unClient) == true)
            {
                throw new Exception("Un client similaire existe déjà");
            }
            else
            {
                for (int i = 0; i < lesPros.Count; i++)
                {
                    if (lesPros[i].getCode() == unCode)
                    {
                        lesPros[i].setNom(unNom);
                        lesPros[i].setRaison(uneRaisonSociale);
                        lesPros[i].setAdresse(uneAdresse);
                        lesPros[i].setType(unType);
                        lesPros[i].setTel(unTelephone);
                        lesPros[i].setMail(unMail);
                        DAOClient.modifierClient(lesPros[i]);
                    }
                }
            }
        }

        private void supprimerProfessionnel(String unCode)
        {
            for (int i = 0; i < lesPros.Count; i++)
            {
                if (lesPros[i].getCode() == unCode)
                {
                    DAOClient.supprimerClient(unCode);
                    lesPros.Remove(lesPros[i]);
                }
            }
        }

        #endregion

        #region evenements Compte-Rendu

        private void buttonCRPro_Click(object sender, EventArgs e)
        {
            activerCR(2);
        }

        private void buttonValiderCRPro_Click(object sender, EventArgs e)
        {

                    creerCompteRendu(comboBoxFiltrerPro.SelectedItem.ToString(), textBoxTexteCR.Text,
                                textBoxVisiteurPro.Text, dateTimePickerPro.Value.ToShortDateString());

                    //modifCompteRendu(comboBoxFiltrerPro.SelectedItem.ToString(), textBoxTexteCR.Text,
                                //textBoxVisiteurPro.Text, dateTimePickerPro.Value.ToShortDateString());

                remplirDatagridViewComptesRendus(comboBoxFiltrerPro.SelectedItem.ToString());
                viderGroupBoxCR();
                activerCR(1);

        }

        private void dataGridViewCR_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            activerCR(3);
            remplirGroupBoxCRSelection(dataGridViewCR[1, e.RowIndex].Value.ToString(),
                dataGridViewCR[0, e.RowIndex].Value.ToString(), dataGridViewCR[2, e.RowIndex].Value.ToString());

            old.setVisiteur(dataGridViewCR[0, e.RowIndex].Value.ToString());
            old.setDate(dataGridViewCR[2, e.RowIndex].Value.ToString());
            old.setProfessionnel(comboBoxFiltrerPro.SelectedItem.ToString());
        }

        private void buttonAnnulerCRPro_Click(object sender, EventArgs e)
        {
            activerCR(1);
            viderGroupBoxCR();
            activerCR(1);
        }

        #endregion

        #region fonctions Compte-Rendu

        private void remplirDatagridViewComptesRendus(String unProfessionnel)
        {
            dataGridViewCR.Rows.Clear();
            for (int i = 0; i < lesPros.Count; i++)
            {
                if (lesPros[i].getNom() == unProfessionnel)
                {
                    if (lesPros[i].getLesComptesRendus().Count > 0)
                    {
                        for (int j = 0; j < lesPros[i].getLesComptesRendus().Count; j++)
                        {
                            CompteRendu CR = lesPros[i].getLesComptesRendus()[j];
                            dataGridViewCR.Rows.Add(CR.getVisiteur(), CR.getProfessionnel(), CR.getDate());
                        }
                    }
                }
            }
        }

        private void remplirGroupBoxCRSelection(String unProfessionnel, String unVisiteur, String uneDate)
        {
            for (int i = 0; i < lesPros.Count; i++)
            {
                if (lesPros[i].getNom() == unProfessionnel)
                {
                    if (lesPros[i].getLesComptesRendus().Count > 0)
                    {
                        for (int j = 0; j < lesPros[i].getLesComptesRendus().Count; j++)
                        {
                            if (lesPros[i].getLesComptesRendus()[j].getVisiteur() == unVisiteur && lesPros[i].getLesComptesRendus()[j].getDate() == uneDate)//JJ/MM/ANNEE
                            {
                                CompteRendu CR = lesPros[i].getLesComptesRendus()[j];
                                textBoxVisiteurPro.Text = CR.getVisiteur();
                                textBoxTexteCR.Text = CR.getTexte();
                                dateTimePickerPro.Value = new DateTime(int.Parse(uneDate.Substring(6, 4)), int.Parse(uneDate.Substring(3, 2)), int.Parse(uneDate.Substring(0, 2)),0,0,0);
                            }
                        }
                    }
                }
            }
        }

        private void creerCompteRendu(String unProfessionnel, String unTexte, String unVisiteur, String uneDate)
        {
            CompteRendu CR = new CompteRendu(unProfessionnel, unTexte, unVisiteur, uneDate);
            for (int i = 0; i < lesPros.Count; i++)
            {
                if (lesPros[i].getNom() == unProfessionnel)
                {
                    for (int h = 0; h < lesPros[i].getLesComptesRendus().Count; h++)
                    {
                        if (lesPros[i].getLesComptesRendus()[h].getDate() == uneDate && lesPros[i].getLesComptesRendus()[h].getVisiteur() == unVisiteur)
                        {
                            throw new Exception("Existe déjà");
                        }
                    }
                    lesPros[i].getLesComptesRendus().Add(CR);
                    old.setTexte(unTexte);
                    DAOCompteRendu.creerCR(CR);
                }
            }
        }

        private void modifCompteRendu(String unProfessionnel, String unTexte, String unVisiteur, String uneDate)
        {
            CompteRendu CR = new CompteRendu(unProfessionnel, unTexte, unVisiteur, uneDate);
            for (int i = 0; i < lesPros.Count; i++)
            {
                if (lesPros[i].getNom() == unProfessionnel)
                {
                    lesPros[i].getLesComptesRendus().Remove(old);
                    lesPros[i].getLesComptesRendus().Add(CR);
                    old.setDate(null);
                    old.setTexte(null);
                    old.setVisiteur(null);
                    old.setProfessionnel(null);
                }
            }
        }

        private void supprCompteRendu(String unProfessionnel, String unTexte, String unVisiteur, String uneDate)
        {
            CompteRendu CR = new CompteRendu(unProfessionnel, unTexte, unVisiteur, uneDate);
            for (int i = 0; i < lesPros.Count; i++)
            {
                if (lesPros[i].getNom() == unProfessionnel)
                {
                    for (int h = 0; h < lesPros[i].getLesComptesRendus().Count; h++)
                    {
                        if (lesPros[i].getLesComptesRendus()[h].getDate() == uneDate && lesPros[i].getLesComptesRendus()[h].getVisiteur() == unVisiteur)
                        {
                            lesPros[i].getLesComptesRendus().Remove(CR);
                        }
                    }
                }
            }
        }

        private void activerCR(int choix)
        {
            if (choix == 1)
            {
                //remplirDatagridViewComptesRendus(comboBoxFiltrerPro.SelectedItem.ToString());
                buttonAnnulerCRPro.Enabled = false;
                buttonValiderCRPro.Enabled = false;
                buttonCRPro.Enabled = true;
                dataGridViewCR.Enabled = true;
                textBoxTexteCR.Enabled = false;
                textBoxVisiteurPro.Enabled = false;
                dateTimePickerPro.Enabled = false;
            }
            else if (choix == 2)
            {
                buttonAnnulerCRPro.Enabled = true;
                buttonValiderCRPro.Enabled = true;
                buttonCRPro.Enabled = false;
                dataGridViewCR.Enabled = false;
                textBoxTexteCR.Enabled = true;
                textBoxVisiteurPro.Enabled = true;
                dateTimePickerPro.Enabled = true;
            }
            else if (choix == 3)
            {
                buttonAnnulerCRPro.Enabled = true;
                buttonValiderCRPro.Enabled = true;
                buttonCRPro.Enabled = false;
                dataGridViewCR.Enabled = true;
                textBoxTexteCR.Enabled = true;
                textBoxVisiteurPro.Enabled = true;
                dateTimePickerPro.Enabled = true;
            }
        }

        private void viderGroupBoxCR()
        {
            dateTimePickerPro.Value.ToLocalTime();
            textBoxVisiteurPro.Clear();
            textBoxTexteCR.Clear();
            dataGridViewCR.Rows.Clear();
        }

        #endregion

        #endregion

        //gerer modification création cr et verifier si y en a pas un qui existe deja
        //base de données
        //créations d'objets via requete
        //mise en commun
    }
}
