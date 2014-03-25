;;; noise.scm -- CLM -> Snd/Scheme translation of noise.ins

;; Translator/Author: Michael Scholz <scholz-micha@gmx.de>
;; Last: Wed Apr 02 02:47:21 CEST 2003
;; Version: $Revision: 1.9 $

;;; Comments not otherwise noted are taken from noise.ins!

;; Included functions:
;; (attack-point duration attack decay (total-x 100.0))
;; (fm-noise ...)
;; (make-fm-noise len freq ...)

;;; The "noise" instrument (useful for Oceanic Music):

(provide 'snd-noise.scm)
(if (not (provided? 'snd-ws.scm)) (load "ws.scm"))
(if (not (provided? 'snd-env.scm)) (load "env.scm"))

(define *locsig-type* mus-interp-sinusoidal)

(define* (attack-point duration attack decay (total-x 100.0))
  (* total-x (/ (if (= 0.0 attack)
		    (if (= 0.0 decay)
			(/ duration 4)
			(/ (- duration decay) 4))
		    attack)
		duration)))

(definstrument (fm-noise startime dur freq0 amp ampfun ampat ampdc
		   freq1 glissfun freqat freqdc rfreq0 rfreq1 rfreqfun rfreqat rfreqdc
		   dev0 dev1 devfun devat devdc
		   (degree 0.0)
		   (distance 1.0)
		   (reverb-amount 0.005))
  
  ;; ampat = amp envelope attack time, and so on -- this instrument
  ;; assumes your envelopes go from 0 to 100 on the x-axis, and that
  ;; the "attack" portion ends at 25, the "decay" portion starts at
  ;; 75.  "rfreq" is the frequency of the random number generator --
  ;; if below about 25 hz you get automatic composition, above that
  ;; you start to get noise.  well, you get a different kind of noise.
  ;; "dev" is the bandwidth of the noise -- very narrow gives a
  ;; whistle, very broad more of a whoosh.  this is basically "simple
  ;; fm", but the modulating signal is white noise.
  
  (let* ((beg (seconds->samples startime))
	 (end (+ beg (seconds->samples dur)))
	 (carrier (make-oscil freq0))
	 (modulator (make-rand :frequency rfreq0 :amplitude 1.0))
	 (loc (make-locsig :degree degree 
			   :distance distance
			   :reverb reverb-amount
			   :type *locsig-type*))
	 (dev-0 (hz->radians dev0))
	 
	 ;; next fix-up troubles in attack and decay times (there are
	 ;; lots of ways to handle this -- the basic problem is that
	 ;; these durned instruments end up having way too many
	 ;; parameters.  rick taube's common music replacement for pla
	 ;; should help, but just for old time's sake, we'll do it the
	 ;; way the ancients did it.  (we could also package up this
	 ;; stuff in our own function, somewhat like the allvln
	 ;; function in vln.clm, leaving the instrument code to apply
	 ;; envelopes and other data to some patch).
	 
	 (amp-attack (attack-point dur ampat ampdc))
	 (amp-decay (- 100.0 (attack-point dur ampdc ampat)))
	 (freq-attack (attack-point dur freqat freqdc))
	 (freq-decay (- 100.0 (attack-point dur freqdc freqat)))
	 (dev-attack (attack-point dur devat devdc))
	 (dev-decay (- 100.0 (attack-point dur devdc devat)))
	 (rfreq-attack (attack-point dur rfreqat rfreqdc))
	 (rfreq-decay (- 100.0 (attack-point dur rfreqdc rfreqat)))
	 
	 ;; now make the actual envelopes -- these all assume we are
	 ;; thinking in terms of the "value when the envelope is 1"
	 ;; (i.e. dev1 and friends), and the "value when the envelope
	 ;; is 0" (i.e. dev0 and friends) -- over the years this
	 ;; seemed to make beginners happier than various other ways
	 ;; of describing the y-axis behaviour of the envelope.  all
	 ;; this boiler-plate for envelopes might seem overly
	 ;; elaborate when our basic instrument is really simple, but
	 ;; in most cases, and this one in particular, nearly all the
	 ;; musical interest comes from the envelopes, not the
	 ;; somewhat dull spectrum generated by the basic patch.
	 
	 (dev-f (make-env (stretch-envelope devfun 25 dev-attack 75 dev-decay)
			  :duration dur :scaler (hz->radians (- dev1 dev0))))
	 (amp-f (make-env (stretch-envelope ampfun 25 amp-attack 75 amp-decay)
			  :duration dur :scaler amp))
	 (freq-f (make-env (stretch-envelope glissfun 25 freq-attack 75 freq-decay)
			   :duration dur :scaler (hz->radians (- freq1 freq0))))
	 (rfreq-f (make-env (stretch-envelope rfreqfun 25 rfreq-attack 75 rfreq-decay)
			    :duration dur :scaler (hz->radians (- rfreq1 rfreq0)))))
    (run
     (do ((i beg (+ 1 i)))
	 ((= i end))
       (locsig loc i (* (env amp-f)
			(oscil carrier (+ (env freq-f)
					  (* (+ dev-0 (env dev-f)) 
					     (rand modulator (env rfreq-f)))))))))))

;; (let* ((ofile "test.snd")
;;        (snd (find-sound ofile)))
;;   (if snd
;;       (close-sound snd))
;;   (with-sound (:output ofile :play 1 :statistics #t)
;; 	      (fm-noise 0 2.0 500 0.25 '(0 0 25 1 75 1 100 0) 0.1 0.1
;; 			1000 '(0 0 100 1) 0.1 0.1
;; 			10 1000 '(0 0 100 1) 0 0
;; 			100 500 '(0 0 100 1) 0 0)))

;;; And here is a generator-like instrument, see make-fm-violin in
;;; fmv.scm. [MS]

(define* (make-fm-noise len freq
			(amp 0.25)
			(ampfun '(0 0 25 1 75 1 100 0))
			(ampat 0.1)
			(ampdc 0.1)
			(freq1 1000)
			(glissfun '(0 0 100 1))
			(freqat 0.1)
			(freqdc 0.1)
			(rfreq0 10)
			(rfreq1 1000)
			(rfreqfun '(0 0 100 1))
			(rfreqat 0)
			(rfreqdc 0)
			(dev0 100)
			(dev1 500)
			(devfun '(0 0 100 1))
			(devat 0)
			(devdc 0)
			(degree (random 90.0))
			(distance 1.0)
			(reverb-amount 0.005))
  (let* ((dur (/ len (floor (srate))))
	 (amp-attack (attack-point dur ampat ampdc))
	 (amp-decay (- 100.0 (attack-point dur ampdc ampat)))
	 (freq-attack (attack-point dur freqat freqdc))
	 (freq-decay (- 100.0 (attack-point dur freqdc freqat)))
	 (dev-attack (attack-point dur devat devdc))
	 (dev-decay (- 100.0 (attack-point dur devdc devat)))
	 (rfreq-attack (attack-point dur rfreqat rfreqdc))
	 (rfreq-decay (- 100.0 (attack-point dur rfreqdc rfreqat)))
	 (dev-ff (make-env (stretch-envelope devfun 25 dev-attack 75 dev-decay)
			   :duration dur :scaler (hz->radians (- dev1 dev0))))
	 (amp-ff (make-env (stretch-envelope ampfun 25 amp-attack 75 amp-decay)
			   :duration dur :scaler amp))
	 (freq-ff (make-env (stretch-envelope glissfun 25 freq-attack 75 freq-decay)
			    :duration dur :scaler (hz->radians (- freq1 freq))))
	 (rfreq-ff (make-env (stretch-envelope rfreqfun 25 rfreq-attack 75 rfreq-decay)
			     :duration dur :scaler (hz->radians (- rfreq1 rfreq0))))
	 (carrier (make-oscil freq))
	 (modulator (make-rand :frequency rfreq0 :amplitude 1.0))
	 (dev-0 (hz->radians dev0))
	 (dev-f (lambda () (env dev-ff)))
	 (amp-f (lambda () (env amp-ff)))
	 (freq-f (lambda () (env freq-ff)))
	 (rfreq-f (lambda () (env rfreq-ff))))
    (lambda ()
      (* (amp-f) (oscil carrier (+ (freq-f) (* (+ dev-0 (dev-f)) (rand modulator (rfreq-f)))))))))

;; (let* ((beg 0)
;;        (dur 9.8)
;;        (len (+ beg (floor (* dur (srate)))))
;;        (chns 4)
;;        (outfile "test.snd")
;;        (snd (find-sound outfile))
;;        (loc (make-locsig :degree (random 3535.0) :channels chns))
;;        (data (vct-map! (make-vct len) (make-fm-noise len 500))))
;;   (if snd
;;       (close-sound snd))
;;   (set! snd (new-sound outfile mus-next mus-bshort (mus-srate) chns))
;;   (do ((i 0 (+ 1 i)))
;;       ((= i chns))
;;     (mix-vct (vct-scale! (vct-copy data) (locsig-ref loc i)) beg snd i #f))
;;   (let* ((beg (floor (* 10 (srate))))
;; 	 (len (+ beg (floor (* dur (srate)))))
;; 	 (loc (make-locsig :degree (random 3535.0) :channels chns))
;; 	 (data (vct-map! (make-vct len) (make-fm-noise len 200))))
;;     (do ((i 0 (+ 1 i)))
;; 	((= i chns))
;;       (mix-vct (vct-scale! (vct-copy data) (locsig-ref loc i)) beg snd i #f))
;;     (play snd 0)))

;; noise.scm ends here
