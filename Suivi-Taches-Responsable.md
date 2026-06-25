# BRICOMA.ECOMMERCE — Back-Office Fidélité — Suivi des tâches

_Document de suivi destiné au responsable. Mis à jour le 25/06/2026._

---

## ✅ Tâches réalisées et testées

| N° | Tâche | Description |
|---|---|---|
| 1 | Validation temps réel du formulaire « Nouvelle carte » | Chaque champ du formulaire de création de carte affiche un message d'erreur en temps réel à la saisie (ex : nombre de chiffres insuffisant) et le retire automatiquement dès que la correction est faite. La validation serveur reste en miroir comme barrière finale. |
| 2 | Suppression des champs Raison Sociale et Fonction | Les champs « Raison Sociale » et « Fonction » ont été retirés du formulaire de création de carte car ils ne sont plus nécessaires. |
| 3 | Champ Téléphone (ex-GSM) | Le libellé « GSM » a été renommé en « Téléphone ». La saisie est filtrée aux chiffres uniquement, limitée à exactement 10 chiffres, avec validation du préfixe (05, 06 ou 07). Un message en temps réel indique le nombre de chiffres actuellement saisi. |
| 4 | Champ CIN — validation 8 caractères | Le champ CIN est limité à exactement 8 caractères avec maxlength et un message d'erreur en temps réel si le nombre est différent de 8. |
| 5 | Champ NIA — Artisan uniquement, max 20 chiffres | Le champ NIA n'apparaît que lorsque le type de carte sélectionné est « Artisan ». Il est placé sous « Type de carte » et limité à 20 chiffres maximum. Pour les autres types, le champ est masqué et vidé. |
| 6 | Champ Email — optionnel avec validation format | Le champ Email n'est pas obligatoire. S'il est rempli, le format email est vérifié en temps réel (ex : nom@exemple.com). |
| 7 | Date de naissance — calendrier avec sélection rapide de l'année | Le calendrier personnalisé permet de choisir rapidement l'année via un clic sur le titre du mois (affiche une grille d'années paginée). La date doit être strictement antérieure à aujourd'hui (aujourd'hui et les dates futures sont grisés et non sélectionnables). |
| 8 | Réordonnancement des champs du formulaire | L'ordre des champs a été modifié : le champ « Métier » est maintenant placé avant « Genre » et « Adresse » pour plus de cohérence. |
| 9 | Bouton « Renvoyer le code » OTP | Un bouton « Renvoyer le code » a été ajouté sur la page de vérification OTP. Il permet de générer et envoyer un nouveau code par WhatsApp. Un anti-spam de 30 secondes empêche les clics répétitifs. |
| 10 | Maximum 3 tentatives de vérification OTP | Après 3 tentatives erronées de saisie du code OTP, celui-ci est invalidé. Le client doit demander un renvoi du code pour réessayer. |
| 11 | Dashboard — KPI « Vue d'ensemble » | Le dashboard affiche 4 KPI principaux : Total des cartes, Cartes créées ce mois, Cartes actives, et Cartes bloquées. Chaque KPI est présenté dans une carte teintée avec une icône. |
| 12 | Dashboard — KPI par type de carte | Le dashboard affiche un KPI par type de carte (M3alem, Artisan, Gold, etc.) avec le nombre de cartes. Chaque KPI est cliquable et redirige vers la liste filtrée du type correspondant. |
| 13 | Dashboard — Tendance des créations (30 jours) | La section « Répartition par magasin » a été remplacée par un graphe « Tendance des créations (30 derniers jours) » sous forme d'un graphe en aire avec dégradé, points sur les jours avec créations, et étiquettes de dates. |
| 14 | Dashboard — Scoping par magasin | Les KPI du dashboard sont filtrés automatiquement selon le magasin de l'utilisateur connecté. L'administrateur (SUPER_ADMIN) voit la vue globale tous magasins. Un utilisateur sans magasin voit un message d'alerte. |
| 15 | Paramétrage — Création de nouveaux types de cartes | Une page permet de créer de nouveaux types de cartes (ex : Carte Gold, Carte Jeune). Le type est enregistré en base et apparaît automatiquement dans le menu et les formulaires. |
| 16 | Paramétrage — Message de réception paramétrable | Pour chaque type de carte, un message de réception peut être saisi. Ce texte est envoyé au client par WhatsApp après la création de sa carte (le code et le code-barres sont ajoutés automatiquement à la suite). Si aucun message n'est défini, un message par défaut est utilisé. |
| 17 | Paramétrage — Upload de l'image-modèle | L'utilisateur peut uploader une image-modèle (PNG ou JPG) pour chaque type de carte. Cette image sert de fond pour la carte générée. Les dimensions sont imposées à exactement 2305 × 1427 px. Si l'image ne correspond pas, un pop-up affiche les dimensions attendues et le fichier est refusé. |
| 18 | Paramétrage — Position du code-barres par drag & drop | Sur l'aperçu de l'image-modèle, un faux code-barres de démonstration peut être déplacé par glisser-déposer (souris et tactile) ou par clic. La position est sauvegardée en pourcentage (X%, Y%) et utilisée à la génération de la carte client. |
| 19 | Paramétrage — Zoom de la taille du code-barres | Un curseur (slider) permet de régler la taille du code-barres de 50% à 200% de la taille de base. Le faux code-barres se redimensionne en temps réel. La valeur est sauvegardée et appliquée à la génération. |
| 20 | Paramétrage — Suppression et remplacement de l'image | Un bouton ✕ (cercle rouge en coin haut-droit, en dehors de l'image) permet de supprimer l'image-modèle. Le fichier physique est supprimé du serveur à l'enregistrement. L'utilisateur peut ensuite charger une nouvelle image. |
| 21 | Paramétrage — Suppression d'un type de carte | La suppression d'un type de carte nettoie automatiquement le paramétrage associé (table REF_CarteTypeParametrage) avant de supprimer le type, pour éviter une erreur de clé étrangère. |
| 22 | Menu latéral dynamique par type de carte | Les éléments « Cartes M3alem » et « Cartes Artisan » figés dans le menu ont été remplacés par une boucle dynamique sur tous les types de cartes gérés (AMIBRICOMA exclu). Un nouveau type paramétré apparaît automatiquement dans le menu avec une couleur attribuée. |
| 23 | Suppression du bouton « Nouvelle carte » des pages de listes | Le bouton « Nouvelle carte » a été retiré des pages de listes (Artisan, M3alem, et tous types) pour éviter la confusion. La création de carte reste accessible uniquement via le menu latéral « Nouvelle carte ». |
| 24 | Mise en surbrillance dynamique du menu | L'élément actif dans le menu latéral est mis en surbrillance automatiquement selon la page consultée (type de carte sélectionné ou page « Nouvelle carte »). |
| 25 | Page de connexion — Dégradé bleu/rouge BRICOMA | Le panneau gauche de la page de connexion utilise un dégradé stylé allant du bleu (indigo) au rouge BRICOMA, avec des reflets flous pour la profondeur. Le bouton « Se connecter » est en rouge BRICOMA. |
| 26 | Logo BRICOMA dans le menu latéral | Le logo BRICOMA a été ajouté en haut du menu latéral (coin haut-gauche) avec une ombre blanche autour du contour (drop-shadow) pour assurer la lisibilité du sous-texte « Tout pour tout faire » sur le fond sombre. |
| 27 | Logo BRICOMA dans la page de connexion | Le logo BRICOMA a été ajouté dans le panneau gauche de la page de connexion, devant le nom « BRICOMA », avec la même ombre blanche pour la lisibilité. |
| 28 | Création de carte — Flux complet end-to-end | Le flux complet de création de carte a été développé et testé de bout en bout : saisie du formulaire → envoi du code OTP par WhatsApp → vérification du code par le responsable → enregistrement du client dans les 2 bases de données (FIDELITE + MARKET) → génération du code-barres EAN-13 → composition de l'image de carte (fond paramétré + code-barres à la position/taille configurée) → envoi de la carte par WhatsApp au client. |
| 29 | Gestion automatique du code_pri MARKET (nouveaux types) | Pour les nouveaux types de carte (Id ≥ 4), un code_pri unique est attribué automatiquement (Id + 10) dans la table MARKET, afin d'éviter la collision avec les codes existants (03=M3alem, 04=Amibricoma, 10=Artisan). Ce code est temporaire en attendant les codes officiels du responsable. |
| 30 | Système de rôles et permissions | Un système complet de gestion des rôles et permissions a été mis en place : création/modification/suppression de rôles, assignation de permissions granulaires à chaque rôle, et affectation d'un rôle à chaque utilisateur. Les permissions contrôlent l'accès aux fonctionnalités (création de carte, liste, paramétrage, gestion des utilisateurs, etc.). |
| 31 | Scoping par magasin pour les utilisateurs | Chaque utilisateur est rattaché à un magasin. Un responsable de magasin ne voit que les cartes et les statistiques de son propre magasin (dashboard, listes). L'administrateur (SUPER_ADMIN) a accès à la vue globale et peut filtrer librement par magasin. |
| 32 | Gestion des utilisateurs (CRUD) | Page complète de gestion des utilisateurs : création avec email/mot de passe/rôle/magasin, modification (nom, prénom, email, rôle, magasin, mot de passe), suppression (avec protection contre l'auto-suppression), et suspension/réactivation. La liste affiche l'email, le magasin, le rôle et le statut de chaque utilisateur. |
| 33 | Alignement des données utilisateurs sur la table Profil | Les données utilisateurs (nom, prénom, magasin) ont été basculées de la table AspNetUsers vers la table Profil (table officielle du projet, relation 1:1 avec l'utilisateur). Un script de migration des données existantes a été exécuté. Les colonnes redondantes sur AspNetUsers et AspNetRoles ont été supprimées. Ce refactoring a corrigé un bug : le magasin ne s'affichait pas pour tous les utilisateurs dans la liste de gestion. |

---

## 🟠 En attente de votre décision / validation

| N° | Sujet | Situation actuelle | Décision attendue |
|---|---|---|---|
| 34 | Codes code_pri des nouveaux types (MARKET) | Code temporaire automatique (Id + 10) pour éviter les collisions | Communiquer les codes officiels par type de carte |
| 35 | Statut unique « Confirmé » | Le client n'est créé qu'après validation de l'OTP → un seul statut « Confirmé » | Valider cette logique ou demander la réintroduction des statuts Créé/Clôturé |
| 36 | Rendu visuel des cartes des nouveaux types | La génération est fonctionnelle avec le modèle paramétré | Validation visuelle du rendu des cartes |

---

## 🔧 En cours de développement

| N° | Tâche | Description |
|---|---|---|
| 37 | Formulaire « Modifier client » | Aligner le formulaire de modification d'un client sur celui de création : mêmes champs (Téléphone, CIN, NIA, Date de naissance, Email, Métier, etc.) et mêmes validations en temps réel (10 chiffres, 8 caractères, date < aujourd'hui, 20 chiffres max, format email). |
| 38 | Message WhatsApp après création | Restructurer le message envoyé au client après la création de sa carte : message paramétré du type de carte + « Merci de votre confiance. » + nom du magasin où la carte est créée, avec l'image de la carte en pièce jointe (un seul message WhatsApp). |
| 39 | Dashboard — Amélioration du KPI tendance | Améliorer le rendu visuel du graphe de tendance des créations sur 30 jours (selon les retours du responsable). |

---

## 🔜 À venir

| N° | Tâche | Description |
|---|---|---|
| 40 | Bloquer / Débloquer une carte | Fonctionnalité permettant de bloquer ou débloquer la carte d'un client. Les champs nécessaires existent déjà en base de données (IsActif, RemarqueDeactivation, etc.). En attente de confirmation des informations nécessaires par le responsable. |
| 41 | WhatsApp en production | Le mode actuel (sandbox de test Twilio) ne convient pas à la production. Le passage en production nécessite un numéro WhatsApp Business approuvé et des modèles de message validés par Meta. C'est une démarche côté compte Twilio/Meta, indépendante du code applicatif (le code est déjà prêt). |

---

## ⚙️ Points techniques (base de données & déploiement)

| N° | Élément | Description |
|---|---|---|
| 42 | Table REF_CarteTypeParametrage | Nouvelle table créée pour le paramétrage des types de cartes (message de réception, image-modèle, position X/Y du code-barres en %, taille du code-barres en %). Script de création fourni (001). |
| 43 | Colonne BarcodeScale | Nouvelle colonne ajoutée sur REF_CarteTypeParametrage pour stocker la taille du code-barres en pourcentage (défaut 100%). |
| 44 | Script 001 — Création table paramétrage | Script SQL idempotent (IF NOT EXISTS) qui crée la table REF_CarteTypeParametrage avec ses colonnes, contraintes et valeurs par défaut. |
| 45 | Script 002 — Backfill Profil | Script SQL idempotent qui recopie les données nom/prénom/magasin depuis AspNetUsers vers la table Profil pour les utilisateurs existants, afin de ne perdre aucune donnée lors de la bascule. |
| 46 | Script 003 — Suppression colonnes redondantes | Script SQL guardé (IF EXISTS) qui supprime les colonnes en double : AspNetUsers.Nom/Prenom/RefMagasinId et AspNetRoles.Description/CreatedAt. Safe sur les deux bases (staging et ECOMMERCE). |
| 47 | Migrations EF livrées | 4 migrations Entity Framework : AddPermissionsAndRoles (tables Permissions + RolePermissions), AddOtpVerification (table OtpVerifications), RenameOtpIsVerifiedToIsUsed, AddUserProfileFields. |
| 48 | Harmonisation migrations avec base cible | Les migrations et scripts doivent être vérifiés et harmonisés entre l'environnement de travail (staging) et la base cible (BRICOMA.ECOMMERCE) avant la fusion finale du code. |
| 49 | Bascule vers les bases officielles | Tout le développement et les tests ont été faits sur l'environnement de test (staging SRV-PRP). La bascule vers les bases officielles (SRV-reporting / SRV-REPORTS) reste à finaliser. |

---

_Statut global : 33 tâches réalisées et testées. 3 en cours de développement. 2 à venir (dont « Bloquer/Débloquer » en attente de vos précisions). 3 sujets en attente de votre validation._
