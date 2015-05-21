using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;

namespace GSB_backoffice_commercial
{
    public partial class Principal : Form
    {
        public int mode = 0;
        //Pour quoi pas 
        //List<Produit> lesProduits = Produit.listeProduit();
        //?

        List<Produit> lesProduits = new List<Produit>();
        ArrayList lesFamilles = Produit.listeFamille();

        public Principal()
        {
            InitializeComponent();
            buttonModifierProd.Visible = false;
            buttonSupprimerProd.Visible = false;
            buttonValiderProd.Visible = false;
            comboBoxCommandesCom.Visible = false;
            majListeProduit(); 
            majListeFamilleProduit();
            desactiverClient();
            remplirComboBoxFilterPro();
            foreach (Client client in lesPros)
            {
                client.initLesComptesRendus();
            }
        }

        #region Onglet Produits

        #region Procédures Evenementielles
        private void comboBoxSelectionProd_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Récupération des objets produits et placement dans la ComboBox.
            int i = comboBoxSelectionProd.SelectedIndex;

            for (int j = 0; j < lesFamilles.Count; j++)
            {
                if(lesProduits.ElementAt(i).getFamille().Equals(lesFamilles[j]))
                {
                    comboBoxFamilleProd.SelectedIndex = j;
                }
            }

            // Au choix du produit dans la comboBox remplissage de toutes les zones de textes avec les informations correspondantes au produit.
            textBoxNumProd.Text = lesProduits.ElementAt(i).getNum().ToString();
            textBoxNomProd.Text = lesProduits.ElementAt(i).getNom();
            textBoxEffetProd.Text = lesProduits.ElementAt(i).getEffet();
            textBoxContreIndicProd.Text = lesProduits.ElementAt(i).getCI();
            textBoxPresentationProd.Text = lesProduits.ElementAt(i).getPresentation();
            textBoxDosageProd.Text = lesProduits.ElementAt(i).getDosage();
            textBoxPrixHTProd.Text = lesProduits.ElementAt(i).getPrixHT().ToString();
            textBoxPrixEchantProd.Text = lesProduits.ElementAt(i).getPrixEch().ToString();
            textBoxInteractionProd.Text = lesProduits.ElementAt(i).getInteraction();

            //Fais apparaitre les boutons modifier et supprimer.
            buttonModifierProd.Visible = true;
            buttonSupprimerProd.Visible = true;
            buttonValiderProd.Visible = false;
        }

        private void buttonCreerProd_Click(object sender, EventArgs e)
        {
            // Vide les zones de textes, fais disparaitre les boutons Modifier et supprimer et fais apparaitre le bouton Valider.
            mode = 1;
            textBoxVide();
            comboBoxSelectionProd.Visible = false;
            buttonValiderProd.Visible = true;
            buttonModifierProd.Visible = false;
            buttonSupprimerProd.Visible = false;
            comboBoxSelectionProd.Enabled = true;
            textBoxNumProd.Enabled = true;
        }

