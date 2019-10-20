using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public interface ISelfCleaningExpire<T> {

    void setCallback(Action<T> callback);

    void runCallback();

}
