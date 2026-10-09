---
name: create-component
description: "Créer ou modifier un composant UI réutilisable en Blazor dans TournéeVéto. À utiliser pour une nouvelle interface, un formulaire, une carte, une liste ou un composant Razor accessible et adaptatif."
argument-hint: "Décris le composant à créer, son emplacement ou son comportement attendu."
---

# Créer un composant UI TournéeVéto

Créer le composant demandé dans le projet actuel, en respectant l'architecture, les jetons de design et les contraintes d'usage terrain du POC.

## Procédure

1. **Comprendre le besoin et le placement**
   - Examiner les composants, pages, styles, abstractions et tests proches avant de modifier les fichiers.
   - Utiliser `src/TourneeVeto.Ui/` pour un composant réutilisable ou un comportement UI partagé.
   - Utiliser `src/TourneeVeto.Web/` seulement pour une page, le routage ou un comportement propre à l'hôte navigateur.
   - Respecter les dépendances `Web → Ui → Domain` et ne pas déplacer de logique propre au navigateur ou au stockage vers `Domain`.
   - Si une décision de comportement ou de portée change sensiblement la solution et n'est pas précisée, demander une clarification avant d'implémenter.

2. **Concevoir une interface claire**
   - Utiliser des composants Razor `.razor`; ajouter un `.razor.css` pour les styles isolés. Ajouter un fichier `.razor.cs` uniquement si le code-behind améliore réellement la lisibilité.
   - Garder le composant ciblé. Réutiliser les modèles, services, conventions et composants existants; ne pas ajouter de dépendance ou d'abstraction sans nécessité.
   - Déclarer les entrées et événements du composant avec des paramètres typés (`[Parameter]`, `EventCallback<T>`); ne pas incorporer de règle métier dans l'interface.
   - Présenter uniquement des données fictives. Les textes métier et les règles du POC ne doivent pas être présentés comme des avis cliniques ou réglementaires.

3. **Appliquer le système visuel**
   - Consulter `src/TourneeVeto.Ui/wwwroot/tokens.css` et employer ses variables CSS pour les couleurs, espacements, rayons, typographie et cibles tactiles.
   - Ne pas créer une palette locale ni remplacer les jetons par des valeurs littérales. Si un jeton manque, proposer un ajout cohérent au fichier de jetons plutôt qu'une valeur ponctuelle.
   - Concevoir d'abord pour tablette autour de 768 px, puis adapter au téléphone et au poste sans défilement horizontal.
   - Pour l'usage dans une étable, privilégier `--touch-target-comfortable` (56 px); aucune cible interactive ne doit être inférieure à `--touch-target` (44 px).
   - Vérifier que les paires de texte/fond respectent WCAG AA : 4,5:1 pour le texte normal et 3:1 pour le grand texte. Les contrôles et états ne doivent pas dépendre uniquement de la couleur.

4. **Assurer l'accessibilité et les états**
   - Associer un libellé accessible à chaque champ; utiliser les éléments HTML sémantiques et les boutons pour les actions.
   - Préserver un indicateur de focus visible et une utilisation au clavier; fournir des noms accessibles aux contrôles iconographiques.
   - Prévoir les états pertinents : chargement, vide, erreur, désactivé et hors ligne. Expliquer les erreurs et fournir une action de reprise lorsque possible; ne jamais afficher un succès si l'opération a échoué.
   - Pour un rapport destiné à l'impression, considérer aussi une présentation imprimable sans navigation ni contrôles interactifs.

5. **Tester et vérifier**
   - Ajouter ou adapter des tests bUnit pour tout nouveau comportement observable. Utiliser `BunitContext`, déclarer les appels JS avec `JSInterop.SetupModule` et ne jamais ouvrir une vraie base IndexedDB dans les tests.
   - Pour les dates, figer « aujourd'hui » avec `FakeTimeProvider`; utiliser `DemoData.Generate(today, seed)` avec un seed fixe lorsqu'il faut des données de démonstration.
   - Exécuter depuis la racine `dotnet build TourneeVeto.slnx`, puis `dotnet test TourneeVeto.slnx`. Corriger les régressions causées par le changement.
   - Ne pas lancer `dotnet run` ou `dotnet watch`; laisser l'utilisateur démarrer l'application.
   - Résumer le composant créé, les tests et validations exécutés, et signaler explicitement tout blocage.

## Références du projet

- Architecture et limites : `PRODUCT.md` et `docs/adr/0002-structure-solution.md`.
- Critères d'interface et de tablette : `docs/mvp.md`.
- Règles UI Razor : `.github/instructions/razor.instructions.md`.
- Règles des tests : `.github/instructions/tests.instructions.md`.
- Jetons visuels : `src/TourneeVeto.Ui/wwwroot/tokens.css`.
