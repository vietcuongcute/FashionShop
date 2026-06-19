document.addEventListener('DOMContentLoaded', function () {
    var header = document.getElementById('mainHeader');
    var updateHeader = function () {
        if (!header) return;
        if (window.scrollY > 10) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }
    };

    updateHeader();
    window.addEventListener('scroll', updateHeader, { passive: true });

    var observer = null;
    if ('IntersectionObserver' in window) {
        observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('visible');
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.15 });

        document.querySelectorAll('.reveal, .hero-content, .hero-visual, .section-title').forEach(function (el) {
            observer.observe(el);
        });
    } else {
        document.querySelectorAll('.reveal, .hero-content, .hero-visual, .section-title').forEach(function (el) {
            el.classList.add('visible');
        });
    }

    document.querySelectorAll('.modern-alert').forEach(function (alert) {
        window.setTimeout(function () {
            alert.style.transition = 'opacity .35s ease, transform .35s ease';
            alert.style.opacity = '0';
            alert.style.transform = 'translateY(-8px)';
        }, 3600);
    });
});
