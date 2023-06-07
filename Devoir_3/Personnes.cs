using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class Personnes
{
    private string nom;
    private string prenom;
    private int age; 
    private string sexe;
    private string adresse;
    private string telephone;
   

    // Constructeur par défaut
    public Personnes() => Console.WriteLine("Vous n'avez pas de nom, prenom, age, adresse, et meme un numero");

 

    // Constructeur avec tous les attributs en argument
    public Personnes(string nom, string prenom, int age, string sexe, string adresse, string telephone)
    {
        Nom = nom;
        Prenom = prenom;
        Age = age;
        Sexe = sexe;
        Adresse = adresse;
        Telephone = telephone;
    }

    // Méthodes Getter et Setter pour chaque attribut
    public string Nom
    {
        get { return nom; }
        set
        {
            if(value != string.Empty)
            {
                nom = value.Trim().Substring(0, 1).ToUpper() + value.Substring(1);
            }
        }
    }

    public string Prenom
    {
        get { return prenom; }
        set
        {
            if (value != string.Empty)
            {
                prenom = value.Trim().Substring(0, 1).ToUpper() + value.Substring(1);
            }
        }
    }

    public int Age
    {
        get { return age; }
        set
        {
            if (value != null)
            {
                if (value > 0 && value <= 100)
                    age = value;
                else
                    Console.WriteLine("L'âge doit être compris entre 1 et 100.");
            }
           
        }
    }

    public string Sexe
    {
        get { return sexe; }
        set { sexe = value; }
    }

    public string Adresse
    {
        get { return adresse; }
        set
        {
            if (value != string.Empty)
            {
                adresse = value.Substring(0, 1).ToUpper() + value.Substring(1);
            }
        }
    }

    public string Telephone
    {
        get { return telephone; }
        set { telephone = value; }
    }

    // Méthode pour afficher les informations de la personne
    public void Afficher()
    {
        Console.WriteLine($"{Nom} {Prenom} {Age} {Sexe} {Adresse} {Telephone}");

    }
}


