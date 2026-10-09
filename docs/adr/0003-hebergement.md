# ADR 0003 : Où héberger la PWA TournéeVéto ?

- **Statut :** Acceptée pour le POC
- **Date :** 2026-10-09

## Contexte et problème

TournéeVéto est une PWA statique Blazor WebAssembly, sans backend, compte ni synchronisation. Elle doit être déployable en cinq jours, fonctionner hors ligne après une première ouverture en ligne et être accessible sur tablette.

L’hébergement doit servir les fichiers statiques de l’application en HTTPS afin de permettre l’utilisation du service worker. Le déploiement doit aussi gérer le chemin d’URL propre à un site GitHub Pages de projet. Les données métier restent dans IndexedDB sur l’appareil : l’hébergement ne doit pas devenir un stockage de visites.

## Facteurs de décision

- Servir une application Blazor WebAssembly statique en HTTPS.
- Permettre l’installation et le fonctionnement hors ligne de la PWA après son premier chargement.
- Déployer automatiquement le build à partir du dépôt source.
- Prendre en charge le chemin de base d’un site GitHub Pages de projet.
- Limiter la configuration, le coût et la maintenance pour un POC de cinq jours.
- Éviter toute dépendance à un backend ou à un service d’hébergement dynamique non requis.

## Options envisagées

### Option 1 : GitHub Pages avec GitHub Actions

Publier le résultat statique du build Blazor WebAssembly sur GitHub Pages, au moyen d’un workflow GitHub Actions.

**Avantages :**
- Correspond à la plateforme de déploiement déjà prévue dans le périmètre du POC.
- Permet de publier les fichiers statiques sans gérer de serveur applicatif.
- S’intègre au dépôt et à son processus de validation.
- HTTPS permet l’usage du service worker nécessaire à la PWA.

**Inconvénients :**
- Un site de projet est servi sous un chemin préfixé par le nom du dépôt; le `<base href>`, le manifeste et le service worker doivent tenir compte de ce chemin.
- L’hébergement statique ne fournit pas de règle de réécriture configurable pour toutes les routes. L’accès direct ou le rechargement d’une route profonde doit être vérifié et, au besoin, adapté.
- La configuration du workflow doit publier le bon répertoire et gérer correctement les mises à jour du service worker.

### Option 2 : Azure Static Web Apps

Publier l’application sur Azure Static Web Apps, avec un workflow CI/CD relié au dépôt.

**Avantages :**
- Hébergement statique en HTTPS et intégration au déploiement continu.
- Peut offrir des règles de routage et des fonctionnalités supplémentaires si le produit évolue vers des besoins qui les justifient.
- Permet de configurer un domaine personnalisé.

**Inconvénients :**
- Ajoute une ressource Azure, sa configuration et un fournisseur supplémentaire à gérer.
- Les fonctions dynamiques, l’authentification et les autres capacités de la plateforme ne sont pas nécessaires au POC.
- Le chemin de déploiement et le pipeline demandent une validation spécifique sans bénéfice fonctionnel immédiat.

### Option 3 : Un autre hébergeur statique géré

Utiliser un service statique géré tel que Cloudflare Pages ou Netlify.

**Avantages :**
- Peut fournir HTTPS, publication automatique depuis Git et configuration de redirections.
- Certains services facilitent le déploiement sous un domaine personnalisé ou la gestion de routes d’application monopage.

**Inconvénients :**
- Ajoute un fournisseur et une configuration externes au flux de travail existant.
- Les fonctionnalités et limites dépendent du service retenu.
- N’apporte pas d’avantage nécessaire au parcours MVP qui compenserait ce changement.

## Décision

Héberger le POC sur **GitHub Pages**, avec un workflow GitHub Actions qui construit puis publie la PWA Blazor WebAssembly.

Le workflow déduit le chemin de base du nom du dépôt et le configure pour l’application et le service worker. Un fichier `404.html` redirige les demandes de routes profondes vers l’application afin que le routeur Blazor puisse restaurer la route. Le parcours doit être testé sur l’URL effectivement publiée : accueil, ressources, routes directes, ouverture initiale en ligne, puis rechargement et utilisation hors ligne.

GitHub Pages ne stocke que les fichiers de l’application. Les visites, les informations de troupeau et les photos restent dans IndexedDB sur l’appareil, conformément à l’ADR 0001.

### Conséquences positives

- L’hébergement est cohérent avec le périmètre du MVP et réduit les tâches d’infrastructure.
- Le build et la publication peuvent être intégrés au dépôt avec GitHub Actions.
- Le parcours ne dépend d’aucun serveur métier et peut fonctionner hors ligne après le chargement initial.
- Le choix est proportionné à un POC statique de cinq jours.

### Conséquences négatives et risques

- Le chemin de base du site de projet impose une configuration spécifique dans Blazor et dans les ressources de la PWA.
- Une mauvaise configuration ou mise à jour du service worker peut laisser une version périmée de l’application en cache.
- Le fonctionnement hors ligne dépend d’un premier chargement réussi et d’un cache complet des ressources nécessaires.
- Le POC ne bénéficie pas de capacités de routage serveur ou de fonctions dynamiques; leur ajout nécessiterait un changement d’hébergement ou une adaptation de l’architecture.

## Critères de réexamen

Revoir cette décision si :

- un domaine personnalisé, des règles de routage ou des en-têtes HTTP spécifiques deviennent nécessaires et ne peuvent pas être satisfaits avec GitHub Pages;
- les besoins évoluent vers un backend, une authentification, des fonctions serveur ou une intégration avec d’autres services;
- les tests sur l’URL GitHub Pages réelle révèlent des problèmes de chemin de base, de routes directes ou de fonctionnement hors ligne qui ne peuvent pas être corrigés proprement;
- une application destinée à des utilisateurs réels exige des garanties d’exploitation, de disponibilité ou de support incompatibles avec l’hébergement retenu.