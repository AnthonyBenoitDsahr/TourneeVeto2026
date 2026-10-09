# TournéeVéto

**Préparer, réaliser et documenter une visite vétérinaire laitière, dans une seule application utilisable hors ligne.**

TournéeVéto est un prototype (POC) de **progressive web app (PWA)** destiné aux vétérinaires laitiers. Il rassemble les informations utiles à la tournée, la grille de régie par animal, le bilan de biosécurité et le rapport de visite. Le parcours est conçu pour rester accessible dans les étables, même lorsque le réseau ne l’est pas.

> [!WARNING]
> Ce POC utilise des **données fictives** et des **règles métier simplifiées**. Il ne constitue ni un dossier médical, ni un avis clinique ou réglementaire. Ne l’utilisez pas pour prendre des décisions de soins.

## Fonctionnalités prévues

- Consulter la tournée du jour et les fiches d’élevage.
- Créer une visite et suivre les actions de régie par animal : vêlage, tarissement, insémination, diagnostic de gestation et CCS.
- Consigner le statut et une note courte pour chaque action.
- Remplir une liste de contrôle simple de biosécurité.
- Rédiger et modifier le rapport de visite, puis l’exporter en PDF.
- Enregistrer les visites localement pour les retrouver hors ligne.
- Utiliser l’interface sur tablette, en orientation portrait ou paysage.

## Parcours type

1. Sélectionner un élevage dans la tournée et consulter sa fiche.
2. Créer une visite, puis renseigner la grille de régie et le bilan de biosécurité.
3. Rédiger le rapport, le conserver sur l’appareil et l’exporter en PDF.

## Fonctionnement et limites

- **Application :** PWA statique construite avec Blazor WebAssembly et publiée sur GitHub Pages.
- **Stockage :** les données de visite sont enregistrées localement dans IndexedDB, sur l’appareil et le navigateur utilisés.
- **Hors ligne :** après une première ouverture en ligne permettant de charger l’application et ses ressources, le parcours doit rester disponible sans réseau.
- **Données :** le POC fournit des données de démonstration fictives; aucune information réelle n’est nécessaire.

Il n’y a pas de backend, de compte utilisateur, d’authentification, de synchronisation ni de partage entre appareils. Les données locales peuvent être perdues si les données du navigateur sont effacées, si l’appareil est remplacé ou si le navigateur les supprime. Le POC n’inclut pas non plus d’intégrations à des logiciels de troupeau ou à des registres officiels, de recommandations cliniques automatisées ni d’applications natives Windows ou mobiles.

## Utilisation hors ligne

1. Ouvrez l’application une première fois avec une connexion réseau.
2. Attendez que le chargement initial soit terminé.
3. Dans l’étable, rouvrez l’application sur le même appareil et dans le même navigateur. Les visites enregistrées localement doivent rester accessibles sans réseau.

La disponibilité hors ligne dépend notamment du chargement initial et du cache de l’application. Elle doit être vérifiée sur l’URL GitHub Pages réellement déployée et sur l’appareil cible. Ce POC ne fournit pas de sauvegarde distante.

## Publication sur GitHub Pages

Le workflow GitHub Actions publie l’application lors d’un push sur `main` ou à la demande. Dans les paramètres du dépôt, configurez **Pages > Build and deployment > Source** sur **GitHub Actions**. Le workflow déduit le chemin de base du nom du dépôt, configure les ressources et le service worker, puis publie l’application statique. Il prend aussi en charge les URL de routes Blazor en les redirigeant vers l’application.

Après le premier déploiement, vérifiez que l’URL Pages retourne HTTP 200, que les ressources et les routes s’ouvrent sous le chemin du projet, et qu’une ressource inexistante reste en erreur. Le rechargement hors ligne se valide séparément selon les étapes ci-dessus.

## Documentation

- [Contexte produit et personas](PRODUCT.md)
- [Périmètre du POC et critères de succès](docs/mvp.md)

## Périmètre du POC

Le POC est priorisé sur cinq jours. Son objectif est de valider le parcours complet d’une visite sur tablette, avec données fictives et sans dépendance à un service réseau métier. Les filtres avancés, l’impression navigateur et la personnalisation des contrôles de biosécurité sont secondaires; les comptes, les intégrations et les applications natives sont hors périmètre.

## Statut

Les documents produit et MVP définissent le besoin et les critères de succès. Consultez [docs/mvp.md](docs/mvp.md) pour les critères de validation détaillés.
