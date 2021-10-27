using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : Function {

    private JackOfController joc;

    public void Init( JackOfController _joc ) {
        this.joc = _joc;
    }

    public override void ExecuteFunction() {
        joc.velocity.y += joc.gravity * Time.deltaTime;
    }

}
