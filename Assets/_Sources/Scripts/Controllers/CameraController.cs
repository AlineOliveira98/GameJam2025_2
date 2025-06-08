using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public CameraData[] camerasData;
    private Dictionary<CameraType, CinemachineCamera> camerasDisc = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        for (int i = 0; i < camerasData.Length; i++)
        {
            camerasDisc.Add(camerasData[i].cameraType, camerasData[i].camera);
        }

        SetCamera(CameraType.Menu);
    }

    public void SetCamera(CameraType type)
    {
        camerasDisc[type].Priority = 100;

        foreach (var cam in camerasDisc)
        {
            if (cam.Key != type)
                cam.Value.Priority = 0;
            else
                cam.Value.Priority = 100;
        }
    }
}

[Serializable]
public class CameraData
{
    public CameraType cameraType;
    public CinemachineCamera camera; 
}

public enum CameraType
{
    Menu,
    Gameplay,
    EndGame
}