using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDNode : NodeClass{
    // ポイントライト用のオブジェクト
    [SerializeField]
    private GameObject pointLight_object = null;

    // ライト用オブジェクトのRendererComponent
    [SerializeField]
    private Renderer light_object_renderer = null;

    [SerializeField]
    protected Material darkColor = null;

    [SerializeField]
    protected Material shineColor = null;

    protected override void Start(){
        base.Start();
        SetNodeType("LED");
        addNeedNodeType(new List<string>{"Resistor"});

        if(pointLight_object == null){
            this.pointLight_object = this.gameObject.transform.GetChild(0).gameObject.transform.FindChildRecursive("Point_Light").gameObject;
        }
        if(this.gameObject.transform.GetChild(0).gameObject.transform.FindChildRecursive("Light").gameObject.TryGetComponent(out this.light_object_renderer) != true){
            outputError("LEDの頭にレンダーがアタッチされてない");
        }

        if(darkColor == null){
            outputError("Material is not attached to value \"darkColor\"");
        }
        if(shineColor == null){
            outputError("Material is not attached to value \"shineColor\"");
        }
        light_object_renderer.material = darkColor;
    }

    protected override void Update(){
        base.Update();
    }

    public override void runNode(){
        outputLog("run LEDNode.runNode.");
        this.pointLight_object.SetActive(true);
        light_object_renderer.material = shineColor;
    }

    public override void endNode(){
        this.pointLight_object.SetActive(false);
        light_object_renderer.material = darkColor;
    }
}
