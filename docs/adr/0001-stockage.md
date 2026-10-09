# ADR 0001 : Où stocker les visites, le troupeau et les photos dans le navigateur ?

- **Statut :** Acceptée pour le POC
- **Date :** 2026-10-09

## Contexte et problème

TournéeVéto doit permettre de préparer, réaliser, enregistrer et consulter une visite sans réseau, sur l’appareil utilisé. Le POC est une PWA Blazor WebAssembly statique, sans backend, compte ni synchronisation. Les visites comprennent des données structurées — élevages, animaux, actions de régie, contrôles et rapports — et pourraient inclure des photos.

Le stockage doit permettre de retrouver les données après fermeture et rechargement de l’application. Il faut aussi tenir compte du stockage des photos, du tri par date et d’une éventuelle réutilisation future dans une WebView WPF ou MAUI. Ces applications natives ne font pas partie du POC.

Contexte : [PRODUCT.md](../../PRODUCT.md) et [docs/mvp.md](../mvp.md).

## Facteurs de décision

- Fonctionner après une première ouverture en ligne, puis sans réseau.
- Pouvoir conserver les visites et les photos sans être limité à la très faible capacité de `localStorage`.
- Permettre des recherches par index et un tri efficace par date.
- Limiter le poids ajouté au téléchargement initial et la complexité d’implémentation, compte tenu du POC de cinq jours.
- Rester testable dans un vrai navigateur et viable dans les moteurs WebView utilisés par WPF et MAUI.
- Éviter de faire dépendre le POC d’un paquet communautaire de persistance dont le support et le stockage sous-jacent seraient difficiles à garantir.

## Options envisagées

### 1. `localStorage` avec Blazored.LocalStorage

- **Stockage, photos comprises :** capacité faible et soumise aux limites du navigateur. À titre indicatif, la limite courante de `localStorage` est de l’ordre de 5 Mio par origine; les quotas exacts et leur comptabilisation varient selon les navigateurs. Les valeurs sont des chaînes : encoder une photo en Base64 augmente sa taille d’environ un tiers, avant même les coûts liés au JSON et au stockage. Cette option n’est pas adaptée aux photos ni à un historique de visites appelé à croître.
- **Requêtes :** aucune requête ni index natif. Il faut relire et désérialiser les données pour filtrer ou trier. Le tri par date et les recherches seraient à gérer en mémoire.
- **Poids ajouté au téléchargement initial :** faible, mais dépend de la version du paquet et de la publication Blazor. Le poids réellement transféré devrait être mesuré sur le build publié.
- **Maturité :** `localStorage` est une API navigateur largement disponible et stable. Blazored.LocalStorage fournit une intégration .NET pratique, mais ne modifie pas les limites du stockage sous-jacent.
- **Facilité de test :** simple à tester avec un faux stockage pour la logique unitaire; les limites de quota, les erreurs de stockage et le comportement réel nécessitent des tests dans un navigateur.
- **WebView WPF et MAUI :** généralement disponible dans les moteurs WebView modernes. Les données restent toutefois attachées à l’origine et au profil de la WebView; elles ne sont pas automatiquement partagées avec le stockage d’une autre application ou d’un autre navigateur.

### 2. IndexedDB via un petit module JavaScript appelé par JS interop

- **Stockage, photos comprises :** quota géré par le navigateur et l’appareil, généralement beaucoup plus important que celui de `localStorage`, mais sans capacité fixe garantie pour tous les navigateurs. IndexedDB peut conserver des objets `Blob` directement, sans conversion Base64. L’application doit traiter les erreurs de quota et ne pas promettre une capacité illimitée.
- **Requêtes :** prises en charge par magasins d’objets, index et curseurs. Un index sur une date permet d’ordonner les visites efficacement; les filtres peuvent s’appuyer sur des index adaptés aux besoins. Les requêtes sont moins expressives que SQL, mais suffisantes pour les parcours et l’historique envisagés dans le POC.
- **Poids ajouté au téléchargement initial :** très faible pour un module JavaScript ciblé et sans dépendance; quelques kilo-octets bruts sont plausibles. Le poids exact dépend du code livré et doit être vérifié sur la publication.
- **Maturité :** API navigateur établie et largement prise en charge. Le module d’interopérabilité reste du code à maintenir, mais l’option évite une dépendance de persistance tierce.
- **Facilité de test :** les règles métier et les interfaces de stockage peuvent être testées séparément; il faut aussi tester le module dans un vrai navigateur pour valider transactions, index, stockage des `Blob` et erreurs de quota. La frontière JS/.NET ajoute des cas d’intégration à couvrir.
- **WebView WPF et MAUI :** IndexedDB est disponible dans les moteurs WebView modernes, notamment WebView2 et les WebView mobiles. Les quotas, la persistance et la gestion du profil dépendent du moteur et de sa configuration; ils devront être vérifiés sur les appareils ciblés si une application native est développée.

