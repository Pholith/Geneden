using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class QuestSystem  : MonoBehaviour
{
    private static int currentLevel;
    private static int maxLevel;
    private static List<Quest> questsLevel0;
    private static List<Quest> questsLevel1;
    private static List<Quest> questsLevel2;
    private static List<Quest> questsLevel3;
    private static List<Quest> questsLevel4;
    private static List<Quest> questsLevel5;
    private static List<Quest> questsLevel6;

    private static TextMeshProUGUI questsDescription;

    [SerializeField]
    private static ResourceManager resourceManager;

    public void Start()
    {
        resourceManager = ResourceManager.Instance;
        questsLevel0 = new List<Quest>();
        questsLevel1 = new List<Quest>();
        questsLevel2 = new List<Quest>();
        questsLevel3 = new List<Quest>();
        questsLevel4 = new List<Quest>();
        questsLevel5 = new List<Quest>();
        questsLevel6 = new List<Quest>();
        currentLevel = 0;
        maxLevel = 7;

        /*Niveau 0 (Introduction):
        -Placer de la terre sur le monde 0/1
        -Mélanger de la Terre et de l'eau pour faire de la boue 0/1
        -Mélanger du Feu et de l'Air pour créer de la Foudre 0/1
        -Mélanger la Foudre et la Boue pour obtenir l'ADN 0/1  */
        questsLevel0.Add(new ElementQuest(1, "Terre"));
        questsLevel0.Add(new CraftQuest(1, "Boue"));
        questsLevel0.Add(new CraftQuest(1, "Foudre"));
        //questsLevel0.Add(new CraftQuest(1, "ADN"));

        /*
        (APPARTITION DE LA VIE)
 
        Niveau 1  (Besoin primaires)
        -Générer de la nourriture 0/1
        -Générer du bois 0/1               - nécessaire? (on peut passer par pierre direct?)
        -Placer une maison (Seul bâtiment disponible au niveau 1) 0/1 */
        questsLevel1.Add(new ResourceQuest(1, ResourceManager.RessourceType.Food));
        questsLevel1.Add(new ResourceQuest(1, ResourceManager.RessourceType.Wood));
        questsLevel1.Add(new BuildQuest(1, "Maison"));
    
        /*
        Niveau 2 (Pistes de gameplay)
        -Placer une mine 0/1
        -Placer une roche 0/1
         OU
        -Placer une forêt 0/1
        -Placer un camp de bûcherons 0/1 */
        questsLevel2.Add(new BuildQuest(1, "Mine"));
        questsLevel2.Add(new ElementQuest(1, "Roche"));
        questsLevel2.Add(new ElementQuest(1, "Végétation"));
        questsLevel2.Add(new BuildQuest(1, "Camp de bûcherons"));

        /*Niveau 3 (Développement)
        -Avoir 5 bâtiments               2/5
        -Atteindre une population de 30  14/30
        -Découvrir le fer                0/1 */
        questsLevel3.Add(new CraftQuest(1, "Fer"));
        questsLevel3.Add(new ResourceQuest(30, ResourceManager.RessourceType.Population));

        /*Niveau 4 (Prospérer)
        -Atteindre une génération de nourriture de 50/s
        -Atteindre une population de 75   30/75
        -Créer un bâtiment militaire */
        questsLevel4.Add(new ResourceQuest(75, ResourceManager.RessourceType.Population));
        questsLevel4.Add(new BuildQuest(1, "Militaire"));

        /*Niveau 5 (Explosion technologique)
        -Obtenir l'élément Or
        -Atteindre une population de 150  75/150*/
        questsLevel5.Add(new CraftQuest(1, "Or"));
        questsLevel5.Add(new ResourceQuest(1, ResourceManager.RessourceType.Population));

        /*Niveau 6 (AutoWin)
        Construire la merveille des dieux*/
        questsLevel6.Add(new BuildQuest(1, "Merveille"));

        questsDescription = gameObject.GetComponent<TextMeshProUGUI>();
        RefreshDescription();
    }

    private List<Quest> getCurrentQuestLevel() {
        switch (currentLevel) {
            case 0:
                return questsLevel0;
            case 1:
                return questsLevel1;
            case 2:
                return questsLevel2;
            case 3:
                return questsLevel3;
            case 4:
                return questsLevel4;
            case 5:
                return questsLevel5;
            case 6:
                return questsLevel6;
            default:
                return null;
        }
    }

    public void Crafted(string elementName) {
        UnityEngine.Debug.Log("Launched Crafted function with " + elementName);
        List<Quest> currentQuestLevel = getCurrentQuestLevel();
        for (int i = 0; i < currentQuestLevel.Count ; i++) {
            if (currentQuestLevel[i] is CraftQuest) {
                CraftQuest quest = (CraftQuest) currentQuestLevel[i];
                if (!quest.IsAccomplished() && elementName.Equals(quest.ObjectiveElement)) {
                    quest.SetCurrentAmount(1);
                    UnityEngine.Debug.Log("Crafting quest accomplished : " + elementName);
                    CheckNextLevel(currentQuestLevel);
                    break;
                }
            }
        }
    }

    public void ElementInvoked(string elementName) {
        List<Quest> currentQuestLevel = getCurrentQuestLevel();
        for (int i = 0; i < currentQuestLevel.Count ; i++) {
            if (currentQuestLevel[i] is ElementQuest) {
                ElementQuest quest = (ElementQuest) currentQuestLevel[i];
                if (!quest.IsAccomplished() && elementName.Equals(quest.ObjectiveElement)) {
                    quest.SetCurrentAmount(1);
                    UnityEngine.Debug.Log("Element quest accomplished : " + elementName);
                    CheckNextLevel(currentQuestLevel);
                }
            }
        }
    }

    public void Built(string buildName) {
        List<Quest> currentQuestLevel = getCurrentQuestLevel();
        for (int i = 0; i < currentQuestLevel.Count ; i++) {
            if (currentQuestLevel[i] is BuildQuest) {
                BuildQuest quest = (BuildQuest) currentQuestLevel[i];
                if (!quest.IsAccomplished() && buildName.Equals(quest.ObjectiveBuild)) {
                    quest.SetCurrentAmount(1);
                    UnityEngine.Debug.Log("Build quest accomplished : " + buildName);
                    CheckNextLevel(currentQuestLevel);
                }
            }
        }
    }

    private bool IsAllAccomplished(List<Quest> currentQuestLevel) {
        foreach (Quest quest in currentQuestLevel) {
            if (!quest.IsAccomplished())
                return false;
        }
        return true;
    }

    private void CheckNextLevel(List<Quest> currentQuestLevel) {
        if (IsAllAccomplished(currentQuestLevel))
            GetNextLevel();
        RefreshDescription();
    }

    private void RefreshDescription() {
        List<Quest> currentQuestLevel = getCurrentQuestLevel();
        //questsDescription.text = currentQuestLevel[0].description; //TODO
    }

    private void GetNextLevel() {
        currentLevel++;
        UnityEngine.Debug.Log("resource manager is " + resourceManager);
        resourceManager.AddRessource(ResourceManager.RessourceType.CivLevel, 1);
        if (resourceManager.GetCivLevel() != currentLevel)
            throw new InvalidOperationException("Civ level does not match with quest level!");
        UnityEngine.Debug.Log("Level Up!");

        if (currentLevel >= maxLevel)
            gameObject.GetComponent<GameManager>().EndGame();
    }

    private void Update() {        
    }
}

