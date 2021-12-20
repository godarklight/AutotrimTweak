using System;
using UnityEngine;
using BalsaCore;



namespace AutotrimTweak
{
    public class AutotrimMain : MonoBehaviour
    {
        bool shouldEnable = false;

        public void Update()
        {
            if (GameLogic.CurrentScene != GameScenes.FLIGHT || GameLogic.LocalPlayerVehicle == null || GameLogic.LocalPlayerVehicle.Autotrim == null)
            {
                return;
            }
            double pitchAxis = Math.Abs(InputSettings.Axis_Pitch.GetAxis());
            double rollAxis = Math.Abs(InputSettings.Axis_Roll.GetAxis());
            double yawAxis = Math.Abs(InputSettings.Axis_Yaw.GetAxis());
            bool playerInput = pitchAxis > 0.05f || rollAxis > 0.05f || yawAxis > 0.05f;
            bool atEnabled = GameLogic.LocalPlayerVehicle.Autotrim.autoTrimEnabled;
            if (atEnabled && playerInput)
            {
                shouldEnable = true;
                GameLogic.LocalPlayerVehicle.Autotrim.DisableAT();
            }
            if (shouldEnable && !playerInput)
            {
                shouldEnable = false;
                GameLogic.LocalPlayerVehicle.Autotrim.EnableAT();
            }
        }
    }
}
