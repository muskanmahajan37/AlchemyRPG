using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPipeLine<T> {

    void addPipe(Pipe<T> p);

    bool removePipe(Pipe<T> p);

    T pump(string flagKey, T startingValue);

}

public class PipeLine<T> : IPipeLine<T> {
    
    /**
     * TODO: self cleaning pipeline
     *    - requires pipes to have an experation function this cleaner can read
     *    
     *    
     *    Ok, what if I wanted a pipe that effects every stat (like an all stats up pipe)
     *    
     *    
     *    Or, consider a Boolean pipeline. What happens if you have two conflicting pipes?
     *      It'd be nice if the pipeline was smart enough to alert about previous pipes
     *      Like, the pipeline was smart enough to automatically determine the best order
     *    I suppose the above is a todo for usefull IPipeLine< T > classes.
     */

    private Dictionary<string, HashSet<Pipe<T>>> pipes;

    public PipeLine() {
        this.pipes = new Dictionary<string, HashSet<Pipe<T>>>();
    }

    private HashSet<Pipe<T>> getPipeSet(string flagKey) {
        // TODO: Error handeling

        if ( ! this.pipes.ContainsKey(flagKey)) {
            //throw new System.InvalidOperationException("Unrecgonized target stat (The key was missing): " + targetStat);
            return new HashSet<Pipe<T>>();
        }
        return pipes[flagKey];
    }

    #region PipeAddition
    private void addPipe(string flagKey, Pipe<T> p) {
        /**
         * WARNING: You should probably use the public addPipe() function
         * 
         * A private addPipe function. The provided flagKey is assumed to 
         * be a flag of the provided pipe.
         */

        if (flagKey == FlagConstants.NO_FLAG) {
            throw new System.InvalidOperationException("Provided pipe had a FlagConstants.NO_FLAG! This is an invalid " +
                "flag and cannot be added to a pipeline! Pipe<T>: " + p.GetType());
        }

        if ( ! this.pipes.ContainsKey(flagKey)) {
            this.pipes[flagKey] = new HashSet<Pipe<T>>();
        }

        p.addCallback(cleanUpCallback); // Tell the pipe how to clean itself up once it's expired
        this.pipes[flagKey].Add(p);
    }

    public void addPipe(Pipe<T> pipe) {
        ICollection<string> pipesFlags = pipe.getFlags();
        if (pipesFlags.Count == 0) {
            throw new System.InvalidOperationException("Provided pipe had no associated flags! Pipes should always be " +
                "provided flags before being added to a pipeline! Pipe<T>: " + pipe.GetType());
        }

        foreach(string f in pipe.getFlags()) {
            this.addPipe(f, pipe);
        }
    }

    private void cleanUpCallback(Pipe<T> expiredPipe) {
        // Callback with a void return type for IExpireable<Pipe<T>> 
        removePipe(expiredPipe);
    }
    #endregion


    #region PipeRemoval
    private bool removePipe(string flagKey, Pipe<T> p) {
        /**
         * WARNING: you should probably use the public removePipe(Pipe<T> p) function
         * 
         * A private, internal only pipe removal service. 
         * This function assumes that the provided pipe has the provided 
         * flagKey as a flag. 
         * 
         * TODO: Testing and error handeling. Provide more detailed errors
         */
        HashSet<Pipe<T>> pipeSet = this.pipes[flagKey];
        if ( ! pipeSet.Remove(p)) {
            // if pipe element was NOT found and therefore NOT removed
            throw new System.InvalidOperationException("Target stat did exist but could NOT find provided pipe. Target Stat name: " + flagKey);
        }

        // Cleanup
        if (this.pipes[flagKey].Count == 0) {
            // If, after removal, the pipe set is empty for this stat then remove it
            return this.pipes.Remove(flagKey);
        }
        return true;
    }

    public bool removePipe(Pipe<T> p) {
        // TODO: Possibly better error handeling/ messaging?
        foreach (string flag in p.getFlags()) {
            this.removePipe(flag, p);
        }
        return true;
    }

    public void removePipe(IEnumerable<Pipe<T>> pipesToRemove) {
        // TODO: Make this function return a boolean
        foreach(Pipe<T> p in pipesToRemove) {
            removePipe(p);
        }
    }

    #endregion


    private HashSet<Pipe<T>> intersectFlags(IEnumerable<string> flagKeys) {
        IEnumerator<string> enumerator = flagKeys.GetEnumerator();
        if (!enumerator.MoveNext())      // IEnumerator objects start 1 before the first element
            { new HashSet<Pipe<T>>(); }  // A false result => enumerable is empty

        string firstFlagKey = enumerator.Current;
        HashSet<Pipe<T>> initialSet = this.getPipeSet(firstFlagKey);
        HashSet<Pipe<T>> result = new HashSet<Pipe<T>>(initialSet); // Initialize result with first pipe set

        while (enumerator.MoveNext()) {
            string nextFlagKey = enumerator.Current;
            HashSet<Pipe<T>> nextSet = this.getPipeSet(nextFlagKey);
            result.IntersectWith(nextSet);  // Modify result inplace
            if (result.Count == 0) { return result; } // End early case
        }
        return result;
    }

    public T pumpIntersection(IEnumerable<string> flags, T startingValue) {
        HashSet<Pipe<T>> pipeSet = this.intersectFlags(flags);

        foreach(Pipe<T> p in pipeSet) {
            startingValue = p.pump(startingValue);
            p.cycle();
            // Pipe cleanup is done automatically via self-cleaning pipe action
        }
        return startingValue;
    }

    public T pump(string flagKey, T startingValue) {
        // Note, this pump function is a self cleaning action.
        // IE:  if the Expireable object attached to a pipe reaches 0, it will be removed 

        // TODO: Consider using linked list to do clean up and pump action in one loop instead of the current O(2n)

        if (flagKey == FlagConstants.NO_FLAG) {
            throw new System.Exception("Provided flagKey was FlagConstants.No_Flag: " + flagKey + 
                ". This is an invalid flagKey.");
        }

        HashSet<Pipe<T>> pipeSet = this.getPipeSet(flagKey);

        foreach (Pipe<T> p in pipeSet) {
            startingValue = p.pump(startingValue);
            p.cycle();
            // Pipe cleanup is done automatically via self-cleaning pipe action
        }
        return startingValue;
    }
}

