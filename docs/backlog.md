# TournéeVéto — backlog du POC

Backlog dérivé de [mvp.md](./mvp.md). Les neuf fonctionnalités Must sont représentées par un Epic chacune. Chaque Epic porte l’étiquette `epic` et une étiquette fonctionnelle; ses stories portent `story` et cette même étiquette fonctionnelle. Chaque story comprend deux critères Gherkin, dont au moins un cas d’erreur.

Les données de démonstration sont fictives et les règles métier simplifiées. Le POC est une PWA Blazor WebAssembly statique, hébergée sur GitHub Pages, sans backend ni compte; les données de visite sont stockées dans IndexedDB.

## Epic 1 — Charger et utiliser l’application hors ligne

**Étiquettes :** `epic`, `pwa-hors-ligne`  
**Description :** Les ressources essentielles de la PWA sont disponibles après une première ouverture en ligne, puis restent utilisables hors ligne. L’application fonctionne sous le chemin de déploiement GitHub Pages.

### Story 1.1 — Reprendre une visite après rechargement hors ligne

**Étiquettes :** `story`, `pwa-hors-ligne`  
**User story :** En tant que vétérinaire, je veux rouvrir l’application sans réseau afin de poursuivre une visite dans une étable.

**Critères d’acceptation**
- Étant donné que j’ai ouvert l’application en ligne et que ses ressources essentielles ont été mises en cache, quand je passe le navigateur hors ligne et recharge la page, alors l’écran d’accueil et le parcours de visite s’affichent sans requête réseau réussie.
- Étant donné qu’une ressource nécessaire n’est pas disponible hors ligne, quand l’application tente de l’utiliser, alors elle indique clairement l’indisponibilité au lieu d’afficher un écran vide ou un succès fictif.

### Story 1.2 — Charger l’application depuis le chemin GitHub Pages

**Étiquettes :** `story`, `pwa-hors-ligne`  
**User story :** En tant que vétérinaire, je veux accéder à la PWA sur son URL GitHub Pages afin de l’utiliser sans serveur applicatif.

**Critères d’acceptation**
- Étant donné l’URL de publication GitHub Pages configurée, quand je l’ouvre dans un navigateur pris en charge, alors la page d’accueil retourne HTTP 200 et ses routes et ressources se chargent sous le chemin du projet.
- Étant donné un chemin de ressource invalide, quand le navigateur le demande, alors l’application ne présente pas cette ressource comme chargée et l’erreur reste détectable.

## Epic 2 — Fournir un jeu de données fictives

**Étiquettes :** `epic`, `donnees-demo`  
**Description :** Au premier démarrage, l’application présente des élevages et animaux fictifs suffisants pour parcourir le POC, sans données réelles.

### Story 2.1 — Initialiser les élevages et animaux de démonstration

**Étiquettes :** `story`, `donnees-demo`  
**User story :** En tant que vétérinaire, je veux trouver des élevages et des animaux de démonstration afin d’essayer le parcours sans préparer moi-même les données.

**Critères d’acceptation**
- Étant donné une base IndexedDB vierge, quand je démarre l’application, alors au moins 2 élevages et 5 animaux fictifs associés sont affichés.
- Étant donné que le jeu initial de données ne peut pas être chargé, quand l’application démarre, alors elle signale l’erreur et ne prétend pas que les données de démonstration sont disponibles.

### Story 2.2 — Réinitialiser les données de démonstration sans les dupliquer

**Étiquettes :** `story`, `donnees-demo`  
**User story :** En tant que vétérinaire testant le POC, je veux que le chargement initial soit répétable afin de retrouver un jeu de démonstration cohérent.

**Critères d’acceptation**
- Étant donné que le jeu de démonstration est déjà initialisé, quand je redémarre l’application, alors les mêmes élevages et animaux restent disponibles sans doublons.
- Étant donné que l’initialisation rencontre un identifiant invalide ou un conflit, quand elle échoue, alors une erreur explicite est affichée et aucune entrée partielle n’est annoncée comme complète.

## Epic 3 — Consulter la tournée du jour et une fiche d’élevage

**Étiquettes :** `epic`, `tournee-elevage`  
**Description :** Le vétérinaire consulte les élevages prévus, ouvre une fiche d’élevage et revient à la tournée.

### Story 3.1 — Parcourir et sélectionner les élevages de la tournée

**Étiquettes :** `story`, `tournee-elevage`  
**User story :** En tant que vétérinaire, je veux voir et sélectionner les élevages de ma tournée du jour afin de préparer la visite voulue.

**Critères d’acceptation**
- Étant donné plusieurs élevages de démonstration, quand j’ouvre la tournée, alors chaque élevage prévu est présenté et je peux ouvrir sa fiche.
- Étant donné qu’aucun élevage n’est disponible dans la tournée, quand je l’ouvre, alors un état vide explicite apparaît et aucune visite n’est sélectionnée par défaut.

### Story 3.2 — Consulter la fiche et les animaux d’un élevage

