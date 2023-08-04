// Colors the name overlay in case of offender/murderer status.
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameColor : MonoBehaviour {
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color offenderColor = Color.magenta;
    [SerializeField] Color murdererColor = Color.red;

    void Update() {
        // note: murderer has higher priority (a player can be a murderer and an
        // offender at the same time)
        var player = GetComponent<Player>();
        if (player.IsMurderer())
            GetComponentInChildren<Text>().color = murdererColor;
        else if (player.IsOffender())
            GetComponentInChildren<Text>().color = offenderColor;
        else
            GetComponentInChildren<Text>().color = defaultColor;
    }
}
