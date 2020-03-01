using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHub
{
    //Multiton structure granting access to major game system via connected hub

    private static LevelController levelController;

    public static LevelController LevelController => ObjRefUtility.FindReferenceIfNull(ref levelController);

    private static AmmoManager ammoManager;

    public static AmmoManager AmmoManager => ObjRefUtility.FindReferenceIfNull(ref ammoManager);

}
