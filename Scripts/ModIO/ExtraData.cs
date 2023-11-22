using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Godot;

namespace CoreLauncher.Scripts.ModIO; 

public class ExtraData {
    private static readonly Regex ExtraDataVariableRegex = new Regex(@"(.+?):(.+)");
    
    public List<int> Dependencies;
    public string DonationLink = "";
    public bool ServerSide = true;
    public bool ClientSide = true;
    public bool Library = false;

    public static ExtraData FromString(string extraDataString) {
        ExtraData output = new ExtraData();
        
        string[] splitVariables = extraDataString.Split('|');

        foreach (string splitVariable in splitVariables) {
            output.SetVariableFromString(splitVariable);
        }

        return output;
    }
    
    public void SetVariableFromString(string variableString) {
        Match match = ExtraDataVariableRegex.Match(variableString);

        if (!match.Success) {
            return;
        }

        string variableName = match.Groups[1].ToString();
        string variableValueString = match.Groups[2].ToString();

        switch (variableName) {
            case "Depend":
                string[] splitValueStrings = variableValueString.Split(',');
                Dependencies = new List<int>();

                foreach (string variableItemString in splitValueStrings) {
                    if (int.TryParse(variableItemString, out int variableValue)) {
                        Dependencies.Add(variableValue);
                    }
                }

                break;
            case "Donate":
                DonationLink = variableValueString;
                break;
            case "ServerSide":
                if (bool.TryParse(variableValueString, out bool serverSide)) {
                    ServerSide = serverSide;
                }

                break;
            case "ClientSide":
                if (bool.TryParse(variableValueString, out bool clientSide)) {
                    ClientSide = clientSide;
                }

                break;
            case "Library":
                if (bool.TryParse(variableValueString, out bool library)) {
                    Library = library;
                }

                break;
        }
    }
}