using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NocabFantasyTile : IModelTile {
  public NocabFantasyTile(int q, int r) : base(q, r) { }

  public NocabFantasyTile(AxialCord ac) : base(ac) { }

  public NocabFantasyTile(NocabFantasyTile toBeCloned, AxialCord initialPos) : base(initialPos) { 
    this.height = toBeCloned.height;
    this.landType = toBeCloned.landType;
    
  }

  public NocabFantasyTile(int height, ELandType landType, AxialCord ac) : base(ac) {
    this.height = height;
    this.landType = landType;
  }

  public int height;
  public ELandType landType;

  public override Color tileColor() {
    if (this.colorOverride.Equals(Color.clear)) {
      // If colorOverride is empty 
      // TODO: make this work for clear tiles. Someone may want to override using a clear tile color but currently cant 
      return this.landType.getColor();
    }
    else { return this.colorOverride; }
    
  }


  // TODO: edges that are shared between model tiles

}