### 3. EF Core avec SQLite compilé en WebAssembly via un paquet communautaire

- **Stockage, photos comprises :** une base SQLite peut stocker des données structurées et des photos en BLOB. La capacité effective dépend néanmoins du système de fichiers virtuel et du mécanisme de persistance implémentés par le paquet choisi; ce n’est pas une garantie inhérente à SQLite compilé en WebAssembly.
- **Requêtes :** SQL, index et tri par date sont bien adaptés aux visites structurées et aux futurs filtres. EF Core permet de manipuler les données depuis .NET avec un modèle familier.
- **Poids ajouté au téléchargement initial :** le plus élevé des choix. EF Core et le moteur SQLite compilé en WebAssembly ajoutent des dépendances et du code d’exécution; l’impact peut atteindre un ordre de grandeur de centaines de kilo-octets à plusieurs mégaoctets selon le paquet et la publication. Il faudrait mesurer le paquet retenu et son impact sur le chargement de la PWA.
- **Maturité :** SQLite et EF Core sont matures, mais leur combinaison dans le navigateur dépend d’un paquet communautaire, de sa maintenance, de sa compatibilité WebAssembly et de son système de fichiers virtuel. Ces éléments doivent être évalués pour un paquet précis.
- **Facilité de test :** les requêtes et le modèle EF Core sont familiers à tester. En revanche, les tests dans le navigateur doivent valider le runtime WebAssembly, le fournisseur et la persistance réelle; des tests sur SQLite natif ne suffiraient pas à prouver le fonctionnement dans la PWA.
- **WebView WPF et MAUI :** potentiellement utilisable si le moteur WebView prend en charge le runtime et le système de fichiers virtuel requis. Le fonctionnement et la persistance ne peuvent pas être déduits de la seule compatibilité SQLite ou EF Core; ils dépendent du paquet et des WebView ciblées. Une application native pourrait par ailleurs préférer un fournisseur SQLite natif distinct.

## Décision

Utiliser **IndexedDB, appelé par un petit module JavaScript via JS interop**, comme stockage local des élevages, du troupeau, des visites et des photos dans le POC.

Les photos seront enregistrées sous forme de `Blob`, séparément des données de visite, et associées par identifiant. Les visites seront indexées selon les champs nécessaires au parcours, notamment leur date, afin de permettre le tri chronologique. La couche applicative gardera l’accès au stockage derrière une interface .NET pour isoler les écrans de l’implémentation IndexedDB.

`localStorage` pourra servir à de petites préférences non critiques, mais pas aux visites, au troupeau ni aux photos. EF Core avec SQLite en WebAssembly n’est pas retenu pour le POC : son coût initial et sa dépendance à un paquet et à un système de fichiers virtuel spécifiques ne sont pas justifiés par les besoins actuels.

## Conséquences

### Positives

- Le choix convient aux données structurées, à leur indexation et au tri par date.
- Le stockage de photos sous forme de `Blob` évite l’expansion Base64 et la limite de capacité de `localStorage`.
- Le POC évite d’ajouter un moteur de base de données WebAssembly et ses dépendances au chargement initial.
- IndexedDB est disponible dans les navigateurs modernes et dans les moteurs WebView modernes, ce qui préserve une voie de réutilisation future.
- Une interface de stockage distincte permet de remplacer ou compléter l’implémentation si les besoins natifs évoluent.

### Négatives et risques

- Le quota est géré par le navigateur et n’est pas garanti. L’application doit signaler les erreurs d’écriture, notamment quand le stockage est insuffisant; elle ne doit pas présenter une sauvegarde comme réussie si elle a échoué.
- Les données restent propres à l’appareil et au profil navigateur ou WebView. Elles peuvent être perdues lors d’un effacement des données, d’un changement d’appareil ou d’une éviction par le navigateur. Le POC ne fournit ni sauvegarde distante ni synchronisation.
- Le module JavaScript et la frontière JS interop ajoutent du code et des tests d’intégration à maintenir.
- La compatibilité des WebView ne garantit pas des quotas et une persistance identiques sur WPF, Android et iOS. Une validation sur appareils cibles sera nécessaire avant une livraison native.

## Critères de réexamen

Revenir sur cette décision si l’un des constats suivants se vérifie :

- les volumes de troupeau, l’historique ou les photos dépassent de façon répétée les quotas disponibles ou provoquent des échecs d’écriture sur les appareils cibles;
- des requêtes relationnelles, des filtres avancés ou des rapports de volume rendent IndexedDB trop complexe ou trop lente;
- la synchronisation, la sauvegarde ou le partage multiappareil deviennent des exigences;
- une livraison WPF ou MAUI devient prioritaire et les essais montrent qu’IndexedDB ne garantit pas la persistance nécessaire dans les WebView retenues;
- un paquet SQLite WebAssembly maintenu et éprouvé dans les navigateurs et WebView ciblés offre un avantage mesurable, sans impact de téléchargement incompatible avec les objectifs de chargement.