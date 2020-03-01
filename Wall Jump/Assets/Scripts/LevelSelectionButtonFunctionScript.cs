using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionButtonFunctionScript : MonoBehaviour
{
    public SceneTransitionerScript transitionerScript;

    public void WorldSelection(int i)
    {
        transitionerScript.LoadLevel("World"+i);
    }
}
