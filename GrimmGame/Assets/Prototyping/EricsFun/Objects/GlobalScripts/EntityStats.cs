//========== Grimm - EntityStats.cs - 20/07/2020 ==========//
// Author:  Eric Brkic
// Purpose: This class serves as a central manager for any
//          entity in scene that requires data such as HP.
//          Used to retrieve and deminish the data of any
//          entity when required.
//=========================================================//
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Passive increase stamina, after a set amount of time. Also change how fast staminia regains on a per entity bases
// Once stamina is <= 0, no longer allow for attacks.
// Replenish health when the boss reaches zero.

public class EntityStats : MonoBehaviour
{
    /* Error value for when an entity does not exists.
     */
    private const int ENTITY_INDEX_OUT_OF_RANGE = -1;

    /* Struct containing the different data each entity within the list has attached to it.
     */
    [Serializable]
    private struct entityData
    {
        public string name;             // This modifies the 'Element #' within the inspector view. Used to make things a bit more readable.
        public GameObject entityObject; // This is here in case it is ever needed. I have my doubts at this point in time.
        public float health;            // The health, or hit points, of the entity.
        public float stamina;           // The maximum stamina the entity has.
        public float timeBeforeStaminaRegain;  // The amount of time the player must of not used stamina before it beings to regain.
        public float speedOfStaminaRegain;
        [HideInInspector] public float maxStamina;
        [HideInInspector] public float timeSinceLastStaminaDeminish;

        /* Default constructor on a per entity bases. Trying to get this to still work within inspector view.
           Note: Beginning to think it isnt possible to have structs have base values by default within the inspector prior runtime. Going to keep this here,
           for when and if a new entity needs to be instantiated.
        */
        private entityData(string _name = "New Entity", GameObject _entityObject = null, float _health = 100f, float _stamina = 50f, float _timeBeforeStaminaRegain = 3f, float _timeSinceLastStaminaDeminish = 0f)
        {
            name = _name;
            entityObject = _entityObject;
            health = _health;
            stamina = _stamina;
            timeBeforeStaminaRegain = _timeBeforeStaminaRegain;
            maxStamina = stamina;
            speedOfStaminaRegain = 0f;
            timeSinceLastStaminaDeminish = _timeSinceLastStaminaDeminish;
        }
    }

    /* Store a list of the different entities in the scene that should have stats like health, stamina, etc.
     */
    [SerializeField] private List<entityData> entityList = new List<entityData>();

    /* Instance of the singleton class
     */
    private static EntityStats instance;

    /* @brief Set the instance of the singleton to this class data.
     */
    private void Awake()
    {
        // Assure that the object can be destroyed if has to be created again later.
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        for (int i = 0; i < entityList.Count; i++)
        {
            entityData newData = entityList[i];
            newData.maxStamina = entityList[i].stamina;
            entityList[i] = newData;
        }
    }

    /* @brief Get the instance of the static singleton.
     */
    public static EntityStats Instance { get { return instance; } }

    private void Update()
    {
        ReplenishStamina();
    }

    private void ReplenishStamina()
    {
        for (int i = 0; i < entityList.Count; i++)
        {
            if (entityList[i].stamina != entityList[i].maxStamina)
            {
                if ((entityList[i].timeSinceLastStaminaDeminish + entityList[i].timeBeforeStaminaRegain) < Time.time)
                {
                    entityData newData = entityList[i];
                    newData.stamina += entityList[i].speedOfStaminaRegain;
                    entityList[i] = newData;
                }
            }
            else
            {
                entityData newData = entityList[i];
                newData.stamina = entityList[i].maxStamina;
                entityList[i] = newData;
            }
        }
    }

    /* @brief This function serves as a way to retrieve the health of any entity within the list at a given string key (name).
     * @param The string key (name) to lookup within the entity list.
     * @return The value of the entities health.
     */
    public float GetHealthOfEntity(string _entityName)
    {
        // Return the health of the entity at the index found within the entity list.
        return entityList[DoesEntityExist(_entityName)].health;
    }

    /* @brief This function serves as a way to retrieve the health of any entity within the list at a given string key (name).
     * @param The string key (name) to lookup within the entity list.
     * @return The value of the entities health.
     */
    public float GetStaminaOfEntity(string _entityName)
    {
        // Return the stamina of the entity at the index found within the entity list.
        return entityList[DoesEntityExist(_entityName)].stamina;
    }

