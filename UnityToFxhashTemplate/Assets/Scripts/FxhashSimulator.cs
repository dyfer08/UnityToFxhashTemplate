using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Linq;


public class FxhashSimulator : MonoBehaviour{

    public static FxhashSimulator instance;
    string alphabet = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
    public static string fxhash = "oo";
    static uint[] hashes;

    void Awake(){

        instance = this;

        for(int i=0; i<49; i++){
            fxhash += alphabet[(int)(UnityEngine.Random.value * alphabet.Length)|0];
        }
        string fxhashTrunc = fxhash.Remove(0,2);
        Regex regex = new Regex(".{" + ((fxhash.Length/4)|0) + "}", RegexOptions.None);
        MatchCollection matches = regex.Matches(fxhashTrunc);
        string[] segments = new string[matches.Count];
        for(int i=0; i<matches.Count; i++){
            segments[i] = matches[i].Value;
        }
        hashes = segments.Select(s => b58dec(s)).ToArray();
    }

    public static float fxrand(){
        return sfc32(hashes[0], hashes[1], hashes[2], hashes[3]);
    }

    uint b58dec(string segment){
        int hash = (int)segment.Aggregate(0, (p, c) =>p * alphabet.Length + System.Array.IndexOf(alphabet.ToCharArray(),c)|0);
        return (uint)hash;
    }

    static float sfc32(uint a, uint b, uint c, uint d){
        a |= 0;
        b |= 0;
        c |= 0;
        d |= 0;
        uint t = (a + b | 0) + d | 0;
        hashes[3] = d = d + 1 | 0;
        hashes[0] = a = b ^ b >> 9;
        hashes[1] = b = c + (c << 3) | 0;
        c = c << 21 | c >> 11;
        hashes[2] = c = c + t | 0;
        return (t >> 0)/ 4294967296f;
    }



}
