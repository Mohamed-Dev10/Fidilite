// Datepicker léger (vanilla) thématisé BRICOMA.
// Usage markup :
//   <div class="dp" data-dp data-max="today" [data-exclude-today]>
//     <input type="hidden" name="..." value="yyyy-MM-dd" />
//     <button type="button" class="dp-trigger ..."><span class="dp-value">jj/mm/aaaa</span></button>
//     <div class="dp-pop" hidden></div>
//   </div>
// data-exclude-today : aujourd'hui (et le futur) est désactivé → sélection strictement antérieure.
// Astuce : cliquer sur le titre (mois année) ouvre la sélection rapide de l'année.
(function () {
    const MONTHS = ['Janvier', 'Février', 'Mars', 'Avril', 'Mai', 'Juin',
        'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'];
    const DOW = ['Lu', 'Ma', 'Me', 'Je', 'Ve', 'Sa', 'Di'];
    const YEARS_PER_PAGE = 12;

    function pad(n) { return n < 10 ? '0' + n : '' + n; }
    function fmtIso(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function fmtDisplay(d) { return pad(d.getDate()) + '/' + pad(d.getMonth() + 1) + '/' + d.getFullYear(); }
    function parseIso(s) {
        if (!s) return null;
        const p = s.split('-');
        if (p.length !== 3) return null;
        const d = new Date(+p[0], +p[1] - 1, +p[2]);
        return isNaN(d.getTime()) ? null : d;
    }
    function startOfDay(d) { const x = new Date(d); x.setHours(0, 0, 0, 0); return x; }

    function init(root) {
        const hidden = root.querySelector('input[type=hidden]');
        const trigger = root.querySelector('.dp-trigger');
        const valueEl = root.querySelector('.dp-value');
        const pop = root.querySelector('.dp-pop');
        if (!hidden || !trigger || !valueEl || !pop) return;

        const maxAttr = root.getAttribute('data-max');
        let maxDate = maxAttr === 'today' ? startOfDay(new Date()) : (parseIso(maxAttr) ? startOfDay(parseIso(maxAttr)) : null);

        // Exclure aujourd'hui : la borne max recule d'un jour (today devient désactivé).
        if (root.hasAttribute('data-exclude-today')) {
            const yest = startOfDay(new Date());
            yest.setDate(yest.getDate() - 1);
            maxDate = maxDate ? new Date(Math.min(maxDate.getTime(), yest.getTime())) : yest;
        }

        let selected = parseIso(hidden.value);
        let view = selected ? new Date(selected) : new Date();
        view.setDate(1);

        let mode = 'days';        // 'days' | 'years'
        let yearPageStart = 0;    // première année de la page en mode 'years'

        if (selected) {
            valueEl.textContent = fmtDisplay(selected);
            valueEl.classList.remove('is-placeholder');
        }

        function renderDays() {
            const y = view.getFullYear(), m = view.getMonth();
            const startDow = (new Date(y, m, 1).getDay() + 6) % 7; // Lundi = 0
            const daysInMonth = new Date(y, m + 1, 0).getDate();
            const today = startOfDay(new Date());
            const selKey = selected ? startOfDay(selected).getTime() : null;

            let html = '<div class="dp-head">'
                + '<button type="button" class="dp-nav" data-nav="-12" title="Année précédente"><i class="ti ti-chevrons-left"></i></button>'
                + '<button type="button" class="dp-nav" data-nav="-1" title="Mois précédent"><i class="ti ti-chevron-left"></i></button>'
                + '<button type="button" class="dp-title dp-title-btn" data-open-years title="Choisir l\'année">' + MONTHS[m] + ' ' + y + ' <i class="ti ti-chevron-down"></i></button>'
                + '<button type="button" class="dp-nav" data-nav="1" title="Mois suivant"><i class="ti ti-chevron-right"></i></button>'
                + '<button type="button" class="dp-nav" data-nav="12" title="Année suivante"><i class="ti ti-chevrons-right"></i></button>'
                + '</div>';
            html += '<div class="dp-grid dp-dow">' + DOW.map(d => '<span>' + d + '</span>').join('') + '</div>';
            html += '<div class="dp-grid dp-days">';
            for (let i = 0; i < startDow; i++) html += '<span class="dp-empty"></span>';
            for (let d = 1; d <= daysInMonth; d++) {
                const cur = startOfDay(new Date(y, m, d));
                const disabled = maxDate && cur.getTime() > maxDate.getTime();
                let cls = 'dp-day';
                if (selKey !== null && cur.getTime() === selKey) cls += ' is-selected';
                if (cur.getTime() === today.getTime()) cls += ' is-today';
                html += '<button type="button" class="' + cls + '"' + (disabled ? ' disabled' : '') + ' data-day="' + d + '">' + d + '</button>';
            }
            html += '</div>';
            pop.innerHTML = html;
        }

        function renderYears() {
            const maxYear = maxDate ? maxDate.getFullYear() : 9999;
            const curY = view.getFullYear();
            const start = yearPageStart;
            const end = start + YEARS_PER_PAGE - 1;

            let html = '<div class="dp-head">'
                + '<button type="button" class="dp-nav" data-yearnav="-' + YEARS_PER_PAGE + '" title="Années précédentes"><i class="ti ti-chevron-left"></i></button>'
                + '<button type="button" class="dp-title dp-title-btn" data-open-days title="Retour au calendrier">' + start + ' – ' + end + '</button>'
                + '<button type="button" class="dp-nav" data-yearnav="' + YEARS_PER_PAGE + '" title="Années suivantes"><i class="ti ti-chevron-right"></i></button>'
                + '</div>';
            html += '<div class="dp-grid dp-years">';
            for (let yy = start; yy <= end; yy++) {
                const disabled = yy > maxYear;
                let cls = 'dp-year';
                if (yy === curY) cls += ' is-selected';
                html += '<button type="button" class="' + cls + '"' + (disabled ? ' disabled' : '') + ' data-year="' + yy + '">' + yy + '</button>';
            }
            html += '</div>';
            pop.innerHTML = html;
        }

        function render() { mode === 'years' ? renderYears() : renderDays(); }

        function open() { mode = 'days'; render(); pop.hidden = false; root.classList.add('is-open'); document.addEventListener('mousedown', onDoc, true); }
        function close() { pop.hidden = true; root.classList.remove('is-open'); document.removeEventListener('mousedown', onDoc, true); }
        function onDoc(e) { if (!root.contains(e.target)) close(); }

        trigger.addEventListener('click', function (e) {
            e.preventDefault();
            if (pop.hidden) open(); else close();
        });

        pop.addEventListener('click', function (e) {
            // Navigation mois / année (mode jours)
            const nav = e.target.closest('[data-nav]');
            if (nav) {
                view.setMonth(view.getMonth() + parseInt(nav.getAttribute('data-nav'), 10));
                render();
                return;
            }
            // Ouvrir la sélection d'année
            if (e.target.closest('[data-open-years]')) {
                mode = 'years';
                yearPageStart = view.getFullYear() - Math.floor(YEARS_PER_PAGE / 2) + 1;
                render();
                return;
            }
            // Revenir au calendrier
            if (e.target.closest('[data-open-days]')) {
                mode = 'days';
                render();
                return;
            }
            // Pagination des années
            const ynav = e.target.closest('[data-yearnav]');
            if (ynav) {
                yearPageStart += parseInt(ynav.getAttribute('data-yearnav'), 10);
                render();
                return;
            }
            // Choix d'une année → retour au calendrier de cette année
            const yearBtn = e.target.closest('[data-year]');
            if (yearBtn && !yearBtn.disabled) {
                view.setFullYear(parseInt(yearBtn.getAttribute('data-year'), 10));
                mode = 'days';
                render();
                return;
            }
            // Choix d'un jour
            const day = e.target.closest('[data-day]');
            if (day && !day.disabled) {
                selected = new Date(view.getFullYear(), view.getMonth(), parseInt(day.getAttribute('data-day'), 10));
                hidden.value = fmtIso(selected);
                valueEl.textContent = fmtDisplay(selected);
                valueEl.classList.remove('is-placeholder');
                hidden.dispatchEvent(new Event('change', { bubbles: true }));
                close();
            }
        });
    }

    function boot() { document.querySelectorAll('[data-dp]').forEach(init); }
    if (document.readyState !== 'loading') boot();
    else document.addEventListener('DOMContentLoaded', boot);
})();
