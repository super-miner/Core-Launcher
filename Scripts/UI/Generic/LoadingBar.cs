using System;
using System.Collections.Generic;
using System.Linq;
using CoreLauncher.Scripts.Systems;
using Godot;
using Godot.Collections;

namespace CoreLauncher.Scripts.UI.Generic; 

[GlobalClass]
public partial class LoadingBar : ProgressBar {
    [Export] public double SmoothingCoefficient = 0.0f;
    [Export] public double TargetValue = 0.0f;
    
    [Export] private Label _label;
    private Mutex _targetValueMutex = new Mutex();
    private string _oldText = "";

    public override void _Ready() {
        _targetValueMutex.Lock();
        Value = TargetValue;
        _targetValueMutex.Unlock();
    }

    public override void _Process(double delta) {
        _targetValueMutex.Lock();
        
        double targetDistance = TargetValue - Value;
        if (targetDistance > 0) {
            Value += Math.Sqrt(targetDistance) * SmoothingCoefficient;
        }
        
        _targetValueMutex.Unlock();
    }

    public void SetValue(string segmentName, double value, string text) {
        _targetValueMutex.Lock();
        
        LoadingBarSegment segment = GetSegment(segmentName);

        if (segment == null) {
            GD.PrintErr($"Loading Bar: Could not find segment {segmentName}.");
            return;
        }
        
        segment.Percent = value;

        if (_oldText != text) {
            _label.Text = text;
            GD.Print($"Loading Bar: {text}");
            _oldText = text;
        }
        
        UpdateTargetValue();
        
        _targetValueMutex.Unlock();
    }

    public double GetValue() {
        _targetValueMutex.Lock();
        double targetValue = TargetValue;
        _targetValueMutex.Unlock();
        return targetValue;
    }

    public LoadingBarSegment GetSegment(string name) {
        Node segmentNode = GodotUtil.GetChildrenWithName(this, name).FirstOrDefault(segment => segment is LoadingBarSegment);
        return (LoadingBarSegment)segmentNode;
    }

    private void UpdateTargetValue() {
        TargetValue = GodotUtil.GetChildrenWithType<LoadingBarSegment>(this).Sum(segment => segment.Percent);
    }
}