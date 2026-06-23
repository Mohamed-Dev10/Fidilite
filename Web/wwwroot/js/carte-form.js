// Validation temps réel du formulaire « Nouvelle carte ».
// Le message d'erreur apparaît à la sortie d'un champ invalide et disparaît dès correction.
// La validation serveur reste la barrière finale ; ceci n'est qu'un confort de saisie.
(function () {
    "use strict";

    const form = document.querySelector(".page-body form");
    if (!form) return;

    const ARTISAN_TYPE = "3"; // RefCarteTypeId Artisan
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    const typeSelect = document.getElementById("RefCarteTypeId");
    const niaBlock = document.getElementById("niaBlock");
    const niaInput = document.getElementById("Nia");
    const niaLabel = document.getElementById("niaLabel");

    function isArtisan() {
        return typeSelect && typeSelect.value === ARTISAN_TYPE;
    }

    // Renvoie le message d'erreur du champ, ou "" s'il est valide.
    function validateField(el) {
        if (!el) return "";
        const val = (el.value || "").trim();

        switch (el.id) {
            case "Nom":    return val ? "" : "Le nom est obligatoire.";
            case "Prenom": return val ? "" : "Le prénom est obligatoire.";
            case "Gsm": {
                const digits = val.replace(/\D/g, "");
                if (!digits) return "Le téléphone est obligatoire.";
                if (digits.length !== 10)
                    return "Le téléphone doit contenir exactement 10 chiffres (actuellement " + digits.length + ").";
                if (!/^0[567]/.test(digits))
                    return "Le numéro doit commencer par 05, 06 ou 07.";
                return "";
            }
            case "Cin":
                if (!val) return "La CIN est obligatoire.";
                if (val.length !== 8) return "La CIN doit contenir exactement 8 caractères.";
                return "";
            case "Email":
                if (!val) return ""; // optionnel
                return emailRegex.test(val) ? "" : "Format d'email invalide (ex : nom@exemple.com).";
            case "DateNaissance": {
                if (!val) return "La date de naissance est obligatoire.";
                const d = new Date(val);
                if (isNaN(d.getTime())) return "Date de naissance invalide.";
                d.setHours(0, 0, 0, 0);
                const today = new Date(); today.setHours(0, 0, 0, 0);
                if (d.getTime() >= today.getTime())
                    return "La date de naissance doit être antérieure à aujourd'hui.";
                return "";
            }
            case "RefMetierId":
                return val ? "" : "Le métier est obligatoire.";
            case "RefCarteTypeId":
                return val ? "" : "Le type de carte est obligatoire.";
            case "RefMagasinId":
                return val ? "" : "Le magasin est obligatoire.";
            case "Nia":
                if (!isArtisan()) return "";
                if (!val) return "Le NIA est obligatoire pour une carte Artisan.";
                const digits = val.replace(/\D/g, "");
                if (digits.length < 18 || digits.length > 20)
                    return "Le NIA doit contenir entre 18 et 20 chiffres.";
                return "";
            default:
                return "";
        }
    }

    // Localise (ou crée) l'élément qui affiche le message d'erreur du champ.
    function errorSpan(el) {
        const name = el.getAttribute("name") || el.id;
        let span = form.querySelector('[data-valmsg-for="' + name + '"]');
        if (!span) {
            span = document.createElement("span");
            span.className = "text-danger small";
            span.setAttribute("data-valmsg-for", name);
            el.parentNode.appendChild(span);
        }
        return span;
    }

    function showError(el, msg) {
        errorSpan(el).textContent = msg;
        el.classList.toggle("is-invalid", !!msg);
    }

    const touched = new Set();

    function runValidation(el) {
        showError(el, validateField(el));
    }

    // Champs validés en continu (message affiché dès la saisie, sans attendre le blur).
    const liveFields = new Set(["Gsm", "DateNaissance"]);

    function register(el) {
        if (!el) return;
        const leaveEvent = el.tagName === "SELECT" ? "change" : "blur";
        el.addEventListener(leaveEvent, function () {
            touched.add(el.id);
            runValidation(el);
        });
        el.addEventListener("input", function () {
            // Téléphone : on ne garde que les chiffres pendant la frappe.
            if (el.id === "Gsm") {
                const onlyDigits = el.value.replace(/\D/g, "");
                if (el.value !== onlyDigits) el.value = onlyDigits;
            }
            // NIA : 20 chiffres maximum (type=number ignore maxlength → on tronque).
            if (el.id === "Nia") {
                const capped = el.value.replace(/\D/g, "").slice(0, 20);
                if (el.value !== capped) el.value = capped;
            }
            if (liveFields.has(el.id) || touched.has(el.id)) {
                touched.add(el.id);
                runValidation(el);
            }
        });
    }

    const fields = ["Nom", "Prenom", "Gsm", "Cin", "Email", "DateNaissance", "RefMetierId", "RefCarteTypeId", "RefMagasinId", "Nia"]
        .map(function (id) { return document.getElementById(id); })
        .filter(Boolean);

    fields.forEach(register);

    // La date de naissance est posée par le datepicker (champ caché) → revalider à son changement.
    const dn = document.getElementById("DateNaissance");
    if (dn) {
        dn.addEventListener("change", function () {
            touched.add("DateNaissance");
            runValidation(dn); // date = champ temps réel
        });
    }

    // Affiche/masque le NIA selon le type de carte (Artisan uniquement).
    function toggleNia() {
        const show = isArtisan();
        if (niaBlock) niaBlock.hidden = !show;
        if (niaInput) niaInput.required = show;
        if (niaLabel) niaLabel.classList.toggle("required", show);
        if (!show && niaInput) {
            niaInput.value = "";
            showError(niaInput, "");
            touched.delete("Nia");
        }
    }
    if (typeSelect) {
        typeSelect.addEventListener("change", toggleNia);
        toggleNia();
    }

    // Validation globale au moment de soumettre.
    form.addEventListener("submit", function (e) {
        let firstInvalid = null;
        fields.forEach(function (el) {
            if (el.id === "Nia" && niaBlock && niaBlock.hidden) {
                showError(el, "");
                return;
            }
            touched.add(el.id);
            const msg = validateField(el);
            showError(el, msg);
            if (msg && !firstInvalid) firstInvalid = el;
        });
        if (firstInvalid) {
            e.preventDefault();
            firstInvalid.focus();
            firstInvalid.scrollIntoView({ behavior: "smooth", block: "center" });
        }
    });
})();