        private void buttonValiderProd_Click(object sender, EventArgs e)
        {
            // Mode pour la création du médicament :
            if (mode == 1)
            {
                // Vérifie le remplissage de toutes les zones de textes
                if (caseRemplies() == false)
                {
                    MessageBox.Show("Veuillez remplir toutes les zones de textes pour la création de l'objet (Ne pas oublier l'interaction)");
                }
                else
                {
                    // Vérifie si les prix sont bien délimités par des virgules et non des points.
                    if (textBoxPrixHTProd.Text.Contains(".") || textBoxPrixEchantProd.Text.Contains("."))
                    {
                        MessageBox.Show("Le Prix Hors Taxe et le prix des échantillons ne doivent pas contenir de points mais des virgules.");
                    }
                    else
                    {
                        try
                        {
                            // Création du produit
                            Produit p = new Produit(int.Parse(textBoxNumProd.Text), textBoxNomProd.Text,
                            textBoxEffetProd.Text, textBoxContreIndicProd.Text, textBoxPresentationProd.Text,
                            textBoxDosageProd.Text, comboBoxFamilleProd.SelectedIndex, double.Parse(textBoxPrixHTProd.Text.Trim()),
                            double.Parse(textBoxPrixEchantProd.Text.Trim()), textBoxInteractionProd.Text);

                            string message = Produit.creerProduit(p);

                            MessageBox.Show(message);
                            majListeProduit();
                            comboBoxSelectionProd.Visible = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
                // Mode pour la modification : 
            else if (mode == 2)
            {
                // Vérifie le remplissage de toutes les zones de textes
                if (caseRemplies() == false)
                {
                    MessageBox.Show("Veuillez remplir toutes les zones de textes pour la modification de l'objet (Ne pas oublier l'interaction)");
                }
                else
                {
                    // Vérifie si les prix sont bien délimités par des virgules et non des points.
                    if (textBoxPrixHTProd.Text.Contains(".") || textBoxPrixEchantProd.Text.Contains("."))
                    {
                        MessageBox.Show("Le Prix Hors Taxe et le prix des échantillons ne doivent pas contenir de points mais des virgules.");
                    }
                    else
                    {
                        try
                        {
                            // Modification du produit
                            Produit p = new Produit(int.Parse(textBoxNumProd.Text), textBoxNomProd.Text,
                            textBoxEffetProd.Text, textBoxContreIndicProd.Text, textBoxPresentationProd.Text,
                            textBoxDosageProd.Text, comboBoxFamilleProd.SelectedIndex, double.Parse(textBoxPrixHTProd.Text.Trim()),
                            double.Parse(textBoxPrixEchantProd.Text.Trim()), textBoxInteractionProd.Text);

                            DialogResult result = MessageBox.Show("Etes-vous sûr de vouloir modifier le produit ?", "caption", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                string message = Produit.modifierProduit(p);

                                majListeProduit();

                                comboBoxSelectionProd.Enabled = true;
                                textBoxNumProd.Enabled = true;
                                buttonValiderProd.Visible = false;
                                buttonCreerProd.Visible = true;

                                MessageBox.Show(message);
                            }
                            else if (result == DialogResult.No)
                            {
                                // Ne fais rien si la personne clique sur non.
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void buttonSupprimerProd_Click(object sender, EventArgs e)
        {
            // Suppression du produit
            DialogResult result = MessageBox.Show("Etes-vous sûr de vouloir supprimer le produit ?", "caption", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                int i = comboBoxSelectionProd.SelectedIndex;
                try
                {
                    string message = Produit.supprimerProduit(lesProduits.ElementAt(i));
                    majListeProduit();
                    MessageBox.Show(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                // Vide les zones de textes et cache les boutons modifier et supprimer.
                textBoxVide();
                buttonSupprimerProd.Visible = false;
                buttonModifierProd.Visible = false;
                comboBoxSelectionProd.SelectedIndex = 1;
            }
            else if (result == DialogResult.No)
            {
                // Ne fais rien si la personne clique sur non.
            } 
        }

        private void buttonAnnulerProd_Click(object sender, EventArgs e)
        {
            // Vide toutes les zones de textes
            textBoxVide();
            comboBoxSelectionProd.Enabled = true;
            textBoxNumProd.Enabled = true;
            buttonCreerProd.Visible = true;
            buttonSupprimerProd.Visible = false;
            buttonValiderProd.Visible = false;
            buttonModifierProd.Visible = false;
            comboBoxSelectionProd.Visible = true;
        }

        private void buttonModifierProd_Click(object sender, EventArgs e)
        {
            // Au clique du bouton modifier, les cases inchangables se grisent (Id Produit).
            mode = 2;
            textBoxNumProd.Text = lesProduits.ElementAt(comboBoxSelectionProd.SelectedIndex).getNum().ToString();
            textBoxNumProd.Enabled = false;
            comboBoxSelectionProd.Enabled = false;
            buttonValiderProd.Visible = true;
            buttonSupprimerProd.Visible = false;
            buttonCreerProd.Visible = true;
            buttonModifierProd.Visible = false;
        }

        private void dataGridViewProd_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Fais apparaitre l'intéraction au clique sur une ligne de produit.
            int test = int.Parse(dataGridViewProd[0, e.RowIndex].Value.ToString());
            for (int i = 0; i < lesProduits.Count(); i++)
            {
                if (lesProduits[i].getNum() == test)
                {
                    textBoxInteractionProd.Text = lesProduits[i].getInteraction();
                }
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            // Ajoute les produits à la ComboBox à l'ouverture de l'onglet.
            for (int i = 0; i < lesProduits.Count(); i++)
            {
                comboBoxSelectionProd.Items.Add(lesProduits.ElementAt(i).getNom());
            }

        }
        #endregion

        #region Procédure/Fonction

        // Vérifie que toute sle scases sont remplies
        public Boolean caseRemplies()
        {
            if (textBoxNumProd.Text == "" || textBoxNomProd.Text == "" || textBoxEffetProd.Text == "" || textBoxContreIndicProd.Text == "" ||
                textBoxPresentationProd.Text == "" || textBoxDosageProd.Text == "" || textBoxPrixHTProd.Text == "" || textBoxPrixEchantProd.Text == "" || 
                textBoxInteractionProd.Text =="")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Met à jour le dataGriedView et la ComboBox.
        public void majListeProduit()
        {
            lesProduits = Produit.listeProduit();

            comboBoxSelectionProd.Items.Clear();
            for (int i = 0; i < lesProduits.Count; i++)
            {
                comboBoxSelectionProd.Items.Add(lesProduits.ElementAt(i).getNom());
            }

            dataGridViewProd.Rows.Clear();
            for (int i = 0; i < lesProduits.Count; i++)
            {
                dataGridViewProd.Rows.Add(lesProduits.ElementAt(i).getNum(), lesProduits.ElementAt(i).getNom(),
                    lesProduits.ElementAt(i).getEffet(), lesProduits.ElementAt(i).getCI(), lesProduits.ElementAt(i).getPresentation(),
                    lesProduits.ElementAt(i).getDosage(), lesProduits.ElementAt(i).getFamille(), lesProduits.ElementAt(i).getPrixHT(),
                    lesProduits.ElementAt(i).getPrixEch());
            }
        }

        // Met à jour la ComboBox de famille des produits.
        public void majListeFamilleProduit()
        {
            comboBoxFamilleProd.Items.Clear();

            for (int i = 0; i < lesFamilles.Count; i++)
            {
                comboBoxFamilleProd.Items.Add(lesFamilles[i]);
            }
        }

        // Vide toutes les TextBox.
        public void textBoxVide()
        {
            textBoxNumProd.Text = "";
            textBoxNomProd.Text = "";
            textBoxEffetProd.Text = "";
            textBoxContreIndicProd.Text = "";
            textBoxPresentationProd.Text = "";
            textBoxDosageProd.Text = "";
            textBoxPrixHTProd.Text = "";
            textBoxPrixEchantProd.Text = "";
            textBoxInteractionProd.Text = "";
        }
        #endregion

        #endregion

        #region Onglet Clients / Prospects

        #region variables

        List<Client> lesPros = DAOClient.listeClient();
        CompteRendu old = new CompteRendu("","","","");//Ancien Compte Rendu qui sera potentiellment modifié

        #endregion

        #region evenements professionnels

        private void comboBoxFiltrerPro_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            activerClient(2);
            viderGroupBoxCR();
            activerCR(1);
            remplirTextBoxSelectionClients(comboBoxFiltrerPro.SelectedItem.ToString());
            remplirDatagridViewComptesRendus(comboBoxFiltrerPro.SelectedItem.ToString());
        }

        private void buttonModifierPro_Click_1(object sender, EventArgs e)
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

        private void buttonSupprimerPro_Click_1(object sender, EventArgs e)
        {
            supprimerProfessionnel(textBoxCodePro.Text);
            viderTextBoxPro();
            desactiverClient();
            remplirComboBoxFilterPro();
            comboBoxFiltrerPro.Text = "";
            MessageBox.Show("Professionel supprimé avec succès");
        }

        private void buttonValiderPro_Click_1(object sender, EventArgs e)
        {
            try
            {
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

        private void buttonAnnulerPro_Click_1(object sender, EventArgs e)
        {
            desactiverClient();
            viderTextBoxPro();
            viderGroupBoxCR();

        }

        private void buttonCreerPro_Click_1(object sender, EventArgs e)
        {
            activerClient(1);
        }

        #endregion

        #region fonctions professionels

        //rempli la combobox avec les professionnels trouvés dans la base
        private void remplirComboBoxFilterPro()
        {
            comboBoxFiltrerPro.Items.Clear();
            for (int i = 0; i < lesPros.Count; i++)
            {
                comboBoxFiltrerPro.Items.Add(lesPros[i].getNom());
            }
        }

        //Rempli les champs des informations correspondant au client séléctionné dans la combobox
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
                    remplirDatagridViewComptesRendus(lesPros[i].getNom());
                }
            }
        }

        //active/désactive les champs et groupbox en fonction du choix de l'utilisateur
        private void activerClient(int choix)
        {
            if (choix == 1)//Correspond au choix de créer un nouveau professionnel
            {
                buttonValiderPro.Enabled = true;
                groupBoxPro.Enabled = true;
                textBoxCodePro.Enabled = true;
                buttonModifierPro.Enabled = false;
                buttonSupprimerPro.Enabled = false;
                comboBoxFiltrerPro.Enabled = false;
                buttonModifierPro.Enabled = false;

            }
            else if (choix == 2)//Correspond au choix de modifier un nouveau professionnel
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

        //réinitialise l'onglet clients + compte rendu
        private void desactiverClient()
        {
            groupBoxPro.Enabled = false;
            groupBoxCompteRenduPro.Enabled = false;
            comboBoxFiltrerPro.Enabled = true;
            buttonCreerPro.Enabled = true;
        }

        //crée un nouveau client
        private void creerClient(String unCode, String unNom, String uneRaisonSociale, String uneAdresse,
            String unType, String unTelephone, String unMail)
        {
            Client unClient = new Client(unCode, unNom, uneRaisonSociale, uneAdresse, unType, unTelephone, unMail);
            if (lesPros.Contains(unClient) == true)
            {
                MessageBox.Show("Un client similaire existe déjà");
            }
            else
            {
                DAOClient.creerClient(unClient);
                lesPros.Add(unClient);
                remplirComboBoxFilterPro();
            }
        }

        //réinitialise la groupbox des clients
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

        //modifie un client
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

        //supprime un client
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

        private void buttonCRPro_Click_1(object sender, EventArgs e)
        {
            activerCR(2);
            old.setProfessionnel("");
        }

        private void dataGridViewCR_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            activerCR(3);
            remplirGroupBoxCRSelection(dataGridViewCR[1, e.RowIndex].Value.ToString(),
                dataGridViewCR[0, e.RowIndex].Value.ToString(), dataGridViewCR[2, e.RowIndex].Value.ToString());

            old.setVisiteur(dataGridViewCR[0, e.RowIndex].Value.ToString());
            old.setDate(dataGridViewCR[2, e.RowIndex].Value.ToString());
            old.setProfessionnel(comboBoxFiltrerPro.SelectedItem.ToString());
        }

        private void buttonValiderCRPro_Click_1(object sender, EventArgs e)
        {
            if (old.getProfessionnel() == "")
            {
                creerCompteRendu(comboBoxFiltrerPro.SelectedItem.ToString(), textBoxTexteCR.Text,
                           textBoxVisiteurPro.Text, dateTimePickerPro.Value.ToShortDateString());
            }
            else
            {
                modifCompteRendu(comboBoxFiltrerPro.SelectedItem.ToString(), textBoxTexteCR.Text,
                           textBoxVisiteurPro.Text, dateTimePickerPro.Value.ToShortDateString());
            }
            activerCR(1);
        }

        private void buttonAnnulerCRPro_Click_1(object sender, EventArgs e)
        {
            activerCR(1);
        }

        private void buttonSupprCRPro_Click(object sender, EventArgs e)
        {
            supprCompteRendu(comboBoxFiltrerPro.SelectedItem.ToString(), textBoxTexteCR.Text,
                           textBoxVisiteurPro.Text, dateTimePickerPro.Value.ToShortDateString());
            activerCR(1);
        }

        #endregion

        #region fonctions Compte-Rendu

        //rempli la datagridView avec les comptes rendus du professionnel selectionné
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

        //rempli les champs avec les données du compte rendu séléctionné
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
                                dateTimePickerPro.Value = new DateTime(int.Parse(uneDate.Substring(6, 4)), int.Parse(uneDate.Substring(3, 2)), int.Parse(uneDate.Substring(0, 2)), 0, 0, 0);
                                old.setProfessionnel(lesPros[i].getNom());
                                old.setTexte(CR.getTexte());
                                old.setDate(uneDate);
                                old.setVisiteur(unVisiteur);
                            }
                        }
                    }
                }
            }
        }

        //crée un compte rendu
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

        //modifie un compte rendu
        private void modifCompteRendu(String unProfessionnel, String unTexte, String unVisiteur, String uneDate)
        {
            CompteRendu oldCR = old;
            CompteRendu CR = new CompteRendu(unProfessionnel, unTexte, unVisiteur, uneDate);
            for (int i = 0; i < lesPros.Count; i++)
            {
                if (lesPros[i].getNom() == unProfessionnel)
                {
                    DAOCompteRendu.modifierCR(CR, oldCR);
                    lesPros[i].initLesComptesRendus();
                    old.setProfessionnel("");
                }
            }
        }

        //supprime un compte rendu
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
                            DAOCompteRendu.supprimerCR(CR);
                            lesPros[i].initLesComptesRendus();
                            textBoxTexteCR.Clear();
                            textBoxVisiteurPro.Clear();
                            dateTimePickerPro.ResetText();
                        }
                    }
                }
            }
        }

