window.cineterra = window.cineterra || {
    getRect: function (element) {
        if (!element || !element.getBoundingClientRect) return null;
        var r = element.getBoundingClientRect();
        return { left: r.left, width: r.width, top: r.top, height: r.height };
    },

    // Scroll-Driven Scenery Observer for Full-Bleed Immersion
    _sceneryObserver: null,
    
    initScrollScenery: function (dotNetHelper) {
        return this.initSceneryScroll(dotNetHelper);
    },

    initSceneryScroll: function (dotNetHelper) {
        this.disposeScrollScenery();

        var sections = document.querySelectorAll('.scenery-scroll-section');
        var backdrops = document.querySelectorAll('.stage-backdrop, .scenery-backdrop');
        var dockItems = document.querySelectorAll('.scenery-dock-dot');

        if (!sections || sections.length === 0) return;

        var setActiveIndex = function (index, locationId) {
            backdrops.forEach(function (el, i) {
                var elLoc = el.getAttribute('data-location-id');
                if (i === index || (locationId && elLoc === locationId)) {
                    el.classList.add('stage-backdrop--active', 'scenery-backdrop--active');
                } else {
                    el.classList.remove('stage-backdrop--active', 'scenery-backdrop--active');
                }
            });
            dockItems.forEach(function (el, i) {
                if (i === index) {
                    el.classList.add('scenery-dock-dot--active');
                } else {
                    el.classList.remove('scenery-dock-dot--active');
                }
            });
            sections.forEach(function (el, i) {
                if (i === index) {
                    el.classList.add('scenery-section--active');
                } else {
                    el.classList.remove('scenery-section--active');
                }
            });
            if (dotNetHelper) {
                try {
                    dotNetHelper.invokeMethodAsync('OnChapterChanged', index);
                } catch (e) {}
                try {
                    if (locationId) {
                        dotNetHelper.invokeMethodAsync('OnScenerySectionChanged', locationId);
                    }
                } catch (e) {}
            }
        };

        if ('IntersectionObserver' in window) {
            var observer = new IntersectionObserver(function (entries) {
                entries.forEach(function (entry) {
                    if (entry.isIntersecting) {
                        var idxAttr = entry.target.getAttribute('data-chapter-index') || entry.target.getAttribute('data-scenery-index');
                        var locAttr = entry.target.getAttribute('data-location-id');
                        var idx = parseInt(idxAttr, 10);
                        if (!isNaN(idx)) {
                            setActiveIndex(idx, locAttr);
                        } else if (locAttr) {
                            var secIdx = Array.prototype.indexOf.call(sections, entry.target);
                            setActiveIndex(secIdx, locAttr);
                        }
                    }
                });
            }, {
                threshold: [0.35, 0.5, 0.7],
                rootMargin: '-10% 0px -10% 0px'
            });

            sections.forEach(function (sec) {
                observer.observe(sec);
            });

            this._sceneryObserver = observer;
        }
    },

    disposeScrollScenery: function () {
        if (this._sceneryObserver) {
            this._sceneryObserver.disconnect();
            this._sceneryObserver = null;
        }
    },

    jumpToChapter: function (index) {
        var el = document.getElementById('scenery-chapter-' + index);
        if (el) {
            el.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    },

    // Ambient Soundscape Synthesizer via Web Audio API (Zero external assets needed)
    _audioCtx: null,
    _gainNode: null,
    _ambientPlaying: false,

    toggleAmbientSound: function () {
        if (this._ambientPlaying) {
            this.stopAmbientSound();
            return false;
        } else {
            this.startAmbientSound();
            return true;
        }
    },

    startAmbientSound: function () {
        try {
            var AudioContext = window.AudioContext || window.webkitAudioContext;
            if (!AudioContext) return false;

            if (!this._audioCtx) {
                this._audioCtx = new AudioContext();
            }

            if (this._audioCtx.state === 'suspended') {
                this._audioCtx.resume();
            }

            // Master Gain
            this._gainNode = this._audioCtx.createGain();
            this._gainNode.gain.setValueAtTime(0.01, this._audioCtx.currentTime);
            this._gainNode.gain.exponentialRampToValueAtTime(0.10, this._audioCtx.currentTime + 2.5);
            this._gainNode.connect(this._audioCtx.destination);

            // Pink noise / wind generator buffer
            var bufferSize = this._audioCtx.sampleRate * 2;
            var noiseBuffer = this._audioCtx.createBuffer(1, bufferSize, this._audioCtx.sampleRate);
            var output = noiseBuffer.getChannelData(0);
            var b0 = 0, b1 = 0, b2 = 0, b3 = 0, b4 = 0, b5 = 0, b6 = 0;
            for (var i = 0; i < bufferSize; i++) {
                var white = Math.random() * 2 - 1;
                b0 = 0.99886 * b0 + white * 0.0555179;
                b1 = 0.99332 * b1 + white * 0.0750759;
                b2 = 0.96900 * b2 + white * 0.1538520;
                b3 = 0.86650 * b3 + white * 0.3104856;
                b4 = 0.55000 * b4 + white * 0.5329522;
                b5 = -0.7616 * b5 - white * 0.0168980;
                output[i] = (b0 + b1 + b2 + b3 + b4 + b5 + b6 + white * 0.5362) * 0.05;
                b6 = white * 0.115926;
            }

            var whiteNoise = this._audioCtx.createBufferSource();
            whiteNoise.buffer = noiseBuffer;
            whiteNoise.loop = true;

            // Lowpass filter for smooth distant wind effect
            var filter = this._audioCtx.createBiquadFilter();
            filter.type = 'lowpass';
            filter.frequency.setValueAtTime(240, this._audioCtx.currentTime);

            // Subtle LFO for gentle wind swells
            var lfo = this._audioCtx.createOscillator();
            lfo.frequency.setValueAtTime(0.15, this._audioCtx.currentTime);
            var lfoGain = this._audioCtx.createGain();
            lfoGain.gain.setValueAtTime(120, this._audioCtx.currentTime);
            lfo.connect(lfoGain);
            lfoGain.connect(filter.frequency);
            lfo.start();

            whiteNoise.connect(filter);
            filter.connect(this._gainNode);
            whiteNoise.start();

            // Subtle harmonic warm tone (calm cinematic ambient drone)
            var drone = this._audioCtx.createOscillator();
            drone.type = 'sine';
            drone.frequency.setValueAtTime(110, this._audioCtx.currentTime); // A2
            var droneGain = this._audioCtx.createGain();
            droneGain.gain.setValueAtTime(0.03, this._audioCtx.currentTime);
            drone.connect(droneGain);
            droneGain.connect(this._gainNode);
            drone.start();

            this._ambientPlaying = true;
            this._noiseSource = whiteNoise;
            this._droneSource = drone;
            return true;
        } catch (e) {
            console.warn("Audio Context init error:", e);
            return false;
        }
    },

    stopAmbientSound: function () {
        if (!this._ambientPlaying || !this._gainNode || !this._audioCtx) return;
        try {
            var currTime = this._audioCtx.currentTime;
            this._gainNode.gain.setValueAtTime(this._gainNode.gain.value, currTime);
            this._gainNode.gain.exponentialRampToValueAtTime(0.0001, currTime + 1.2);
            var self = this;
            setTimeout(function () {
                if (self._noiseSource) try { self._noiseSource.stop(); } catch(e){}
                if (self._droneSource) try { self._droneSource.stop(); } catch(e){}
                self._ambientPlaying = false;
            }, 1250);
        } catch (e) {
            this._ambientPlaying = false;
        }
    },

    isAmbientPlaying: function () {
        return this._ambientPlaying;
    },

    scrollToElement: function (elementId) {
        var el = document.getElementById(elementId);
        if (el) {
            el.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }
};
