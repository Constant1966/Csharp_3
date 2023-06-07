//using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Devoir_3
{
    public partial class Form1 : Form
    {

        ArrayList list = new ArrayList();
        //List<Personnes> personnesList = new List<Personnes>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            listView1.Columns.Add("Nom", 120);
            listView1.Columns.Add("Prénom", 120);
            listView1.Columns.Add("Age", 120);
            listView1.Columns.Add("Sexe", 120);
            listView1.Columns.Add("Adress", 120);
            listView1.Columns.Add("Tel:", 120);


            // Lire dans le fichier pour afficher dans le tableau apres la fermeture de l'application.

            AfficherListView();


        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtNom.Text) || string.IsNullOrEmpty(txtPrenom.Text) || string.IsNullOrEmpty(txtAge.Text) || string.IsNullOrEmpty(txtSexe.Text) || string.IsNullOrEmpty(txtAdresse.Text) || string.IsNullOrEmpty(txtTelephone.Text))
            {
                MessageBox.Show("Le champ ne doit pas être vide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (!int.TryParse(txtAge.Text, out int age) || age < 0 || age > 100)
                {
                    MessageBox.Show("Erreur : l'age doit être compris entre 0 et 100!", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Personnes personnes = new Personnes()
                {
                    Nom = txtNom.Text,
                    Prenom = txtPrenom.Text,
                    Age = age,
                    Sexe = txtSexe.Text,
                    Adresse = txtAdresse.Text,
                    Telephone = txtTelephone.Text
                };

                list.Add(personnes);

                ListViewItem item = new ListViewItem(personnes.Nom);
                item.SubItems.Add(personnes.Prenom);
                item.SubItems.Add(personnes.Age.ToString());
                item.SubItems.Add(personnes.Sexe.ToString());
                item.SubItems.Add(personnes.Adresse);
                item.SubItems.Add(personnes.Telephone.ToString());
                listView1.Items.Add(item);

                _ = MessageBox.Show("Les informations ont été ajoutés avec succès dans la liste.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);


                txtNom.Clear();
                txtPrenom.Clear();
                txtAge.Clear();
                txtSexe.Clear();
                txtAdresse.Clear();
                txtTelephone.Clear();
            }
        }


        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=databaseNew.db"))
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM Personnes WHERE nom=@Nom AND Prenom=@Prenom AND Age=@Age AND Sexe=@Sexe AND Adresse=@Adresse AND Telephone=@Telephone";

                    using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS personnes ( nom TEXT, prenom TEXT, age INTEGER, sexe TEXT, adresse TEXT, telephone TEXT)", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    bool hasNewInfo = false; // Ajout d'un booléen pour vérifier s'il y a de nouvelles informations

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        foreach (ListViewItem item in listView1.Items)
                        {
                            string nom = item.SubItems.Count > 0 ? item.SubItems[0].Text : "";
                            string prenom = item.SubItems.Count > 1 ? item.SubItems[1].Text : "";
                            int age = item.SubItems.Count > 2 ? int.Parse(item.SubItems[2].Text) : 0;
                            string sexe = item.SubItems.Count > 3 ? item.SubItems[3].Text : "";
                            string adresse = item.SubItems.Count > 4 ? item.SubItems[4].Text : "";
                            string telephone = item.SubItems.Count > 5 ? item.SubItems[5].Text : "";

                            command.Parameters.AddWithValue("@nom", nom);
                            command.Parameters.AddWithValue("@prenom", prenom);
                            command.Parameters.AddWithValue("@age", age);
                            command.Parameters.AddWithValue("@sexe", sexe);
                            command.Parameters.AddWithValue("@adresse", adresse);
                            command.Parameters.AddWithValue("@telephone", telephone);

                            int count = Convert.ToInt32(command.ExecuteScalar());
                            if (count > 0)
                            {
                            }
                            else
                            {
                                hasNewInfo = true; // S'il y a de nouvelles informations, on change la valeur du booléen à true

                                sql = "INSERT INTO Personnes (Nom, Prenom, Age, Sexe, Adresse, Telephone) VALUES (@Nom, @Prenom, @Age, @Sexe, @Adresse, @Telephone)";
                                using (SQLiteCommand insertCommand = new SQLiteCommand(sql, connection))
                                {
                                    insertCommand.Parameters.AddWithValue("@Nom", nom);
                                    insertCommand.Parameters.AddWithValue("@Prenom", prenom);
                                    insertCommand.Parameters.AddWithValue("@Age", age);
                                    insertCommand.Parameters.AddWithValue("@Sexe", sexe);
                                    insertCommand.Parameters.AddWithValue("@Adresse", adresse);
                                    insertCommand.Parameters.AddWithValue("@Telephone", telephone);

                                    int result = insertCommand.ExecuteNonQuery();
                                    if (result > 0)
                                    {
                                        MessageBox.Show("Informations enregistrées avec succès!", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                        }
                    }

                    if (!hasNewInfo) // Si aucune nouvelle information n'a été ajoutée, afficher un message d'erreur
                    {
                        MessageBox.Show("Aucune nouvelle information à ajouter!", "Erreur!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AfficherListView()
        {
            try
            {
                string connectionString = "Data Source=database.db";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM personnes";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Personnes personne = new Personnes()
                                {
                                    Nom = reader.GetString(0),
                                    Prenom = reader.GetString(1),
                                    Age = reader.GetInt32(2),
                                    Sexe = reader.GetString(3),
                                    Adresse = reader.GetString(4),
                                    Telephone = reader.GetString(5)
                                };

                                ListViewItem item = new ListViewItem(personne.Nom);
                                item.SubItems.Add(personne.Prenom);
                                item.SubItems.Add(personne.Age.ToString());
                                item.SubItems.Add(personne.Sexe);
                                item.SubItems.Add(personne.Adresse);
                                item.SubItems.Add(personne.Telephone);
                                listView1.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("La base de données n'existe pas !!! Vous devriez ajouter des informations avant ... " + ex.Message);
            }
        }
    }
}