    /* @brief This function serves as a way to remove health from an entity at the given string key (name).
     *        Once the entity is found, remove the amount of health according to the argument passed.
     * @param The string key (name) to lookup within the entity list.
     * @param The amount of health to deplete from the entity.
     */
    public void DeminishHealthOffEntity(string _entityName, float _amountToDeminish)
    {
        // The index of the entity is stored because it is used multiple times throughout the function.
        // Rather than doign the loop each time, just store it the first and only time.
        int entityIndex = DoesEntityExist(_entityName);

        // If the entityIndex is the error value returned, no reason to continue the function.
        if (entityIndex == ENTITY_INDEX_OUT_OF_RANGE)
            return;

        // See the last two lines of commented code in this function.
        entityData newData = entityList[entityIndex];

        // If the entity already has zero health, don't continue the function.
        if (entityList[entityIndex].health <= 0f)
        {
            newData.health = 0;
            entityList[entityIndex] = newData;
            
            return;
        }

        // Due to C# poor design in handling struct properties, I cant just assign an individual member of the entityData struct
        // for the given entity at the index. Instead I had to assign an entire struct... Please modifiy this if there is a work around.
        newData.health -= _amountToDeminish;
        entityList[entityIndex] = newData;
    }

    /* @brief This function serves as a way to remove stamina from an entity at the given string key (name).
     *        Once the entity is found, remove the amount of stamina according to the argument passed.
     * @param The string key (name) to lookup within the entity list.
     * @param The amount of stamina to deplete from the entity.
     */
    public void DeminishStaminaOffEntity(string _entityName, float _amountToDeminish)
    {
        // The index of the entity is stored because it is used multiple times throughout the function.
        // Rather than doign the loop each time, just store it the first and only time.
        int entityIndex = DoesEntityExist(_entityName);

        // If the entityIndex is the error value returned, no reason to continue the function.
        if (entityIndex == ENTITY_INDEX_OUT_OF_RANGE)
            return;

        // See the last two lines of commented code in this function.
        entityData newData = entityList[entityIndex];

        // If the entity already has zero stamina, don't continue the function.
        if (entityList[entityIndex].stamina <= 0f)
        {
            newData.stamina = 0;
            entityList[entityIndex] = newData;

            return;
        }

        // Due to C# poor design in handling struct properties, I cant just assign an individual member of the entityData struct
        // for the given entity at the index. Instead I had to assign an entire struct... Please modifiy this if there is a work around.
        newData.stamina -= _amountToDeminish;
        entityList[entityIndex] = newData;
    }

    /* @brief Checks to see if an entity will be able to make a move (attacking etc.). Based on how much stamina the
     *        entity has remaining as well as the cost of the move, calculate if the move can actualy occur.
     */
    public bool CanEntityMoveOccur(string _entityName, float _animStaminaCost)
    {
        // The index of the entity is stored because it is used multiple times throughout the function.
        // Rather than doign the loop each time, just store it the first and only time.
        int entityIndex = DoesEntityExist(_entityName);

        // Check that the amount of stamina the entity currently has minus the amount of stamina the move will cost.
        // If the amount of stamina remaining is a valid amount (greater than zero), then the move can occur.
        if ((entityList[entityIndex].stamina - _animStaminaCost) >= 0)
        {
            DeminishStaminaOffEntity(_entityName, _animStaminaCost);

            // Update the time since the last stamina deminish to now.
            entityData newData = entityList[entityIndex];
            newData.timeSinceLastStaminaDeminish = Time.time;
            entityList[entityIndex] = newData;

            return true;
        }

        return false;     
    }

    /* @brief This function will search the entity list to see if the entity desired at the specified key (name) exists.
     *        If the entity key is found, the index of the entity returned. Else an error value is returned.
     * @param The string key (name) to lookup within the entity list.
     * @return The index of the entity, if entity was not found then an error value.
     */
    private int DoesEntityExist(string _entityName)
    {
        // Loop over the size of the entity list.
        for (int i = 0; i < entityList.Count; i++)
        {
            // If the entity at this index, i, has the same key (name) as the passed key (name),
            // return its index value.
            if (entityList[i].name == _entityName)
                return i;
        }

        // If no entity was found with the appropriate key (name), log an error displaying the incorrect key.
        // Return an error index.
        Debug.LogError("ERROR: DoesEntityExist(string) call. The entity: " + _entityName + ", does not exists within the entity list.");
        return ENTITY_INDEX_OUT_OF_RANGE;
    }
}
