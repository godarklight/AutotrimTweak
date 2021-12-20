using System;
using BalsaCore;
using UnityEngine;
//using FSControl;

namespace AutotrimTweak
{
    [BalsaAddon]
    public class Loader
    {
        //Game start
        [BalsaAddonInit(invokeTime = AddonInvokeTime.Flight)]
        public static void BalsaInit()
        {
            GameObject go = new GameObject();
            go.AddComponent<AutotrimMain>();
        }

        //Game exit
        [BalsaAddonFinalize]
        public static void BalsaFinalize()
        {
        }
    }
}
