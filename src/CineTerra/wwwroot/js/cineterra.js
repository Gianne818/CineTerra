window.cineterra = window.cineterra || {
    getRect: function (element) {
        if (!element || !element.getBoundingClientRect) return null;
        var r = element.getBoundingClientRect();
        return { left: r.left, width: r.width, top: r.top, height: r.height };
    },

    // Scroll-Driven Scenery Observer for Full-Bleed Immersion
    _sceneryObserver: null,
    _sceneryScrollHandler: null,
    _sceneryResizeHandler: null,

    initScrollScenery: function (dotNetHelper) {
        return this.initSceneryScroll(dotNetHelper);
    },

    initSceneryScroll: function (dotNetHelper) {
        this.disposeScrollScenery();

        var sections = document.querySelectorAll('.scenery-scroll-section');
        var backdrops = document.querySelectorAll('.stage-backdrop, .scenery-backdrop');
        var dockItems = document.querySelectorAll('.scenery-dock-dot');

        if (!sections || sections.length === 0) return;

        var currentIndex = -1;

        var setActiveIndex = function (index, locationId) {
            if (index < 0 || index >= sections.length) return;
            if (index === currentIndex) return;
            currentIndex = index;

            backdrops.forEach(function (el, i) {
                var elLoc = el.getAttribute('data-location-id');
                var elIdx = el.getAttribute('data-backdrop-index');
                var matches = (i === index) || (elIdx !== null && parseInt(elIdx, 10) === index) || (locationId && elLoc === locationId);
                if (matches) {
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

        // Viewport focal calculation: accurately tracks which section is centered at reading level
        var ticking = false;
        var checkScroll = function () {
            var vHeight = window.innerHeight || document.documentElement.clientHeight || 800;
            var focalPoint = vHeight * 0.45;

            var bestIdx = -1;
            var minDistance = Infinity;

            for (var i = 0; i < sections.length; i++) {
                var rect = sections[i].getBoundingClientRect();
                if (rect.top <= focalPoint && rect.bottom >= focalPoint) {
                    bestIdx = i;
                    break;
                }
                var secCenter = (rect.top + rect.bottom) / 2;
                var dist = Math.abs(secCenter - focalPoint);
                if (dist < minDistance) {
                    minDistance = dist;
                    bestIdx = i;
                }
            }

            if (bestIdx >= 0 && bestIdx < sections.length) {
                var locAttr = sections[bestIdx].getAttribute('data-location-id');
                setActiveIndex(bestIdx, locAttr);
            }
        };

        var onScroll = function () {
            if (!ticking) {
                window.requestAnimationFrame(function () {
                    checkScroll();
                    ticking = false;
                });
                ticking = true;
            }
        };

        window.addEventListener('scroll', onScroll, { passive: true });
        window.addEventListener('resize', onScroll, { passive: true });
        this._sceneryScrollHandler = onScroll;
        this._sceneryResizeHandler = onScroll;

        if ('IntersectionObserver' in window) {
            var observer = new IntersectionObserver(function () {
                onScroll();
            }, {
                threshold: [0.1, 0.3, 0.5, 0.7],
                rootMargin: '-5% 0px -5% 0px'
            });

            sections.forEach(function (sec) {
                observer.observe(sec);
            });

            this._sceneryObserver = observer;
        }

        // Initialize immediately
        checkScroll();
    },

    disposeScrollScenery: function () {
        if (this._sceneryObserver) {
            this._sceneryObserver.disconnect();
            this._sceneryObserver = null;
        }
        if (this._sceneryScrollHandler) {
            window.removeEventListener('scroll', this._sceneryScrollHandler);
            this._sceneryScrollHandler = null;
        }
        if (this._sceneryResizeHandler) {
            window.removeEventListener('resize', this._sceneryResizeHandler);
            this._sceneryResizeHandler = null;
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
