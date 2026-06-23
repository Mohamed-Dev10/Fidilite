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
            case "Gsm":    return val ? "" : "Le numéro GSM est obligatoire.";
            case "Cin":
                if (!val) return "La CIN est obligatoire.";
                if (val.length !== 8) return "La CIN doit contenir exactement 8 caractères.";
                return "";
            case "Email":
                if (!val) return ""; // optionnel
                return emailRegex.test(val) ? "" : "Format d'email invalide (ex : nom@exemple.com).";
            case "DateNaissance":
                return val ? "" : "La date de naissance est obligatoire.";
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

    function register(el) {
        if (!el) return;
        const leaveEvent = el.tagName === "SELECT" ? "change" : "blur";
        el.addEventListener(leaveEvent, function () {
            touched.add(el.id);
            runValidation(el);
        });
        el.addEventListener("input", function () {
            if (touched.has(el.id)) runValidation(el);
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
            if (touched.has("DateNaissance")) runValidation(dn);
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
