using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    public Toggle[] toggles;
    public int activeTile;

    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i; // Capture the current index
            toggles[i].onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, index));
        }
    }

    public void OnToggleChanged(bool isOn, int index)
    {
        if (isOn)
        {
            activeTile = index;
            for (int i = 0; i < toggles.Length; i++)
            {
                if (i != index)
                {
                    toggles[i].isOn = false;
//                    Debug.Log(index);
                }
            }
        }
    }
}