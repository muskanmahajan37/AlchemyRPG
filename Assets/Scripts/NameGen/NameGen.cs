using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGen : MonoBehaviour
{

    public void Start() {
        // string newName = simpleScrambleAlg("ender".ToCharArray());
        for (int i = 0; i < 300; i++) {
            string newName = SylableNameGen.genSylableName();
            lc.addEvent(new LogEntry("Name Generator: ", newName));

        }
    }

    public LogController lc;

    public void genNameTest() {
        string newName = simpleScrambleAlg("testabcde".ToCharArray());
        lc.addEvent(new LogEntry("Name Generator: ", newName));
    }
    
    private static string simpleScrambleAlg(char[] oldName) {
        // NOTE: Will always return a new string, and never modify oldName arg
        // 1) pick rand char to convert keeping v/c type
        // 2) pick rand v to convert to v
        // 3) pick rand c to convert to c
        
        char[] safeOldName = (char[])oldName.Clone();

        int randIndex = UnityEngine.Random.Range(0, safeOldName.Length);
        char randChar = safeOldName[randIndex];
        char replacement;
        if (isVowel(randChar)) { replacement = convertChar(randChar, getRandVowel); }
        else { replacement = convertChar(randChar, getRandConsonat); }
        safeOldName[randIndex] = replacement;

        // 2)
        // Find all the vowels in the oldName
        List<int> vowelIndex = new List<int>();
        List<int> consonantIndex = new List<int>();
        for (int i = 0; i < safeOldName.Length; i++) {
            if (isVowel(safeOldName[i])) { vowelIndex.Add(i); }
            else { consonantIndex.Add(i); }
        }

        // Pull a random index from the vowel list
        int randomVowelIndex = vowelIndex[UnityEngine.Random.Range(0, vowelIndex.Count)];
        char randomVowel = safeOldName[randomVowelIndex];
        replacement = convertChar(randomVowel, getRandVowel);
        safeOldName[randomVowelIndex] = replacement;

        // 3)
        int randomConsonantIndex = consonantIndex[UnityEngine.Random.Range(0, consonantIndex.Count)];
        char randomCons = safeOldName[randomConsonantIndex];
        replacement = convertChar(randomCons, getRandConsonat);
        safeOldName[randomConsonantIndex] = replacement;

        return new string(safeOldName);

    }

    public static char convertChar(char oldChar, Func<char> charBuilder) {
        /**
         * Provided an old character and a function that randomly generates characters,
         *   this method will generate a random new character (guaranteed different from oldChar)
         *   from the outputs of charBuilder arg.
         * 
         * If oldChar is empty string, a random new char will be returned from the charBuilder's pool
         */
        char newChar = charBuilder();
        while(oldChar == newChar) {
            newChar = charBuilder();
        }
        return newChar;
    }




    public static bool isVowel(char c) { return vowels.Contains(c); }

    public static bool isConsonant(char c) { return !isVowel(c); }

    public static char getRandVowel() {
        int randIndex = UnityEngine.Random.Range(0, NameGen.vowels.Count);
        return NameGen.vowels[randIndex];
    }

    public static char getRandConsonat() {
        int randIndex = UnityEngine.Random.Range(0, NameGen.consonants.Count);
        return NameGen.consonants[randIndex];
    }

    public static char getRandChar() {
        List<char> allLetters = new List<char>(vowels);
        allLetters.AddRange(consonants);
        int randIndex = UnityEngine.Random.Range(0, allLetters.Count);
        return allLetters[randIndex];
    }


    public static readonly List<char> vowels = new List<char>() {
        'a', 'e', 'i', 'o', 'u'
    };

    public static readonly List<char> consonants = new List<char>() {
        'b', 'c', 'd', 'f', 'g',
        'h', 'j', 'k', 'l', 'm',
        'n', 'p', 'q', 'r', 's',
        't', 'v', 'w', 'x', 'y',
        'z'
    };
}

