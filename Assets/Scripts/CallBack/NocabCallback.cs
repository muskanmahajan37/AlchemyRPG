using System.Collections;
using System.Collections.Generic;
using LightJson;
using UnityEngine;
using System;


/*
public interface ICallback : JsonConvertable  {

    bool execute();

}


public class NocabCallback : ICallback {

    private int _waitingForACall;
    

    public NocabCallback()  {

    }


    public bool execute() {


        return true;
    }

    #region Json save/ Load
    public void loadJson(JsonObject jo) {
        throw new System.NotImplementedException();
    }

    public string myType() {
        throw new System.NotImplementedException();
    }

    public JsonObject toJson() {
        throw new System.NotImplementedException();
    }
    #endregion

}

public class tempReciever {

    private int myId;

    public tempReciever(int id) {
        myId = id;
    }

    public int callbackOne() {
        Debug.Log("Callback One");
        return myId;
    }

    public int callbackTwo(tempCallbaker cbr) {
        Debug.Log("Callback two");
        return 0;
    }

}


public class tempCallbaker {

    private Action<tempCallbaker> action;
    private int otherUUID;

    public tempCallbaker(int uuid, Action<tempCallbaker> cb) {
        this.otherUUID = uuid;
        this.action = cb;
    }


    public void runCallback() {
        this.action.Invoke(this);
    }

}

public class testMonobehavior : MonoBehaviour {

    private void Start() {
        tempReciever tr = new tempReciever(6);
        //tempCallbaker tcbr = new tempCallbaker(6, tr.callbackOne);
    }

}
*/
