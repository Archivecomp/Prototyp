﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuesmodell.Services;
public class Dictionaries
{
    public static readonly Dictionary<string, string> LetterNumberDict = new Dictionary<string, string>()
    {
        {"A", "0"},
        {"B", "1"},
        {"C", "2"},
        {"D", "3"},
        {"E", "4"},
        {"F", "5"},
        {"G", "6"},
        {"H", "7"},
        {"I", "8"},
        {"K", "9"},
        {"L", "10"},
        {"M", "11"},
        {"N", "12"},
        {"O", "13"},
        {"P", "14"},
        {"Q", "15"},
        {"R", "16"},
        {"S", "17"},
        {"T", "18"},
        {"U", "19"},
        {"X", "20"},
        {"Y", "21"},
        {"Z", "22"},
        {"Aa", "23"},
        {"Bb", "24"},
        {"Cc", "25"},
        {"Dd", "26"},
        {"Ee", "27"},
        {"Ff", "28"},
        {"Gg", "29"},
        {"Hh", "30"},
        {"Ii", "31"},
        {"Ij", "31"},
        {"Kk", "32"},
        {"Ll", "33"},
        {"Mm", "34"},
        {"Nn", "35"},
        {"Oo", "36"},
        {"Pp", "37"},
        {"Qq", "38"},
        {"Rr", "39"},
        {"Ss", "40"},
        {"Tt", "41"},
        {"Uu", "42"},
        {"Xx", "43"},
        {"Yy", "44"},
        {"Zz", "45"},
        {"AAa", "46"},
        {"BBb", "47"},
        {"CCc", "48"},
        {"DDd", "49"},
        {"EEe", "50"},
        {"FFf", "51"},
        {"GGg", "52"},
        {"HHh", "53"},
        {"IIj", "54"},
        {"KKk", "55"},
        {"LLl", "56"},
        {"MMm", "57"},
        {"NNn", "58"},
        {"OOo", "59"},
        {"PPp", "60"},
        {"QQq", "61"},
        {"RRr", "62"},
        {"SSs", "63"},
        {"TTt", "64"},
        {"UUu", "65"},
        {"XXx", "66"},
        {"YYy", "67"},
        {"ZZz", "68"}
    };

    public static readonly Dictionary<string, string> FormatDict = new Dictionary<string, string>()
    {
        {"2", "4"},
        {"4", "8"},
        {"8", "16"},
        {"12", "24"},
        {"16", "32"},
        {"18", "36"},
        {"20", "40"},
        {"24", "48"},
        {"Folio", "4"},
        {"Quart", "8"},
        {"Oktav", "16"},
        {"Duodez", "24"},
        {"Sedez", "32"},
        {"Oktodez", "36"},
        {"Vigesimo", "40"},
        {"Vigesimoquart", "48"},
        {"folio", "4"},
        {"quart", "8"},
        {"oktav", "16"},
        {"duodez", "24"},
        {"sedez", "32"},
        {"oktodez", "36"},
        {"vigesimo", "40"},
        {"vigesimoquart", "48"}
    };

    public static readonly Dictionary<string, string> NumberLetterdict = new Dictionary<string, string>()
    {
        { "0", "A" },
        { "1", "B" },
        { "2", "C" },
        { "3", "D" },
        { "4", "E" },
        { "5", "F" },
        { "6", "G" },
        { "7", "H" },
        { "8", "I" },
        { "9", "K" },
        { "10", "L" },
        { "11", "M" },
        { "12", "N" },
        { "13", "O" },
        { "14", "P" },
        { "15", "Q" },
        { "16", "R" },
        { "17", "S" },
        { "18", "T" },
        { "19", "U" },
        { "20", "X" },
        { "21", "Y" },
        { "22", "Z" },
        { "23", "Aa" },
        { "24", "Bb" },
        { "25", "Cc" },
        { "26", "Dd" },
        { "27", "Ee" },
        { "28", "Ff" },
        { "29", "Gg" },
        { "30", "Hh" },
        { "31", "Ij" },
        { "32", "Kk" },
        { "33", "Ll" },
        { "34", "Mm" },
        { "35", "Nn" },
        { "36", "Oo" },
        { "37", "Pp" },
        { "38", "Qq" },
        { "39", "Rr" },
        { "40", "Ss" },
        { "41", "Tt" },
        { "42", "Uu" },
        { "43", "Xx" },
        { "44", "Yy" },
        { "45", "Zz" },
        { "46", "AAa" },
        { "47", "BBb" },
        { "48", "CCc" },
        { "49", "DDd" },
        { "50", "EEe" },
        { "51", "FFf" },
        { "52", "GGg" },
        { "53", "HHh" },
        { "54", "IIj" },
        { "55", "KKk" },
        { "56", "LLl" },
        { "57", "MMm" },
        { "58", "NNn" },
        { "59", "OOo" },
        { "60", "PPp" },
        { "61", "QQq" },
        { "62", "RRr" },
        { "63", "SSs" },
        { "64", "TTt" },
        { "65", "UUu" },
        { "66", "XXx" },
        { "67", "YYy" },
        { "68", "ZZz" }
    };

    public static readonly Dictionary<string, string> SigNumber = new Dictionary<string, string>()
    {
        { "1", "1" },
        { "2", " (1 verso)" },
        { "3", "2" },
        { "4", " (2 verso)" },
        { "5", "3" },
        { "6", " (3 verso)" },
        { "7", "4" },
        { "8", " (4 verso)" },
        { "9", "5" },
        { "10", " (5 verso)" },
        { "11", "6" },
        { "12", " (6 verso)" },
        { "13", "7" },
        { "14", " (7 verso)" },
        { "15", "8" },
        { "16", " (8 verso)" },
        { "17", "9" },
        { "18", " (9 verso)" },
        { "19", "10" },
        { "20", " (10 verso)" },
        { "21", "11" },
        { "22", " (11 verso)" },
        { "23", "12" },
        { "24", " (12 verso)" },
        { "25", "13" },
        { "26", " (13 verso)" },
        { "27", "14" },
        { "28", " (14 verso)" },
        { "29", "15" },
        { "30", " (15 verso)" },
        { "31", "16" },
        { "32", " (16 verso)" },
        { "33", "17" },
        { "34", " (17 verso)" },
        { "35", "18" },
        { "36", " (18 verso)" },
        { "37", "19" },
        { "38", " (19 verso)" },
        { "39", "20" },
        { "40", " (20 verso)" },
        { "41", "21" },
        { "42", " (21 verso)" },
        { "43", "22" },
        { "44", " (22 verso)" },
        { "45", "23" },
        { "46", " (23 verso)" },
        { "47", "24" },
        { "48", " (24 verso)" }
    };

    public static readonly Dictionary<char, int> RomanNumbers = new Dictionary<char, int>()
    {
        {'I', 1},
        {'V', 5},
        {'X', 10},
        {'L', 50},
        {'C', 100},
        {'D', 500},
        {'M', 1000}
    };

    public static readonly Dictionary<string, int> MonthNumbers = new Dictionary<string, int>()
    {
        {"Ianuarius", 1},
        {"Februarius", 2},
        {"Martius", 3},
        {"Aprilis", 4},
        {"Maius", 5},
        {"Iunius", 6},
        {"Quintilis/Iulius", 7},
        {"Sextilis/Augustus", 8},
        {"September", 9},
        {"October", 10},
        {"November", 11},
        {"December", 12}
    };
}