        //active/désactive les champs et groupbox en fonction du choix de l'utilisateur.
        private void activerCR(int choix)
        {
            if (choix == 1)//initialise la partie compte rendu pour le professionnel séléctionné
            {
                remplirDatagridViewComptesRendus(comboBoxFiltrerPro.SelectedItem.ToString());
                buttonAnnulerCRPro.Enabled = false;
                buttonValiderCRPro.Enabled = false;
                buttonCRPro.Enabled = true;
                buttonSupprCRPro.Enabled = false;
                dataGridViewCR.Enabled = true;
                textBoxTexteCR.Enabled = false;
                textBoxVisiteurPro.Enabled = false;
                dateTimePickerPro.Enabled = false;
            }
            else if (choix == 2)//Correspond au choix de créer un compte rendu
            {
                buttonAnnulerCRPro.Enabled = true;
                buttonValiderCRPro.Enabled = true;
                buttonCRPro.Enabled = false;
                dataGridViewCR.Enabled = false;
                textBoxTexteCR.Enabled = true;
                textBoxTexteCR.Clear();
                textBoxVisiteurPro.Enabled = true;
                textBoxVisiteurPro.Clear();
                dateTimePickerPro.Enabled = true;
                dateTimePickerPro.ResetText();
            }
            else if (choix == 3)//Correspond au choix d'afficher un compte rendu
            {
                buttonAnnulerCRPro.Enabled = true;
                buttonValiderCRPro.Enabled = true;
                buttonCRPro.Enabled = false;
                buttonSupprCRPro.Enabled = true;
                dataGridViewCR.Enabled = true;
                textBoxTexteCR.Enabled = true;
                textBoxVisiteurPro.Enabled = true;
                dateTimePickerPro.Enabled = true;
            }
        }

