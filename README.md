# PACMAN
01/09/2022 --------- Bastien
<br /> v1.0.1
<br /> Globalement, mise en place des différents éléments graphiques du jeu ainsi qu'un script initial pour gérer les GameStates, l'UI et le MainMenu.
<br /> <br />MainMenu :
- Création d'un GameObject MainMenu
- Ajout du Titre
- Ajout d'un bouton Play
- Ajout d'un bouton Quit
- Création d'un script MainMenu pour gérer les boutons Play et Quit ainsi qu'une fonction Hide/Show du MainMenu

UIManager :
- Création d'un GameObject UI
- Ajout d'un écran de Victoire
- Ajout d'un écran de GameOver
- Ajout de l'UI de jeux (vies,fruits,scores)
- Création d'un script UIManager avec des fonctions Hide/Show pour les différents écrans de l'UI

GameManager :
- Ajout d'une variable MainMenu
- Ajout d'une variable UIManager
- Ajout d'une variable map (en attendant d'avoir un script pour gérer le jeux et la map)
- Création des fonctions associées aux GameStates Lose,Victory,MainMenu,Game,Quit

04/09/2022
<br /> v1.0.2 
<br /> Ajout de la capacité de manger les pièces à Pacman, changement dynamique du score lorsque Pacman entre en colition avec une pièce.
<br /> Début d'implémentation d'une fin de partie avec reset de la Game.
<br /> <br />GameManager :
- Ajout d'une variable Transform coins
- Ajout d'une variable Transform pacman
- Ajout d'une fonction setGame()
- Ajout d'une fonction CoinEaten()
- Ajout d'une fonction hasCoin()
- Modification de la fonction onGame()

ScoreManager :
- Création du script ScoreManager
- Ajout d'une variable GameObject gameUI
- Ajout d'une variable TextMeshProUGUI upScore
- Ajout d'une variable TextMeshProUGUI globalScore
- Ajout d'une variable int gScoreVar
- Ajout d'une variable int upScoreVar
- Création des fonctions addUpScore() et addGlobalScore()

Coins :
- Création de 2 script Coin et Large_Coin
- Ajout d'une variable int value
- Ajout d'une fonction OnTriggerEnter2D()
- Ajout d'une fonction getValue()

