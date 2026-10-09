# Instructions Copilot — TournéeVéto

## Contexte et limites

- TournéeVéto est un POC québécois de visite de troupeaux laitiers.

- Utiliser uniquement des données fictives; ne jamais introduire de données réelles ou nominatives.

- Les règles et seuils sont simplifiés, non cliniques et ne remplacent pas le jugement vétérinaire ni les protocoles officiels.

- Pas de backend, compte, synchronisation ni intégration externe dans le périmètre du POC.

## Architecture

- Respecter les dépendances `Web → Ui → Domain` et `Tests → Domain, Ui`.

- Garder le Domain indépendant de Blazor, JS interop et du stockage.

- Les composants réutilisables et le stockage IndexedDB appartiennent à Ui; l’hôte Web porte les fonctions propres au navigateur.

- Préserver l’usage hors ligne, la sauvegarde locale et l’adaptation tablette.


## C# et changements

- Suivre le nullable, les implicit usings et `TreatWarningsAsErrors` définis dans `Directory.Build.props`.

- Réutiliser les conventions et abstractions existantes; préférer les types explicites et les modèles immuables.

- Ajouter ou adapter des tests pour tout changement de comportement; ne pas modifier les tests pour masquer une régression.

- Garder les changements ciblés et mettre à jour la documentation directement concernée.

## Vérification

- Utiliser le SDK épinglé par `global.json` (.NET 10).

- Après un changement pertinent, exécuter `dotnet build TourneeVeto.sln`, puis `dotnet test TourneeVeto.sln`.

- Ne pas lancer `dotnet run` ni `dotnet watch`; l'utilisateur démarre l'application lui-même.

## Références produit

- Glossaire, parcours et limites : [PRODUCT.md](../PRODUCT.md).

- Critères du POC : [docs/mvp.md](../docs/mvp.md).

- Architecture : [ADR 0002](../docs/adr/0002-structure-solution.md).

- Stockage local : [ADR 0001](../docs/adr/0001-stockage.md).

- Publication : [ADR 0003](../docs/adr/0003-hebergement.md).