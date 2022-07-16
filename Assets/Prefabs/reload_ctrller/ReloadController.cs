using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadController : MonoBehaviour{
    protected OVRGrabbable object_ovrGrabbable_component = null;



    void Start(){
        if(this.gameObject.TryGetComponent(out this.object_ovrGrabbable_component) == false){
            Debug.LogError("reload Controller: must have \"OVRGrabbable\" Component.");
        }
    }

    void Update(){
        bool isGrabbed = this.object_ovrGrabbable_component.isGrabbed;
        bool pullTrigger = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
        if(isGrabbed == true && pullTrigger == true){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
