using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnitionController : MonoBehaviour{
    // トリガーが惹かれてたらTrue
    // 引かれてなかったらFalse
    // 正直命名しくじった 要リファクタリング
    private bool ignitionFlag = false;

    protected OVRGrabbable object_ovrGrabbable_component = null;

    [SerializeField]
    protected AudioClip soundEffect = null;

    protected AudioSource object_audioSource_component = null;

    protected void Start(){
        if(this.gameObject.TryGetComponent(out this.object_ovrGrabbable_component) == false){
            outputError("this object must have \"OVRGrabbable\" component.");
        }
        if(this.soundEffect == null){
            outputError("Value \"soundEffect\" must not be null.");
        }
        if(this.gameObject.TryGetComponent(out this.object_audioSource_component) == false){
            outputError("this object must have \"Audio Source\" component.");
        }
    }
    protected void Update(){
        bool isGrabbed = this.object_ovrGrabbable_component.isGrabbed;
        bool pullTrigger = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
        if(isGrabbed == true && pullTrigger == true){
            this.object_audioSource_component.PlayOneShot(soundEffect);
            ignitionFlag = true;
        }else{
            ignitionFlag = false;
        }
    }

    public bool getIgnitionFlag(){
        return ignitionFlag;
    }
//
// ---------------------------------------------
// ログ生成関連
// 標準出力に出力する
//
    // ログを生成
    protected void outputLog(string msg){
        Debug.Log("IgnitionController:" + msg);
    }

    // エラーログを生成
    protected void outputError(string msg){
        Debug.LogError("IgnitionController:" + msg);
    }

    // ワーニングログを生成
    protected void outputWarning(string msg){
        Debug.LogWarning("IgnitionController:" + msg);
    }
}