        //réinitialise la group box des comptes-rendus
        private void viderGroupBoxCR()
        {
            dateTimePickerPro.Value.ToLocalTime();
            textBoxVisiteurPro.Clear();
            textBoxTexteCR.Clear();
            dataGridViewCR.Rows.Clear();
        }

        #endregion


        #endregion

        #region Onglet commandes
        #region variables et initialisations
        List<Commande> lesCommandes = Commande.listeCommandes();

        //Vide jusqu'a la sélection d'une commande
        List<Ligne> lesLignes = new List<Ligne>();
        #endregion
        #region fonctions&procédures
        //Affecte les professionnels à la combobox professionnels
        private void chargerComboBoxProfessionnelsCom()
        {
            comboBoxProfessionnelsCom.Items.Clear();
            comboBoxProfessionnelsCreaCom.Items.Clear();
            foreach(Client leClient in lesPros){
                comboBoxProfessionnelsCom.Items.Add(leClient.getNom());
                comboBoxProfessionnelsCreaCom.Items.Add(leClient.getNom());
            }
        }

        //affecte les commandes d'un client à la combobox commandes, partie affichage
        private void chargerComboBoxCommandesCom(Client unPro)
        {
            comboBoxCommandesCom.Items.Clear();
            foreach(Commande laCommande in lesCommandes){
                if (laCommande.getPro().Equals(unPro.getCode()))
	            {
                    comboBoxCommandesCom.Items.Add(laCommande.getIdCommande());
	            }
			}
            comboBoxCommandesCom.Visible = true;
        }

