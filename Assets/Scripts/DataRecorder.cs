using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRecorder : MonoBehaviour
{
    public Transform RedAgentTransform;
    public Transform GreenAgentTransform;
    public Transform BlueAgentTransform;
    public Transform MagentaAgentTransform;

    private float PrevRedAgentZ = 0f;
    private float PrevRedAgentT = 0f;
    private float PrevGreenAgentZ = 0f;
    private float PrevGreenAgentT = 0f;
    private float PrevBlueAgentZ = 0f;
    private float PrevBlueAgentT = 0f;
    private float PrevMagentaAgentZ = 0f;
    private float PrevMagentaAgentT = 0f;

    private List<float> RedAgentX = new List<float>();
		private List<float> RedAgentZ = new List<float>();
    private List<float> RedAgentV = new List<float>();
    private List<float> RedAgentW = new List<float>();

    private List<float> GreenAgentX = new List<float>();
		private List<float> GreenAgentZ = new List<float>();
    private List<float> GreenAgentV = new List<float>();
    private List<float> GreenAgentW = new List<float>();

    private List<float> BlueAgentX = new List<float>();
		private List<float> BlueAgentZ = new List<float>();
    private List<float> BlueAgentV = new List<float>();
    private List<float> BlueAgentW = new List<float>();

    private List<float> MagentaAgentX = new List<float>();
		private List<float> MagentaAgentZ = new List<float>();
    private List<float> MagentaAgentV = new List<float>();
    private List<float> MagentaAgentW = new List<float>();

    void Update()
    {
        // Get current values
        RedAgentX.Add(RedAgentTransform.position.x);
        RedAgentZ.Add(RedAgentTransform.position.z);
        RedAgentV.Add(RedAgentTransform.position.z - PrevRedAgentZ);
        RedAgentW.Add(RedAgentTransform.eulerAngles.y - PrevRedAgentT);

        GreenAgentX.Add(GreenAgentTransform.position.x);
        GreenAgentZ.Add(GreenAgentTransform.position.z);
        GreenAgentV.Add(GreenAgentTransform.position.z - PrevGreenAgentZ);
        GreenAgentW.Add(GreenAgentTransform.eulerAngles.y - PrevGreenAgentT);

        BlueAgentX.Add(BlueAgentTransform.position.x);
        BlueAgentZ.Add(BlueAgentTransform.position.z);
        BlueAgentV.Add(BlueAgentTransform.position.z - PrevBlueAgentZ);
        BlueAgentW.Add(BlueAgentTransform.eulerAngles.y - PrevBlueAgentT);

        MagentaAgentX.Add(MagentaAgentTransform.position.x);
        MagentaAgentZ.Add(MagentaAgentTransform.position.z);
        MagentaAgentV.Add(MagentaAgentTransform.position.z - PrevMagentaAgentZ);
        MagentaAgentW.Add(MagentaAgentTransform.eulerAngles.y - PrevMagentaAgentT);

        // Update previous values
        PrevRedAgentZ = RedAgentTransform.position.z;
        PrevRedAgentT = RedAgentTransform.eulerAngles.y;

        PrevGreenAgentZ = GreenAgentTransform.position.z;
        PrevGreenAgentT = GreenAgentTransform.eulerAngles.y;

        PrevBlueAgentZ = BlueAgentTransform.position.z;
        PrevBlueAgentT = BlueAgentTransform.eulerAngles.y;

        PrevMagentaAgentZ = MagentaAgentTransform.position.z;
        PrevMagentaAgentT = MagentaAgentTransform.eulerAngles.y;
    }

    public void GenerateLog()
    {
        string RedAgentXString = "Red Agent X: ";
        foreach(var item in RedAgentX)
        {
          RedAgentXString += item.ToString() + " ";
        }
        Debug.Log(RedAgentXString);

        string RedAgentZString = "Red Agent Z: ";
        foreach(var item in RedAgentZ)
        {
          RedAgentZString += item.ToString() + " ";
        }
        Debug.Log(RedAgentZString);

        string RedAgentVString = "Red Agent V: ";
        foreach(var item in RedAgentV)
        {
          RedAgentVString += item.ToString() + " ";
        }
        Debug.Log(RedAgentVString);

        string RedAgentWString = "Red Agent W: ";
        foreach(var item in RedAgentW)
        {
          RedAgentWString += item.ToString() + " ";
        }
        Debug.Log(RedAgentWString);



        string GreenAgentXString = "Green Agent X: ";
        foreach(var item in GreenAgentX)
        {
          GreenAgentXString += item.ToString() + " ";
        }
        Debug.Log(GreenAgentXString);

        string GreenAgentZString = "Green Agent Z: ";
        foreach(var item in GreenAgentZ)
        {
          GreenAgentZString += item.ToString() + " ";
        }
        Debug.Log(GreenAgentZString);

        string GreenAgentVString = "Green Agent V: ";
        foreach(var item in GreenAgentV)
        {
          GreenAgentVString += item.ToString() + " ";
        }
        Debug.Log(GreenAgentVString);

        string GreenAgentWString = "Green Agent W: ";
        foreach(var item in GreenAgentW)
        {
          GreenAgentWString += item.ToString() + " ";
        }
        Debug.Log(GreenAgentWString);



        string BlueAgentXString = "Blue Agent X: ";
        foreach(var item in BlueAgentX)
        {
          BlueAgentXString += item.ToString() + " ";
        }
        Debug.Log(BlueAgentXString);

        string BlueAgentZString = "Blue Agent Z: ";
        foreach(var item in BlueAgentZ)
        {
          BlueAgentZString += item.ToString() + " ";
        }
        Debug.Log(BlueAgentZString);

        string BlueAgentVString = "Blue Agent V: ";
        foreach(var item in BlueAgentV)
        {
          BlueAgentVString += item.ToString() + " ";
        }
        Debug.Log(BlueAgentVString);

        string BlueAgentWString = "Blue Agent W: ";
        foreach(var item in BlueAgentW)
        {
          BlueAgentWString += item.ToString() + " ";
        }
        Debug.Log(BlueAgentWString);



        string MagentaAgentXString = "Magenta Agent X: ";
        foreach(var item in MagentaAgentX)
        {
          MagentaAgentXString += item.ToString() + " ";
        }
        Debug.Log(MagentaAgentXString);

        string MagentaAgentZString = "Magenta Agent Z: ";
        foreach(var item in MagentaAgentZ)
        {
          MagentaAgentZString += item.ToString() + " ";
        }
        Debug.Log(MagentaAgentZString);

        string MagentaAgentVString = "Magenta Agent V: ";
        foreach(var item in MagentaAgentV)
        {
          MagentaAgentVString += item.ToString() + " ";
        }
        Debug.Log(MagentaAgentVString);

        string MagentaAgentWString = "Magenta Agent W: ";
        foreach(var item in MagentaAgentW)
        {
          MagentaAgentWString += item.ToString() + " ";
        }
        Debug.Log(MagentaAgentWString);
    }
}
