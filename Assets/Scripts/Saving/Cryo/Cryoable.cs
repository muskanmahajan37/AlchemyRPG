using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CryoLink  {

    string cryoUUID();

    void recieveMessage(string incomingMessage);

}
