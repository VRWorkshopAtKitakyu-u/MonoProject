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
    protected GameObject[] NodeGameObjecetArray = {};

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
        NodeGameObjecetArray = new GameObject[SlotGameObjectArray.Length];
        //  作るときに，数/NodeClass不足なら弾く なんならreturn falseする
        for(int num = 0; num < SlotGameObjectArray.Length; num++){
            // 孫(子の子)オブジェクトを取得
            GameObject NodeGameObject = SlotGameObjectArray[num].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;

            // 数不足なら弾く
            if(NodeGameObject == null){
                return false;
            }
            // NodeClassが無いなら弾く
            if(NodeGameObject.TryGetComponent(out NodeClass nc) == false){
                return false;
            }

            NodeGameObjecetArray[num] = NodeGameObject;
        }

        return true;
    }


    // getter/setter

    // とりあえずそのまま返すだけ
    public GameObject[] getBaseGameObjectArray(){
        return SlotGameObjectArray;
    }

    // チェッカー
    public bool checkExistInBaseGameObjectArray(GameObject add_Object){
        // そもそも空オブジェクトだったらreject
        if(add_Object == null){
            return false;
        }

        if(Array.IndexOf(SlotGameObjectArray, add_Object) != -1){
            return true;
        }else{
            return false;
        }
    }

//
// ---------------------------------------------
// ログ生成関連
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
