using UnityEngine;
using UnityEngine.UI;

public class TextCopyName : MonoBehaviour {
    public GameObject source;

    void Update() {
        GetComponent<Text>().text = source.name;
    }
}
