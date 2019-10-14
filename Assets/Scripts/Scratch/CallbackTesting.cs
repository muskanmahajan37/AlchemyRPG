using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackTesting : MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        tempTestFunc();
    }
    


    public void tempTestFunc() {
        //INocabNameable obj = new CallbackTestingPrototype();
        //AbstractCallBackExpire<int>.lazyLookupAndRunCallback("CallbackTestingPrototypeNocabName", "badMethodName", 222);

        IExpire<int> testExpire = new ExpireCountCycle<int>(1);
        testExpire.addCallback(CallbackTestingPrototype.HitMe);
    }

}


public class CallbackTestingPrototype : ANocabNameable {

    public CallbackTestingPrototype() : base("CallbackTestingPrototypeNocabName") { }

    public static void HitMe(int num) {
        Debug.Log("I've been hit! Arg: " + num);
    }

}
