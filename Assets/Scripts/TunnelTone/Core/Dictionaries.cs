using System.Collections.Generic;
using TunnelTone.Singleton;
using TunnelTone.UI.Reference;
using UnityEngine;

namespace TunnelTone.GameSystem
{
    public class Dictionaries : Singleton<Dictionaries>
    {
        internal readonly Dictionary<int, string> levelDictionary = new()
        {
            {0, "?"},
            {1, "1"},
            {2, "2"},
            {3, "3"},
            {4, "4"},
            {5, "5"},
            {6, "6"},
            {7, "7"},
            {8, "7+"},
            {9, "8"},
            {10, "8+"},
            {11, "9"},
            {12, "9+"},
            {13, "10"},
            {14, "10+"}
        };
        
        internal readonly Dictionary<int, string> difficultyDictionary = new()
        {
            {0, "Easy"},
            {1, "Hard"},
            {2, "Intense"},
            {3, "Insane"}
        };
    }
}