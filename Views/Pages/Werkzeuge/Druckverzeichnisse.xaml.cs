
using HtmlAgilityPack;
using Microsoft.UI.Xaml.Documents;
using neuesmodell.Services;
using Newtonsoft.Json;
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

namespace neuesmodell.Views.Pages.Werkzeuge;

public sealed partial class Druckverzeichnisse : Page
{
    public Druckverzeichnisse()
    {
        this.InitializeComponent();
    }

    private const string BaseUrl = "https://sru.gbv.de/gvk";

    private async void SearchButton_Click(object sender, RoutedEventArgs e)
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

    private void DisplayResults(string response)
    {
        resultrichtextblock.Blocks.Clear();

        // Zeige die gesamte XML-Antwort an
        var paragraph = new Paragraph();
        paragraph.Inlines.Add(new Run { Text = response.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") });
        resultrichtextblock.Blocks.Add(paragraph);
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
                paragraph.Inlines.Add(new Run { Text = $"{title}\n"});

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

}



