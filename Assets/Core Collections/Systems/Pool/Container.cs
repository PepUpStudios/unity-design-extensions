using System.Collections.Generic;

public class Container  {    

    public static Container instance {
        set {
            m_instance = value;
        }
        get {
            if (m_instance == null) {
                m_instance = new Container();
                m_instance.reservoir = new Dictionary<string, Dictionary<string, object>>();
            }
            return m_instance;
        }
    }

    protected static Container m_instance;
    protected Dictionary<string, Dictionary<string, object>> reservoir;

    public T GetData<T>(string type, string name, string source = "N/A") {
        if (reservoir.ContainsKey(type)) {
            if (reservoir[type].ContainsKey(name)) {
                T obj = (T)reservoir[type][name];
                //Debug.Log("Peeked for: " + obj);
                return obj;
            }
        }
        //Debug.Log("Object not found: " + name);
        return default(T);
    }

    public T[] GetDataList<T>(string type, string name, string source = "N/A") {        
        if (reservoir.ContainsKey(type)) {
            if (reservoir[type].ContainsKey(name)) {
                T[] obj = (T[])reservoir[type][name];
                //Debug.Log("Peeked for: " + obj);
                return obj;
            }
        }
        //Debug.Log("Object not found: " + name);
        return default(T[]);
    }

    public void PutData<T>(string type, string name, T[] data) {
        if (!reservoir.ContainsKey(type)) {
            reservoir.Add(type, new Dictionary<string, object>());
        }
        if (!reservoir[type].ContainsKey(name)) {
            reservoir[type].Add(name, data);
        } else {
            reservoir[type][name] = data;
        }
        //Debug.Log("Pushing to Pool: " + name);        
    }

    public void PutData<T>(string type, string name, T data) {
        if (!reservoir.ContainsKey(type)) {
            reservoir.Add(type, new Dictionary<string, object>());
        }
        if (!reservoir[type].ContainsKey(name)) {
            reservoir[type].Add(name, data);
        } else {
            reservoir[type][name] = data;
        }
        //Debug.Log("Pushing to Pool: " + name);        
    }
}
