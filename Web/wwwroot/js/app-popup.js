/* ============================================================
   BRICOMA — Popup modal centré (style SweetAlert), 100% maison
   Usage : bricoPopup('success' | 'danger' | 'warning' | 'info', 'message', 'Titre optionnel')
   ============================================================ */
(function () {
    var ICONS = {
        success: 'ti-check',
        danger:  'ti-x',
        warning: 'ti-alert-triangle',
        info:    'ti-info-circle'
    };
    var TITLES = {
        success: 'Succès',
        danger:  'Erreur',
        warning: 'Attention',
        info:    'Information'
    };

    var queue = [];
    var active = false;
    var escHandler = null;

    function close(overlay) {
        if (overlay.classList.contains('is-leaving')) return;
        overlay.classList.add('is-leaving');
        var card = overlay.querySelector('.bpop');
        if (card) card.classList.add('is-leaving');
        if (escHandler) { document.removeEventListener('keydown', escHandler); escHandler = null; }
        overlay.addEventListener('animationend', function () {
            if (overlay.parentNode) overlay.parentNode.removeChild(overlay);
            active = false;
            next();
        });
    }

    function show(item) {
        active = true;
        var type = ICONS[item.type] ? item.type : 'info';

        var overlay = document.createElement('div');
        overlay.className = 'bpop-overlay';
        overlay.innerHTML =
            '<div class="bpop bpop-' + type + '" role="alertdialog" aria-modal="true">' +
                '<div class="bpop-ic"><i class="ti ' + ICONS[type] + '"></i></div>' +
                '<div class="bpop-title">' + (item.title || TITLES[type]) + '</div>' +
                '<div class="bpop-msg">' + item.message + '</div>' +
                '<button type="button" class="bpop-btn">OK</button>' +
            '</div>';

        document.body.appendChild(overlay);

        var btn = overlay.querySelector('.bpop-btn');
        btn.addEventListener('click', function () { close(overlay); });
        // Clic sur le fond grisé = fermer
        overlay.addEventListener('mousedown', function (e) { if (e.target === overlay) close(overlay); });
        // Touche Échap = fermer
        escHandler = function (e) { if (e.key === 'Escape') close(overlay); };
        document.addEventListener('keydown', escHandler);

        setTimeout(function () { btn.focus(); }, 50);
    }

    function next() {
        if (active) return;
        var item = queue.shift();
        if (item) show(item);
    }

    window.bricoPopup = function (type, message, title) {
        if (!message) return;
        queue.push({ type: type, message: message, title: title });
        next();
    };

    // Affiche les messages serveur déposés par le partial _Toasts
    document.addEventListener('DOMContentLoaded', function () {
        (window.__bricoToasts || []).forEach(function (t) {
            window.bricoPopup(t.type, t.message, t.title);
        });
    });
})();
