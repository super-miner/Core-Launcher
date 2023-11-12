using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

[GlobalClass]
public partial class LoadingBarSegment : Node {
    [Export] public double TotalPercent;
    public double Percent {
        get {
            return _percent * TotalPercent;
        }
        set {
            _percent = value;
        }
    }

    private double _percent;
}