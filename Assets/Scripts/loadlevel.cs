using UnityEngine;
using System.Collections;

public class loadlevel : MonoBehaviour {

	public int levelid = 0;
	AsyncOperation aop = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator LoadLevel()
	{
		aop = Application.LoadLevelAsync(levelid);

		while(!aop.isDone)
		{
			yield return aop;
		}

	}

	void OnGUI()
	{

		if (GUI.Button(new Rect(0,0,100,100), "Load"))
		{
			StartCoroutine("LoadLevel");
		}

		GUI.Box(new Rect(0,200, 120, 50), "loading");
		if (aop != null)
		{
			GUI.Box(new Rect(10,210, aop.progress*100, 30), "");
		}
	}
}
