using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeClass : MonoBehaviour{
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
