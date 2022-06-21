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

    // 接続している前のノードのオブジェクトを格納するリスト
    private List<GameObject> prevNodeObject = new List<GameObject>{};

    // 接続されている次のノードのオブジェクトを格納するリスト
    private List<GameObject> nextNodeList = new List<GameObject>{};

    // ノードのNodeClassコンポーネントを一時格納するための変数
    private NodeClass tempNodeClass = null;

    // 導通(Continuity,コンティニュイティ)テスト
    // 通電して大丈夫かを確認する関数ではない
    // 繋がっていれば，後ろに続くノードの配列を返す
    // 若干再帰的な処理になる…？
    /* 引数
    | _route_Num : 配列nextNode用の引数
    |         : その名の通り，次のノードの番号を返す
    */
    /* 返り値
    | Object[] : オブジェクト型配列
    |          : 接続されているなら，後ろに続くノードの配列を返す
    |          : 接続されていないなら，空の配列が返される
    */
    public GameObject[] ContinuityTest(int _route_Num){
        GameObject[] nextNodes = {};

        // 次のノードが接続されていなければ処理を終わる
        if(nextNodeList.Count == 0){
            return nextNodes;
        }

        // TODO: ここのブロックは要調整
        // reason: 配線の並列対応のため
        tempNodeClass = null;
        if(nextNodeList[_route_Num].TryGetComponent(out this.tempNodeClass)){
            this.tempNodeClass.ContinuityTest(0);
        }

        return nextNodes;
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
            return false;
        }
    }

    // ノード種別が存在するか確認する関数
    // 存在するならtrue / 存在しないならfalse を返す
    public bool ExistNodeType(string _newNodeType){
        return this.nodeTypeList.Contains(_newNodeType);
    }


    // nextNodeListを返す関数
    public List<GameObject> GetNextNodeList(){
        return nextNodeList;
    }

    // nextNodeListの内，引数_numに格納されたGameObjectを返す
    // 引数_numがnextNodeListの範囲外(=>存在しない)の場合，nullを返す
    public GameObject GetNextNode(int _num){
        if(nextNodeList.Count > _num){
            return nextNodeList[_num];
        }else{
            return null;
        }
    }

    // nextNodeListにノードを追加する関数
    // 当該GameObjectにNodeClassコンポーネントが載ってなければ弾く
    // 返り値は追加できたかの確認用
    //   1  : 追加できた
    //   0  : 追加できなかった
    //  -1  : そもそもNodeClassコンポーネントがアタッチされてなかった
    public int AddNode2nextNodeList(GameObject _node_Object){
        // nodeObjectにNodeClassコンポーネントがアタッチされているかチェック
        tempNodeClass = null;
        if(_node_Object.TryGetComponent(out this.tempNodeClass) == false){
            return -1;
        }

        if(this.CheckExistNodeInNextNodeList(_node_Object) == false){
            // nextNodeListに既に存在していなければ，追加する
            this.nextNodeList.Add(_node_Object);
            return 1;
        }else{
            // 存在しているならば，追加しない
            return 0;
        }
    }

    // nextNodeList内に引数node_ObjectのGameObjectが入ってるか確認する
    // 入ってる   : true
    // 入ってない : false
    public bool CheckExistNodeInNextNodeList(GameObject _node_Object){
        return this.nextNodeList.Contains(_node_Object);
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
