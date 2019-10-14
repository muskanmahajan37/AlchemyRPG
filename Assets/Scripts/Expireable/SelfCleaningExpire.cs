using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public interface ISelfCleaningExpire<T> {

    void setCallback(Action<T> callback);

    void runCallback();

}


// TODO: What is this class? Is it used anywhere? 
/*
public class tempClass : ISelfCleaningExpire<tempClass> {


    private Action<tempClass> callback;

    public tempClass() {
        this.callback = defaultCallback;
    }

    private static void defaultCallback(tempClass c) { } // Empty default callback function

    public void setCallback(Action<tempClass> callback) {
        this.callback = callback;
    }

    public void runCallback() {
        this.callback(this);
    }

}
*/
