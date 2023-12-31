// We developed a simple but useful MMORPG style camera. The player can zoom in
// and out with the mouse wheel and rotate the camera around the hero by holding
// down the right mouse button.
//
// Note: we turned off the linecast obstacle detection because it would require
// colliders on all environment models, which means additional physics
// complexity (which is not needed due to navmesh movement) and additional
// components on many gameobjects. Even if performance is not a problem, there
// is still the weird case where if a tent would have a collider, the inside
// would still be part of the navmesh, but it's not clickable because of to the 
// collider. Clicking on top of that collider would move the agent into the tent
// though, which is not very good. Not worrying about all those things and
// having a fast server is a better tradeoff.
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMMO : MonoBehaviour {
    public Transform target;

    public int mouseButton = 1; // right button by default

    public float distance = 20f;
    public float minDistance = 3f;
    public float maxDistance = 20f;

    public float zoomSpeed = 0.2f;
    public float rotationSpeed = 2f;

    public float yMinAngle = -40f;
    public float yMaxAngle = 80f;

    // the target position can be adjusted by an offset in order to foucs on a
    // target's head for example
    public Vector3 offset = Vector3.zero;

    // the layer mask to use when trying to detect view blocking
    // (this way we dont zoom in all the way when standing in another entity)
    // (-> create a entity layer for them if needed)
    //public LayerMask layerMask;

    // store rotation so that unity never modifies it, otherwise unity will put
    // it back to 360 as soon as it's <0, which makes a negative min angle
    // impossible
    Vector3 rot;
    Transform tr;

    void Awake () {
        tr = transform;
        rot = tr.eulerAngles;
    }

    void LateUpdate () {
        if (!target) return;

        var targetPos = target.position + offset;

        // rotation and zoom should only happen if not in a UI right now
        // note: this only works if the UI's CanvasGroup blocks Raycasts
        if (!EventSystem.current.IsPointerOverGameObject()) {
            // mouse button down?
            if (Input.GetMouseButton(mouseButton)) {
                // note: mouse x is for y rotation and vice versa
                rot.y += Input.GetAxis("Mouse X") * rotationSpeed;
                rot.x -= Input.GetAxis("Mouse Y") * rotationSpeed;
                rot.x = Mathf.Clamp(rot.x, yMinAngle, yMaxAngle);
                tr.rotation = Quaternion.Euler(rot.x, rot.y, 0f);
            }

            // zoom (speed scales with distance)
            // we use a custom getaxis function to get same scroll speed on all
            // platforms
            var scroll = Utils.GetAxisRawScrollUniversal();
            var step = scroll * zoomSpeed * Mathf.Abs(distance);
            distance = Mathf.Clamp(distance - step, minDistance, maxDistance);
        }
        
        // target follow
        tr.position = targetPos - (tr.rotation * Vector3.forward * distance);

        // avoid view blocking (disabled, see comment at the top)
        //RaycastHit hit;
        //if (Physics.Linecast(targetPos, tr.position, out hit, layerMask)) {
        //    // calculate a better distance (with some space between it)
        //    float d = Vector3.Distance(targetPos, hit.point) - 0.1f;
        //
        //    // set the final cam position
        //    tr.position = targetPos - (tr.rotation * Vector3.forward * d);
        //}
    }
}
