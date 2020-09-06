using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T m_Instance;


    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {
            if(m_Instance==null)
            {
                m_Instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            }
            return m_Instance;
        }
    }

}
