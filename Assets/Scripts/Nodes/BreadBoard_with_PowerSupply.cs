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

    // 導通チェック起点関数
    /* 返り値
    |   導通 : true
    | 非導通 : false
    */
    public bool runContinuityTest(){
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

            // 子のオブジェクトを取得
            GameObject ChildGameObject = SlotGameObject.transform.GetChild(0).gameObject;
            // 子のオブジェクトが空
            if(ChildGameObject == null){
                // gameObjectは子オブジェクトを持たなければならない
                outputWarning("SlotGameObjectArray["+num+"].gameObject should has child GameObject.");
                continue;
            }

            // 孫のオブジェクトを子のオブジェクトから取得
            // NodeObjectがなければ弾く
            GameObject NodeGameObject = ChildGameObject.transform.GetChild(0).gameObject;
            if(NodeGameObject == null){
                // NodeObjectがいなければならない
                outputWarning("GameObject in SlotGameObjectArray["+num+"].gameObject should has Node GameObject.");
                return false;
            }

            // NodeClassが無いなら弾く
            if(NodeGameObject.TryGetComponent(out NodeClass nc) == false){
                return false;
            }

            NodeGameObjectArray[num] = NodeGameObject;
        }

        // 最終チェック
        // NodeGameObjectArray に null Object が存在しないことを確認して，その結果を返す
        if(Array.IndexOf(NodeGameObjectArray, null) == -1){
            return true;
        }else{
            return false;
        }

        // 一応残しておくが，多分消す
        return false;
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
