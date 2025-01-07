
using System.Globalization;
using neuesmodell.Services;
using neuesmodell.Views.Pages.Werkzeuge;

namespace neuesmodell.Views.Pages.Werkzeuge;

public sealed partial class Datumsrechner : Page
{
    public Datumsrechner()
    {
        this.InitializeComponent();
    }

    private void Datumsrechnerrechner_combobox_Loaded(object sender, RoutedEventArgs e)
    {
        Datumsrechner_combobox.SelectedItem = Datumsrechner_combobox.Items[0];
    }
    private void Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

                resultoutput.Text = "Die gesuchte Jahreszahl ist";
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

                resultoutput.Text = "Das gesuchte Datum ist";
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

                resultoutput.Text = "Das gesuchte Jahr ist";
            }
        }
    }
    private void Combobox_SelectionChanged2(object sender, SelectionChangedEventArgs e)
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
    private void BerechnenButton_Click(object sender, RoutedEventArgs e)
    {
        if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Jahreszahl")
        {
            string roman = textboxyear1.Text;
            try
            {
                int Jahreszahl = RomanToArabic(roman);

                resultoutput.Text = $"Die gesuchte Jahreszahl ist {Jahreszahl.ToString()}";
            }
            catch (ArgumentException ex)
            {
                resultoutput.Text = ex.Message;
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

                resultoutput.Text = $"Das Datum {dayPart} {typePart} {monthPart} {roman} entspricht dem {resultDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}.";
            }
            catch (ArgumentException ex)
            {
                resultoutput.Text = ex.Message;
            }
        }
        else if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Chronogramm")
        {
            string chronogram = textboxchronogramm3.Text;

            try
            {
                int year = ConvertChronogramToYear(chronogram);

                resultoutput.Text = $"Das Chronogramm entspricht dem Jahr {year.ToString()}.";
            }
            catch (ArgumentException ex)
            {
                resultoutput.Text = ex.Message;
            }
        }
    }
    private void LöschenButton_Click(Object sender, RoutedEventArgs e)
    {
        if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Jahreszahl")
        {
            textboxyear1.Text = "";

            resultoutput.Text = "Die gesuchte Jahreszahl ist";
        }
        else if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Julianischer Kalender")
        {
            textboxyear2.Text = "";

            resultoutput.Text = "Das Datum   entspricht dem   .";
        }
        else if (Datumsrechner_combobox.SelectedItem != null && (Datumsrechner_combobox.SelectedItem as ComboBoxItem).Content.ToString() == "Chronogramm")
        {
            textboxchronogramm3.Text = "";

            resultoutput.Text = "Das Chronogramm entspricht dem Jahr   .";
        }
    }
}

