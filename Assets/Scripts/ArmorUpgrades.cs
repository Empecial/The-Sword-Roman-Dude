using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArmorUpgrades : MonoBehaviour
{
   [Header("Armor")]
   public RawImage[] Chestplates;
   public RawImage[] Helmets;
   public RawImage[] Pans;

   [Header("Buttons")]
   public Button Chestplatebtn;
   public Button Helmetbtn;
   public Button Panbtn;

   [Header("Store text elements")]
   public TextMeshProUGUI ChestplateText;
   public TextMeshProUGUI HelmetText;
   public TextMeshProUGUI PanText;
   public TextMeshProUGUI NotEnoughSkulls2;
   public TextMeshProUGUI ChestArmorPrice;
   public TextMeshProUGUI HelmetArmorPrice;
   public TextMeshProUGUI PanArmorPrice;

   [Header("Bools")]
   public bool LowTierChestplateBool;
   public bool MidTierChestplateBool;
   public bool HighTierChestplateBool;

   public int chestplateswitch;

   private float Skulls;

   public GameObject GameManagerGB;

   public GameManager GameManagerRef;

   // Start is called before the first frame update
   void Start()
   {

      Chestplatebtn.onClick.AddListener(ChestPlateUpgrade);

      ChestArmorPrice.text = 15.ToString();

   }

   // Update is called once per frame
   void Update()
   {
      //updates skull amount
      Skulls = GameManagerRef.Skulls;

      //float value checker
      ChestPlateSwitch();
   }


   void ChestPlateSwitch()
   {
      //checks if the float value corresponds to the chestplate number
      switch (chestplateswitch)
      {
         case 1:
            Chestplates[0].gameObject.SetActive(true);
            LowTierChestplateBool = true;
            break;

         case 2:
            Chestplates[1].gameObject.SetActive(true);
            Chestplates[0].gameObject.SetActive(false);
            MidTierChestplateBool = true;
            //LowTierChestplateBool = false;
            break;

         case 3:
            Chestplates[2].gameObject.SetActive(true);
            Chestplates[1].gameObject.SetActive(false);
            HighTierChestplateBool = true;
            //MidTierChestplateBool = false;
            break;

         default:
            break;
      }

   }

   void ChestPlateUpgrade()
   {

	  if (Skulls >= 15)
	  {
		 if (LowTierChestplateBool == false)
		 {
            UpgradingChestArmor(15, 5);
		 }
	  }
	  else
	  {
         GameManagerRef.NotEnoughSkulls(Chestplatebtn);
	  }


	  if (Skulls >= 30)
	  {
         if (LowTierChestplateBool == true)
		 {
           UpgradingChestArmor(30, 10);
         }
	  }
	  else
	  {
         GameManagerRef.NotEnoughSkulls(Chestplatebtn);
	  }
   }


   //activates the different kinds of armor set logic with function parameters
   void UpgradingChestArmor( int ArmorPrice, int ArmorDamageVar)
   {
      GameManagerRef.ArmorDamageOff += ArmorDamageVar;

      Skulls -= ArmorPrice;

      GameManagerRef.CloseStore();

      chestplateswitch += 1;
   }


   void NotEnoughSkulls2Triggered(Button UpgradeButton)
   {
      NotEnoughSkulls2.gameObject.SetActive(true);

      NotEnoughSkulls2.transform.position = new Vector2(UpgradeButton.transform.position.x + 10, UpgradeButton.transform.position.y - 40);

   }

}
