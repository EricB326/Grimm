using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Serializable]
    private struct entityData
    {
        public string name;
        public GameObject entityObject;
        public float health;
        public float stamina;

        entityData(string _name = "New Entity", GameObject _entityObject = null, float _health = 100.0f, float _stamina = 50.0f)
        {
            name = _name;
            entityObject = _entityObject;
            health = _health;
            stamina = _stamina;
        }
    }

    [SerializeField] private List<entityData> entityList;

    private void Update()
    {
        foreach (entityData thisEntity in entityList)
        {
            if (thisEntity.name == "Player")
            {
                Debug.Log(thisEntity.name);
            }
        }
    }

    public void RetrieveHealthAt(string _entityName)
    {
        foreach(entityData thisEntity in entityList)
        {
            if (thisEntity.name == _entityName)
            {
                Debug.Log(thisEntity.name);
            }
        }
    }
}
