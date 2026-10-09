# TournéeVéto — contexte produit

## Problème

Un vétérinaire laitier prépare et documente chaque visite à partir d’informations réparties entre tournée, dossier d’élevage et notes de terrain; cela peut entraîner ressaisie et oublis.
En bâtiment, l’accès réseau peut être absent : les informations et les actions prévues doivent rester utilisables hors ligne et disponibles au retour au bureau.
Hypothèse à mesurer au Québec : préparer puis mettre au propre une visite représente de l’ordre de 15 à 30 minutes par élevage (**à vérifier**).

## Personas

| Persona | Objectif | Frustration | Contexte d’usage |
|---|---|---|---|
| Vétérinaire laitier | Préparer sa tournée, consulter l’historique utile et consigner les actions et constats. | Informations dispersées, ressaisie et réseau peu fiable dans les étables. | Prépare au bureau; consulte et saisit sur ordinateur ou appareil mobile en élevage, parfois sans réseau. |
| Producteur laitier | Comprendre les constats et repartir avec des actions claires pour son troupeau. | Recommandations difficiles à retrouver ou à relier aux animaux concernés. | Échange avec le vétérinaire pendant la visite et consulte le rapport après celle-ci. |
| Personnel d’élevage | Savoir quelles actions réaliser sur quelles vaches et en assurer le suivi. | Consignes imprécises, oubliées ou transmises oralement sans trace commune. | Travaille dans les installations, avec un accès variable aux appareils et au réseau. |

## Proposition de valeur et parcours clés

**Proposition de valeur :** préparer, réaliser et documenter une visite laitière dans une même application navigateur, utilisable hors ligne et sans compte.

1. **Préparer la tournée :** sélectionner les élevages du jour, consulter leur fiche et repérer les éléments à traiter.
2. **Réaliser la visite :** consulter et compléter la grille de régie pour les vaches, puis le bilan de biosécurité, y compris sans réseau.
3. **Clore et transmettre :** rédiger le rapport de visite, l’enregistrer localement et l’exporter en PDF.

## Hors périmètre du POC

- Backend, comptes, authentification, synchronisation réseau ou partage multiutilisateur; les données restent sur l’appareil.
- Intégrations aux logiciels de troupeau, aux systèmes d’identification animale, aux laboratoires ou aux registres officiels.
- Diagnostic automatisé, prescriptions, calculs ou alertes cliniques validés, et règles métier exhaustives.
- Livraison d’applications natives WPF ou MAUI. Le POC est une PWA navigateur publiée sur GitHub Pages; la réutilisation future des écrans dans ces applications est une contrainte de conception, pas un livrable.

## Glossaire

- **Régie :** pratiques de gestion du troupeau et interventions planifiées; dans l’application, la grille regroupe les actions à faire sur des vaches.
- **Vêlage :** mise bas d’une vache.
- **Tarissement :** arrêt de la traite avant le vêlage, pendant la période de repos mammaire.
- **Insémination :** dépôt de semence dans l’appareil reproducteur de la vache en vue de la fécondation.
- **Diagnostic de gestation :** examen visant à déterminer si une vache est gestante.
- **CCS :** comptage des cellules somatiques du lait, indicateur notamment utilisé dans le suivi de la santé mammaire.
- **Biosécurité :** mesures destinées à prévenir l’introduction et la propagation d’agents infectieux dans un élevage.
- **Élevage :** exploitation et troupeau laitier concernés par une visite.
- **Visite :** intervention du vétérinaire dans un élevage, comprenant sa préparation, les constats, les actions et le rapport.

## Hypothèses du POC

Les données présentées sont **fictives** et les règles métier sont **simplifiées**; elles ne constituent ni un dossier médical réel, ni une recommandation clinique ou réglementaire. La durée indiquée dans le problème est une hypothèse à vérifier auprès de vétérinaires au Québec.