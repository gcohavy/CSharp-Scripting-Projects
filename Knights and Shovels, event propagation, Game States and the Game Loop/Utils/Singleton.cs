//Class to be extended by any object that needs to have only a single instance,
// like the Game Manager

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //Create a private reference to the instance of the class
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    //Create a method to check if the class has already been initialized once
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    //This method will run on instantiation
    protected virtual void Awake()
    {
        //Send a log error if there is already an instance, otherwise assign the instance
        if ( instance != null )
        {
            Debug.LogError("[Singleton] Trying to instantiate a second instance of a Singleton class");
        }
        else
        {
            instance = (T) this;
        }
    }

    //Create an overridable method for OnDestroy(), which should set the
    // instance to null again so that it can once again be instantiated
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
