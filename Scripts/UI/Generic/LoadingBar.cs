using System;
using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

[GlobalClass]
public partial class LoadingBar : ProgressBar {
    [Export] public double SmoothingCoefficient = 0.0f;
    [Export] public double TargetValue = 0.0f;

    [Export] private Label _label;

    private string _oldText = "";

    public override void _Ready() {
        Value = TargetValue;
    }

    public override void _Process(double delta) {
        double targetDistance = TargetValue - Value;
        Value += Math.Sqrt(targetDistance) * SmoothingCoefficient;
    }

    public void SetValue(double value, string text) {
        TargetValue = value;

        if (_oldText != text) {
            _label.Text = text;
            GD.Print($"Loading Bar: {text}");

            _oldText = text;
        }
    }

    public double GetValue() {
        return TargetValue;
    }
}