using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IModelTile {


  private Color ColorOverridePrivate;
  public Color colorOverride { 
		get { return ColorOverridePrivate; }
		set { this.ColorOverridePrivate = value; }
	}

	private AxialCord AC;
	public AxialCord axialCord { 
		get { return AC; }
		set { this.AC = new AxialCord(value.q, value.r); }
	} // TODO: should this be read only?

	public int q { 
		get { return AC.q; }
		set { this.AC = new AxialCord(value, AC.r); }
	} // TODO: should this be read only?

	public int r { 
		get { return AC.r; }
		set { this.AC = new AxialCord(AC.q, value); } 
	} // TODO: should this be read only?
		

	public IModelTile(int q, int r) { axialCord = new AxialCord(q,r); }

	public IModelTile(AxialCord ac) { axialCord = ac; }

  public IModelTile(IModelTile copy) { this.AC = copy.AC; }

  public abstract Color tileColor();

}
