using System;
using System.Collections;
using System.Collections.Generic;
using LightJson;
using UnityEngine;


// TODO: Pipes should NOT be INocabNamable objects. I can't see any reason for an other
// object to need a refrence to a Pipe instance. If anything, Pipes themselves need a 
// refrence to other things (like mobs or the weather or something), so the other thing
// should have/be a INocabNameable so the pipe can find it later.
// The only reason why Pipe is INocabNamable is because I wanted to practice
// inplimenting the interface along with the JsonConvertable 
public interface Pipe<T> : IExpire<Pipe<T>>, IFlagable, JsonConvertable, INocabNameable {

    T pump(T incomingValue);

    // TODO: Activation conditions
    // TODO: Pipe self clean up
}
