using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResiNode : NodeClass{
    protected override void Start(){
        base.Start();
        SetNodeType("Resistor");
        addNeedNodeType(new List<string>{});
    }

    protected override void Update(){
        base.Update();
    }

    public override void runNode(){
        return;
    }

    public override void endNode(){
        return;
    }
}