**Étiquettes :** `story`, `tournee-elevage`  
**User story :** En tant que vétérinaire, je veux consulter le contexte d’un élevage et ses animaux afin de préparer les actions à réaliser.

**Critères d’acceptation**
- Étant donné un élevage de démonstration, quand j’ouvre sa fiche depuis la tournée, alors son nom, son adresse fictive et ses animaux associés sont visibles.
- Étant donné un identifiant d’élevage inconnu, quand je tente d’ouvrir sa fiche, alors un message d’erreur ou d’indisponibilité est présenté avec un moyen de revenir à la tournée.

## Epic 4 — Créer une visite et suivre les actions de régie

**Étiquettes :** `epic`, `grille-regie`  
**Description :** Le vétérinaire crée une visite et consigne des actions simples par animal : action prévue, statut et note.

### Story 4.1 — Créer une visite pour un élevage

**Étiquettes :** `story`, `grille-regie`  
**User story :** En tant que vétérinaire, je veux créer une visite associée à un élevage afin de regrouper mes observations et actions du jour.

**Critères d’acceptation**
- Étant donné un élevage sélectionné, quand je crée une visite, alors celle-ci est associée à cet élevage et apparaît dans son contexte.
- Étant donné qu’aucun élevage valide n’est sélectionné, quand je tente de créer une visite, alors la création est refusée et l’interface explique ce qui manque.

### Story 4.2 — Consigner les actions de régie par animal

**Étiquettes :** `story`, `grille-regie`  
**User story :** En tant que vétérinaire, je veux consigner une action, son statut et une note pour un animal afin de suivre le travail prévu pendant la visite.

**Critères d’acceptation**
- Étant donné une visite et un animal associé, quand je sélectionne une action parmi vêlage, tarissement, insémination, diagnostic de gestation ou CCS, que je définis son statut et saisis une note, alors ces valeurs sont visibles pour cet animal dans la grille.
- Étant donné qu’un animal ne fait pas partie de l’élevage de la visite, quand je tente de lui attribuer une action, alors l’action est refusée et aucune entrée de régie ne lui est associée.

## Epic 5 — Compléter le bilan de biosécurité

**Étiquettes :** `epic`, `bilan-biosecurite`  
**Description :** Le vétérinaire renseigne une liste simple de contrôles et de commentaires dans le cadre de la visite.

### Story 5.1 — Répondre aux points de contrôle

**Étiquettes :** `story`, `bilan-biosecurite`  
**User story :** En tant que vétérinaire, je veux renseigner les points d’une liste de biosécurité afin de documenter les constats de la visite.

**Critères d’acceptation**
- Étant donné une visite ouverte, quand je renseigne au moins 3 points de contrôle, alors chaque réponse est affichée dans le bilan de cette visite.
- Étant donné qu’aucune visite n’est sélectionnée, quand je tente d’enregistrer une réponse, alors l’application refuse l’enregistrement et indique qu’une visite est nécessaire.

### Story 5.2 — Ajouter un commentaire à un point de contrôle

**Étiquettes :** `story`, `bilan-biosecurite`  
**User story :** En tant que vétérinaire, je veux commenter un point de biosécurité afin de préciser le constat ou le suivi attendu.

**Critères d’acceptation**
- Étant donné un point de contrôle d’une visite, quand je saisis un commentaire et quitte le champ, alors le commentaire demeure visible sur ce point.
- Étant donné que le commentaire dépasse la limite de saisie définie par l’application, quand je tente de l’enregistrer, alors l’application signale la limite et ne tronque pas silencieusement le texte.

## Epic 6 — Enregistrer et reprendre une visite hors ligne

**Étiquettes :** `epic`, `persistance-hors-ligne`  
**Description :** Une visite complète est conservée localement dans IndexedDB et restaurée après rechargement, sans backend.

### Story 6.1 — Retrouver une visite complète après rechargement

**Étiquettes :** `story`, `persistance-hors-ligne`  
**User story :** En tant que vétérinaire, je veux retrouver mes saisies après rechargement sans réseau afin de ne pas perdre le travail effectué dans l’étable.

**Critères d’acceptation**
- Étant donné une visite contenant élevage, grille de régie, bilan de biosécurité et rapport, quand je passe hors ligne, recharge l’application puis rouvre la visite, alors tous les champs saisis sont identiques.
- Étant donné une visite qui n’a pas pu être enregistrée avant fermeture, quand je rouvre l’application, alors l’interface ne prétend pas que les dernières modifications ont été sauvegardées.

### Story 6.2 — Signaler les erreurs d’enregistrement local

**Étiquettes :** `story`, `persistance-hors-ligne`  
**User story :** En tant que vétérinaire, je veux savoir si IndexedDB refuse un enregistrement afin de réagir avant de quitter la visite.

