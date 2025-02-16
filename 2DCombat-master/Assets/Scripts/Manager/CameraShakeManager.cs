using Cinemachine;
using CustomUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : SingleTon<CameraShakeManager>
{
    [SerializeField] private float globalShakeForce = 1.0f;
    [SerializeField] private CinemachineImpulseListener _impulseListener;

    private CinemachineImpulseDefinition _impulseDefinition;

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {  
        impulseSource?.GenerateImpulseWithForce(globalShakeForce);      
    }

    public void CameraShakeFromProfile(CinemachineImpulseSource impulseSource, ScreenShakeSO profile)
    {
        SetupScreenShake(impulseSource, profile);

        impulseSource?.GenerateImpulseWithForce(profile.impactForce);
    }

    private void SetupScreenShake(CinemachineImpulseSource impulseSource, ScreenShakeSO profile)
    {
        _impulseDefinition = impulseSource.m_ImpulseDefinition;

        _impulseDefinition.m_ImpulseDuration = profile.impactForce;
        _impulseDefinition.m_CustomImpulseShape = profile.impulseCurve;

        impulseSource.m_DefaultVelocity = profile.defaultVelocity;

        _impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
        _impulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrequency;
        _impulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration;

    }
}
