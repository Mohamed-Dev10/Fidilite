/* ============================================================
   BRICOMA — Filtrage + pagination côté client (temps réel)
   Attendu dans la page :
     #searchInput  (texte)        — optionnel
     #statutFilter (select)       — optionnel
     #resetBtn     (bouton)       — optionnel
     #listBody     (tbody)        — contient les <tr class="data-row"
                                     data-search="..." data-statut="...">
     #emptyRow     (tr message vide)
     #totalCount   (span total)   — optionnel
     #pager        (div pagination)
     #pageInfo     (texte info page) — optionnel
   ============================================================ */
function bricoListFilter(opts) {
    opts = opts || {};
    var perPage = opts.perPage || 6;
    var label = opts.label || "élément(s)";

    var body = document.getElementById('listBody');
    if (!body) return;

    var rows = Array.prototype.slice.call(body.querySelectorAll('tr.data-row'));
    var searchInput = document.getElementById('searchInput');
    var statutSelect = document.getElementById('statutFilter');
    var resetBtn = document.getElementById('resetBtn');
    var emptyRow = document.getElementById('emptyRow');
    var totalCount = document.getElementById('totalCount');
    var pager = document.getElementById('pager');
    var pageInfo = document.getElementById('pageInfo');

    var filtered = rows.slice();
    var current = 1;

    function applyFilter() {
        var q = (searchInput && searchInput.value || '').trim().toLowerCase();
        var st = (statutSelect && statutSelect.value) || '';
        filtered = rows.filter(function (r) {
            var okText = !q || (r.dataset.search || '').indexOf(q) !== -1;
            var okSt = !st || r.dataset.statut === st;
            return okText && okSt;
        });
        current = 1;
        render();
    }

    function render() {
        var total = filtered.length;
        var pages = Math.max(1, Math.ceil(total / perPage));
        if (current > pages) current = pages;

        rows.forEach(function (r) { r.style.display = 'none'; });
        var start = (current - 1) * perPage;
        filtered.slice(start, start + perPage).forEach(function (r) { r.style.display = ''; });

        if (emptyRow) emptyRow.style.display = total ? 'none' : '';
        if (totalCount) totalCount.textContent = total;
        if (pageInfo) {
            pageInfo.textContent = total
                ? ((start + 1) + ' – ' + Math.min(start + perPage, total) + ' sur ' + total + ' ' + label)
                : '';
        }
        renderPager(pages);
    }

    function renderPager(pages) {
        if (!pager) return;
        if (pages <= 1) { pager.innerHTML = ''; return; }

        var html = '';
        html += '<button class="pg-btn" data-pg="' + (current - 1) + '"' + (current === 1 ? ' disabled' : '') + '><i class="ti ti-chevron-left"></i></button>';

        var from = Math.max(1, current - 2);
        var to = Math.min(pages, current + 2);
        if (from > 1) {
            html += '<button class="pg-btn" data-pg="1">1</button>';
            if (from > 2) html += '<span class="px-1 text-muted">…</span>';
        }
        for (var i = from; i <= to; i++) {
            html += '<button class="pg-btn ' + (i === current ? 'active' : '') + '" data-pg="' + i + '">' + i + '</button>';
        }
        if (to < pages) {
            if (to < pages - 1) html += '<span class="px-1 text-muted">…</span>';
            html += '<button class="pg-btn" data-pg="' + pages + '">' + pages + '</button>';
        }

        html += '<button class="pg-btn" data-pg="' + (current + 1) + '"' + (current === pages ? ' disabled' : '') + '><i class="ti ti-chevron-right"></i></button>';
        pager.innerHTML = html;

        Array.prototype.forEach.call(pager.querySelectorAll('.pg-btn'), function (b) {
            if (b.disabled) return;
            b.addEventListener('click', function () {
                var p = parseInt(b.getAttribute('data-pg'), 10);
                if (p >= 1 && p <= pages) { current = p; render(); window.scrollTo({ top: 0, behavior: 'smooth' }); }
            });
        });
    }

    if (searchInput) searchInput.addEventListener('input', applyFilter);
    if (statutSelect) statutSelect.addEventListener('change', applyFilter);
    if (resetBtn) resetBtn.addEventListener('click', function (e) {
        e.preventDefault();
        if (searchInput) searchInput.value = '';
        if (statutSelect) statutSelect.value = '';
        applyFilter();
    });

    applyFilter();
}
