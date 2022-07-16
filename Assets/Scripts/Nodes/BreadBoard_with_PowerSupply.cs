using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BreadBoard_with_PowerSupply : MonoBehaviour{
    // 配置用オブジェクトの配列
    // 各種NodeはこいつらSlotオブジェクトの子になってないといけない
    // あとSlotとNodeは1-1の関係じゃないとダメ 死ぬ
    [SerializeField]
    protected GameObject[] SlotGameObjectArray = {};

    // 素子用オブジェクトの配列
    protected GameObject[] NodeGameObjectArray = {};

    // スロットに設置されたノードのタイプ一覧
    // [SerializeField]
    protected List<string> NodeTypeInSlotList = new List<string>{};

    // 作るモノ
    [SerializeField]
    protected GameObject target_object = null;

    // 発火用のコントローラ(という名のスイッチ代理)
    [SerializeField]
    protected GameObject Ignition_Ctrler_Object = null;
    protected IgnitionController object_ignition_Ctrler_component = null;

    // SE
    [SerializeField]
    protected AudioClip se_accept = null;
    [SerializeField]
    protected AudioClip se_reject = null;

    protected AudioSource object_audioSource = null;


    // ランプ用
    [SerializeField]
    protected Material mat_lamp_accept = null;

    [SerializeField]
    protected Material mat_lamp_reject = null;

    [SerializeField]
    protected Material mat_lamp_null = null;

    [SerializeField]
    protected GameObject object_lamp = null;

    protected Renderer object_lamp_renderer_component = null;

    // テキスト用
    [SerializeField]
    protected GameObject object_text = null;
    protected Text object_text_text_component = null;


    private bool tempFlag = false;
    private bool activeFlag = false;

    [SerializeField]
    protected GameObject object_clearText = null;

    protected void Start(){
        if(this.Ignition_Ctrler_Object == null){
            outputError("Value \"Ignition_Ctrler_Object\" must not be null.");
        }else{
            if(this.Ignition_Ctrler_Object.TryGetComponent(out this.object_ignition_Ctrler_component) != true){
                outputError("Cant found \"IgnoreController\" Component on \"Ignition_Ctrler_Object\".");
            }
        }
        if(this.target_object == null){
            outputError("Value \"target_object\" must not be null.");
        }
        if(this.gameObject.TryGetComponent(out this.object_audioSource) == false){
            outputError("this object must have \"Audio Source\" component.");
        }
        if(this.se_accept == null){
            outputError("Value \"se_accept\" must not be null.");
        }
        if(this.se_reject == null){
            outputError("Value \"se_reject\" must not be null.");
        }
        this.target_object.SetActive(false);

        if(this.object_lamp == null){
            outputError("Value \"object_lamp\" must not be null.");
        }else{
            if(this.object_lamp.TryGetComponent(out this.object_lamp_renderer_component) == false){
                outputError("lamp ni Renderer ga nai nante koto aru???");
            }
        }

        if(this.object_text == null){
            outputError("Value \"object_text\" must not be null.");
        }
        if(this.object_text.TryGetComponent(out this.object_text_text_component) == false){
            outputError("  ");
        }else{
            dispNull();
        }

        if(this.object_clearText == null){
            outputError("Value \"object_clearText\" must not be null.");
        }
        this.object_clearText.SetActive(false);
    }

    protected void Update(){
        // トリガーを引かれたとき
        // トリガーを引く度に，runとendを交互に実行する
        if(this.object_ignition_Ctrler_component.getIgnitionFlag() == true){
            if(tempFlag != true){
                tempFlag = true;
                if(activeFlag != true){
                    if(runContinuityTest() == true){
                        runAllNode();
                        activeFlag = true;
                    }
                }else{
                    if(activeFlag == true){
                        endAllNode();
                        activeFlag = false;
                    }
                }
            }
        }else{
            tempFlag = false;
        }
    }

    // 導通チェック起点関数
    /* 返り値
    |   導通 : true
    | 非導通 : false
    */
    public bool runContinuityTest(){
        // 回路認識

        outputLog("runContinuityTest() : started");
        // そもそもスロットが配置用オブジェクトが空なら動作させない
        if(SlotGameObjectArray.Length == 0){
            return false;
        }

        // 素子用オブジェクトの配列を作成
        NodeGameObjectArray = new GameObject[SlotGameObjectArray.Length];

        //  作るときに，数/NodeClass不足なら弾く なんならreturn falseする
        for(int num = 0; num < SlotGameObjectArray.Length; num++){
            // SlotオブジェクトArrayにtag:Slotのオブジェクトが入ってるかを確認
            // 入ってなかったら処理を飛ばす
            GameObject SlotGameObject = SlotGameObjectArray[num].gameObject;
            if(SlotGameObject.tag != "Slot"){
                // gameObjectは tag:Slot を持たなければならない
                outputWarning("SlotGameObjectArray["+num+"].gameObject has not tag:Slot.¥nGameObject in SlotGameObjectArray must have tag:Slot.");
                continue;
            }

            // tag: Nodeを持つ子のオブジェクトを取得
            GameObject NodeGameObject = FindChildWithTag(SlotGameObject, "Node");
            // 子のオブジェクトが空
            if(NodeGameObject == null){
                // gameObjectは子オブジェクトを持たなければならない
                outputWarning("SlotGameObjectArray["+num+"].gameObject must have child GameObject.");
                continue;
            }

            // NodeClassが無いなら弾く
            if(NodeGameObject.TryGetComponent(out NodeClass nc) == false){
                return false;
            }

            // 追加
            NodeGameObjectArray[num] = NodeGameObject;

            // ノードタイプ一覧への追加
            if(NodeGameObject.TryGetComponent(out NodeClass nc2) == true){
                if(this.NodeTypeInSlotList.Contains(nc2.getNodeType()) == false){
                    this.NodeTypeInSlotList.Add(nc2.getNodeType());
                }
            }
        }

        // 最終チェック
        // NodeGameObjectArray に null Object が存在しないことを確認して，その結果を返す
        if(Array.IndexOf(NodeGameObjectArray, null) == -1){
            outputLog("return 0");
            return true;
        }else{
            dispReject();
            return false;
        }
    }

    // 必要なノードタイプが揃ってるか確認して，
    // 全Slot内のNodeObjectのrun関数を叩く
    public void runAllNode(){
        outputLog("runAllNode() start");
        foreach(string nodeType in this.NodeTypeInSlotList){
            switch(nodeType){
                case "LED":
                    if(this.NodeTypeInSlotList.Contains("Resistor")==false){
                        outputError("this circuit need \"Resistor\" Node.");
                        this.object_audioSource.PlayOneShot(se_reject);
                        dispReject();
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
        dispAccept();
        this.object_audioSource.PlayOneShot(se_accept);

        foreach (GameObject nodeGameObject in this.NodeGameObjectArray){
            if(nodeGameObject.TryGetComponent(out NodeClass nc) == true){
                nc.runNode();
            }
        }
        Invoke(nameof(objective), 0.5f);
        dispNull();
    }

    // 全Slot内のNodeObjectのendNode関数を叩く
    public void endAllNode(){
        outputLog("endAllNode() start");
        foreach (GameObject nodeGameObject in this.NodeGameObjectArray){
            if(nodeGameObject.TryGetComponent(out NodeClass nc) == true){
                nc.endNode();
            }
        }
    }

    protected void objective(){
        //Instantiate(this.target_object,Vector3.zero,Quaternion.Euler(0,0,0));
        this.target_object.SetActive(true);
        this.object_clearText.SetActive(true);
    }

    // 引数のオブジェクトの子オブジェクト(直下のみ)の中から，指定したタグを持つオブジェクトを返す
    // 無かったらnullを返す
    // return "object" if exist object had tag, "null" otherwise
    public GameObject FindChildWithTag(GameObject _parent_Object, string _tag){
        int child_count = _parent_Object.transform.childCount;
        if(child_count == 0){
            return null;
        }

        for(int i = 0; i < child_count; i++){
            GameObject child_Object =  _parent_Object.transform.GetChild(i).gameObject;
            if(child_Object.tag == _tag){
                return child_Object;
            }
        }
        return null;
    }

    public void dispAccept(){
        this.object_lamp_renderer_component.material = this.mat_lamp_accept;
        this.object_text_text_component.text = "Accept";
    }

    public void dispReject(){
        this.object_lamp_renderer_component.material = this.mat_lamp_reject;
        this.object_text_text_component.text = "Reject";
    }
    public void dispNull(){
        this.object_lamp_renderer_component.material = this.mat_lamp_null;
        this.object_text_text_component.text = "";
    }

    // -------------
    // getter/setter
    // -------------

    // とりあえずそのまま返すだけ
    public GameObject[] getBaseGameObjectArray(){
        return SlotGameObjectArray;
    }

    // チェッカー
    public bool checkExistInSlotGameObjectArray(GameObject add_Object){
        // そもそも空オブジェクトだったらreject
        if(add_Object == null){
            return false;
        }

        // add_Object が SlotGameObjectArray 内に存在しないことの確認
        if(Array.IndexOf(SlotGameObjectArray, add_Object) != -1){
            return true;
        }else{
            return false;
        }
    }

//
// ---------------------------------------------
// ログ生成関連
// 標準出力に出力する
//
    // ログを生成
    protected void outputLog(string msg){
        Debug.Log("BreadBoard:" + msg);
    }

    // エラーログを生成
    protected void outputError(string msg){
        Debug.LogError("BreadBoard:" + msg);
    }

    // ワーニングログを生成
    protected void outputWarning(string msg){
        Debug.LogWarning("BreadBoard:" + msg);
    }
}
