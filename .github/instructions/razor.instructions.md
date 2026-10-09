---
applyTo: ["**/*.razor", "**/*.razor.css", "**/*.razor.cs", "src/TourneeVeto.Ui/wwwroot/**/*.css"]
description: "Règles UI Razor et CSS : composants, styles, jetons de design et accessibilité"
---
- Utiliser `src/TourneeVeto.Ui/wwwroot/tokens.css` comme source de vérité pour les couleurs sémantiques, espacements, rayons, typographie et tailles tactiles. Ne pas réintroduire de valeurs littérales pour ces jetons dans les styles d'interface.
- Pour les composants Razor, placer les styles propres au composant dans son fichier `.razor.css`; employer les jetons CSS existants plutôt que de définir une palette ou une échelle locale.
- Respecter les paires de couleurs à contraste AA définies par les jetons (au moins 4,5:1 pour le texte normal et 3:1 pour les grands textes et composants visuels). Un statut ne doit jamais être indiqué uniquement par la couleur.
- Concevoir tablette d'abord autour de 768 px, puis adapter au téléphone et au poste sans défilement horizontal; privilégier `--touch-target-comfortable` (56 px) en contexte d'étable, sans jamais descendre sous `--touch-target` (44 px).
- Accessibilité : associer un libellé à chaque champ, conserver un focus visible, utiliser des noms accessibles et fournir des états vides et d'erreur explicites.
- Aucune logique métier : appeler TourneeVeto.Domain ; données via IVisitRepository injecté.
- Jamais de MarkupString avec une donnée saisie ou importée.