using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeClass : MonoBehaviour{
    // このスクリプトがアタッチされたオブジェクトに OVRGrabbable コンポーネントが存在することを要求する
    // OVRGrabbable 自体が rigidbody と ~Collider を要求するから，この2つは改めて要求しないが，今後することになるかもしれない
    [RequireComponent(typeof(OVRGrabbable))]

    /* ノードタイプ
    | node = "Node"
    | 電源 = "PowerSupply"
    | 抵抗 = "Resistance"
    | LED  = "LED"
    | PIC  = "PIC"
    */
    // タグで管理する予定だが，一応仮宣言
    private string nodeType = "Node";

    // ノードタイプのリスト
    private List<string> nodeTypeList = new List<string>{"Node", "PowerSupply", "Resistance", "LED", "PIC"};

    // Slot Objectにいるか
    private bool isSloted = false;

    // TODO: tag:Slotにくっついたときにその子オブジェクトになる処理を書く
    private void OnCollisionEnter(Collision other) {
        // ここ
        // Colliderからgameobjectを取り出す
        GameObject Slot_Object = other.gameObject;

        GameObject Child_Object = Slot_Object.transform.GetChild(0).gameObject;
        if(Child_Object == null){
            break;
        }

        // OVRGrabbableの呼び出しで Grab を解除する
        OVRGrabbable object_Grabbable_component;
        if(this.gameObject.TryGetComponent(out object_Grabbable_component) == false){
            break;
        }
        OVRGrabber object_grabbedBy_component = object_Grabbable_component.grabbedBy();
        object_grabbedBy_component.ForceRelease; // ここで解除


        // 子オブジェクトへ
        this.gameObject.parent = Child_Object;

        // 座標角度をあわせる
        this.gameObject.transform.position = Child_Object.transform.position;
        this.gameObject.transform.rotation = Child_Object.transform.rotation;

        // Slotに配置されているフラグを立てとく
        this.isSloted = true;
    }
    // TOOD: 離れたときの処理も書く

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
            return false;
        }
    }

    // ノード種別が存在するか確認する関数
    // 存在するならtrue / 存在しないならfalse を返す
    public bool ExistNodeType(string _newNodeType){
        return this.nodeTypeList.Contains(_newNodeType);
    }

    // --------------------------------------
    // ログ生成関連

    // ログを生成
    protected void outputLog(string msg){
        Debug.Log(this.name + ":" + msg);
    }

    // エラーログを生成
    protected void outputError(string msg){
        Debug.LogError(this.name + ":" + msg);
    }

    // ワーニングログを生成
    protected void outputWarning(string msg){
        Debug.LogWarning(this.name + ":" + msg);
    }
}
