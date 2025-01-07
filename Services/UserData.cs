using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace neuesmodell.Services;
public class UserData
{
    public string Potenzieller_Werktitel { get; set; }
    public string Verfasser { get; set; }
    public string Informationen_zum_Verfasser { get; set; }
    public string Titelblatttext { get; set; }
    public string Inhalt { get; set; }
    public string Auflage { get; set; }
    public string Sprache { get; set; }
    public string Umfang { get; set; }
    public string Ort { get; set; }
    public string Verleger_Drucker { get; set; }
    public string Informationen_zum_Verleger_Drucker { get; set; }
    public string Erscheinungsjahr { get; set; }
    public string Weitere_Druckinformationen { get; set; }
    public string Provenienzmerkmale { get; set; }
    public string VD_Nummer { get; set; }
    public string Digitalisat { get; set; }
}

public class UserCollection
{
    public Dictionary<string, UserData> Users { get; set; } = new Dictionary<string, UserData>();
}
