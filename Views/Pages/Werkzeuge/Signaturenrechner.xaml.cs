
using neuesmodell.Services;
using neuesmodell.Views.Pages.Werkzeuge;

namespace neuesmodell.Views.Pages.Werkzeuge;

public sealed partial class Signaturenrechner : Page
{
    public Signaturenrechner()
    {
        this.InitializeComponent();
    }

    private void signaturrechner_combobox_Loaded(object sender, RoutedEventArgs e)
    {
        signaturrechner_combobox.SelectedItem = signaturrechner_combobox.Items[0]; // Wählt das erste Element (Signatur zu Pagina)
    }
    private void Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (signaturrechner_combobox.SelectedItem != null)
        {
            string selectedItem = (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString();
            if (selectedItem == "Signatur zu Pagina")
            {
                textboxsigltr1.Visibility = Visibility.Visible;
                textboxsignmbr1.Visibility = Visibility.Visible;
                textboxformat1.Visibility = Visibility.Visible;
                textboxformat2.Visibility = Visibility.Collapsed;
                textboxformat3.Visibility = Visibility.Collapsed;
                textboxseitenzahl2.Visibility = Visibility.Collapsed;
                textboxalphabet3.Visibility = Visibility.Collapsed;
                textboxbogenzahl3.Visibility = Visibility.Collapsed;

                resultoutput.Text = "Die Bogensignatur entspricht der Seitenzahl";
            }
            else if (selectedItem == "Pagina zu Signatur")
            {
                textboxsigltr1.Visibility = Visibility.Collapsed;
                textboxsignmbr1.Visibility = Visibility.Collapsed;
                textboxformat1.Visibility = Visibility.Collapsed;
                textboxformat2.Visibility = Visibility.Visible;
                textboxformat3.Visibility = Visibility.Collapsed;
                textboxseitenzahl2.Visibility = Visibility.Visible;
                textboxalphabet3.Visibility = Visibility.Collapsed;
                textboxbogenzahl3.Visibility = Visibility.Collapsed;

                resultoutput.Text = "Die Seitenzahl entspricht der Bogensignatur";
            }
            else if (selectedItem == "Bogenzahl zu Seitenzahl")
            {
                textboxsigltr1.Visibility = Visibility.Collapsed;
                textboxsignmbr1.Visibility = Visibility.Collapsed;
                textboxformat1.Visibility = Visibility.Collapsed;
                textboxformat2.Visibility = Visibility.Collapsed;
                textboxformat3.Visibility = Visibility.Visible;
                textboxseitenzahl2.Visibility = Visibility.Collapsed;
                textboxalphabet3.Visibility = Visibility.Visible;
                textboxbogenzahl3.Visibility = Visibility.Visible;

                resultoutput.Text = "Die Alphabet- und Bogenzahl ergibt einen Seitenumfang von  Seiten";
            }
        }
    }

    public static string Converter(string sigLtr)
    {
        if (Dictionaries.LetterNumberDict.ContainsKey(sigLtr))
        {
            Console.WriteLine($"Der Signaturbuchstabe entspricht der Zahl {Dictionaries.LetterNumberDict[sigLtr]}");
            return Dictionaries.LetterNumberDict[sigLtr];
        }
        else
        {
            Console.WriteLine("Signaturbuchstabe nicht erfasst");
            return null;
        }
    }

    public static string FormatConverter(string format)
    {
        if (Dictionaries.FormatDict.ContainsKey(format))
        {
            Console.WriteLine($"Das Format entspricht der Zahl {Dictionaries.FormatDict[format]}");
            return Dictionaries.FormatDict[format];
        }
        else
        {
            Console.WriteLine(" Format nicht erfasst");
            return null;
        }
    }

    public static int SigToPag(string sigLtr, string sigNmb, string format)
    {
        var sigLtr1 = Converter(sigLtr);
        var format1 = FormatConverter(format);

        if (sigLtr1 != null && format1 != null)
        {
            int seite = int.Parse(sigLtr1) * int.Parse(format1) + int.Parse(sigNmb) + (int.Parse(sigNmb) - 1);
            Console.WriteLine($"Berechnetes Ergebnis für Seite: {seite}");
            return seite;
        }
        else
        {
            Console.WriteLine("Fehler bei der Berechnung, ungültige Eingabe.");
            return -1;
        }
    }