**Critères d’acceptation**
- Étant donné que l’écriture IndexedDB réussit, quand je modifie une donnée de visite, alors l’interface confirme l’enregistrement et la valeur est présente après réouverture.
- Étant donné que l’écriture IndexedDB échoue, quand je modifie une donnée, alors l’interface affiche une erreur explicite, ne confirme pas le succès et conserve la saisie à l’écran tant que possible.

## Epic 7 — Consulter et exporter un rapport de visite PDF

**Étiquettes :** `epic`, `rapport-pdf`  
**Description :** Le vétérinaire consulte le rapport associé à la visite et en exporte une copie PDF hors ligne.

### Story 7.1 — Générer un PDF contenant le rapport de visite

**Étiquettes :** `story`, `rapport-pdf`  
**User story :** En tant que vétérinaire, je veux exporter le rapport en PDF afin de pouvoir le remettre ou l’archiver.

**Critères d’acceptation**
- Étant donné une visite contenant un élevage, une date, une action de régie, un résultat de biosécurité et le texte du rapport, quand je génère le PDF hors ligne, alors un fichier est téléchargé et son contenu comprend ces cinq éléments.
- Étant donné que la génération PDF échoue, quand je lance l’export, alors une erreur explicite est affichée et aucun téléchargement réussi n’est annoncé.

### Story 7.2 — Nommer et exporter le PDF hors ligne

**Étiquettes :** `story`, `rapport-pdf`  
**User story :** En tant que vétérinaire, je veux obtenir un fichier PDF identifiable même sans réseau afin de conserver le rapport de la visite.

**Critères d’acceptation**
- Étant donné une visite complète, quand je télécharge son rapport, alors un fichier PDF au nom non vide est produit sans requête à un service distant.
- Étant donné que le navigateur bloque le téléchargement ou que le fichier ne peut pas être produit, quand je lance l’export, alors l’interface indique l’échec et permet de réessayer.

## Epic 8 — Utiliser les écrans sur tablette en portrait et paysage

**Étiquettes :** `epic`, `interface-tablette`  
**Description :** Les parcours Must sont consultables et utilisables au toucher aux dimensions tablette indiquées dans le MVP.

### Story 8.1 — Utiliser les écrans en portrait et paysage

**Étiquettes :** `story`, `interface-tablette`  
**User story :** En tant que vétérinaire, je veux utiliser les écrans sur tablette dans les deux orientations afin de saisir mes constats selon les conditions de l’étable.

**Critères d’acceptation**
- Étant donné une fenêtre de 768 × 1024 puis de 1024 × 768 pixels, quand j’ouvre tournée, grille, bilan et rapport, alors leur contenu principal reste accessible sans défilement horizontal de page.
- Étant donné une largeur inférieure à la largeur tablette prise en charge, quand j’ouvre un écran, alors le contenu ne se chevauche pas et reste accessible par défilement vertical.

### Story 8.2 — Saisir au toucher sans perdre les données lors d’une rotation

**Étiquettes :** `story`, `interface-tablette`  
**User story :** En tant que vétérinaire, je veux activer les contrôles au toucher et conserver mes saisies lors d’une rotation afin de poursuivre la visite sans clavier ni souris.

**Critères d’acceptation**
- Étant donné un écran de visite sur tablette, quand je touche les contrôles principaux, alors je peux sélectionner un élevage, renseigner une action et répondre à un point de biosécurité.
- Étant donné une saisie en cours, quand l’orientation de la tablette change ou que le navigateur ne fournit pas l’événement d’orientation, alors la saisie n’est pas perdue et les contrôles restent accessibles.

## Epic 9 — Rédiger et modifier le rapport avec résumé de visite

**Étiquettes :** `epic`, `edition-rapport`  
**Description :** Le vétérinaire rédige un rapport modifiable et retrouve les actions de régie et constats de biosécurité associés.

### Story 9.1 — Rédiger et modifier le texte du rapport

**Étiquettes :** `story`, `edition-rapport`  
**User story :** En tant que vétérinaire, je veux rédiger et modifier le rapport de visite afin de consigner ma synthèse.

**Critères d’acceptation**
- Étant donné une visite ouverte, quand je saisis puis modifie le texte du rapport, alors la version la plus récente est affichée.
- Étant donné que l’enregistrement de la modification échoue, quand je quitte le champ, alors l’échec est signalé et le texte n’est pas présenté comme sauvegardé.

### Story 9.2 — Relire les actions et constats depuis le rapport

**Étiquettes :** `story`, `edition-rapport`  
**User story :** En tant que vétérinaire, je veux retrouver les actions et constats de la visite dans le contexte du rapport afin de rédiger une synthèse cohérente.

**Critères d’acceptation**
- Étant donné une visite avec au moins une action de régie et un point de biosécurité renseignés, quand j’ouvre son rapport, alors ces éléments sont consultables avec le texte du rapport.
- Étant donné que la visite n’a pas d’action de régie ou de point de biosécurité renseigné, quand j’ouvre le rapport, alors l’absence est indiquée comme telle et aucun élément n’est inventé.
