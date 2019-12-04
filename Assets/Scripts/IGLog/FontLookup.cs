using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class FontLookup : MonoBehaviour {

    public static FontLookup singleton; // Still needs the monobehavior obj in the scene somehwere to instantiate the singleton

    // TODO: this is currently error prone. Make the UI accept a dictionary or something
    public List<TMP_FontAsset> defaultFonts;
    public List<EFontName> defaultFontNames;
    
    public Dictionary<EFontName, TMP_FontAsset> alreadyLoadedFonts;

    // Start is called before the first frame update
    void Start() {
        if (singleton != null) { return; }
        singleton = this;
        alreadyLoadedFonts = new Dictionary<EFontName, TMP_FontAsset>();

        for(int i = 0; i < defaultFonts.Count; i++) {
            alreadyLoadedFonts[defaultFontNames[i]] = defaultFonts[i];
        }
    }
    
    public TMP_FontAsset getFont(EFontName fontName) {
        if ( ! alreadyLoadedFonts.ContainsKey(fontName)) { loadFont(fontName); }
        return alreadyLoadedFonts[fontName];
    }

    private void loadFont(EFontName fontName) {
        switch(fontName) {
            case EFontName.Manaspc:

                break;
            case EFontName.Default:
            default:
                // Default font should always be pre-loaded
                break;
        }
    }

}

/**
 * These Enumerations represent different fonts.
 * They are mapped to a specific font resource via the 
 * FontLookup class in the loadFont(...) function.
 * 
 * To add a new font:
 * 1) Create a unique enumeration name to represent the font.
 * 2) Create the Font in the Unity main window via
 *   2.1) Window
 *   2.2) TextMeshPro
 *   2.3) Font Asset Creator
 * 3.a) Default fonts are added into this dictionary as a prefab via the public list.
 * 3.b) Specilty/ Lazy Loaded fonts must be placed into the "Resources/Fonts/" folder
 * 4) Add a new line in the switch statement in the FontLookup.loadFont(...) function
 *    to point towards the new file location of the font.
 */
public enum EFontName {

    Manaspc,
    Default,

}

