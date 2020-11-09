//========== Grimm - EntityStats.cs - 20/07/2020 ==========//
// Author:  Eric Brkic
// Purpose: This class serves as a central manager for any
//          entity in scene that requires data such as HP.
//          Used to retrieve and deminish the data of any
//          entity when required.
//=========================================================//
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public string name;                                          // This modifies the 'Element #' within the inspector view. Used to make things a bit more readable.
        public GameObject entityObject;                              // This is here in case it is ever needed. I have my doubts at this point in time.
        public float health;                                         // The health, or hit points, of the entity.
        public int lives;                                            // 
        public float stamina;                                        // The maximum stamina the entity has.
        public float mass;                                           // The mass of the entity;
        public float inputBuffer;                                    // 
        public float timeBeforeStaminaRegain;                        // The amount of time the player must of not used stamina before it beings to regain.
        public float speedOfHealthRegain;                            // How fast the entity will regain their stamina after a period of time.
        public float speedOfStaminaRegain;                           // 
        [HideInInspector] public int maxLives;                       // 
        [HideInInspector] public float maxHealth;                    // Maximum amount of health the entity has.
        [HideInInspector] public float maxStamina;                   // Maximum amount of stamina the entity has.
        [HideInInspector] public float timeSinceLastStaminaDeminish; // The amount of time since the entity last lost stamina.

        /* Default constructor on a per entity bases. Trying to get this to still work within inspector view.
           Note: Beginning to think it isnt possible to have structs have base values by default within the inspector prior runtime. Going to keep this here,
           for when and if a new entity needs to be instantiated.
        */
        //private entityData(string _name = "New Entity", GameObject _entityObject = null, float _health = 100f, float _stamina = 50f, float _timeBeforeStaminaRegain = 3f, float _speedOfHealthRegain = 3f, float _speedOfStaminaRegain = 3f)
        //{
        //    name = _name;
        //    //entityObject = _entityObject;
        //    health = _health;
        //    stamina = _stamina;
        //    timeBeforeStaminaRegain = _timeBeforeStaminaRegain;
        //    speedOfHealthRegain = _speedOfHealthRegain;
        //    speedOfStaminaRegain = _speedOfStaminaRegain;
        //    maxHealth = health;
        //    maxStamina = stamina;
        //    timeSinceLastStaminaDeminish = 0f;
        //}
    }

    public event EventHandler onDamageTaken;
    public event EventHandler onStaminaTaken;
    public event EventHandler onHealthReplenish;
    public event EventHandler onStaminaReplenish;
    public event EventHandler onPlayerLifeLost;
    public event EventHandler onBossPhaseChange;

    private entityData newData;

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

        // Loop over all entities in the entity list and set the max health/stamina to the first state of current health/stamina.
        for (int i = 0; i < entityList.Count; i++)
        {
            newData = entityList[i];

            newData.maxHealth = entityList[i].health;
            newData.maxStamina = entityList[i].stamina;

            if (entityList[i].name == "Boss")
            {
                newData.lives = entityList[i].entityObject.GetComponent<BossBrain>().m_bossPhaseList.Count;
            }

            newData.maxLives = entityList[i].lives;

            entityList[i] = newData;
        }
    }

    /* @brief Get the instance of the static singleton.
     */
    public static EntityStats Instance { get { return instance; } }

    /* @brief Updates runs once per frame.
     */
    private void Update()
    {
        // Begin replenishing the stamina for the appropriate entities.
        ReplenishStamina();

        // If the boss loses a life from a phase, each entity should regain.
        if (restoreInProcess)
        {
            RestoreAllStats();
        }

        for (int i = 0; i < entityList.Count; i++)
        {
            if (entityList[i].health <= 0)
            {
                HandleZeroHealthEntity(i);
            }
        }
    }

    private void HandleZeroHealthEntity(int _entityIndex)
    {
        newData = entityList[_entityIndex];
        //newData.health = newData.maxHealth;
        newData.lives--;
        entityList[_entityIndex] = newData;

        // Still lives available.
        if (entityList[_entityIndex].lives > 0)
        {
            // Handle player losing a life.
            // if(entityList[_entityIndex].name == "Player")
            if (entityList[_entityIndex].name == "Player")
            {
                if (onPlayerLifeLost != null)
                    onPlayerLifeLost(this, EventArgs.Empty);

                PhaseReset(_entityIndex);
            }
            else // Handle boss losing a life (phase).
            {
                if (onBossPhaseChange != null)
                    onBossPhaseChange(this, EventArgs.Empty);

                NextPhase(_entityIndex);

                entityList[_entityIndex].entityObject.GetComponent<BossBrain>().m_bossPhaseList.RemoveAt(0);
            }
        }
        else // No lives available, end of game.
        {
            if (entityList[_entityIndex].name == "Player") // Handle player losing final life.
            {
                // Remove the final life indicator.
                if (onPlayerLifeLost != null)
					onPlayerLifeLost(this, EventArgs.Empty);

				GameOverLose();
            }
            else // Handle boss losing final life.
            {
                // Remove the final phase indicator.
                if (onBossPhaseChange != null)
                    onBossPhaseChange(this, EventArgs.Empty);
                entityList[_entityIndex].entityObject.GetComponent<BossBrain>().m_bossPhaseList.RemoveAt(0);

                GameOverWin();
            }

            newData.lives = 0;
            entityList[_entityIndex] = newData;
        }
    }

    private void NextPhase(int _entityIndex)
    {
		switch (entityList[_entityIndex].lives)
		{
            case 4:
                Transition.PhaseOneTransition();
                break;
			case 3:
                Transition.PhaseTwoTransition();
				break;
			case 2:
                Transition.PhaseThreeTransition();
                break;
			case 1:
                Transition.PhaseFourTransition();
                break;
			default:
                Transition.DefaultBossTransision();
                break;
		}
	}

    private void PhaseReset(int _entityIndex)
    {
        Transition.PlayerPhaseResetTransiton();
    }

    private void GameOverWin()
    {
        Transition.BossDefeatedTransition();

        // Also should have some sort of end game thing go off to stop the game.
    }

    private void GameOverLose()
    {
        Transition.PlayerDefeatedTransition();

        // Also should have some sort of end game thing go off to stop the game.
    }

    /* @brief Handles replenishing the entities stamina based on set conditions on if they are able to replenish as well as
     *        if they actually need to replenish.
     */
    private void ReplenishStamina()
    {
        // Loop over each entity in the entity list.
        for (int i = 0; i < entityList.Count; i++)
        {  
            // If the entity is not at the maximum amount of stamina they have elegated, they can move onto further checks.
            if (entityList[i].stamina < entityList[i].maxStamina)
            {
                // If the entity has not deminished stamina after a set period, they can begin to regain stamina.
                if ((entityList[i].timeSinceLastStaminaDeminish + entityList[i].timeBeforeStaminaRegain) < Time.time)
                {
                    newData = entityList[i];
                    newData.stamina += newData.speedOfStaminaRegain;
                    entityList[i] = newData;

                    if (onStaminaReplenish != null)
                        onStaminaReplenish(this, EventArgs.Empty);
                }
            }
            else
            {
                // If the entity is at full stamina, have a fail safe to insure they do not go above max.
                newData = entityList[i];
                newData.stamina = newData.maxStamina;

                entityList[i] = newData;
            }
        }
    }

    private bool restoreInProcess = false;

    public void RestoreAllStats()
    {
        restoreInProcess = true;

		int bossIndex = DoesEntityExist("Boss");
		int playerIndex = DoesEntityExist("Player");

        if (entityList[bossIndex].health >= entityList[bossIndex].maxHealth && entityList[playerIndex].health >= entityList[playerIndex].maxHealth)
        { 
			restoreInProcess = false;

            // Make sure both entities are not about the max.
            newData = entityList[bossIndex];
            newData.health = newData.maxHealth;
            entityList[bossIndex] = newData;

			newData = entityList[playerIndex];
			newData.health = newData.maxHealth;
			entityList[playerIndex] = newData;
		}

		if (onHealthReplenish != null)
            onHealthReplenish(this, EventArgs.Empty);

        for (int i = 0; i < entityList.Count; i++)
        {
            RestoreStatsOfEntity(entityList[i].name);
        }
    }

	public void RestoreAllStatsInstant()
	{
		int bossIndex = DoesEntityExist("Boss");
		int playerIndex = DoesEntityExist("Player");

        newData = entityList[bossIndex];
        newData.health = newData.maxHealth;
        entityList[bossIndex] = newData;

		newData = entityList[playerIndex];
		newData.health = newData.maxHealth;
		entityList[playerIndex] = newData;

		if (onHealthReplenish != null)
			onHealthReplenish(this, EventArgs.Empty);
	}

	public void RestoreStatsOfEntity(string _entityName)
    {
        int entityIndex = DoesEntityExist(_entityName);

        newData = entityList[entityIndex];

        if (entityList[entityIndex].name != "Player")
            newData.stamina = newData.maxStamina;

        if (entityList[entityIndex].health != entityList[entityIndex].maxHealth)
            newData.health += newData.speedOfHealthRegain;
        else
            newData.health = newData.maxHealth;

        entityList[entityIndex] = newData;

	}

	public float GetMaxHealthOfEntity(string _entityName)
	{
		// Return the health of the entity at the index found within the entity list.
		return entityList[DoesEntityExist(_entityName)].maxHealth;
	}

	public void SetMaxHealthOfEntity(string _entityName, float newMaxHP)
    {
		int entityIndex = DoesEntityExist(_entityName);

		newData = entityList[entityIndex];

		newData.maxHealth = newMaxHP;

		entityList[entityIndex] = newData;
	}

    public GameObject GetObjectOfEntity(string _entityName)
    {
        // Return the health of the entity at the index found within the entity list.
        return entityList[DoesEntityExist(_entityName)].entityObject;
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
    
    public void SetStaminaOfEntity(string _entityName, float _newStamina)
    {
        int entityIndex = DoesEntityExist(_entityName);

        newData = entityList[entityIndex];

        newData.maxStamina = _newStamina;

        entityList[entityIndex] = newData;
    }

    public float GetMaxStaminaOfEntity(string _entityName)
    {
        return entityList[DoesEntityExist(_entityName)].maxStamina;
    }

    public void SetMaxStaminaOfEntity(string _entityName, float _newMaxStamina)
    {
        int entityIndex = DoesEntityExist(_entityName);

        newData = entityList[entityIndex];

        newData.maxStamina = _newMaxStamina;

        entityList[entityIndex] = newData;
    }

    public float GetMassOfEntity(string _entityName)
    {
        return entityList[DoesEntityExist(_entityName)].stamina;
    }
    
    public float GetBufferOfPlayer()
    {
        return entityList[DoesEntityExist("Player")].inputBuffer;
    }

    public float GetNormalisedHealthOfEntity(string _entityName)
    {
        // The index of the entity is stored because it is used multiple times throughout the function.
        // Rather than doign the loop each time, just store it the first and only time.
        int entityIndex = DoesEntityExist(_entityName);

        // If the entityIndex is the error value returned, no reason to continue the function.
        if (entityIndex == ENTITY_INDEX_OUT_OF_RANGE)
            return -1f;

        return entityList[entityIndex].health / entityList[entityIndex].maxHealth;
    }

    public float GetNormalisedStaminaOfEntity(string _entityName)
    {
        // The index of the entity is stored because it is used multiple times throughout the function.
        // Rather than doign the loop each time, just store it the first and only time.
        int entityIndex = DoesEntityExist(_entityName);

        // If the entityIndex is the error value returned, no reason to continue the function.
        if (entityIndex == ENTITY_INDEX_OUT_OF_RANGE)
            return -1f;

        return entityList[entityIndex].stamina / entityList[entityIndex].maxStamina;
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

        restoreInProcess = false;

        // See the last two lines of commented code in this function.
        newData = entityList[entityIndex];

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
        if (newData.health <= 0f)
        {
            newData.health = 0f;
        }

        entityList[entityIndex] = newData;

        if (onDamageTaken != null)
            onDamageTaken(this, EventArgs.Empty);
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
        newData = entityList[entityIndex];

        // Due to C# poor design in handling struct properties, I cant just assign an individual member of the entityData struct
        // for the given entity at the index. Instead I had to assign an entire struct... Please modifiy this if there is a work around.
        newData.stamina -= _amountToDeminish;

        // Begin the countdown for stamina regeneration.
        newData.timeSinceLastStaminaDeminish = Time.time;

        entityList[entityIndex] = newData;

        if (onStaminaTaken != null)
            onStaminaTaken(this, EventArgs.Empty);
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
        if ((entityList[entityIndex].stamina /*- _animStaminaCost*/) >= 0)
        {
            //if (entityList[entityIndex].name != "Boss")
            //{ 
            //    DeminishStaminaOffEntity(_entityName, _animStaminaCost);
            //
            //    // Update the time since the last stamina deminish to now.
            //    newData = entityList[entityIndex];
            //    newData.timeSinceLastStaminaDeminish = Time.time;
            //    entityList[entityIndex] = newData;
            //}

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

    // yo sorry bro
    public int GetLivesOfEntity(string _entityName)
    {
        return entityList[DoesEntityExist(_entityName)].lives;
    }
    public void SetHealthOfEntity(string _entityName, float _newHealth)
    {
        int playerIndex = DoesEntityExist(_entityName);

        newData = entityList[playerIndex];
        newData.health = _newHealth;
        entityList[playerIndex] = newData;

        if (onHealthReplenish != null)
            onHealthReplenish(this, EventArgs.Empty);
    }



}
