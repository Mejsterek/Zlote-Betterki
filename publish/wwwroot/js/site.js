document.addEventListener("DOMContentLoaded", () => {
    const videos = document.querySelectorAll("video");

    videos.forEach(video => {
        try {
            const AudioContext = window.AudioContext || window.webkitAudioContext;
    const audioCtx = new AudioContext();
    const source = audioCtx.createMediaElementSource(video);
    const gainNode = audioCtx.createGain();

    gainNode.gain.value = 2;

    source.connect(gainNode).connect(audioCtx.destination);

            video.addEventListener("play", () => {
                if (audioCtx.state === "suspended") {
        audioCtx.resume();
                }
            });
        } catch (e) {
        console.warn("Nie udało się podgłośnić klipu:", e);
        }
    });
});
