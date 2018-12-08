using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModelGenerator<IModelTile> {

	Dictionary<AxialCord, IModelTile> buildModel(int width, int height);

}
