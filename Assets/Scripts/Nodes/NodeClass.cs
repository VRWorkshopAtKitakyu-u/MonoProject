using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// このスクリプトがアタッチされたオブジェクトに OVRGrabbable コンポーネントが存在することを要求する
// OVRGrabbable 自体が rigidbody と ~Collider を要求するから，この2つは改めて要求しないが，今後することになるかもしれない
[RequireComponent(typeof(OVRGrabbable))]

public class NodeClass : MonoBehaviour{
    /* ノードタイプ
    | node = "Node"
    | 電源 = "PowerSupply"
    | 抵抗 = "Resistor"
    | LED  = "LED"
    | PIC  = "PIC"
    */
    // タグで管理する予定だが，一応仮宣言
    private string nodeType = "Node";

    // ノードタイプのリスト
    private List<string> nodeTypeList = new List<string>{"Node", "PowerSupply", "Resistor", "LED", "PIC"};

    // 必要とするノードタイプのリスト
    private List<string> need_nodeList = new List<string>{};

    // Slot Objectにいるか
    //private bool isSloted = false;
    [SerializeField]
    private OVRGrabbable ovrGrabbable_component = null;

    //private Collision contacted_SlotObject = null;

    private Rigidbody rigidbody_component = null;

    protected virtual void Start(){
        if(this.gameObject.TryGetComponent(out this.ovrGrabbable_component) != true){
            outputError("this object shold have \"OVRGrabbable\".");
            return;
        }

        if(this.gameObject.TryGetComponent(out this.rigidbody_component) != true){
            outputError("this object shold have\"Rigidbody\"");
            return;
        }
    }

    protected virtual void Update(){
        if(this.ovrGrabbable_component.isGrabbed == true){
            this.gameObject.transform.parent = null;
            //this.rigidbody_component.isKinematic = false;
        }
    }

    protected virtual void OnCollisionStay(Collision other){
        if(this.ovrGrabbable_component.isGrabbed != false){
            return;
        }
        if(other.gameObject.tag == "Slot"){
            if(FindChildWithTag(other.gameObject, "Node") == null){
                this.gameObject.transform.parent = other.gameObject.transform;
                this.rigidbody_component.isKinematic = true;
                this.gameObject.transform.position = other.gameObject.transform.position;
                this.gameObject.transform.rotation = other.gameObject.transform.rotation;
            }
        }
    }

    public virtual void runNode(){
        // 各継承先でオーバーライドしてね
        outputLog("You called NodeClass.runNode().");
    }

    public virtual void endNode(){
        // 各継承先でオーバーライドしてね
        outputLog("You called NodeClass.endNode().");
    }

/*****************************
    getter / setter その他
*****************************/

    // ノード種別の取得
    public string getNodeType(){
        return this.nodeType;
    }

    // ノード種別の設定
    // 設定出来たらtrue / 出来なかったらfalse を返す
    public bool SetNodeType(string _newNodeType){
        if(this.ExistNodeType(_newNodeType) == true){
            this.nodeType = _newNodeType;
            return true;
        }else{
            outputError(_newNodeType + " is not NodeType.");
            return false;
        }
    }

    // ノード種別が存在するか確認する関数
    // 存在するならtrue / 存在しないならfalse を返す
    public bool ExistNodeType(string _newNodeType){
        return this.nodeTypeList.Contains(_newNodeType);
    }

    // 必要なノードタイプの追加
    protected void addNeedNodeType(List<string> _newNodeType_list){
        foreach(string _newNodeType in _newNodeType_list){
            if(this.need_nodeList.Contains(_newNodeType) == false){
                this.need_nodeList.Add(_newNodeType);
            }
        }
    }

    public List<string> getNeedNodeTypeList(){
        return need_nodeList;
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
