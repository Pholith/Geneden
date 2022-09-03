# PACMAN
01/09/2022 --------- Bastien
<br /> v1.0.1 : Globalement, mise en place des différents éléments graphiques du jeu ainsi qu'un script initial pour gérer les GameStates, l'UI et le MainMenu.
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
