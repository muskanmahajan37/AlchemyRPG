using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PipeFactory {

    /*
    public static Pipe<int> sumPipe(int delta, string flag, int lifetime = -1) {
        IExpire e =  lifetime < 0 ? 
                    (IExpire) new ExpireNever() : 
                    (IExpire) new ExpireCountCycle(lifetime);
        IFlagable f = new Flagable(flag);
        return new PipeSum(e, f, delta);
    }

    public static Pipe<int> sumPipe(int delta, IEnumerable<string> flags, int lifetime = -1) {
        IExpire e = lifetime < 0 ?
            (IExpire)new ExpireNever() :
            (IExpire)new ExpireCountCycle(lifetime);
        IFlagable f = new Flagable(flags);
        return new PipeSum(e, f, delta);
    }
    */

}