        //affecte les produits à la combobox Produits, partie création
        private void chargercomboBoxProduitCreaCom()
        {
            comboBoxProduitCreaCom.Items.Clear();
            foreach(Produit leProduit in lesProduits){
                comboBoxProduitCreaCom.Items.Add(leProduit.getNom());
            }
        }

        //affecte les lignes à la combobox Lignes; partie modification
        private void chargercomboBoxLigneModifCom()
        {
            comboBoxLigneModifCom.Items.Clear();
            foreach (Ligne laLigne in lesLignes){
                comboBoxLigneModifCom.Items.Add(laLigne);
            }
        }

        //Affecte les lignes au datagridview
        private void chargerDataGridView(Commande uneCom){
            lesLignes = uneCom.getLignes();
            dataGridViewAfficherCommande.Rows.Clear();
            foreach(Ligne laLigne in lesLignes){
                dataGridViewAfficherCommande.Rows.Add(
                    laLigne.getNomProduit(),
                    laLigne.getPrixProduit(),
                    laLigne.getQte(),
                    laLigne.getTotal()
                    );
            }
        }
        #endregion

        #region evenements

        //Charge les clients lors de la sélection de l'onglet
        private void tabPage3_Click(object sender, EventArgs e)
        {
            chargerComboBoxProfessionnelsCom();
        }

