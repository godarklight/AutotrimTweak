using System;
using BalsaCore;
using UnityEngine;
using FSControl;

namespace AutotrimTweak
{
    [BalsaAddon]
    public class Loader
    {
        //Game start
        [BalsaAddonInit]
        public static void BalsaInit()
        {
            GameEvents.Vehicles.OnVehicleSpawned.AddListener(OnVehicleSpawned);
        }

        //Game exit
        [BalsaAddonFinalize]
        public static void BalsaFinalize()
        {
        }

        private static void OnVehicleSpawned(Vehicle v)
        {
            if (!v.IsLocalPlayerVehicle)
            {
                return;
            }
            FBWHostBase host = v.Autotrim.host;
            IFlyByWire autotrimTweak = new AutotrimTweakFBW();
            host.RegisterFBWModule(autotrimTweak);
        }
    }
}
