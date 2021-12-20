using System;
using UnityEngine;
using UI;
using BalsaCore;
using FSControl;
using System.IO;
using DirectLineRendering;
namespace AutotrimTweak
{
    public class AutotrimTweakFBW : IFlyByWire
    {
        PID32 pitchPID;
        float lastPitchPIDOutput;
        PID32 rollPID;
        float lastRollPIDOutput;
        float lastTime;

        public void OnRegistered(FBWHostBase host)
        {
            lastTime = Time.realtimeSinceStartup;
            //300 degrees per second is full swing
            float maxValue = 1f;
            float kP = 1f / 100f;
            float kI = kP / 2f;
            float kD = 0f;
            pitchPID = new PID32(kP, kI, kD, -maxValue, maxValue);
            rollPID = new PID32(kP, kI, kD, -maxValue, maxValue);
        }

        public void OnUnregistered(FBWHostBase host)
        {
        }

        public void OnProcessCtrlState(ref FSInputState data, Vehicle vehicle)
        {
            if (vehicle.Autotrim.enabled)
            {
                return;
            }
            //State
            float currentTime = Time.realtimeSinceStartup;
            float deltaTime = currentTime - lastTime;
            float pitchAxis = InputSettings.Axis_Pitch.GetAxis();
            float rollAxis = InputSettings.Axis_Roll.GetAxis();

            //Rotation, eulerAngles is in degrees
            Vector3 unrotated = vehicle.Rb.rotation.Inverse() * vehicle.Physics.AngularVelocity;
            //Get angles
            float pitch = unrotated.x;
            float roll = unrotated.z;
            float yaw = unrotated.y;

            //Normalise to degrees per second
            pitch = pitch / deltaTime;
            roll = roll / deltaTime;
            yaw = yaw / deltaTime;

            //Don't update the PID loops if there is control input
            if (Math.Abs(pitchAxis) < 0.05d && Math.Abs(rollAxis) < 0.05d)
            {
                //What the fuck is s? It seems to be some delta offset for D, so no fucking idea.
                lastPitchPIDOutput = Mathf.Clamp(pitchPID.Loop(0f, -pitch, deltaTime, 0), pitchPID.iMin, pitchPID.iMax);
                lastRollPIDOutput = Mathf.Clamp(rollPID.Loop(0f, -roll, deltaTime, 0), rollPID.iMin, rollPID.iMax);
            }

            //Apply changes
            data.pitch += lastPitchPIDOutput;
            data.roll += lastRollPIDOutput;
            lastTime = currentTime;
        }
    }
}
