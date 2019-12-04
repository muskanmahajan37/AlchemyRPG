using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorConstants {

    // To convert from RGB to unity color do value/255

    #region Text Colors
    public static readonly Color TEXT_SYSTEM =             new Color(1.00f, 0.40f, 0.10f, 1f); // Orange
    public static readonly Color TEXT_BATTLE_HEADER =      new Color(1.0f, 0.31f, 0.31f, 1f); // Red
    public static readonly Color TEXT_DIALOGUE_HEADER =    new Color(0.4f, 0f, 1f, 1f); // Purple
    public static readonly Color TEXT_WORLD_EVENT_HEADER = new Color(0f, 0.4f, 0f, 1f); // Dark Green

    public static readonly Color TEXT_DEFAULT_HEADER = new Color(0.15f, 0.15f, 0.48f, 1f); // Dark Navy
    public static readonly Color TEXT_DEFAULT_BODY = NocabColorUtility.grey(0.2f); // 80% dark grey
    #endregion

    #region Basic Colors
    public static readonly Color GREY = Color.grey;
    public static readonly Color WHITE = Color.white;
    #endregion
}