        //Affiche la groupBox de Création après clic sur le bouton Créer, partie affichage
        private void buttonCreaCom_Click(object sender, EventArgs e)
        {
            groupBoxCreationCom.Enabled = true;
            groupBoxModifCom.Enabled = false;
            chargercomboBoxProduitCreaCom();
        }

        //Charges les commandes lors de la sélection d'un professionnel, partie affichage
        private void comboBoxProfessionnelsCom_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            chargerComboBoxCommandesCom(lesPros.ElementAt(comboBoxProfessionnelsCom.SelectedIndex));
            //TODO : reinitialiser tous les champs
            dataGridViewAfficherCommande.Rows.Clear();
        }

        //Charge la date, l'état et montant lors de la sélection d'une commande, partie affichage
        private void comboBoxCommandesCom_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            labelDateAffCom.Text = lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).getDate().ToString();
            labelEtatAffCom.Text = lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).getEtat();
            labelTotalCom.Text = lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).getMontantTotal().ToString();

            chargerDataGridView(lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex));
        }


        //Débloque la groupbox modification, bloque la création et affecte la combobox lignes, partie affichage
        private void buttonModifCom_Click(object sender, EventArgs e)
        {
            groupBoxModifCom.Enabled = true;
            groupBoxCreationCom.Enabled = false;
            chargercomboBoxLigneModifCom();
        }


        //Supprime la commande, partie affichage
        private void buttonSupprCom_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sur de vouloir supprimer la commande?", "Supprimer la commande?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).supprimerCommande();
            }
            dataGridViewAfficherCommande.Refresh();
        }

        //Charge le prix lors de la selection d'un produit dans combobox, partie création
        private void comboBoxProduitCreaCom_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            labelPrixAffCreaCom.Text = lesProduits.ElementAt(comboBoxProduitCreaCom.SelectedIndex).getPrixHT().ToString();
        }


        //Charge le prix lors de la selection d'un produit dans combobox, partie modification
        private void comboBoxProduitModifCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelPrixAffModifCom.Text = lesProduits.ElementAt(comboBoxProduitModifCom.SelectedIndex).getPrixHT().ToString();
        }

        //Créé une ligne et l'ajoute à la comande puis rafraichis le tableau, partie création
        private void buttonValiderLigneCreaCom_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Commande.idNouvelleCommande().ToString());
            Ligne laLigne = new Ligne(
                lesProduits.ElementAt(comboBoxProduitCreaCom.SelectedIndex).getNum(),
                int.Parse(textBoxQteCreaCom.Text),
                double.Parse(labelTotalAffCreaCom.Text),
                Commande.idNouvelleCommande()
            );
            //TODO: Ajouter lignes au tableau sans créer de commande, via liste ligne temp
            lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).ajouterLigne(laLigne);
            chargerDataGridView(lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex));
        }

        //Calcule le prix total au changement de la TextBox quantitée, affiche une messagebox si non numérique, partie création
        private void textBoxQteCreaCom_TextChanged(object sender, EventArgs e)
        {
            if (textBoxQteCreaCom.Text != "")
            {
                double qte;
                if (double.TryParse(textBoxQteCreaCom.Text, out qte))
                {
                    labelTotalAffCreaCom.Text = (qte * lesProduits.ElementAt(comboBoxProduitCreaCom.SelectedIndex).getPrixHT()).ToString();
                }
                else
                {
                    MessageBox.Show("Caractères non numériques dans la quantitée : " + textBoxQteCreaCom.Text);
                    textBoxQteCreaCom.ResetText();
                }
            }
        }

        //Calcule le prix total au changement de la TextBox quantitée, affiche une messagebox si non numérique, partie modification
        private void textBoxQteModifCom_TextChanged(object sender, EventArgs e)
        {
            if (textBoxQteModifCom.Text != "")
            {
                double qte;
                if (double.TryParse(textBoxQteModifCom.Text, out qte)){
                    labelTotalAffModifCom.Text = (qte * lesProduits.ElementAt(comboBoxProduitModifCom.SelectedIndex).getPrixHT()).ToString();
                }
                else{
                    MessageBox.Show("Caractères non numériques dans la quantitée : " + textBoxQteModifCom.Text);
                    textBoxQteModifCom.ResetText();
                }
            }
        }

        //Selection de la ligne dans la combobox et chargement des données de la ligne, partie modification
        private void comboBoxLigneModifCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(Produit leProduit in lesProduits){
                if(lesLignes.ElementAt(comboBoxLigneModifCom.SelectedIndex).getIdProduit().Equals(leProduit.getNum())){
                    comboBoxProduitModifCom.SelectedIndex = comboBoxProduitModifCom.FindString(leProduit.getNom());
                }
            }

        }

        //Supprime la ligne, partie modification
        private void buttonSupprLigneModifCom_Click(object sender, EventArgs e)
        {
            lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).supprimerLigne(lesLignes.ElementAt(comboBoxLigneModifCom.SelectedIndex));
            dataGridViewAfficherCommande.Refresh();
        }

        //Créé ou modifie une ligne pour la commande selectionnée, gère l'ajout de quantité, partie modification
        private void buttonValiderLigneModifCom_Click(object sender, EventArgs e)
        {
            //check l'existance d'une ligne avec le produit
            bool check = false;
            List<Ligne> lesLignesCommandeSelect = lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).getLignes();
            foreach(Ligne laLigne in lesLignesCommandeSelect){
                foreach (Produit leProduit in lesProduits) {
                    if (lesProduits.ElementAt(comboBoxProduitModifCom.SelectedIndex).getNum().Equals(laLigne.getIdProduit())){
                        check = true;
                    }
                }
            }
            //Produit existant, donc modification
            if (check){
                Ligne laLignePrécédente = lesLignes.ElementAt(comboBoxLigneModifCom.SelectedIndex);
                int nouvelleQuant = int.Parse(textBoxQteModifCom.Text) + laLignePrécédente.getQte();
                Ligne laLigne = new Ligne(comboBoxProduitModifCom.SelectedIndex,
                                            nouvelleQuant,
                                            lesProduits.ElementAt(comboBoxProduitModifCom.SelectedIndex).getPrixHT() * nouvelleQuant,
                                            lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).getIdCommande()
                                        );
                lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).supprimerLigne(laLignePrécédente);
                lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).ajouterLigne(laLigne);
            }
            //Produit non existant, création
            else{
                Ligne laLigne = new Ligne(comboBoxProduitModifCom.SelectedIndex,
                                            int.Parse(textBoxQteModifCom.Text),
                                            lesProduits.ElementAt(comboBoxProduitModifCom.SelectedIndex).getPrixHT()*int.Parse(textBoxQteModifCom.Text),
                                            lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).getIdCommande()
                                        );
                lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).ajouterLigne(laLigne);
            }

        }
        
        #endregion
        #endregion

    }
}
