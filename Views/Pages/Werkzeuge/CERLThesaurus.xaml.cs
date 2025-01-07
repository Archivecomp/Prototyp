
using HtmlAgilityPack;
using Microsoft.UI.Xaml.Documents;
using neuesmodell.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.System;

namespace neuesmodell.Views.Pages.Werkzeuge;

public sealed partial class CERLThesaurus : Page
{
    public CERLThesaurus()
    {
        this.InitializeComponent();
    }

    private const string BaseUrl = "https://data.cerl.org/thesaurus/_search";


    private async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string query = textboxsuche.Text;

        if (string.IsNullOrWhiteSpace(query))
        {
            resultoutput.Text = "Bitte geben sie einen Suchbegriff ein";
            return;
        }

        try
        {
            string result = await SearchThesaurusAsync(query);
            DisplayParsedHtml(result);

        }

        catch (Exception ex)
        {
            resultoutput.Text = $"Fehler: {ex.Message}";
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
        const string BaseUrl = "https://data.cerl.org/";

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
            resultrichtextblock.Blocks.Add(errorParagraph);
        }
    }

    private async void FetchAndDisplayJsonContent(string href)
    {
        const string BaseUrl = "https://data.cerl.org/";

        try
        {
            // Vollständige URL zusammenbauen
            string fullUrl = $"{BaseUrl.TrimEnd('/')}/{href}?_format=json";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");

                // HTTP-GET-Anfrage senden
                HttpResponseMessage response = await client.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();

                // JSON-Inhalt abrufen
                string jsonContent = await response.Content.ReadAsStringAsync();

                // JSON-Daten im RichTextBlock anzeigen
                DisplayJsonInRichTextBlock(jsonContent);

            }
        }
        catch (Exception ex)
        {
            // Fehlerbehandlung
            var errorParagraph = new Paragraph();
            errorParagraph.Inlines.Add(new Run
            {
                Text = $"Fehler beim Abrufen der Daten: {ex.Message}",

            });
            resultrichtextblock.Blocks.Add(errorParagraph);
        }
    }

    private void DisplayJsonInRichTextBlock(string jsonContent)
    {
        // RichTextBlock-Inhalt leeren
        resultrichtextblock.Blocks.Clear();

        try
        {
            // JSON-Objekt parsen
            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonContent);

            // Beispiel: Wenn das JSON eine Liste von Objekten enthält
            if (jsonObject?.result != null)
            {
                foreach (var item in jsonObject.result)
                {
                    // Name oder Beschreibung aus dem JSON extrahieren
                    string name = item.name?.ToString() ?? "Unbekannter Name";
                    string description = item.description?.ToString() ?? "";

                    // Standardtext hinzufügen
                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(new Run { Text = name });

                    // Beschreibung als untergeordneten Absatz hinzufügen
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        var descriptionRun = new Run
                        {
                            Text = $"\n{description}",
                            FontStyle = Windows.UI.Text.FontStyle.Italic
                        };
                        paragraph.Inlines.Add(descriptionRun);
                    }

                    resultrichtextblock.Blocks.Add(paragraph);
                }
            }
            else
            {
                // Wenn keine relevanten Daten vorhanden sind
                var noDataParagraph = new Paragraph();
                noDataParagraph.Inlines.Add(new Run { Text = "Keine relevanten Daten im JSON gefunden." });
                resultrichtextblock.Blocks.Add(noDataParagraph);
            }
        }
        catch (Exception ex)
        {
            // Fehler beim JSON-Parsen
            var errorParagraph = new Paragraph();
            errorParagraph.Inlines.Add(new Run
            {
                Text = $"Fehler beim Verarbeiten der JSON-Daten: {ex.Message}",
            });
            resultrichtextblock.Blocks.Add(errorParagraph);
        }
    }


    private void DisplayHtmlInRichTextBlock(string htmlContent)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        // RichTextBlock-Inhalt leeren
        resultrichtextblock.Blocks.Clear();

        // HTML-Inhalt verarbeiten
        foreach (var node in htmlDoc.DocumentNode.Descendants())
        {
            if (node.Name == "p")
            {
                // Absätze hinzufügen
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run { Text = node.InnerText });
                resultrichtextblock.Blocks.Add(paragraph);
            }
            else if (node.Name == "b" || node.Name == "strong")
            {
                // Fettschrift
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run
                {
                    Text = node.InnerText,
                });
                resultrichtextblock.Blocks.Add(paragraph);
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
                resultrichtextblock.Blocks.Add(paragraph);
            }
            else if (!string.IsNullOrWhiteSpace(node.InnerText))
            {
                // Standardtext
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run { Text = node.InnerText });
                resultrichtextblock.Blocks.Add(paragraph);
            }
        }
    }

    private void DisplayParsedHtml(string htmlContent)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        // RichTextBlock-Inhalt leeren
        resultrichtextblock.Blocks.Clear();

        // Alle Knoten mit der Klasse "ct-infobox" finden
        var headingNodes = htmlDoc.DocumentNode.SelectNodes("//h2[@class='ample-heading '][following-sibling::*[1][@class='ct-infobox']]");

        if (headingNodes == null || headingNodes.Count == 0)
        {
            // Wenn keine Knoten gefunden wurden
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run { Text = "Keine Inhalte mit der Klasse 'ct-infobox' gefunden." });
            resultrichtextblock.Blocks.Add(paragraph);
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
            resultrichtextblock.Blocks.Add(headingParagraph);

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
                        resultrichtextblock.Blocks.Add(linkParagraph);
                    }
                }
                // Absatz nach der ct-infobox hinzufügen
                resultrichtextblock.Blocks.Add(new Paragraph());
            }
        }
    }

    private void DisplayFilteredHtmlInRichTextBlock(string htmlContent)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        // RichTextBlock-Inhalt leeren
        resultrichtextblock.Blocks.Clear();

        // Alle Knoten mit relevanten Klassen suchen
        var containerNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@class='container ample-content']");

        if (containerNode == null)
        {
            // Nachricht anzeigen, falls kein Knoten gefunden wurde
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run { Text = "Keine Inhalte mit der Klasse 'container ample-content' gefunden." });
            resultrichtextblock.Blocks.Add(paragraph);
            return;
        }

        // Alle Knoten mit der Klasse "ct-infobox" finden
        var infoboxNodes = containerNode.SelectNodes(".//*[@class='ct-infobox' or @class='ct-moreinfo']");

        if (infoboxNodes == null || infoboxNodes.Count == 0)
        {
            // Nachricht anzeigen, falls keine "ct-infobox" gefunden wurde
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run { Text = "Keine 'ct-infobox'-Inhalte gefunden." });
            resultrichtextblock.Blocks.Add(paragraph);
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
                resultrichtextblock.Blocks.Add(headingParagraph);
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
                                    resultrichtextblock.Blocks.Add(pairParagraph);
                                    resultrichtextblock.Blocks.Add(new Paragraph()); // Leerzeile hinzufügen
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
                            resultrichtextblock.Blocks.Add(pairParagraph);
                            resultrichtextblock.Blocks.Add(new Paragraph()); // Leerzeile hinzufügen
                        }
                    }
                }
            }
            resultrichtextblock.Blocks.Add(new Paragraph()); // Leerzeile hinzufügen
        }
    }
}



