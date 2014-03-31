using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyTest : MonoBehaviour {
	
	private List<LoadAssetFromBundle> assetsToLoad = new List<LoadAssetFromBundle>();
	// Use this for initialization
	
	//Keep track if the current downloadable bundle is downloaded(Plain copy from LoadBundlesScene.cs)
	private bool isDownloaded = true;
	
	private string baseURL;//(Plain copy from LoadBundlesScene.cs)
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (assetsToLoad.Count > 0)
		{			
			for(int i = (assetsToLoad.Count -1); i >= 0; i--)
			{
				LoadAssetFromBundle asset = assetsToLoad[i];
				if(asset.IsDownloadDone)
				{

//					string fileName = Application.persistentDataPath + "/" + FILE_NAME;
//					fileWriter = File.CreateText(fileName);
//					fileWriter.WriteLine("Hello world");
//					fileWriter.Close();

					//The download is done, instantiate the asset from the bundle
					asset.InstantiateAsset();
					//Remove the asset from the loading list
					assetsToLoad.RemoveAt(i);
					//Destroy the LoadAssetFromBundle Script
					Destroy(asset);
					//This means an asset is downloaded, which means you can start on the next one
					isDownloaded = true;
				}
			}
			
			if (isDownloaded) //The download is complete
			{
				//Start the next download
				foreach (LoadAssetFromBundle asset in assetsToLoad)
				{
					if (!asset.HasDownloadStarted)
					{
						//Start the download
						asset.DownloadAsset();
						
						//set the isDownloaded to false again
						isDownloaded = false;
						
						//break the loop
						break;
					}
				}
			}
		}
		else //If there is nothing left to load, then destroy this game object
		{
			//Destroy(this.gameObject);
		}
	}

	void OnGUI()
	{
		GUILayout.Label("Caching.enabled" + Caching.enabled.ToString());
		GUILayout.Label("Caching.ready" + Caching.ready.ToString());
		GUILayout.Label("Caching.expirationDelay" + Caching.expirationDelay.ToString());
		GUILayout.Label("Caching.maximumAvailableDiskSpace" + Caching.maximumAvailableDiskSpace.ToString());
		GUILayout.Label("Caching.spaceFree" + Caching.spaceFree.ToString());
		GUILayout.Label("Caching.spaceOccupied" + Caching.spaceOccupied.ToString());

		if (GUILayout.Button("Load url"))
		{
			//StartCoroutine (DownloadAndCache());

			LoadAssetFromBundle cryWolfLogo = this.gameObject.AddComponent<LoadAssetFromBundle>();
			cryWolfLogo.QueueBundleDownload("pre_cryWolfLogo", "android_logobundle_01.unity3d", 1);
			cryWolfLogo.baseURL = "https://googledrive.com/host/0B0zQPJH0W58oRVpsUXUyUS13OFk/";
		                           //https://googledrive.com/host/0B0zQPJH0W58oRVpsUXUyUS13OFk/android_logobundle_01.unity3d
			assetsToLoad.Add(cryWolfLogo);
		}
		
		if (GUILayout.Button("Res load"))
		{
			Object obj = Resources.Load("pre_cryWolfLogo");
			GameObject gob = (GameObject)Instantiate(obj);

		}

		if (GUILayout.Button("Alert"))
		{
			ShowAlertBox("Alert", "Hello");
			Debug.Log("Alert!");
		}

//		if (GUILayout.Button("Inst test"))
//		{
//
//			GameObject obj = (GameObject)Resources.Load("pre_cryWolfLogo");
//			Instantiate(obj);
//		}
	}
	public static void ShowAlertBox ( string title, string message ) {
		using( AndroidJavaClass player = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ) )
		{
			AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
			using( AndroidJavaObject javaClass = new AndroidJavaObject( "com.Company.AssetBundle.Alert", activity ) )
			{
				javaClass.Call("showAlert", title, message);
			}
		}
	}

	IEnumerator DownloadAndCache ()
	{
		// Wait for the Caching system to be ready
		while (!Caching.ready)
			yield return null;

		using(WWW www = WWW.LoadFromCacheOrDownload ("https://googledrive.com/host/0B0zQPJH0W58oRVpsUXUyUS13OFk/", 1))
		{
			yield return www;
			
	
			if (www.error != null)
			{
				throw new System.Exception("WWW download had an error:" + www.error);
			}

			AssetBundle bundle = www.assetBundle;

			//string AssetName = "pre_cryWolfLogo.prefab";
			string AssetName = "";
			if (AssetName == "")
			{
			//	Instantiate(bundle.mainAsset);
				Object [] objs = bundle.LoadAll();
				foreach (Object ob in objs)
				{
					System.Type objType = ob.GetType();
					System.Reflection.PropertyInfo objProperty = objType.GetProperty("name");
					string objName = "null";
					if(objProperty != null) {
						objName = (string)objProperty.GetValue(ob, null);
					}
					Debug.Log("Object  Type = " + objType.Name+", Name=" + objName);
			

				}
			}
			else
				Instantiate(bundle.Load(AssetName));
			// Unload the AssetBundles compressed contents to conserve memory
			bundle.Unload(false);
		}


	}
}
