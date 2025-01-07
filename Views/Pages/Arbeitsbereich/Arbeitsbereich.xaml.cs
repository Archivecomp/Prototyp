
using HtmlAgilityPack;
using Microsoft.UI.Xaml.Documents;
using neuesmodell.Services;
using Microsoft.UI.Xaml.Shapes;
using Newtonsoft.Json.Linq;
using Windows.System;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;
using Castle.Components.DictionaryAdapter.Xml;
using System.Diagnostics.Eventing.Reader;
using Microsoft.UI.Xaml.Controls;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using System.Reflection;
using System.Text.Json;
using WinUICommunity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;


namespace neuesmodell.Views.Pages.Arbeitsbereich;

public sealed partial class Arbeitsbereich : Page
{

    private readonly string fileName = "data.json";

    private string selectedName;

    public Arbeitsbereich()
    {
        this.InitializeComponent();
        _ = LoadHyperlinksAsync();

    }


    // Lädt die Namen aus der JSON-Datei und erstellt Hyperlinks
    private async Task LoadHyperlinksAsync()
    {
        var entries = await LoadDataAsync();

        if (entries.Count > 0)
        {
            // Sortiere die Namen alphabetisch und gruppiere nach Anfangsbuchstaben
            var groupedNames = entries.Keys
                                       .OrderBy(name => name)
                                       .GroupBy(name => name[0].ToString().ToUpperInvariant());

            HyperlinkContainer.Children.Clear(); // Entferne bestehende Einträge

            foreach (var group in groupedNames)
            {
                // Überschrift für den Anfangsbuchstaben
                HyperlinkContainer.Children.Add(new TextBlock
                {
                    Text = group.Key,
                    FontSize = 20,
                    FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                    Margin = new Thickness(-10, 10, 0, 5)
                });

                // Linie unter der Überschrift
                HyperlinkContainer.Children.Add(new Rectangle
                {
                    Height = 2,
                    Fill = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray),
                    Margin = new Thickness(-10, 0, 0, 10)
                });

                // Füge die Namen als Hyperlinks hinzu
                foreach (var name in group)
                {
                    var textBlock = new TextBlock();
                    var hyperlink = new Hyperlink();
                    hyperlink.Inlines.Add(new Run { Text = name });
                    hyperlink.Click += (s, e) => OnHyperlinkClick(name);
                    textBlock.Inlines.Add(hyperlink);
                    HyperlinkContainer.Children.Add(textBlock);
                }
            }

            StatusTextBlock.Text = $"{entries.Count} Einträge geladen.";
        }
        else
        {
            StatusTextBlock.Text = "Keine Einträge gefunden.";
        }
    }

    // Lädt die JSON-Datei als Dictionary
    private async Task<Dictionary<string, UserData>> LoadDataAsync()
    {
        try
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            string jsonString = await FileIO.ReadTextAsync(file);
            return JsonSerializer.Deserialize<Dictionary<string, UserData>>(jsonString) ?? new Dictionary<string, UserData>();
        }
        catch (FileNotFoundException)
        {
            StatusTextBlock.Text = "Datei nicht gefunden.";
            return new Dictionary<string, UserData>();
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = $"Fehler: {ex.Message}";
            return new Dictionary<string, UserData>();
        }
    }

    // Aktion beim Klicken auf den Hyperlink
    private void OnHyperlinkClick(string name)
    {
        Einträge.Visibility = Visibility.Collapsed;
        DetailsContainer.Visibility = Visibility.Visible;
        detail_buttonpanel.Visibility = Visibility.Visible;
        _ = LoadDetailsAsync(name);

        selectedName = name;
    }

    // Lädt die Details zu einem Namen und zeigt sie in der DetailsContainer-Region an
    private async Task LoadDetailsAsync(string name)
    {
        var data = await LoadDataAsync();

        DetailsContainer.Children.Clear(); // Alte Details entfernen

        if (data.ContainsKey(name))
        {
            var userDetails = data[name];

            // Dynamisch TextBlöcke für alle gespeicherten Daten erzeugen
            DetailsContainer.Children.Add(new TextBlock { Name = "EintragTextBlock", Text = $"Name: {name}", Margin = new Thickness(50, 0, 30, 5), FontWeight = Microsoft.UI.Text.FontWeights.Bold });

            if (!string.IsNullOrEmpty(userDetails.Potenzieller_Werktitel))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Potenzieller Werktitel: {userDetails.Potenzieller_Werktitel}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Verfasser))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Verfasser: {userDetails.Verfasser}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Informationen_zum_Verfasser))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Informationen zum Verfasser: {userDetails.Informationen_zum_Verfasser}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Titelblatttext))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Titelblatttext: {userDetails.Titelblatttext}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Inhalt))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Inhalt: {userDetails.Inhalt}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Auflage))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Auflage: {userDetails.Auflage}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Sprache))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Sprache: {userDetails.Sprache}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Umfang))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Umfang: {userDetails.Umfang}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Ort))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Ort: {userDetails.Ort}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Verleger_Drucker))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Verleger/Drucker: {userDetails.Verleger_Drucker}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Informationen_zum_Verleger_Drucker))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Informationen zum Verleger/Drucker: {userDetails.Informationen_zum_Verleger_Drucker}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Erscheinungsjahr))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Erscheinungsjahr: {userDetails.Erscheinungsjahr}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Weitere_Druckinformationen))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Weitere Druckinformationen: {userDetails.Weitere_Druckinformationen}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.Provenienzmerkmale))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Provenienzmerkmale: {userDetails.Provenienzmerkmale}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            if (!string.IsNullOrEmpty(userDetails.VD_Nummer))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"VD Nummer: {userDetails.VD_Nummer}", Margin = new Thickness(50, 0, 30, 5), TextWrapping = TextWrapping.Wrap });
            }

            

            if (!string.IsNullOrEmpty(userDetails.Digitalisat))
            {
                DetailsContainer.Children.Add(new TextBlock { Text = $"Digitalisat: {userDetails.Digitalisat}", Margin = new Thickness(50, 0, 30, 30) });
            }

            

           StatusTextBlock.Text = $"Details für '{name}' geladen.";
        }
        else
        {
            StatusTextBlock.Text = $"Keine Details für '{name}' gefunden.";
        }
    }

    private async Task EditDetailsAsync(string name)
    {
        var data = await LoadDataAsync();

        DetailsContainer.Children.Clear(); // Alte Details entfernen

        if (data.ContainsKey(name))
        {
            var userDetails = data[name];

            // Bearbeitbares Feld für den Namen hinzufügen
            DetailsContainer.Children.Add(new TextBlock { Text = "Name:", Margin = new Thickness(40, 0, 30, 30), FontWeight = Microsoft.UI.Text.FontWeights.Bold });
            DetailsContainer.Children.Add(new TextBox { Text = name, Width = 500, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(50, 0, 30, 10), Tag = "Name" }); // Speichere den aktuellen Namen im Tag

            DetailsContainer.Children.Add(new TextBlock { Text = "Potenzieller Werktitel:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox {Text = userDetails.Potenzieller_Werktitel, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Titel" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Verfasser:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Verfasser, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Autor" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Informationen zum Verfasser:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Informationen_zum_Verfasser, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Informationen_zum_Verfasser" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Titelblatttext:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Titelblatttext, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Titelblatttext" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Inhalt:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Inhalt, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Inhalt" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Auflage:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Auflage, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Auflage" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Sprache:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Sprache, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Sprache" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Umfang:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox {Text = userDetails.Umfang, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Umfang" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Ort:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Ort, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Ort" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Verleger/Drucker:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Verleger_Drucker, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Verleger" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Informationen zum Verleger_Drucker:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Informationen_zum_Verleger_Drucker, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Informationen_zum_Verleger_Drucker" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Erscheinungsjahr:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Erscheinungsjahr, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Erscheinungsjahr" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Weitere Druckinformationen:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Weitere_Druckinformationen, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Weitere_Druckinformationen" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Provenienzmerkmale:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox { Text = userDetails.Provenienzmerkmale, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Provenienzmerkmale" });

            DetailsContainer.Children.Add(new TextBlock { Text = "VD Nummer:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox {Text = userDetails.VD_Nummer, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "VD_Nummer" });

            DetailsContainer.Children.Add(new TextBlock { Text = "Digitalisat:", Margin = new Thickness(40, 0, 30, 5) });
            DetailsContainer.Children.Add(new TextBox {Text = userDetails.Digitalisat, Width = 500, Margin = new Thickness(50, 0, 30, 10), TextWrapping = TextWrapping.Wrap, Tag = "Digitalisat" });

            StatusTextBlock.Text = $"Details für '{name}' geladen.";
        }
        else
        {
            StatusTextBlock.Text = $"Keine Details für '{name}' gefunden.";
        }
    }

    // Schließt die Details-Ansicht und gibt die Einträge wieder zurück
    private async void ZurückButton_Click(object sender, RoutedEventArgs e)
    {
        HyperlinkContainer.Children.Clear();
        await LoadHyperlinksAsync();
        DetailsContainer.Children.Clear();
        DetailsContainer.Visibility = Visibility.Collapsed;
        detail_buttonpanel.Visibility = Visibility.Collapsed;
        Werkzeugauswahl.Visibility = Visibility.Collapsed;
        VDPanel.Visibility = Visibility.Collapsed;
        CERLThesaurusPanel.Visibility = Visibility.Collapsed;
        DatumsrechnerPanel.Visibility = Visibility.Collapsed;
        SignaturenrechnerPanel.Visibility = Visibility.Collapsed;
        Einträge.Visibility = Visibility.Visible;
    }

    private async void NeuZurückButton_Click(object sender, RoutedEventArgs e)
    {
        Neuentrypanel.Visibility = Visibility.Collapsed;
        Werkzeugauswahl.Visibility = Visibility.Collapsed;
        VDPanel.Visibility = Visibility.Collapsed;
        CERLThesaurusPanel.Visibility = Visibility.Collapsed;
        DatumsrechnerPanel.Visibility = Visibility.Collapsed;
        SignaturenrechnerPanel.Visibility = Visibility.Collapsed;
        HyperlinkContainer.Children.Clear();
        await LoadHyperlinksAsync();
        Einträge.Visibility = Visibility.Visible;
    }

    private async void NeuButton_Click(object sender, RoutedEventArgs e)
    {
        Einträge.Visibility = Visibility.Collapsed;
        Neuentrypanel.Visibility = Visibility.Visible;
        Werkzeugauswahl.Visibility = Visibility.Visible;
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        string Potenzieller_Werktitel = NeuTitelTextBox.Text;
        string Verfasser = NeuAutorTextBox.Text;
        string Informationen_zum_Verfasser = NeuVerfasserInformationsTextBox.Text;
        string Titelblatttext = NeuTitelblatttextTextBox.Text;
        string Inhalt = NeuInhaltTextBox.Text;
        string Auflage = NeuAuflageTextBox.Text;
        string Sprache = NeuSpracheTextBox.Text;
        string Umfang = NeuUmfangTextBox.Text;
        string Ort = NeuOrtTextBox.Text;
        string Verleger_Drucker = NeuVerlegerTextBox.Text;
        string Informationen_zum_Verleger_Drucker = NeuVerlegerInformationenTextBox.Text;
        string Erscheinungsjahr = NeuErscheinungsjahrTextBox.Text;
        string Weitere_Druckinformationen = NeuDruckinformationenTextBox.Text;
        string Provenienzmerkmale = NeuProvenienzmerkmaleTextBox.Text;
        string VD_Nummer = NeuVDNummerTextBox.Text;
        string Digitalisat = NeuDigitalisatTextBox.Text;

        var newEntry = new UserData
        {
            Potenzieller_Werktitel = Potenzieller_Werktitel,
            Verfasser = Verfasser,
            Informationen_zum_Verfasser = Informationen_zum_Verfasser,
            Titelblatttext = Titelblatttext,
            Inhalt = Inhalt,
            Auflage = Auflage,
            Sprache = Sprache,
            Umfang = Umfang,
            Ort = Ort,
            Verleger_Drucker = Verleger_Drucker,
            Informationen_zum_Verleger_Drucker = Informationen_zum_Verleger_Drucker,
            Erscheinungsjahr = Erscheinungsjahr,
            Weitere_Druckinformationen = Weitere_Druckinformationen,
            Provenienzmerkmale = Provenienzmerkmale,
            VD_Nummer = VD_Nummer,
            Digitalisat = Digitalisat,
        };

        var data = await NeuLoadDataAsync();
        data[Potenzieller_Werktitel] = newEntry;

        string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        await SaveToFileAsync(jsonString);

        StatusTextBlock.Text = "Daten gespeichert!";

        Neuentrypanel.Visibility = Visibility.Collapsed;
        Werkzeugauswahl.Visibility = Visibility.Collapsed;
        VDPanel.Visibility = Visibility.Collapsed;
        CERLThesaurusPanel.Visibility = Visibility.Collapsed;
        DatumsrechnerPanel.Visibility = Visibility.Collapsed;
        SignaturenrechnerPanel.Visibility = Visibility.Collapsed;
        HyperlinkContainer.Children.Clear();
        await LoadHyperlinksAsync();
        Einträge.Visibility = Visibility.Visible;

    }
    private async Task<Dictionary<string, UserData>> NeuLoadDataAsync()
    {
        try
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            string jsonString = await FileIO.ReadTextAsync(file);
            return JsonSerializer.Deserialize<Dictionary<string, UserData>>(jsonString) ?? new Dictionary<string, UserData>();
        }
        catch (FileNotFoundException)
        {
            return new Dictionary<string, UserData>();
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = $"Fehler: {ex.Message}";
            return new Dictionary<string, UserData>();
        }
    }
    private async void BearbeitenButton_Click(object sender, RoutedEventArgs e)
    {
        string name = selectedName;
        DetailsContainer.Children.Clear();
        await EditDetailsAsync(name);
        SpeichernButton.Visibility = Visibility.Visible;
        Werkzeugauswahl.Visibility = Visibility.Visible;

    }

    private async void SpeichernButton_Click(object sender, RoutedEventArgs e)
    {
        var data = await LoadDataAsync();

        if (string.IsNullOrEmpty(selectedName))
        {
            StatusTextBlock.Text = "Kein Name ausgewählt!";
            return;
        }

        if (data.ContainsKey(selectedName))
        {
            var userDetails = data[selectedName];
            string newName = selectedName; // Standardmäßig der aktuelle Name

            // Aktualisiere die Daten aus den TextBoxen
            foreach (var child in DetailsContainer.Children)
            {
                if (child is TextBox textBox)
                {
                    string updatedValue = textBox.Text;
                    string fieldTag = textBox.Tag?.ToString();

                    switch (fieldTag)
                    {
                        case "Name":
                            newName = updatedValue; // Aktualisierter Name
                            break;
                        case "Titel":
                            userDetails.Potenzieller_Werktitel = updatedValue;
                            break;
                        case "Autor":
                            userDetails.Verfasser = updatedValue;
                            break;
                        case "Erscheinungsjahr":
                            userDetails.Erscheinungsjahr = updatedValue;
                            break;
                        case "Umfang":
                            userDetails.Umfang = updatedValue;
                            break;
                        case "Sprache":
                            userDetails.Sprache = updatedValue;
                            break;
                        case "VD_Nummer":
                            userDetails.VD_Nummer = updatedValue;
                            break;
                        case "Verleger":
                            userDetails.Verleger_Drucker = updatedValue;
                            break;
                        case "Digitalisat":
                            userDetails.Digitalisat = updatedValue;
                            break;
                        case "Ort":
                            userDetails.Ort = updatedValue;
                            break;
                        case "Informationen_zum_Verfasser":
                            userDetails.Informationen_zum_Verfasser = updatedValue;
                            break;
                        case "Titelblatttext":
                            userDetails.Titelblatttext = updatedValue;
                            break;
                        case "Inhalt":
                            userDetails.Inhalt = updatedValue;
                            break;
                        case "Auflage":
                            userDetails.Auflage = updatedValue;
                            break;
                        case "Informationen_zum_Verleger_Drucker":
                            userDetails.Informationen_zum_Verleger_Drucker = updatedValue;
                            break;
                        case "Weitere_Druckinformationen":
                            userDetails.Weitere_Druckinformationen = updatedValue;
                            break;
                        case "Provenienzmerkmale":
                            userDetails.Provenienzmerkmale = updatedValue;
                            break;
                    }
                }
            }

            // Wenn der Name geändert wurde
            if (newName != selectedName)
            {
                data.Remove(selectedName); // Entferne den alten Schlüssel
                data[newName] = userDetails; // Füge den aktualisierten Eintrag mit dem neuen Namen hinzu
                selectedName = newName; // Aktualisiere die Klassenvariable
            }

            // Speichere die aktualisierten Daten in die JSON-Datei
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await SaveToFileAsync(jsonString);

            StatusTextBlock.Text = $"Änderungen für '{selectedName}' gespeichert!";
        }
        else
        {
            StatusTextBlock.Text = $"Kein Eintrag für '{selectedName}' gefunden.";
        }

        SpeichernButton.Visibility = Visibility.Collapsed;
        DetailsContainer.Children.Clear();
        await LoadDetailsAsync(selectedName);
    }

    // Speichert die JSON-Daten in die Datei
    private async Task SaveToFileAsync(string jsonString)
    {
        StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteTextAsync(file, jsonString);
    }

    private string BaseUrl;
    private void ToggleWerkzeug_Checked(object sender, RoutedEventArgs e)
    {
        if (Druckverzeichnisse.IsChecked == true)
        {
            VDPanel.Visibility = Visibility.Visible;
            CERLThesaurusPanel.Visibility = Visibility.Collapsed;
            DatumsrechnerPanel.Visibility = Visibility.Collapsed;
            SignaturenrechnerPanel.Visibility = Visibility.Collapsed;
            BaseUrl = "https://sru.gbv.de/gvk";
        }
        else if (CERLThesaurus.IsChecked == true)
        {
            CERLThesaurusPanel.Visibility = Visibility.Visible;
            VDPanel.Visibility = Visibility.Collapsed;
            DatumsrechnerPanel.Visibility = Visibility.Collapsed;
            SignaturenrechnerPanel.Visibility = Visibility.Collapsed;
            BaseUrl = "https://data.cerl.org/thesaurus/_search";
        }
        else if (Datumsrechner.IsChecked == true)
        {
            DatumsrechnerPanel.Visibility = Visibility.Visible;
            VDPanel.Visibility = Visibility.Collapsed;
            CERLThesaurusPanel.Visibility = Visibility.Collapsed;
            SignaturenrechnerPanel.Visibility = Visibility.Collapsed;

        }
        else if (Signaturenrechner.IsChecked == true)
        {
            SignaturenrechnerPanel.Visibility = Visibility.Visible;
            VDPanel.Visibility = Visibility.Collapsed;
            CERLThesaurusPanel.Visibility = Visibility.Collapsed;
            DatumsrechnerPanel.Visibility = Visibility.Collapsed;

        }
        else
        {
            VDPanel.Visibility = Visibility.Collapsed;
            CERLThesaurusPanel.Visibility = Visibility.Collapsed;
            DatumsrechnerPanel.Visibility = Visibility.Collapsed;
            SignaturenrechnerPanel.Visibility = Visibility.Collapsed;
        }
    }

    private async void VDSearchButton_Click(object sender, RoutedEventArgs e)
    {
        string searchQuery = textboxsuche.Text.Trim();

        if (string.IsNullOrEmpty(searchQuery))
        {
            resultoutput.Text = "Bitte geben sie einen Suchbegriff ein";
            return;
        }

        try
        {
            string response = await FetchGvkDataAsync(searchQuery);
            if (VD18.IsChecked == true)
            {
                DisplayStructuredXmlResponsevd18(response);
            }

            else if (VD17.IsChecked == true)
            {
                DisplayStructuredXmlResponsevd17(response);
            }

        }
        catch (Exception ex)
        {
            resultoutput.Text = $"Fehler: {ex.Message}";
        }
    }

    private async Task<string> FetchGvkDataAsync(string query)
    {
        string baseUrl = string.Empty;

        // Überprüfen, welche Checkbox aktiviert ist, und die entsprechende URL setzen
        if (VD18.IsChecked == true)
        {
            baseUrl = "http://sru.k10plus.de/vd18!rec=1";
        }
        else if (VD17.IsChecked == true)
        {
            baseUrl = "http://sru.k10plus.de/vd17!rec=1";
        }
        else
        {
            // Wenn keine Checkbox ausgewählt ist, Fehler zurückgeben
            return "Bitte wählen Sie eine Checkbox aus (VD17 oder VD18).";
        }

        // Anfrage-URL zusammenbauen
        string requestUrl = $"{baseUrl}?version=1.1&operation=searchRetrieve&query=pica.all={Uri.EscapeDataString(query)}&maximumRecords=10&recordSchema=mods";

        // HTTP-Anfrage senden
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    private void DisplayStructuredXmlResponsevd18(string xmlResponse)
    {
        resultrichtextblock.Blocks.Clear();

        try
        {
            XDocument xmlDoc = XDocument.Parse(xmlResponse);

            var records = xmlDoc.Descendants(XName.Get("record", "http://www.loc.gov/zing/srw/"));

            if (!records.Any())
            {
                DisplayError("Keine Ergebnisse gefunden.");
                return;
            }

            // HashSet für eindeutige VD18-Nummern
            HashSet<string> displayedVd18Numbers = new HashSet<string>();

            foreach (var record in records)
            {
                var modsElement = record.Descendants(XName.Get("mods", "http://www.loc.gov/mods/v3")).FirstOrDefault();
                if (modsElement == null) continue;

                var vd18 = modsElement.Descendants(XName.Get("identifier", "http://www.loc.gov/mods/v3"))
                                      .Where(e => e.Attribute("type")?.Value == "vd18")
                                      .FirstOrDefault()?.Value;

                // Wenn keine VD18-Nummer vorhanden ist, überspringen
                if (string.IsNullOrEmpty(vd18)) continue;

                // Prüfen, ob die VD18-Nummer bereits angezeigt wurde
                if (displayedVd18Numbers.Contains(vd18)) continue;

                // VD18-Nummer zur HashSet hinzufügen
                displayedVd18Numbers.Add(vd18);

                // Extrahiere weitere Informationen
                var title = modsElement.Descendants(XName.Get("title", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Kein Titel";
                var author = modsElement.Descendants(XName.Get("namePart", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Unbekannter Autor";
                var dateIssued = modsElement.Descendants(XName.Get("dateIssued", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Unbekanntes Jahr";
                var publisher = modsElement.Descendants(XName.Get("publisher", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Unbekannter Verlag";
                var place = modsElement.Descendants(XName.Get("placeTerm", "http://www.loc.gov/mods/v3")).FirstOrDefault(x => (string?)x.Attribute("type") == "text")?.Value ?? "Unbekannter Ort";
                var extent = modsElement.Descendants(XName.Get("extent", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Kein Umfang";
                var responsibilityNote = modsElement.Descendants(XName.Get("note", "http://www.loc.gov/mods/v3"))
                    .Where(e => e.Attribute("type")?.Value == "statement of responsibility").FirstOrDefault()?.Value ?? "Keine Verantwortlichkeitsangabe";

                var translatedLanguage = modsElement.Descendants(XName.Get("languageTerm", "http://www.loc.gov/mods/v3"))
                    .FirstOrDefault()?.Value ?? "Unbekannte Sprache";

                var originalLanguage = modsElement.Descendants(XName.Get("languageTerm", "http://www.loc.gov/mods/v3"))
                    .Skip(1).FirstOrDefault()?.Value ?? "Keine Übersetzung";

                var urlElement = modsElement.Descendants(XName.Get("url", "http://www.loc.gov/mods/v3")).FirstOrDefault();
                var url = urlElement?.Value ?? "Keine URL";

                var paragraph = new Paragraph();

                // Titel fett darstellen
                paragraph.Inlines.Add(new Run { Text = $"Potenzieller Werktitel:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{title}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Verfasser:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{author}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Titelblatttext:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{title} {responsibilityNote}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Umfang:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{extent}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Sprache:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $" Originalsprache {originalLanguage}, übersetzt in {translatedLanguage}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Ort:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{place}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Verleger/Drucker:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{publisher}\n" });
                // Erscheinungsjahr anzeigen
                paragraph.Inlines.Add(new Run { Text = $"Erscheinungsjahr:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{dateIssued}\n" });

                // VD18-Nummer anzeigen
                paragraph.Inlines.Add(new Run { Text = $"VD18-Nummer:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{vd18}\n" });


                // URL als Hyperlink darstellen
                if (!string.IsNullOrEmpty(urlElement?.Value))
                {
                    var hyperlink = new Hyperlink
                    {
                        NavigateUri = new Uri(url),

                    };
                    hyperlink.Inlines.Add(new Run { Text = $"Link zur Ressource: {url}\n" });
                    hyperlink.Click += async (sender, args) =>
                    {
                        await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
                    }; ;
                    paragraph.Inlines.Add(hyperlink);
                }

                // Absatz hinzufügen
                resultrichtextblock.Blocks.Add(paragraph);
            }
        }
        catch (Exception ex)
        {
            DisplayError($"Fehler beim Verarbeiten der XML-Antwort: {ex.Message}");
        }
    }

    private void DisplayStructuredXmlResponsevd17(string xmlResponse)
    {
        resultrichtextblock.Blocks.Clear();

        try
        {
            XDocument xmlDoc = XDocument.Parse(xmlResponse);

            var records = xmlDoc.Descendants(XName.Get("record", "http://www.loc.gov/zing/srw/"));

            if (!records.Any())
            {
                DisplayError("Keine Ergebnisse gefunden.");
                return;
            }

            // HashSet für eindeutige VD18-Nummern
            HashSet<string> displayedVd17Numbers = new HashSet<string>();

            foreach (var record in records)
            {
                var modsElement = record.Descendants(XName.Get("mods", "http://www.loc.gov/mods/v3")).FirstOrDefault();
                if (modsElement == null) continue;

                var vd17 = modsElement.Descendants(XName.Get("identifier", "http://www.loc.gov/mods/v3"))
                                      .Where(e => e.Attribute("type")?.Value == "vd17")
                                      .FirstOrDefault()?.Value;

                // Wenn keine VD18-Nummer vorhanden ist, überspringen
                if (string.IsNullOrEmpty(vd17)) continue;

                // Prüfen, ob die VD18-Nummer bereits angezeigt wurde
                if (displayedVd17Numbers.Contains(vd17)) continue;

                // VD18-Nummer zur HashSet hinzufügen
                displayedVd17Numbers.Add(vd17);

                // Extrahiere weitere Informationen
                var title = modsElement.Descendants(XName.Get("title", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Kein Titel";
                var author = modsElement.Descendants(XName.Get("namePart", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Unbekannter Autor";
                var dateIssued = modsElement.Descendants(XName.Get("dateIssued", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Unbekanntes Jahr";
                var publisher = modsElement.Descendants(XName.Get("publisher", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Unbekannter Verlag";
                var place = modsElement.Descendants(XName.Get("placeTerm", "http://www.loc.gov/mods/v3")).FirstOrDefault(x => (string?)x.Attribute("type") == "text")?.Value ?? "Unbekannter Ort";
                var extent = modsElement.Descendants(XName.Get("extent", "http://www.loc.gov/mods/v3")).FirstOrDefault()?.Value ?? "Kein Umfang";
                var responsibilityNote = modsElement.Descendants(XName.Get("note", "http://www.loc.gov/mods/v3"))
                    .Where(e => e.Attribute("type")?.Value == "statement of responsibility").FirstOrDefault()?.Value ?? "Keine Verantwortlichkeitsangabe";

                var translatedLanguage = modsElement.Descendants(XName.Get("languageTerm", "http://www.loc.gov/mods/v3"))
                    .FirstOrDefault()?.Value ?? "Unbekannte Sprache";

                var originalLanguage = modsElement.Descendants(XName.Get("languageTerm", "http://www.loc.gov/mods/v3"))
                    .Skip(1).FirstOrDefault()?.Value ?? "Keine Übersetzung";

                var urlElement = modsElement.Descendants(XName.Get("url", "http://www.loc.gov/mods/v3")).FirstOrDefault();
                var url = urlElement?.Value ?? "Keine URL";

                var paragraph = new Paragraph();

                // Titel fett darstellen
                paragraph.Inlines.Add(new Run { Text = $"Potenzieller Werktitel:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{title}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Verfasser:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{author}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Titelblatttext:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{title} {responsibilityNote}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Umfang:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{extent}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Sprache:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $" Originalsprache {originalLanguage}, übersetzt in {translatedLanguage}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Ort:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{place}\n" });

                paragraph.Inlines.Add(new Run { Text = $"Verleger/Drucker:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{publisher}\n" });
                // Erscheinungsjahr anzeigen
                paragraph.Inlines.Add(new Run { Text = $"Erscheinungsjahr:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{dateIssued}\n" });

                // VD18-Nummer anzeigen
                paragraph.Inlines.Add(new Run { Text = $"VD17-Nummer:    ", FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run { Text = $"{vd17}\n" });


                // URL als Hyperlink darstellen
                if (!string.IsNullOrEmpty(urlElement?.Value))
                {
                    var hyperlink = new Hyperlink
                    {
                        NavigateUri = new Uri(url),

                    };
                    hyperlink.Inlines.Add(new Run { Text = $"Link zur Ressource: {url}\n" });
                    hyperlink.Click += async (sender, args) =>
                    {
                        await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
                    }; ;
                    paragraph.Inlines.Add(hyperlink);
                }

                // Absatz hinzufügen
                resultrichtextblock.Blocks.Add(paragraph);
            }
        }
        catch (Exception ex)
        {
            DisplayError($"Fehler beim Verarbeiten der XML-Antwort: {ex.Message}");
        }
    }

    private void DisplayError(string message)
    {
        resultrichtextblock.Blocks.Clear();
        var paragraph = new Paragraph();
        paragraph.Inlines.Add(new Run { Text = message });
        resultrichtextblock.Blocks.Add(paragraph);
    }

    private void ToggleWebViewCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (ToggleWebViewCheckBox.IsChecked == true)
        {
            vd15viewer.Visibility = Visibility.Visible;
            resultrichtextblock.Visibility = Visibility.Collapsed;
        }
        else
        {
            vd15viewer.Visibility = Visibility.Collapsed;
            resultrichtextblock.Visibility = Visibility.Visible;
        }
    }

    private async void CERLSearchButton_Click(object sender, RoutedEventArgs e)
    {
        string query = CERLtextboxsuche.Text;

        if (string.IsNullOrWhiteSpace(query))
        {
            CERLresultoutput.Text = "Bitte geben sie einen Suchbegriff ein";
            return;
        }

        try
        {
            string result = await SearchThesaurusAsync(query);
            DisplayParsedHtml(result);

        }

        catch (Exception ex)
        {
            CERLresultoutput.Text = $"Fehler: {ex.Message}";
        }
    }

    private async Task<string> SearchThesaurusAsync(string query)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"{BaseUrl}?query={Uri.EscapeDataString(query)}";

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();


        }
    }

    private async void FetchAndDisplayHtmlContent(string href)
    {
        string BaseUrl = "https://data.cerl.org/";

        try
        {
            string fullUrl = $"{BaseUrl.TrimEnd('/')}/{href}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();

                string htmlContent = await response.Content.ReadAsStringAsync();

                DisplayFilteredHtmlInRichTextBlock(htmlContent);
            }
        }
        catch (Exception ex)
        {
            var errorParagraph = new Paragraph();
            errorParagraph.Inlines.Add(new Run
            {
                Text = $"Fehler beim Abrufen der Daten: {ex.Message}",
            });
            CERLresultrichtextblock.Blocks.Add(errorParagraph);
        }
    }

    private void DisplayHtmlInRichTextBlock(string htmlContent)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        // RichTextBlock-Inhalt leeren
        CERLresultrichtextblock.Blocks.Clear();

        // HTML-Inhalt verarbeiten
        foreach (var node in htmlDoc.DocumentNode.Descendants())
        {
            if (node.Name == "p")
            {
                // Absätze hinzufügen
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run { Text = node.InnerText });
                CERLresultrichtextblock.Blocks.Add(paragraph);
            }
            else if (node.Name == "b" || node.Name == "strong")
            {
                // Fettschrift
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run
                {
                    Text = node.InnerText,
                });
                CERLresultrichtextblock.Blocks.Add(paragraph);
            }
            else if (node.Name == "i" || node.Name == "em")
            {
                // Kursivschrift
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run
                {
                    Text = node.InnerText,
                    FontStyle = Windows.UI.Text.FontStyle.Italic
                });
                CERLresultrichtextblock.Blocks.Add(paragraph);
            }
            else if (!string.IsNullOrWhiteSpace(node.InnerText))
            {
                // Standardtext
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run { Text = node.InnerText });
                CERLresultrichtextblock.Blocks.Add(paragraph);
            }
        }
    }

    private void DisplayParsedHtml(string htmlContent)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        // RichTextBlock-Inhalt leeren
        CERLresultrichtextblock.Blocks.Clear();

        // Alle Knoten mit der Klasse "ct-infobox" finden
        var headingNodes = htmlDoc.DocumentNode.SelectNodes("//h2[@class='ample-heading '][following-sibling::*[1][@class='ct-infobox']]");

        if (headingNodes == null || headingNodes.Count == 0)
        {
            // Wenn keine Knoten gefunden wurden
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run { Text = "Keine Inhalte mit der Klasse 'ct-infobox' gefunden." });
            CERLresultrichtextblock.Blocks.Add(paragraph);
            return;
        }

        foreach (var headingNode in headingNodes)
        {
            // Überschrift aus ample-heading extrahieren
            string headingText = headingNode.InnerText.Trim();

            // Überschrift hinzufügen
            var headingParagraph = new Paragraph();
            headingParagraph.Inlines.Add(new Run
            {
                Text = headingText,
                FontSize = 16,
            });
            CERLresultrichtextblock.Blocks.Add(headingParagraph);

            // Links innerhalb der aktuellen ct-infobox finden
            // Nachfolgende ct-infobox extrahieren
            var infoboxNode = headingNode.SelectSingleNode("following-sibling::*[@class='ct-infobox']");

            if (infoboxNode != null)
            {
                // Links innerhalb der ct-infobox finden
                var links = infoboxNode.SelectNodes(".//a");

                if (links != null)
                {

                    // Links aus der ct-infobox verarbeiten
                    foreach (var link in links)
                    {
                        string href = link.GetAttributeValue("href", "Kein Link");
                        string text = link.InnerText.Trim();

                        // Hyperlink erstellen
                        var hyperlink = new Hyperlink();
                        hyperlink.Inlines.Add(new Run { Text = text });

                        // Klick-Ereignis registrieren
                        hyperlink.Click += (sender, args) => FetchAndDisplayHtmlContent(href);

                        // Hyperlink zu einem neuen Absatz hinzufügen
                        var linkParagraph = new Paragraph();
                        linkParagraph.Inlines.Add(hyperlink);
                        CERLresultrichtextblock.Blocks.Add(linkParagraph);
                    }
                }
                // Absatz nach der ct-infobox hinzufügen
                CERLresultrichtextblock.Blocks.Add(new Paragraph());
            }
        }
    }

    private void DisplayFilteredHtmlInRichTextBlock(string htmlContent)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        // RichTextBlock-Inhalt leeren
        CERLresultrichtextblock.Blocks.Clear();

        // Alle Knoten mit relevanten Klassen suchen
        var containerNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@class='container ample-content']");

        if (containerNode == null)
        {
            // Nachricht anzeigen, falls kein Knoten gefunden wurde
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run { Text = "Keine Inhalte mit der Klasse 'container ample-content' gefunden." });
            CERLresultrichtextblock.Blocks.Add(paragraph);
            return;
        }

        // Alle Knoten mit der Klasse "ct-infobox" finden
        var infoboxNodes = containerNode.SelectNodes(".//*[@class='ct-infobox' or @class='ct-moreinfo']");

        if (infoboxNodes == null || infoboxNodes.Count == 0)
        {
            // Nachricht anzeigen, falls keine "ct-infobox" gefunden wurde
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run { Text = "Keine 'ct-infobox'-Inhalte gefunden." });
            CERLresultrichtextblock.Blocks.Add(paragraph);
            return;
        }

        foreach (var infobox in infoboxNodes)
        {
            // H3-Element als Überschrift
            var h3Node = infobox.SelectSingleNode(".//h3");
            if (h3Node != null)
            {
                var headingParagraph = new Paragraph();
                headingParagraph.Inlines.Add(new Run
                {
                    Text = h3Node.InnerText.Trim(),

                    FontSize = 16
                });
                CERLresultrichtextblock.Blocks.Add(headingParagraph);
            }

            // Paare aus Label und Content
            var displayLines = infobox.SelectNodes(".//div[@class='ample-display-line']");
            if (displayLines != null)
            {
                foreach (var line in displayLines)
                {
                    var children = line.SelectNodes("./span");
                    if (children != null)
                    {
                        Paragraph pairParagraph = null;

                        foreach (var child in children)
                        {
                            if (child.HasClass("ample-display-label"))
                            {
                                // Neues Label gefunden, neuen Paragraph starten
                                if (pairParagraph != null)
                                {
                                    // Alten Paragraph abschließen
                                    CERLresultrichtextblock.Blocks.Add(pairParagraph);
                                    CERLresultrichtextblock.Blocks.Add(new Paragraph()); // Leerzeile hinzufügen
                                }

                                pairParagraph = new Paragraph();
                                pairParagraph.Inlines.Add(new Run
                                {
                                    Text = $"{child.InnerText.Trim()}: ",
                                });
                            }
                            else if (child.HasClass("ample-display-content"))
                            {
                                // Content zum aktuellen Label hinzufügen
                                if (pairParagraph != null)
                                {
                                    var linkNode = child.SelectSingleNode(".//a");
                                    if (linkNode != null)
                                    {
                                        string href = linkNode.GetAttributeValue("href", "Kein Link");
                                        string linkText = linkNode.InnerText.Trim();

                                        var hyperlink = new Hyperlink();
                                        hyperlink.Inlines.Add(new Run { Text = linkText });

                                        if (href.Contains("thesaurus"))
                                        {
                                            hyperlink.Click += (sender, args) => FetchAndDisplayHtmlContent(href);
                                        }
                                        else
                                        {
                                            hyperlink.Click += async (sender, args) =>
                                            {
                                                await Windows.System.Launcher.LaunchUriAsync(new Uri(href));
                                            };
                                        }

                                        pairParagraph.Inlines.Add(hyperlink);
                                    }
                                    else
                                    {
                                        pairParagraph.Inlines.Add(new Run { Text = child.InnerText.Trim() });
                                    }

                                    // Leerzeichen zwischen Inhalten hinzufügen
                                    pairParagraph.Inlines.Add(new Run { Text = " " });
                                }
                            }
                        }

                        // Letzten Paragraph hinzufügen, falls vorhanden
                        if (pairParagraph != null)
                        {
                            CERLresultrichtextblock.Blocks.Add(pairParagraph);
                            CERLresultrichtextblock.Blocks.Add(new Paragraph()); // Leerzeile hinzufügen
                        }
                    }
                }
            }
            CERLresultrichtextblock.Blocks.Add(new Paragraph()); // Leerzeile hinzufügen
        }
    }

    private void Datumsrechnerrechner_combobox_Loaded(object sender, RoutedEventArgs e)
    {
        Datumsrechner_combobox.SelectedItem = Datumsrechner_combobox.Items[0];
    }
    private void DatumsrechnerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Datumsrechner_combobox.SelectedItem != null)
        {
            string selectedItem = (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString();
            if (selectedItem == "Jahreszahl")
            {
                textboxyear1.Visibility = Visibility.Visible;

                textblockyear2.Visibility = Visibility.Collapsed;
                textboxyear2.Visibility = Visibility.Collapsed;
                textblockmonth2.Visibility = Visibility.Collapsed;
                comboboxmonth2.Visibility = Visibility.Collapsed;
                textblockarea2.Visibility = Visibility.Collapsed;
                comboboxarea2.Visibility = Visibility.Collapsed;
                textblockday2.Visibility = Visibility.Collapsed;
                comboboxday2_1.Visibility = Visibility.Collapsed;
                comboboxday2_2.Visibility = Visibility.Collapsed;
                comboboxday2_3.Visibility = Visibility.Collapsed;
                comboboxday2_4.Visibility = Visibility.Collapsed;
                comboboxday2_5.Visibility = Visibility.Collapsed;
                comboboxday2_6.Visibility = Visibility.Collapsed;
                comboboxday2_7.Visibility = Visibility.Collapsed;

                textboxchronogramm3.Visibility = Visibility.Collapsed;

                Datumsrechnerresultoutput.Text = "Die gesuchte Jahreszahl ist";
            }
            else if (selectedItem == "Julianischer Kalender")
            {
                textboxyear1.Visibility = Visibility.Collapsed;

                textblockyear2.Visibility = Visibility.Visible;
                textboxyear2.Visibility = Visibility.Visible;
                textblockmonth2.Visibility = Visibility.Visible;
                comboboxmonth2.Visibility = Visibility.Visible;
                textblockarea2.Visibility = Visibility.Visible;
                comboboxarea2.Visibility = Visibility.Visible;
                textblockday2.Visibility = Visibility.Visible;
                comboboxday2_1.Visibility = Visibility.Collapsed;
                comboboxday2_2.Visibility = Visibility.Collapsed;
                comboboxday2_3.Visibility = Visibility.Collapsed;
                comboboxday2_4.Visibility = Visibility.Collapsed;
                comboboxday2_5.Visibility = Visibility.Collapsed;
                comboboxday2_6.Visibility = Visibility.Collapsed;
                comboboxday2_7.Visibility = Visibility.Collapsed;

                textboxchronogramm3.Visibility = Visibility.Collapsed;

                Datumsrechnerresultoutput.Text = "Das gesuchte Datum ist";
            }
            else if (selectedItem == "Chronogramm")
            {
                textboxyear1.Visibility = Visibility.Collapsed;

                textblockyear2.Visibility = Visibility.Collapsed;
                textboxyear2.Visibility = Visibility.Collapsed;
                textblockmonth2.Visibility = Visibility.Collapsed;
                comboboxmonth2.Visibility = Visibility.Collapsed;
                textblockarea2.Visibility = Visibility.Collapsed;
                comboboxarea2.Visibility = Visibility.Collapsed;
                textblockday2.Visibility = Visibility.Collapsed;
                comboboxday2_1.Visibility = Visibility.Collapsed;
                comboboxday2_2.Visibility = Visibility.Collapsed;
                comboboxday2_3.Visibility = Visibility.Collapsed;
                comboboxday2_4.Visibility = Visibility.Collapsed;
                comboboxday2_5.Visibility = Visibility.Collapsed;
                comboboxday2_6.Visibility = Visibility.Collapsed;
                comboboxday2_7.Visibility = Visibility.Collapsed;

                textboxchronogramm3.Visibility = Visibility.Visible;

                Datumsrechnerresultoutput.Text = "Das gesuchte Jahr ist";
            }
        }
    }
    private void DatumsrechnerCombobox_SelectionChanged2(object sender, SelectionChangedEventArgs e)
    {
        if (comboboxmonth2.SelectedItem != null && comboboxarea2.SelectedItem != null)
        {
            string selectedMonth = (comboboxmonth2.SelectedItem as ComboBoxItem).Content.ToString();
            string selectedArea = (comboboxarea2.SelectedItem as ComboBoxItem).Content.ToString();
            if ((selectedMonth == "Ianuarius" || selectedMonth == "Februarius" || selectedMonth == "September") && selectedArea == "Kal.")
            {
                comboboxday2_1.Visibility = Visibility.Visible;
                comboboxday2_2.Visibility = Visibility.Collapsed;
                comboboxday2_3.Visibility = Visibility.Collapsed;
                comboboxday2_4.Visibility = Visibility.Collapsed;
                comboboxday2_5.Visibility = Visibility.Collapsed;
                comboboxday2_6.Visibility = Visibility.Collapsed;
                comboboxday2_7.Visibility = Visibility.Collapsed;
            }
            else if ((selectedMonth == "Aprilis" || selectedMonth == "Iunius" || selectedMonth == "Sextilis/Augustus" || selectedMonth == "November") && selectedArea == "Kal.")
            {
                comboboxday2_1.Visibility = Visibility.Collapsed;
                comboboxday2_2.Visibility = Visibility.Visible;
                comboboxday2_3.Visibility = Visibility.Collapsed;
                comboboxday2_4.Visibility = Visibility.Collapsed;
                comboboxday2_5.Visibility = Visibility.Collapsed;
                comboboxday2_6.Visibility = Visibility.Collapsed;
                comboboxday2_7.Visibility = Visibility.Collapsed;
            }
            else if ((selectedMonth == "Maius" || selectedMonth == "Quintilis/Iulius" || selectedMonth == "October" || selectedMonth == "December") && selectedArea == "Kal.")
            {
                comboboxday2_1.Visibility = Visibility.Collapsed;
                comboboxday2_2.Visibility = Visibility.Collapsed;
                comboboxday2_3.Visibility = Visibility.Visible;
                comboboxday2_4.Visibility = Visibility.Collapsed;
                comboboxday2_5.Visibility = Visibility.Collapsed;
                comboboxday2_6.Visibility = Visibility.Collapsed;
                comboboxday2_7.Visibility = Visibility.Collapsed;
            }
            else if (selectedMonth == "Martius" && selectedArea == "Kal.")
            {
                comboboxday2_1.Visibility = Visibility.Collapsed;
                comboboxday2_2.Visibility = Visibility.Collapsed;
                comboboxday2_3.Visibility = Visibility.Collapsed;
                comboboxday2_4.Visibility = Visibility.Visible;
                comboboxday2_5.Visibility = Visibility.Collapsed;
                comboboxday2_6.Visibility = Visibility.Collapsed;
                comboboxday2_7.Visibility = Visibility.Collapsed;
            }
            else if (selectedMonth != null && selectedArea == "Id.")
            {
                comboboxday2_1.Visibility = Visibility.Collapsed;
                comboboxday2_2.Visibility = Visibility.Collapsed;
                comboboxday2_3.Visibility = Visibility.Collapsed;
                comboboxday2_4.Visibility = Visibility.Collapsed;
                comboboxday2_5.Visibility = Visibility.Visible;
                comboboxday2_6.Visibility = Visibility.Collapsed;
                comboboxday2_7.Visibility = Visibility.Collapsed;
            }
            else if ((selectedMonth == "Martius" || selectedMonth == "Maius" || selectedMonth == "Iunius" || selectedMonth == "October") && selectedArea == "Non.")
            {
                comboboxday2_1.Visibility = Visibility.Collapsed;
                comboboxday2_2.Visibility = Visibility.Collapsed;
                comboboxday2_3.Visibility = Visibility.Collapsed;
                comboboxday2_4.Visibility = Visibility.Collapsed;
                comboboxday2_5.Visibility = Visibility.Collapsed;
                comboboxday2_6.Visibility = Visibility.Visible;
                comboboxday2_7.Visibility = Visibility.Collapsed;
            }
            else if ((selectedMonth != "Martius" && selectedMonth != "Maius" && selectedMonth != "Iunius" && selectedMonth != "October") && selectedArea == "Non.")
            {
                comboboxday2_1.Visibility = Visibility.Collapsed;
                comboboxday2_2.Visibility = Visibility.Collapsed;
                comboboxday2_3.Visibility = Visibility.Collapsed;
                comboboxday2_4.Visibility = Visibility.Collapsed;
                comboboxday2_5.Visibility = Visibility.Collapsed;
                comboboxday2_6.Visibility = Visibility.Collapsed;
                comboboxday2_7.Visibility = Visibility.Visible;
            }
        }
    }
    string GetSelectedContent()
    {
        string activeComboboxContent = null;
        if (comboboxmonth2.SelectedItem != null && comboboxarea2.SelectedItem != null)
        {
            string selectedMonth = (comboboxmonth2.SelectedItem as ComboBoxItem).Content.ToString();
            string selectedArea = (comboboxarea2.SelectedItem as ComboBoxItem).Content.ToString();

            if ((selectedMonth == "Ianuarius" || selectedMonth == "Februarius" || selectedMonth == "September") && selectedArea == "Kal.")
            {
                activeComboboxContent = ((ComboBoxItem)comboboxday2_1.SelectedItem).Content.ToString();
            }
            else if ((selectedMonth == "Aprilis" || selectedMonth == "Iunius" || selectedMonth == "Sextilis/Augustus" || selectedMonth == "November") && selectedArea == "Kal.")
            {
                activeComboboxContent = ((ComboBoxItem)comboboxday2_2.SelectedItem).Content.ToString();
            }
            else if ((selectedMonth == "Maius" || selectedMonth == "Quintilis/Iulius" || selectedMonth == "October" || selectedMonth == "December") && selectedArea == "Kal.")
            {
                activeComboboxContent = ((ComboBoxItem)comboboxday2_3.SelectedItem).Content.ToString();
            }
            else if (selectedMonth == "Martius" && selectedArea == "Kal.")
            {
                activeComboboxContent = ((ComboBoxItem)comboboxday2_4.SelectedItem).Content.ToString();
            }
            else if (selectedMonth != null && selectedArea == "Id.")
            {
                activeComboboxContent = ((ComboBoxItem)comboboxday2_5.SelectedItem).Content.ToString();
            }
            else if ((selectedMonth == "Martius" || selectedMonth == "Maius" || selectedMonth == "Iunius" || selectedMonth == "October") && selectedArea == "Non.")
            {
                activeComboboxContent = ((ComboBoxItem)comboboxday2_6.SelectedItem).Content.ToString();
            }
            else if ((selectedMonth != "Martius" && selectedMonth != "Maius" && selectedMonth != "Iunius" && selectedMonth != "October") && selectedArea == "Non.")
            {
                activeComboboxContent = ((ComboBoxItem)comboboxday2_6.SelectedItem).Content.ToString();
            }
            return activeComboboxContent;
        }
        return activeComboboxContent;
    }
    private static int RomanToArabic(string roman)
    {
        int total = 0;
        int previousValue = 0;

        foreach (char c in roman)
        {
            if (!Dictionaries.RomanNumbers.ContainsKey(c))
            {
                throw new ArgumentException("ungültiges römisches Zeichen.");
            }

            int currentValue = Dictionaries.RomanNumbers[c];

            if (currentValue > previousValue)
            {
                total += currentValue - 2 * previousValue;
            }
            else
            {
                total += currentValue;
            }
            previousValue = currentValue;
        }
        return total;
    }
    private static int GetDay(string dayPart, string typepart, int month, int year)
    {
        int baseDay;

        switch (typepart)
        {
            case "Kal.":
                baseDay = 1;
                break;
            case "Non.":
                baseDay = month == 3 || month == 5 || month == 7 || month == 10 ? 7 : 5;
                break;
            case "Id.":
                baseDay = month == 3 || month == 5 || month == 7 || month == 10 ? 15 : 13;
                break;
            default:
                throw new ArgumentException("Ungültiger römischer Datumstyp.");
        }

        if (dayPart == "-")
        {
            return baseDay;
        }
        else if (dayPart == "pridie")
        {
            return baseDay - 1;
        }
        else
        {
            string[] parts = dayPart.Split(' ');
            if (parts.Length != 2 || parts[0] != "a.d.")
            {
                throw new ArgumentException("Ungültiges römisches Datum");
            }

            int daysBefore = RomanToArabic(parts[1]);
            return baseDay - daysBefore + 1;
        }
    }
    private static DateTime ConvertRomanDate(string dayPart, string typePart, string monthPart, int year)
    {
        if (!Dictionaries.MonthNumbers.ContainsKey(monthPart))
        {
            throw new ArgumentException("Ungültiger römischer Monat");
        }

        int month = Dictionaries.MonthNumbers[monthPart];
        int day = GetDay(dayPart, typePart, month, year);

        if (day <= 0)
        {
            DateTime date = new DateTime(year, month, 1).AddDays(day - 1);
            return date;
        }
        else
        {
            return new DateTime(year, month, day);
        }
    }
    private static int RomanToArabicInt(string roman)
    {
        int total = 0;

        foreach (char c in roman)
        {
            if (!Dictionaries.RomanNumbers.ContainsKey(c))
            {
                throw new ArgumentException("ungültiges römisches Zeichen.");
            }

            int currentValue = Dictionaries.RomanNumbers[c];

            total += currentValue;
        }
        return total;
    }
    private static int ConvertChronogramToYear(string chronogram)
    {
        string romanNumerals = "";
        foreach (char c in chronogram.ToUpper())
        {
            if (Dictionaries.RomanNumbers.ContainsKey(c))
            {
                romanNumerals += c;
            }
        }

        if (string.IsNullOrEmpty(romanNumerals))
        {
            throw new ArgumentException("Keine römischen Ziffern im Chronogramm gefunden.");
        }

        return RomanToArabicInt(romanNumerals);
    }
    private void DatumsrechnerBerechnenButton_Click(object sender, RoutedEventArgs e)
    {
        if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Jahreszahl")
        {
            string roman = textboxyear1.Text;
            try
            {
                int Jahreszahl = RomanToArabic(roman);

                Datumsrechnerresultoutput.Text = $"Die gesuchte Jahreszahl ist {Jahreszahl.ToString()}";
            }
            catch (ArgumentException ex)
            {
                Datumsrechnerresultoutput.Text = ex.Message;
            }

        }
        else if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Julianischer Kalender")
        {
            string dayPart = GetSelectedContent();
            string typePart = ((ComboBoxItem)comboboxarea2.SelectedItem).Content.ToString();
            string monthPart = ((ComboBoxItem)comboboxmonth2.SelectedItem).Content.ToString();
            string roman = textboxyear2.Text;

            try
            {
                int year = RomanToArabic(roman);
                DateTime resultDate = ConvertRomanDate(dayPart, typePart, monthPart, year);

                Datumsrechnerresultoutput.Text = $"Das Datum {dayPart} {typePart} {monthPart} {roman} entspricht dem {resultDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}.";
            }
            catch (ArgumentException ex)
            {
                Datumsrechnerresultoutput.Text = ex.Message;
            }
        }
        else if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Chronogramm")
        {
            string chronogram = textboxchronogramm3.Text;

            try
            {
                int year = ConvertChronogramToYear(chronogram);

                Datumsrechnerresultoutput.Text = $"Das Chronogramm entspricht dem Jahr {year.ToString()}.";
            }
            catch (ArgumentException ex)
            {
                Datumsrechnerresultoutput.Text = ex.Message;
            }
        }
    }
    private void DatumsrechnerLöschenButton_Click(object sender, RoutedEventArgs e)
    {
        if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Jahreszahl")
        {
            textboxyear1.Text = "";

            Datumsrechnerresultoutput.Text = "Die gesuchte Jahreszahl ist";
        }
        else if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Julianischer Kalender")
        {
            textboxyear2.Text = "";

            Datumsrechnerresultoutput.Text = "Das Datum   entspricht dem   .";
        }
        else if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Chronogramm")
        {
            textboxchronogramm3.Text = "";

            Datumsrechnerresultoutput.Text = "Das Chronogramm entspricht dem Jahr   .";
        }
    }

    private void Signaturrechner_combobox_Loaded(object sender, RoutedEventArgs e)
    {
        signaturrechner_combobox.SelectedItem = signaturrechner_combobox.Items[0]; // Wählt das erste Element (Signatur zu Pagina)
    }
    private void SignaturenrechnerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

                Signaturenrechnerresultoutput.Text = "Die Bogensignatur entspricht der Seitenzahl";
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

                Signaturenrechnerresultoutput.Text = "Die Seitenzahl entspricht der Bogensignatur";
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

                Signaturenrechnerresultoutput.Text = "Die Alphabet- und Bogenzahl ergibt einen Seitenumfang von  Seiten";
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

    private void SignaturenrechnerBerechnenButton_Click(object sender, RoutedEventArgs e)
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
            Signaturenrechnerresultoutput.Text = $"Die Bogensignatur entspricht der Seitenzahl {seite.ToString()}";
        }
        else if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Pagina zu Signatur")
        {
            string pag = textboxseitenzahl2.Text;
            string format = textboxformat2.Text;

            string signatur = PagToSig(pag, format);

            Signaturenrechnerresultoutput.Text = $"Die Seitenzahl entspricht der Bogensignatur {signatur.ToString()}";
        }
        else if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Bogenzahl zu Seitenzahl")
        {
            string alphabet = textboxalphabet3.Text;
            string bogen = textboxbogenzahl3.Text;
            string format = textboxformat3.Text;

            int page = AlphabetToPage(alphabet, bogen, format);

            Signaturenrechnerresultoutput.Text = $"Die Alphabet- und Bogenzahl ergibt einen Seitenumfang von {page.ToString()} Seiten";
        }
        else
        {
            // Wenn eine andere Option als "Signatur zu Pagina" ausgewählt wurde, gib eine Fehlermeldung aus
            Signaturenrechnerresultoutput.Text = "Fehler: Wählen Sie 'Signatur zu Pagina' aus";
        }
    }

    private void SignaturenrechnerLöschenButton_Click(object sender, RoutedEventArgs e)
    {
        if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Signatur zu Pagina")
        {
            textboxsigltr1.Text = "";
            textboxsignmbr1.Text = "";
            textboxformat1.Text = "";

            Signaturenrechnerresultoutput.Text = "Die Bogensignatur entspricht der Seitenzahl";
        }
        else if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Pagina zu Signatur")
        {
            textboxseitenzahl2.Text = "";
            textboxformat2.Text = "";

            Signaturenrechnerresultoutput.Text = "Die Seitenzahl entspricht der Bogensignatur";
        }
        else if (signaturrechner_combobox.SelectedItem != null &&
            (signaturrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Bogenzahl zu Seitenzahl")
        {
            textboxalphabet3.Text = "";
            textboxbogenzahl3.Text = "";
            textboxformat3.Text = "";

            Signaturenrechnerresultoutput.Text = "Die Alphabet- und Bogenzahl ergibt einen Seitenumfang von  Seiten";
        }
    }

    
}








