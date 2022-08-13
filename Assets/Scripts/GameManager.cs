using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    public Button PlayerRespawn;
    public Button Ability1Heal;
    public Button Ability2;
    public Button DamageEnemy;

    [Header("Store")]
    public Button UpgradeHealth;
    public Button UpgradeDamage;
    public Button UpgradeAbilityDamage;
    public Button SkipStore;

    [Header("Gameobjects")]

    public GameObject Characters;
    public GameObject Store;
    public GameObject RespawnScreen;
    public GameObject playerGB;
    public GameObject enemyGB;

    [Header("Float values")]

    public float NextAttackTime;
    public float ClickCountNumber;
    public float Skulls;
    public float SkullsAwarded;
    public float Level;
    public float DamageToEnemy;
    public float DamageToPlayer;
    public float StartDamageToEnemy;
    public float WaitUntilAttack;
    public float ArmorDamageOff;
    public float GradualEnemyDamage;

    [Header("Bools")]
    public bool AttackingEnemy;
    public bool EnemyDefeated;
    public bool PlayerDefeated;
    public bool OpenStore;
    public bool LevelDone;
   public bool Ability1Ready;

    [Header("Text elements")]
    public TextMeshProUGUI ClicksCounter;
    public TextMeshProUGUI SkullsText;
    public TextMeshProUGUI NotEnoughSkull;

    [Header("Audio")]
    public AudioClip[] Audioclips;
    public AudioSource audioPlayer;
    public AudioSource BackgroundMusicPlayer;

    [Header("References")]
    Player PlayerRef;
    Enemy EnemyRef;

    void Start()
    {
        //play background music on start
        BackgroundMusicPlayer.Play();


        //saves the audiosource from gamemanager object into audioplayer variable
        audioPlayer = GetComponent<AudioSource>();

        //sets the enemy to attack at the start of the round, which will wait by the amount specified in NextAttackTime in Start
        AttackingEnemy = true;

        //references to player and enemy scripts
        EnemyRef = Characters.GetComponentInChildren<Enemy>();
        PlayerRef = Characters.GetComponentInChildren<Player>();

        
        //when player clicks on buttons, subscribes action to specific methods

        //triggers player's ability 1
      Ability1Heal.onClick.AddListener(Ability1Triggered);

      //when the player clicks on enemy and damages by 1 point pr click
      DamageEnemy.onClick.AddListener(DecreaseEnemyHealth);

        //when the player is dead and presses this button they respawn but with 3 skulls less
        PlayerRespawn.onClick.AddListener(PlayerRespawnTriggered);

        //when the store is open and the player has enough skulls to buy the health upgrade
        UpgradeHealth.onClick.AddListener(UpgradeHealthTriggered);

        //upgrades player's damage to enemy by set amount
        UpgradeDamage.onClick.AddListener(UpgradeDamageTriggered);

        //closes the store without using skulls for upgrades
        SkipStore.onClick.AddListener(CloseStore);

      //so the player cant use ability1 instantly
      Ability1Ready = true;

    }

    // Update is called once per frame
    void Update()
    {
        //checks if enemy health is <=0 and adds +5 to the players skull amount
        if (EnemyRef.Health <= 0)
        {
            DamageEnemy.enabled = false;

            if (EnemyRef.EnemyType[0] == "slime")
            {

                if (EnemyDefeated == true)
                {
                print("enemy is " + EnemyRef.EnemyType[0]);
                Skulls += SkullsAwarded;
                EnemyDefeated = false;
                
                }

                
            }


        }

      
        

        //to make the game give the player 5 skulls only 1 once. when enemy is defeated this value stays false,
        //then true during the one frame when the enemy is dead then comes back to false.
        //thus adds only 5 skulls per round 
        EnemyDefeated = false;

        //value kept false 
        PlayerDefeated = false;

        //attacks the player by the NextAttackTime float
        ContinuouslyAttackPlayer();

        //subtracts 1 when player clicks on button from click amount
        ClicksCounter.text = ClickCountNumber.ToString();

        //checks if player is dead
        PlayerDead();


        //the skullstext in GUI is equal to Skulls value
        SkullsText.text = Skulls.ToString();


        //the amount of clicks the player has
        CheckIfClicksUsed();


        //if enemy health <= 0 and player isnt dead ( health > 0), opens the store after the match
        StoreOpen();

        LevelChecker();

    }

   void Ability1Triggered()
   {
	  if (Ability1Ready==true)
	  {     
         PlayerRef.Health += 5;
         Ability1Ready = false;
	  }

      print("ability 1 activated");
      StartCoroutine(Ability1Cooldown());
   }
   IEnumerator Ability1Cooldown()
   {
      yield return new WaitForSeconds(10);

      Ability1Ready = true;
      
      print("ability 1 ready");
   }


   //if player's health equals to 0 or is below 0 enable respawn screen
   void PlayerDead()
    {
        if (PlayerRef.Health <= 0 )
        {
            RespawnScreen.SetActive(true);
        }
        /*else if (PlayerRef.Health > 0)
        {
            RespawnScreen.SetActive(false);
        }*/
    }


    void LevelChecker()
    {

        /*for (int Levels = 0; Levels > 0; Levels++)
        {
            EnemyRef.Health += 5;
        }*/


    }


    //spawns player back in. subtract 3 skull point and remove respawn screen
    void PlayerRespawnTriggered()
    {
        //remove the respawn screen
        RespawnScreen.SetActive(false);

        //sets player health to a standard starthealth since his actual health has been decreased
        PlayerRef.Health = PlayerRef.StartHealth;

        //EnemyRef.Health = EnemyRef.StartHealth;
        EnemyRef.Health = EnemyRef.StartHealth;
        
        //subtract 3 skulls for respawning
        Skulls -= 3;

        //enables enemy damage button
        DamageEnemy.enabled = true;

      //sets player damage back to start damage. at 1
      DamageToEnemy = StartDamageToEnemy;

      StartCoroutine(WaitForAttack());

        NextAttackTime = NextAttackTime + 2f;
        print("u respawned");

    }

    public void CloseStore()
    {
      print("store close");

     //makes it so when the player kills the enemy again the store will open and level will increment by 1 again
     LevelDone = false;
     OpenStore = false;
        
     //deactivates the store gb, allows enemy and player to attack
     Store.SetActive(false);

      //sets the notenoughskull to false
      NotEnoughSkull.gameObject.SetActive(false);

    //waits set amount of seconds to enable enemy ability to damage player
     StartCoroutine(WaitForAttack());

    //enables the button to damage enemy again
     DamageEnemy.enabled = true;

      DamageToPlayer += GradualEnemyDamage;

      EnemyHealthIncrease();
    }

    void EnemyHealthIncrease()
    {
      print("enemy health increased");
        EnemyRef.StartHealth += 5;
        EnemyRef.Health = EnemyRef.StartHealth;
    }

    void UpgradeHealthTriggered()
    {
        if (Skulls >= 5)
        {

        PlayerRef.StartHealth += 3;
        PlayerRef.Health = PlayerRef.Health +3;


        //subtract skulls since player upgraded 
        Skulls -= 5;
         
        CloseStore();

        }
        else if (Skulls < 5)
        {
            NotEnoughSkull.gameObject.SetActive(true);
            NotEnoughSkull.transform.position = new Vector2(UpgradeHealth.transform.position.x + 10, UpgradeHealth.transform.position.y - 40);
        }
    }

    void UpgradeDamageTriggered()
    {

        if (Skulls >= 5)
        {

        DamageToEnemy += 0.5f;

         //subtract skulls since player upgraded 
         Skulls -= 5;

         CloseStore();
        }

        else if (Skulls < 5)
        {
         NotEnoughSkulls(UpgradeDamage);

        }

    }

   public void NotEnoughSkulls(Button UpgradeButton)
   {
      NotEnoughSkull.gameObject.SetActive(true);

      NotEnoughSkull.transform.position = new Vector2(UpgradeButton.transform.position.x + 10, UpgradeButton.transform.position.y - 40);

      StartCoroutine(RemoveNotEnough());
   }

   IEnumerator RemoveNotEnough()
   {
      yield return new WaitForSeconds(2);

      NotEnoughSkull.gameObject.SetActive(false);
   }

   //after player upgrades something the enemy waits with attacking
   IEnumerator WaitForAttack()
    {
    yield return new WaitForSeconds(WaitUntilAttack);

     AttackingEnemy = true;
    }


    //enemy attacks player
    void ContinuouslyAttackPlayer()
    {
        if (AttackingEnemy == true)
        {

            //if the time elapsed since the game started is higher than or equal to NextAttackTime 
            //then start attacking the player
            if (Time.time >= NextAttackTime)
            {
                NextAttackTime = Time.time + WaitUntilAttack;

			  if (ArmorDamageOff <= DamageToPlayer)
			  {
               PlayerRef.Health = PlayerRef.Health - (DamageToPlayer - ArmorDamageOff);

               print("1: " + (DamageToPlayer-ArmorDamageOff).ToString());
              }

			  else if (ArmorDamageOff > DamageToPlayer)
			  {
               PlayerRef.Health = PlayerRef.Health - (ArmorDamageOff - DamageToPlayer);
               print("2: "+ (DamageToPlayer-ArmorDamageOff).ToString());
              }

              //if player died plays died audio
                if (PlayerRef.Health <= 0)
                {
                    audioPlayer.clip = Audioclips[0];
                    audioPlayer.Play();
                    AttackingEnemy = false;
                }
            }
        }

    }

    //player damages the enemy by 1 on every click and removes 1 from click counter
    void DecreaseEnemyHealth()
    {
        EnemyRef.Health -= DamageToEnemy;
        ClickCountNumber -= 1;
        audioPlayer.clip = Audioclips[3];
        audioPlayer.Play();

        if (EnemyRef.Health <= 0)
        {
            EnemyDefeated = true;
            audioPlayer.clip = Audioclips[1];
            audioPlayer.Play();
        }
        else if (EnemyRef.Health > 0)
        {
            EnemyDefeated = false;
        }
    }

    //ability 1, makes player stronger for 2 hits
    void Ability1Button()
    {
        PlayerRef.Health = 0;
        ClickCountNumber -= 1;
    }

    //checks if all available clicks are used up. if yes, disable gameplay components so player
    //can no longer play
    void CheckIfClicksUsed()
    {
        if (ClickCountNumber <= 0)
        {
            Ability1Heal.enabled = false;
            Ability2.enabled = false;
            DamageEnemy.enabled = false;
            AttackingEnemy = false;
            print("clicks used");

        }

    }

    void StoreOpen()
    {
        if (EnemyRef.Health <= 0)
        {
            if (PlayerRef.Health > 0 && LevelDone == false)
            {

                OpenStore = true;

                Store.SetActive(true);
                
                AttackingEnemy = false;
                
                Level = Level + 1;
                LevelDone = true;

                //EnemyRef.Health += 5;
            }
        }
    }

}
