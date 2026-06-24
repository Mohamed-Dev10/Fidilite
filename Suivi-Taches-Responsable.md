# BRICOMA.ECOMMERCE — Back-Office Fidélité — Suivi des tâches

_Document de suivi destiné au responsable. Mis à jour le 24/06/2026._

---

## ✅ 1. Fonctionnalités réalisées et testées

### Formulaire « Nouvelle carte »
- Validation en temps réel des champs (message affiché à la saisie, retiré dès correction).
- Champs **Raison Sociale** et **Fonction** supprimés.
- Champ **NIA** affiché uniquement pour la carte **Artisan**, placé sous « Type de carte » ; limité à **20 chiffres**.
- **CIN** : exactement 8 caractères.
- **Email** : optionnel (le GSM sert pour l'OTP/la carte).
- **Téléphone** (ex-GSM) : libellé renommé, saisie limitée aux chiffres, **exactement 10 chiffres** avec message en temps réel.
- **Date de naissance** : calendrier amélioré (sélection rapide de l'année), **date antérieure à aujourd'hui obligatoire** (aujourd'hui et futur bloqués).
- Réordonnancement : Métier avant Genre/Adresse.

### Workflow OTP
- Bouton **« Renvoyer le code »** (anti-spam 30 s).
- **Maximum 3 tentatives** de vérification.

### Dashboard
- Remplacement de « Répartition par magasin » par **« Tendance des créations (30 derniers jours) »** sous forme de graphe moderne.
- KPI **filtrés par magasin** pour les responsables de magasin (vue globale réservée à l'administrateur).

### Paramétrage des types de cartes
- Création/édition de **nouveaux types de cartes**.
- **Message de réception** paramétrable (envoyé au client après création).
- **Image-modèle** de la carte : upload + aperçu, avec **dimensions imposées (2305 × 1427 px)** et refus (message + pop-up) si l'image ne correspond pas.
- **Position du code-barres** réglable par glisser-déposer, + **réglage de la taille (zoom)**.
- Possibilité de **supprimer / changer** l'image.

### Menu et navigation
- **Menu latéral dynamique** : un élément par type de carte ; un nouveau type apparaît automatiquement.

### Page de connexion
- Intégration des couleurs **BRICOMA (bleu + rouge)**.

### Création de carte (cœur du métier) — testée de bout en bout
- Saisie → OTP WhatsApp → vérification → enregistrement dans les **2 bases (FIDELITE + MARKET)** → génération du **code-barres EAN-13** + image de carte → envoi WhatsApp.

### Alignement des comptes utilisateurs sur la table `Profil`
- Le nom, prénom et magasin des utilisateurs sont désormais gérés via la table **`Profil`** (conforme à la version existante), au lieu de colonnes ajoutées sur `AspNetUsers`.
- A **corrigé un bug** : le magasin ne s'affichait pas pour tous les utilisateurs ; c'est désormais correct pour tous.

---

## 🟠 2. En attente de votre décision / validation

| Sujet | Situation actuelle | Décision attendue |
|---|---|---|
| **Codes `code_pri` des nouveaux types de carte (MARKET)** | Code temporaire attribué automatiquement (`Id + 10`) pour éviter les collisions | Communiquer les **codes officiels** par type |
| **Statut unique « Confirmé »** | Un client n'est créé qu'après validation de l'OTP → un seul statut « Confirmé » (plus de « Créé »/« Clôturé » séparés) | **Valider** cette logique ou demander la réintroduction des statuts |
| **Rendu visuel des cartes des nouveaux types** | Génération fonctionnelle | **Validation visuelle** |

---

## 🔜 3. À venir / reste à faire

- **Bloquer / Débloquer une carte** : dernière brique fonctionnelle, à développer (les champs nécessaires existent déjà en base).
- **WhatsApp en production** : le mode actuel (sandbox de test) ne convient pas à la production. Nécessite un **numéro WhatsApp Business approuvé** + des **modèles de message validés par Meta** (démarche côté compte, indépendante du code applicatif).

---

## ⚙️ 4. Points techniques (base de données & déploiement)

- **Nouvelle table** `REF_CarteTypeParametrage` (paramétrage des types) — script de création fourni.
- **Scripts SQL livrés** (dossier `Scripts/`), à exécuter lors du déploiement sur la base cible :
  1. `001_REF_CarteTypeParametrage.sql` — crée la table de paramétrage.
  2. `002_Backfill_Profil_From_AspNetUsers.sql` — recopie nom/prénom/magasin vers `Profil`.
  3. `003_Drop_Redundant_User_Role_Columns.sql` — supprime les colonnes en double.
- **Migrations de base à harmoniser** entre l'environnement de travail et la base cible (à vérifier avant la fusion finale du code).
- Tout le développement et les tests ont été faits sur l'environnement de **test** ; la bascule vers les bases officielles reste à finaliser.

---

_Statut global : développement quasi finalisé. Reste principalement « Bloquer/Débloquer », la configuration WhatsApp de production, et vos validations ci-dessus._
