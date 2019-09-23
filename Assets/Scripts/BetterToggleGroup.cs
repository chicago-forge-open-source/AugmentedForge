using UnityEngine;
using UnityEngine.UI;
using System.Linq;
 
public sealed class BetterToggleGroup : ToggleGroup {
    public delegate void ChangedEventHandler(Toggle newActive);
    public event ChangedEventHandler OnChange;

    protected override void Start() {
        foreach (Transform transformToggle in gameObject.transform) {
            var toggle = transformToggle.gameObject.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(isSelected => {
                if (!isSelected) { return; }
                DoOnChange(Active());
            });
        }
    }

    private Toggle Active() {
        return ActiveToggles().FirstOrDefault();
    }

    private void DoOnChange(Toggle newActive)
    {
        var handler = OnChange;
        handler?.Invoke(newActive);
    }
}