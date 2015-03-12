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

        public Principal()
        {
            InitializeComponent();
            buttonModifierProd.Visible = false;
            buttonSupprimerProd.Visible = false;
            buttonValiderProd.Visible = false;
            majListeProduit();
            majListeFamilleProduit();
        }

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
    }
}
