using System;
using UnityEngine;
using UI;
using BalsaCore;
using FSControl;

namespace AutotrimTweak
{
    public class AutotrimTweakFBW : IFlyByWire
    {
        PID32 pitchPID;
        float lastPitchInput;
        float lastPitchPIDOutput;
        PID32 rollPID;
        float lastRollInput;
        float lastRollPIDOutput;
        float lastTime;

        public void OnRegistered(FBWHostBase host)
        {
            lastTime = Time.realtimeSinceStartup;
            pitchPID = new PID32(0.1f, 0.01f, 0f, -0.2f, 0.2f);
            rollPID = new PID32(0.1f, 0.01f, 0f, -0.2f, 0.2f);
        }

        public void OnUnregistered(FBWHostBase host)
        {
        }

        public void OnProcessCtrlState(ref FSInputState data, Vehicle vehicle)
        {
            float currentTime = Time.realtimeSinceStartup;
            float deltaTime = currentTime - lastTime;
            float pitchAxis = InputSettings.Axis_Pitch.GetAxis();
            float rollAxis = InputSettings.Axis_Roll.GetAxis();
            float currentPitch = GetVehiclePitch(vehicle);
            float currentRoll = GetVehicleRoll(vehicle);
            float pitchError = currentPitch - lastPitchInput;
            float rollError = currentRoll - lastRollInput;
            if (Math.Abs(pitchAxis) < 0.05d && Math.Abs(rollAxis) < 0.05d)
            {
                lastPitchPIDOutput = Mathf.Clamp(pitchPID.Loop(0, pitchError, deltaTime, 0), -0.2f, 0.2f);
                lastRollPIDOutput = Mathf.Clamp(rollPID.Loop(0, rollError, deltaTime, 0), -0.2f, 0.2f);
            }
            Debug.Log($"{currentPitch} {pitchError} {lastPitchPIDOutput}");
            //Apply changes
            data.pitch -= lastPitchPIDOutput;
            data.roll += lastRollPIDOutput;
            //Save state data
            lastPitchInput = currentPitch;
            lastRollInput = currentRoll;
            lastTime = currentTime;
        }

        private float GetVehiclePitch(Vehicle vehicle)
        {
            return FSControlUtil.GetVehiclePitch(vehicle) * Mathf.Rad2Deg;
        }
        private float GetVehicleYaw(Vehicle vehicle)
        {
            return FSControlUtil.GetVehicleYaw(vehicle) * Mathf.Rad2Deg;
        }
        private float GetVehicleRoll(Vehicle vehicle)
        {
            return FSControlUtil.GetVehicleRoll(vehicle) * Mathf.Rad2Deg;
        }
    }
}
