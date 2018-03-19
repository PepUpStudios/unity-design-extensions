using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool {

	protected static ObjectPool m_instance;

    protected Dictionary<string, Dictionary<string, Stack>> reservoir;

    public static ObjectPool instance {
        set {
            m_instance = value;
        } get {
            if (m_instance == null) {
                m_instance = new ObjectPool();
                m_instance.reservoir = new Dictionary<string, Dictionary<string, Stack>>();
            }
            return m_instance;
        }
	}

    public static void ClearReservoir() {
        m_instance.reservoir.Clear();
    }

    public GameObject GetGameObject(string type, string name, string source = "N/A"){
		if(reservoir.ContainsKey(type)){
			if(reservoir[type].ContainsKey(name)){
				while(reservoir[type][name].Count > 0){
					GameObject gameObject = (GameObject) reservoir[type][name].Pop();
					if(!gameObject.activeInHierarchy){
						//Debug.Log("Found Object in Pool: " + name + " for " + source);
						//gameObject.SetActive(true);
						//Debug.Log("Poped: " + name);
						return gameObject;
					}
					//Debug.Log("Poped for Free; found Active: " + name);
				}
			}
		}
		//if(type == "scene") Debug.Log("Object not found: " + name);
		return null;
	}

    public void PutGameObject(string type, string name, GameObject gameObject){
		if(!reservoir.ContainsKey(type)){
			reservoir.Add(type,new Dictionary<string, Stack>());
		}
		if(!reservoir[type].ContainsKey(name)){
			Stack poolStack = new Stack();
			reservoir[type].Add(name,poolStack);
		}
		//Debug.Log("Pushing to Pool: " + name);
		if (gameObject.transform.parent != null) gameObject.transform.parent = null;
		gameObject.SetActive(false);
		reservoir[type][name].Push(gameObject);
	}

    public T GetObjectByPop<T>(string type, string name, string source = "N/A") {
        if (reservoir.ContainsKey(type)) {
            if (reservoir[type].ContainsKey(name)) {
                if (reservoir[type][name].Count > 0) {
                    T obj = (T)reservoir[type][name].Pop();
                    //Debug.Log("Poped for: " + obj);
                    return obj;
                }
            }
        }
        //Debug.Log("Object not found: " + name);
        return default(T);
    }

    public T GetObjectByPeek<T>(string type, string name, string source = "N/A") {
        if (reservoir.ContainsKey(type)) {
            if (reservoir[type].ContainsKey(name)) {
                if (reservoir[type][name].Count > 0) {
                    T obj = (T)reservoir[type][name].Peek();
                    //Debug.Log("Peeked for: " + obj);
                    return obj;
                }
            }
        }
        //Debug.Log("Object not found: " + name);
        return default(T);
    }

    public void PutObject<T>(string type, string name, T obj) {
        if (!reservoir.ContainsKey(type)) {
            reservoir.Add(type, new Dictionary<string, Stack>());
        }
        if (!reservoir[type].ContainsKey(name)) {
            Stack poolStack = new Stack();
            reservoir[type].Add(name, poolStack);
        }
        //Debug.Log("Pushing to Pool: " + name);
        reservoir[type][name].Push(obj);
    }

    public void RemoveAllObject(string type, string name) {
        if (!reservoir.ContainsKey(type) || !reservoir[type].ContainsKey(name)) {
            return;
        }
        //Debug.Log("Removed from Pool: " + name);
        reservoir[type][name].Clear();
    }

    public void OverrideLastObject<T>(string type, string name, T obj) {
        if (!reservoir.ContainsKey(type)) {
            reservoir.Add(type, new Dictionary<string, Stack>());
        }
        if (!reservoir[type].ContainsKey(name)) {
            Stack poolStack = new Stack();
            reservoir[type].Add(name, poolStack);
        } else {
            reservoir[type][name].Pop();
        }
        //Debug.Log("Pushing to Pool: " + name);
        reservoir[type][name].Push(obj);
    }
}
