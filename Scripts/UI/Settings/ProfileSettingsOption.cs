using System;
using System.Linq;
using System.Reflection;
using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI.Settings; 

[GlobalClass]
public partial class ProfileSettingsOption : Node {
    [Export] public string SettingName = "";
    
    public bool GetSetting<T>(ref T setting) {
        ItemListEntry selectedEntry = InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry();

        if (selectedEntry == null) {
            GD.PrintErr("The selected profile was null.");
            return false;
        }
        
        if (selectedEntry is ProfileListEntry selectedProfileEntry) {
            string methodName = "Get" + SettingName;
            MethodInfo method = typeof(ProfileListEntry).GetMethod(methodName);

            if (method != null) {
                ParameterInfo[] parameters = method.GetParameters();
                
                object result = method.Invoke(selectedProfileEntry, Enumerable.Repeat(Type.Missing, parameters.Length).ToArray());

                if (result != null) {
                    if (result is T convertedResult) {
                        setting = convertedResult;
                        return true;
                    }
                }
                else {
                    GD.PrintErr($"The function {methodName} did not return a value.");
                }
            }
            else {
                GD.PrintErr($"Could not find a function by the name of {methodName}.");
            }
        }
        else {
            GD.PrintErr("The selected entry was not a ProfileListEntry.");
        }

        return false;
    }

    public void SetSetting(object setting) {
        ItemListEntry selectedEntry = InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry();
        
        if (selectedEntry == null) {
            GD.PrintErr("The selected profile was null.");
            return;
        }

        if (selectedEntry != null && selectedEntry is ProfileListEntry selectedProfileEntry) {
            string methodName = "Set" + SettingName;
            MethodInfo method = typeof(ProfileListEntry).GetMethod(methodName);

            if (method != null) {
                ParameterInfo[] parameters = method.GetParameters();
            
                if (parameters.Length > 0) {
                    Type parameterType = parameters[0].ParameterType;
                    Type settingType = SettingName.GetType();
                    
                    if (parameterType.IsAssignableFrom(settingType)) {
                        object[] inputs = new object[parameters.Length];
                        inputs[0] = setting;

                        for (int i = 1; i < inputs.Length; i++) {
                            inputs[i] = Type.Missing;
                        }
                        
                        method.Invoke(selectedProfileEntry, inputs);
                    }
                    else {
                        GD.PrintErr($"The type {settingType} could not be assigned to {parameterType}.");
                    }
                }
                else {
                    GD.PrintErr($"The method {methodName} had no arguments.");
                }
            }
            else {
                GD.PrintErr($"Could not find a function by the name of {methodName}.");
            }
        }
        else {
            GD.PrintErr("The selected entry was not a ProfileListEntry.");
        }
    }
}