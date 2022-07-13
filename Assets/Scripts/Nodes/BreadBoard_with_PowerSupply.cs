using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField]
    protected List<string> NodeTypeInSlotList = new List<string>{};

    private bool tempFlag= false;

    protected void Update(){
        if(OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) == true && tempFlag == false){
            // 一回動作させてみよう
            if(runContinuityTest()){
                runAllNode();
            }
            tempFlag = true;
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
                outputWarning("SlotGameObjectArray["+num+"].gameObject has not tag:Slot.¥nGameObject in SlotGameObjectArray should has tag:Slot.");
                continue;
            }

            // tag: Nodeを持つ子のオブジェクトを取得
            GameObject NodeGameObject = FindChildWithTag(SlotGameObject, "Node");
            // 子のオブジェクトが空
            if(NodeGameObject == null){
                // gameObjectは子オブジェクトを持たなければならない
                outputWarning("SlotGameObjectArray["+num+"].gameObject should has child GameObject.");
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
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
        foreach (GameObject nodeGameObject in this.NodeGameObjectArray){
            if(nodeGameObject.TryGetComponent(out NodeClass nc) == true){
                nc.runNode();
            }
        }
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