    public static string NumberConverter(string sigltr)
    {
        if (Dictionaries.NumberLetterdict.ContainsKey(sigltr))
        {
            return Dictionaries.NumberLetterdict[sigltr];
        }
        else
        {
            return null;
        }
    }

    public static string SigNumberConverter(string signmb)
    {
        if (Dictionaries.SigNumber.ContainsKey(signmb))
        {
            return Dictionaries.SigNumber[signmb];
        }
        else
        {
            return null;
        }
    }

    public static string PagToSig(string pag, string format)
    {
        var format1 = FormatConverter(format);
        var sig_ltr = NumberConverter((int.Parse(pag) / int.Parse(format1)).ToString());
        var sig_nmb = SigNumberConverter((int.Parse(pag) % int.Parse(format1)).ToString());
        var x = int.Parse(pag) / int.Parse(format1) - 1;
        var sig_ltr1 = NumberConverter(x.ToString());
        var sig_nmb1 = SigNumberConverter(int.Parse(format1).ToString());

        if (sig_ltr != null && sig_nmb != null)
        {
            string sig = $"{sig_ltr}{sig_nmb}";
            return sig;
        }
        else if (sig_ltr != null && sig_nmb == null)
        {
            string sig1 = $"{sig_ltr1}{sig_nmb1}";
            return sig1;
        }
        else
        {
            return null;
        }
    }

    public static int AlphabetToPage(string alphabet, string bogen, string format)
    {
        var format1 = FormatConverter(format);

        int page = (int.Parse(alphabet) * 23 + int.Parse(bogen)) * int.Parse(format1);

        if (page != null)
        {
            return page;
        }
        else
        {
            return -1;
        }
    }

    private void BerechnenButton_Click(object sender, RoutedEventArgs e)
    {
        // Überprüfen, ob "Signatur zu Pagina" ausgewählt wurde
        if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Signatur zu Pagina")
        {
            // Extrahiere Werte aus den Textboxen
            string sigLtr = textboxsigltr1.Text;
            string sigNmb = textboxsignmbr1.Text;
            string format = textboxformat1.Text;

            // Führe die Berechnung durch und erhalte das Ergebnis
            int seite = SigToPag(sigLtr, sigNmb, format);

            // Zeige das Ergebnis in der Ergebnis-Textbox an
            resultoutput.Text = $"Die Bogensignatur entspricht der Seitenzahl {seite.ToString()}";
        }
        else if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Pagina zu Signatur")
        {
            string pag = textboxseitenzahl2.Text;
            string format = textboxformat2.Text;

            string signatur = PagToSig(pag, format);

            resultoutput.Text = $"Die Seitenzahl entspricht der Bogensignatur {signatur.ToString()}";
        }
        else if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Bogenzahl zu Seitenzahl")
        {
            string alphabet = textboxalphabet3.Text;
            string bogen = textboxbogenzahl3.Text;
            string format = textboxformat3.Text;

            int page = AlphabetToPage(alphabet, bogen, format);

            resultoutput.Text = $"Die Alphabet- und Bogenzahl ergibt einen Seitenumfang von {page.ToString()} Seiten";
        }
        else
        {
            // Wenn eine andere Option als "Signatur zu Pagina" ausgewählt wurde, gib eine Fehlermeldung aus
            resultoutput.Text = "Fehler: Wählen Sie 'Signatur zu Pagina' aus";
        }
    }

    private void LöschenButton_Click(object sender, RoutedEventArgs e)
    {
        if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Signatur zu Pagina")
        {
            textboxsigltr1.Text = "";
            textboxsignmbr1.Text = "";
            textboxformat1.Text = "";

            resultoutput.Text = "Die Bogensignatur entspricht der Seitenzahl";
        }
        else if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Pagina zu Signatur")
        {
            textboxseitenzahl2.Text = "";
            textboxformat2.Text = "";

            resultoutput.Text = "Die Seitenzahl entspricht der Bogensignatur";
        }
        else if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Bogenzahl zu Seitenzahl")
        {
            textboxalphabet3.Text = "";
            textboxbogenzahl3.Text = "";
            textboxformat3.Text = "";

            resultoutput.Text = "Die Alphabet- und Bogenzahl ergibt einen Seitenumfang von  Seiten";
        }
    }
}

