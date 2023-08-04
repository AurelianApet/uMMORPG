// The default NetworkProximityChecker requires a collider on the same object,
// but in some cases we want to put the collider onto a child object (e.g. for
// animations).
//
// Originally we used a custom NetworkProximityChecker script that also searches
// for colliders in child objects, but there is a IL2CPP bug which makes WebGL
// builds fail when using that. The bug was reported as #788203.
//
// As a workaround, we will attach this script to all Entities, so that they
// automatically get a collider attached to them. Using this script is better
// than just attaching a small collider, because this way everyone knows the
// reasoning behind that seemingly useless collider.
using UnityEngine;

public class NetworkProximityFix : MonoBehaviour {
    void Awake() {
        if (GetComponent<Collider>() == null) {
            gameObject.AddComponent<BoxCollider>();
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
