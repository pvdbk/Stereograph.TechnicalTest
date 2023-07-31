
## Utilisation

### Prérequis

Installation prérequises :
- Git
- .NET 7.0 SDK

### Installation

Choisir un emplacement de destination où télécharger la solution.
Pour effectuer le téléchargement, ouvrir une invite de commande dans l'emplacement et taper la commande suivante.

`git clone https://github.com/pvdbk/Stereograph.TechnicalTest.git`

L'emplacement contiendra alors un nouveau répertoire, matérialisant la solution : `Stereograph.TechnicalTest`. Ce répertoire en contiendra lui-même deux :
  - `Stereograph.TechnicalTest.Api`
  - `Stereograph.TechnicalTest.Tests`

### Lancements

Pour lancer les tests de la solution, exécuter la commande `dotnet test` dans le deuxième. Pour lancer l'API, exécuter `dotnet run` dans le premier.

Une fois l'API lancée, il est possible d'observer les points de terminaison qu'elle offre en visitant cette URL : <https://localhost:7163/swagger/index.html>

## Difficultés rencontrées

- J'avais de très faibles notions en Entity Framework. J'ai donc dû commencer par faire quelques recherches. La documentation de Microsoft est claire sur le sujet, mais j'ai tout de même passé beaucoup de temps à chercher et à expérimenter.
- Les tests m'ont également pris un temps considérable. J'ai commencé par créer un mock de ApplicationDbContext en l'abstrayant dans une interface. Mais il fallait pouvoir fournir des DbSet à ce mock, et je n'ai pas trouvé de manière simple de le faire. Je me suis donc rabattu sur le framework Moq. Je ne le connaissais pas mais il est proposé par la documentation Microsoft et est simple d'usage. J'ai tout de même buté avec sur la fin après l'ajout d'une seconde entité, à propos des CollectionEntry. J'ai alors opté pour un work around afin de ne pas perdre trop de temps (PersonServiceUnitTest.cs ligne 175 et PersonsService.cs lignes 45 et 46).
- J'ai sévèrement bloqué avec Docker. Là encore, je ne disposais que de faibles notions, il a donc fallu que je commence par des recherches. J'ai tout de même rapidement produit un Dockerfile, mais l'image qu'il produit ne s'est jamais avérée fonctionnelle sur ma machine. L'application tourne bien dans le conteneur, les deux ports qui doivent être occupés dans l'hôte le sont, mais impossible de communiquer avec l'API (message de Thunder Client : "Connection was forcibly closed by a peer.").
