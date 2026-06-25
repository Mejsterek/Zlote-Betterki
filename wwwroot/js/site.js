document.addEventListener("DOMContentLoaded", () => {
    const hash = window.location.hash;
    if (!hash) {
        return;
    }

    const target = document.querySelector(hash);
    if (target) {
        target.scrollIntoView({ behavior: "smooth", block: "start" });
    }
});
