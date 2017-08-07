using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

public class pixelTCP : MonoBehaviour {

	public Camera Cam;

	private int width;
	private int height;
	private string HOST = "localhost";
	private Int32 PORT = 8000;
	private Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private Texture2D tex;
	private RenderTexture ren;



	void SendNewImage()
	{
		RenderTexture currentRT = RenderTexture.active;
		RenderTexture.active = Cam.targetTexture;
		Cam.Render();
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		RenderTexture.active = currentRT;
		byte[] bytes = tex.GetRawTextureData();
		SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
		socketAsyncData.SetBuffer(bytes,0,bytes.Length);
		s.SendAsync (socketAsyncData);
	}

	void Awake()
	{
		Screen.SetResolution(256, 256, false);
		width = Screen.width;
		height = Screen.height;
		Debug.Log ("Screen width: " + width);
		Debug.Log("Screen height: " + height);
		Debug.Log ("Buffer size: " + height * width * 3);
		tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		ren = new RenderTexture(width, height, 16, RenderTextureFormat.ARGB32);
		Cam.targetTexture = ren;
	}
		
	// Use this for initialization
	void Start () {
		s.Connect (HOST, PORT);
		s.Send (Encoding.UTF8.GetBytes ("Connected."));
		Debug.Log ("Connected");
	}

	void Update() {
		SendNewImage ();
		//Byte[] bytesReceived = new Byte[256];
		//int bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
		//string data = Encoding.UTF8.GetString (bytesReceived, 0, bytesReceived.Length);
		//Debug.Log (data);
	}
}
