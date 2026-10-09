# ADR 0002 : Comment structurer la solution .NET ?

- **Statut :** Acceptée pour le POC
- **Date :** 2026-10-09

## Contexte et problème

Le POC doit livrer en cinq jours une PWA Blazor WebAssembly utilisable hors ligne, sans backend. Le stockage local est réalisé avec IndexedDB, conformément à l’ADR 0001.

La conception doit garder les règles métier testables et préserver une éventuelle réutilisation dans une application WPF ou MAUI. Ces applications et le partage d’écrans ne font toutefois pas partie du POC. Il faut donc éviter à la fois un projet monolithique difficile à faire évoluer et une architecture à plusieurs couches qui ralentirait inutilement la livraison.

## Facteurs de décision

- Livrer le parcours MVP dans le délai de cinq jours.
- Tester les règles métier sans navigateur ni WebView.
- Ne pas faire dépendre les règles métier de Blazor, de JavaScript ou d’IndexedDB.
- Préserver une voie de réutilisation de la logique métier dans une application native.
- Garder une structure compréhensible et proportionnée au POC.

## Options envisagées

### Option 1 : Un seul projet Blazor WebAssembly

L’interface, les règles métier et l’accès au stockage sont placés dans le projet Blazor.

**Avantages :**
- Structure initiale simple, avec peu de configuration.
- Livraison rapide pour un POC court.

**Inconvénients :**
- Risque de couplage entre la logique métier et la plateforme Web.
- Tests et réutilisation dans une application native plus difficiles si ce couplage s’étend.

### Option 2 : Une application Blazor WebAssembly et une bibliothèque .NET partagée

Le projet Blazor contient les écrans et les adaptateurs propres au navigateur. Une bibliothèque .NET indépendante de l’interface contient les modèles et les règles métier réutilisables.

**Avantages :**
- Les règles métier peuvent être testées séparément.
- La bibliothèque reste indépendante de Blazor, de JS interop et du stockage navigateur.
- Une application native pourrait la référencer plus tard.

**Inconvénients :**
- Ajoute une frontière de projet.
- Demande de définir ce qui relève de la logique partagée ou de la plateforme.

### Option 3 : Une architecture multi-projets complète

Séparer notamment le domaine, les cas d’utilisation, l’infrastructure, l’interface Web et éventuellement une bibliothèque de composants d’interface partagés.

**Avantages :**
- Responsabilités fortement isolées.
- Adaptée à une application plus vaste ou à plusieurs interfaces livrées en parallèle.

**Inconvénients :**
- Multiplie les projets, interfaces et configurations avant que le POC ait démontré leur utilité.
- Charge disproportionnée pour le délai et le périmètre actuels.

## Décision

Retenir deux projets :

1. Une bibliothèque .NET partagée, contenant les modèles et règles métier sans dépendance à Blazor, au navigateur ou à IndexedDB.
2. Une application Blazor WebAssembly, contenant les écrans, le service worker et les implémentations propres au navigateur, notamment l’accès à IndexedDB par JS interop.

L’accès aux données est exposé à la logique applicative par une abstraction définie dans la bibliothèque partagée; son implémentation IndexedDB reste dans le projet Web. Les vues et composants restent dans Blazor pour le POC. La création d’une bibliothèque de composants partagés avec WPF ou MAUI est reportée jusqu’à ce qu’une application native soit effectivement planifiée.

### Conséquences positives

- Les règles métier peuvent être testées sans navigateur.
- La bibliothèque partagée peut servir de base à une future application native.
- Le code spécifique au navigateur — Blazor, JS interop, service worker et IndexedDB — reste isolé.
- La structure garde un nombre limité de projets et de frontières pour le POC.

### Conséquences négatives et risques

- La séparation en deux projets demande un peu de configuration et de discipline dès le début.
- La bibliothèque partagée risque de devenir une couche artificielle si elle ne contient que des modèles sans règles réutilisables; son contenu doit rester justifié par des usages concrets.
- Le partage de la logique métier ne garantit pas le partage des écrans, de la persistance ni du comportement propre aux plateformes.

## Critères de réexamen

Revoir cette décision si :

- les règles métier deviennent assez riches pour justifier une séparation explicite entre domaine et cas d’utilisation;
- plusieurs interfaces doivent être livrées et partager des composants visuels;
- l’application native devient prioritaire et les frontières actuelles empêchent une réutilisation effective;
- les tests révèlent que les abstractions de stockage ou les dépendances entre projets rendent les changements plus difficiles plutôt que plus simples.