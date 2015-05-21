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
        List<Produit> lesProduits = new List<Produit>();
        ArrayList lesFamilles = Produit.listeFamille();
        List<Professionnel> lesPros = new List<Professionnel>();
        List<Commande> lesCommandes = new List<Commande>();
        List<Ligne> lesLignes = new List<Ligne>();

        public Principal()
        {
            InitializeComponent();
            buttonModifierProd.Visible = false;
            buttonSupprimerProd.Visible = false;
            buttonValiderProd.Visible = false;
            groupBoxCreationCom.Visible = false;
            comboBoxCommandesCom.Visible = false;
            majListeProduit();
            majListeFamilleProduit();
        }

        #region Procédures Evenementielles produit
        private void comboBoxSelectionProd_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBoxSelectionProd.SelectedIndex;

            for (int j = 0; j < lesFamilles.Count; j++)
            {
                if (lesProduits.ElementAt(i).getFamille().Equals(lesFamilles[j]))
                {
                    comboBoxFamilleProd.SelectedIndex = j;
                }
            }
            textBoxNumProd.Text = lesProduits.ElementAt(i).getNum().ToString();
            textBoxNomProd.Text = lesProduits.ElementAt(i).getNom();
            textBoxEffetProd.Text = lesProduits.ElementAt(i).getEffet();
            textBoxContreIndicProd.Text = lesProduits.ElementAt(i).getCI();
            textBoxPresentationProd.Text = lesProduits.ElementAt(i).getPresentation();
            textBoxDosageProd.Text = lesProduits.ElementAt(i).getDosage();
            textBoxPrixHTProd.Text = lesProduits.ElementAt(i).getPrixHT().ToString();
            textBoxPrixEchantProd.Text = lesProduits.ElementAt(i).getPrixEch().ToString();
            textBoxInteractionProd.Text = lesProduits.ElementAt(i).getInteraction();

            buttonModifierProd.Visible = true;
            buttonSupprimerProd.Visible = true;
        }

        private void buttonCreerProd_Click(object sender, EventArgs e)
        {
            mode = 1;
            textBoxVide();
            buttonValiderProd.Visible = true;
            buttonModifierProd.Visible = false;
            buttonSupprimerProd.Visible = false;
        }

        private void buttonValiderProd_Click(object sender, EventArgs e)
        {
            // Mode pour la créationd du médicament :
            if (mode == 1)
            {
                if (caseRemplies() == false)
                {
                    MessageBox.Show("Veuillez remplir toutes les zones de textes pour la création de l'objet (Ne pas oublier l'interaction)");
                }
                else
                {
                    if (textBoxPrixHTProd.Text.Contains(".") || textBoxPrixEchantProd.Text.Contains("."))
                    {
                        MessageBox.Show("Le Prix Hors Taxe et le prix des échantillons ne doivent pas contenir de points mais des virgules.");
                    }
                    else
                    {
                        try
                        {
                            Produit p = new Produit(int.Parse(textBoxNumProd.Text), textBoxNomProd.Text,
                            textBoxEffetProd.Text, textBoxContreIndicProd.Text, textBoxPresentationProd.Text,
                            textBoxDosageProd.Text, comboBoxFamilleProd.SelectedIndex, double.Parse(textBoxPrixHTProd.Text.Trim()),
                            double.Parse(textBoxPrixEchantProd.Text.Trim()), textBoxInteractionProd.Text);

                            string message = Produit.creerProduit(p);

                            MessageBox.Show(message);
                            majListeProduit();
                            buttonValiderProd.Visible = false;
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
                if (caseRemplies() == false)
                {
                    MessageBox.Show("Veuillez remplir toutes les zones de textes pour la modification de l'objet (Ne pas oublier l'interaction)");
                }
                else
                {
                    if (textBoxPrixHTProd.Text.Contains(".") || textBoxPrixEchantProd.Text.Contains("."))
                    {
                        MessageBox.Show("Le Prix Hors Taxe et le prix des échantillons ne doivent pas contenir de points mais des virgules.");
                    }
                    else
                    {
                        try
                        {
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
                                // Ne fais rien
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
                textBoxVide();
                buttonSupprimerProd.Visible = false;
                buttonModifierProd.Visible = false;
                comboBoxSelectionProd.SelectedIndex = 1;
            }
            else if (result == DialogResult.No)
            {
                // Ne fais rien
            }
        }

        private void buttonAnnulerProd_Click(object sender, EventArgs e)
        {
            textBoxVide();
            comboBoxSelectionProd.Enabled = true;
            textBoxNumProd.Enabled = true;
            buttonCreerProd.Visible = true;
            buttonSupprimerProd.Visible = false;
            buttonValiderProd.Visible = false;
            buttonModifierProd.Visible = false;
        }

        private void buttonModifierProd_Click(object sender, EventArgs e)
        {
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
            for (int i = 0; i < lesProduits.Count(); i++)
            {
                comboBoxSelectionProd.Items.Add(lesProduits.ElementAt(i).getNom());
            }

        }
        #endregion

        #region Procédure/Fonction produit

        public Boolean caseRemplies()
        {
            if (textBoxNumProd.Text == "" || textBoxNomProd.Text == "" || textBoxEffetProd.Text == "" || textBoxContreIndicProd.Text == "" ||
                textBoxPresentationProd.Text == "" || textBoxDosageProd.Text == "" || textBoxPrixHTProd.Text == "" || textBoxPrixEchantProd.Text == "" ||
                textBoxInteractionProd.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

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

        public void majListeFamilleProduit()
        {
            comboBoxFamilleProd.Items.Clear();

            for (int i = 0; i < lesFamilles.Count; i++)
            {
                comboBoxFamilleProd.Items.Add(lesFamilles[i]);
            }
        }

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

        #region fonctions&procédures commande
        private void chargerComboBoxProfessionnelsCom()
        {
            for (int i = 0; i < lesProfessionnels.Count; i++)
            {
                comboBoxProfessionnelsCom.Items.Add(lesProfessionnels.ElementAt(i));
            }
        }

        private void chargerComboBoxCommandesCom(Professionnel unPro)
        {
            for (int i = 0; i < lesCommandes.Count; i++)
			{
			    if (lesCommandes.ElementAt(i).getPro().Equals(unPro))
	            {
                    comboBoxCommandesCom.Items.Add(lesCommandes.ElementAt(i));
	            }
			}
        }

        private void chargercomboBoxProduitCreaCom(Commande uneComm)
        {
            for (int i = 0; i < lesProduits.Count; i++)
            {
                comboBoxProduitCreaCom.Items.Add(lesProduits.ElementAt(i));
            }
        }
        #endregion

        #region evenements commandes
        //charges les commandes lors de la selection d'un professionnel, 
        private void comboBoxProfessionnelsCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            chargerComboBoxCommandesCom(lesProfessionnels.ElementAt(comboBoxProfessionnelsCom.SelectedIndex));
        }
        //charges la date, etat et montant lors de la selection d'une commande
        private void comboBoxCommandesCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = comboBoxCommandesCom.SelectedIndex;

            labelDateAffCom.Text = lesCommandes.ElementAt(sel).getDate();
            labelEtatAffCom.Text = lesCommandes.ElementAt(sel).getEtat();
            labelMtTotalCom.Text = lesCommandes.ElementAt(sel).getMontantTotal().ToString();

            groupBoxCreationCom.Visible = true;

            lesLignes = lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).getLignes()
            dataGridViewAfficherCommande.Rows.Clear();
            for (int i = 0; i < lesLignes.Count; i++)
			{
                dataGridViewAfficherCommande.Rows.Add(
                    lesLignes[i].getNomProduit(),
                    lesLignes[i].getPrixProduit(),
                    lesLignes[i].getQte(), 
                    lesLignes[i].getTotal()
                    );
			}
        }

        //charge le prix lors de la selection d'un produit
        private void comboBoxProduitCreaCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = comboBoxProduitCreaCom.SelectedIndex;
            labelPrixAffCreaCom.Text = lesProduits.ElementAt(sel).getPrixHT().ToString();
        }

        //créé une ligne et l'ajoute à la comande puis rafraichis le tableau
        private void buttonValiderLigneCreaCom_Click(object sender, EventArgs e)
        {
            Ligne laLigne =new Ligne(lesProduits.ElementAt(comboBoxProduitCreaCom.SelectedIndex).getNum(), int.Parse(textBoxQteCreaCom.Text), int.Parse(labelTotalAffCreaCom.Text), lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).getIdCommande());
            lesCommandes.ElementAt(comboBoxCommandesCom.SelectedIndex).ajouterLigne(laLigne);
            dataGridViewAfficherCommande.Refresh();
        }

        //calcul le prix total au changement de quantitée, affiche une messagebox si invalide
        private void textBoxQteCreaCom_TextChanged(object sender, EventArgs e)
        {
            double qte;
            bool correct = double.TryParse(textBoxQteCreaCom.Text, out qte);
            if (correct)
            {
                labelTotalAffCreaCom.Text = (qte * lesProduits.ElementAt(comboBoxProduitCreaCom.SelectedIndex).getPrixHT()).ToString();
            }
            else
            {
                MessageBox.Show("Caractères invalides dans la quantitée : {0} ", textBoxQteCreaCom.Text);
            }
        }
        #endregion
    }
}
