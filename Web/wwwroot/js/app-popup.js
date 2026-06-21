/* ============================================================
   BRICOMA — Popup modal centré (style SweetAlert), 100% maison
   Usage info    : bricoPopup('success' | 'danger' | 'warning' | 'info', 'message', 'Titre optionnel')
   Usage confirm : bricoConfirm({ message, title, type, confirmText, cancelText, onConfirm })
   Confirm auto  : <form data-confirm="Message ?" data-confirm-title="..." data-confirm-ok="Supprimer">
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

        var buttons = item.confirm
            ? '<div class="bpop-actions">' +
                  '<button type="button" class="bpop-btn-cancel">' + (item.cancelText || 'Annuler') + '</button>' +
                  '<button type="button" class="bpop-btn">' + (item.confirmText || 'Confirmer') + '</button>' +
              '</div>'
            : '<button type="button" class="bpop-btn">OK</button>';

        overlay.innerHTML =
            '<div class="bpop bpop-' + type + '" role="alertdialog" aria-modal="true">' +
                '<div class="bpop-ic"><i class="ti ' + ICONS[type] + '"></i></div>' +
                '<div class="bpop-title">' + (item.title || TITLES[type]) + '</div>' +
                '<div class="bpop-msg">' + item.message + '</div>' +
                buttons +
            '</div>';

        document.body.appendChild(overlay);

        var okBtn = overlay.querySelector('.bpop-btn');
        var cancelBtn = overlay.querySelector('.bpop-btn-cancel');

        function dismiss(confirmed) {
            close(overlay);
            if (item.confirm && confirmed && typeof item.onConfirm === 'function') item.onConfirm();
        }

        okBtn.addEventListener('click', function () { dismiss(true); });
        if (cancelBtn) cancelBtn.addEventListener('click', function () { dismiss(false); });
        // Clic sur le fond grisé = annuler / fermer
        overlay.addEventListener('mousedown', function (e) { if (e.target === overlay) dismiss(false); });
        // Touche Échap = annuler / fermer
        escHandler = function (e) { if (e.key === 'Escape') dismiss(false); };
        document.addEventListener('keydown', escHandler);

        setTimeout(function () { (cancelBtn || okBtn).focus(); }, 50);
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

    window.bricoConfirm = function (opts) {
        opts = opts || {};
        queue.push({
            confirm: true,
            type: opts.type || 'warning',
            message: opts.message || 'Confirmer cette action ?',
            title: opts.title,
            confirmText: opts.confirmText,
            cancelText: opts.cancelText,
            onConfirm: opts.onConfirm
        });
        next();
    };

    // Intercepte les formulaires marqués data-confirm pour afficher le popup de confirmation
    document.addEventListener('submit', function (e) {
        var form = e.target;
        if (!form || !form.dataset || !form.dataset.confirm || form.__bricoConfirmed) return;
        e.preventDefault();
        window.bricoConfirm({
            message: form.dataset.confirm,
            title: form.dataset.confirmTitle,
            confirmText: form.dataset.confirmOk,
            cancelText: form.dataset.confirmCancel,
            type: form.dataset.confirmType || 'danger',
            onConfirm: function () { form.__bricoConfirmed = true; form.submit(); }
        });
    }, true);

    // Affiche les messages serveur déposés par le partial _Toasts
    document.addEventListener('DOMContentLoaded', function () {
        (window.__bricoToasts || []).forEach(function (t) {
            window.bricoPopup(t.type, t.message, t.title);
        });
    });
})();
