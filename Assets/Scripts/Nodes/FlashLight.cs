using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour{
    [SerializeField]
    protected Material dark_Material = null;

    [SerializeField]
    protected Material bright_Material = null;

    [SerializeField]
    protected GameObject pointLight_object = null;

    [SerializeField]
    protected GameObject lightPart_object = null;

    protected Renderer lightPart_renderer_component = null;

    protected OVRGrabbable object_ovrGrabbable_component = null;

    protected bool activeFlag = false;


    // for SpawnEffect
    protected bool spawnPhase = false;
    protected GameObject flashlight_object = null;
    protected GameObject effect_sphere_object = null;
    protected GameObject ParticleSystem_object = null;
    protected float timer = 0;
    public float spawnEffectTime = 2;

    protected void Start(){
        if(this.lightPart_object.TryGetComponent(out this.lightPart_renderer_component) == false){
            outputError("Can't found \"Renderer\" Component on this Object.");
        }
        if(this.dark_Material == null){
            outputError("Value \"dark_Material\" must not be null.");
        }
        if(this.bright_Material == null){
            outputError("Value \"bright_Material\" must not be null.");
        }
        if(this.gameObject.TryGetComponent(out this.object_ovrGrabbable_component) == false){
            outputError("Can't found \"OVRGrabbable\" Component on this Object.");
        }
        this.activeFlag = false;
        this.pointLight_object.SetActive(false);

        // エフェクト関連
        this.flashlight_object = this.gameObject.transform.FindChildRecursive("flashlight").gameObject;
        if(this.flashlight_object == null){
            outputError("Can't found \"flashlight\" in this object.");
        }
        this.effect_sphere_object = this.gameObject.transform.FindChildRecursive("Effect_Sphere").gameObject;
        if(this.effect_sphere_object == null){
            outputError("Can't found \"Effect_Sphere\" in this object.");
        }
        this.ParticleSystem_object = this.gameObject.transform.FindChildRecursive("Particle System").gameObject;
        if(this.ParticleSystem_object == null){
            outputError("Can't found \"Particle System\" in this object.");
        }

        this.flashlight_object.SetActive(false);
        this.effect_sphere_object.SetActive(true);
        this.ParticleSystem_object.SetActive(true);
        this.effect_sphere_object.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        this.spawnPhase = true;

        Debug.Log("flash light");
    }

    protected void Update(){
        if(this.object_ovrGrabbable_component.isGrabbed == true
        && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)){
            if(activeFlag != true){
                this.lightPart_renderer_component.material = bright_Material;
                activeFlag = true;
                this.pointLight_object.SetActive(true);
            }else{
                this.lightPart_renderer_component.material = dark_Material;
                activeFlag = false;
                this.pointLight_object.SetActive(false);
            }
        }

        if(this.spawnPhase == true){
            timer += Time.deltaTime;
            if(timer >= spawnEffectTime){
                this.spawnPhase = false;
                this.effect_sphere_object.SetActive(false);
                this.flashlight_object.SetActive(true);
                //this.ParticleSystem_object.SetActive(false);
                this.ParticleSystem_object.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
                this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            float scale = (float)0.5 * Mathf.Sin(Mathf.PI * (timer / spawnEffectTime));
            this.effect_sphere_object.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    // --------------------------------------
    // ログ生成関連

    // ログを生成
    protected void outputLog(string _msg){
        Debug.Log(this.name + ":" + _msg);
    }

    // エラーログを生成
    protected void outputError(string _msg){
        Debug.LogError(this.name + ":" + _msg);
    }

    // ワーニングログを生成
    protected void outputWarning(string _msg){
        Debug.LogWarning(this.name + ":" + _msg);
    }
}
