
using UnityEngine;

public class SyncConfigItem : MonoBehaviour, IConfigItem
{
    

    public virtual void SetConfigValue()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void SyncValue(string field, object value)
    {
        throw new System.NotImplementedException();
    }
}