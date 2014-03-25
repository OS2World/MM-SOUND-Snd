\ extensions.fs -- extensions.scm -> extensions.fs

\ Translator/Author: Michael Scholz <mi-scholz@users.sourceforge.net>
\ Created: Sun Dec 18 19:21:00 CET 2005
\ Changed: Thu Mar 03 15:29:03 CET 2011

\ Commentary:
\
\ With comments and doc strings from extensions.scm.
\
\ Snd-7 compatibility
\ color-map constants
\ mus-a0, set-mus-a0, etc
\ back-or-forth-graph 		  ( count -- lst )
\ forward-graph       		  ( count -- lst )
\ backward-graph      		  ( count -- lst )
\ back-or-forth-mix   		  ( count snd chn -- mx )
\ forward-mix         		  ( count snd chn -- mx )
\ backward-mix        		  ( count snd chn -- mx )
\ back-or-forth-mark  		  ( count snd chn -- mk )
\ forward-mark        		  ( count snd chn -- mk )
\ backward-mark       		  ( count snd chn -- mk )
\ mus-bank            		  ( gens amps in1 in2 -- val )
\ oscil-bank          		  ( amps gens in1 in2 -- val )
\ after-save-as-hook-replace-sound ( snd fname from-dialog -- )
\ emacs-style-save-as             ( -- f )
\ set-emacs-style-save-as         ( f -- )
\
\ Snd-8 compatibility
\ samples->sound-data             ( :optional beg len snd chn sd edpos sd-chan -- sd )
\ open-sound-file                 ( :key ... -- fd )
\ close-sound-file                ( fd bytes -- n )
\ vct->sound-file                 ( fd v samps -- n )
\
\ remove-sound-property           ( key :optional snd -- alist )
\ set-sound-property-save-state-ignore   ( key snd -- alist )
\ remove-channel-property         ( key :optional snd chn -- alist )
\ set-channel-property-save-state-ignore ( key snd chn -- alist )
\ channel-sync                    ( snd chn -- val )
\ set-channel-sync                ( snd chn val -- )
\
\ normalize-mix                   ( filename beg in-chn snd chn -- scl )
\ enveloped-mix                   ( filename beg env -- )
\ map-sound-files                 ( func :optional dir -- lst )
\ for-each-sound-file             ( func :optional dir -- )
\ match-sound-files               ( func :optional dir -- ary )
\ selection-members               ( -- array-of-arrays )
\ make-selection                  ( :optional beg end snd chn -- )
\ delete-selection-and-smooth     ( -- )
\ eval-over-selection             ( func -- val )
\
\ yes-or-no?                      ( question action-if-yes action-if-no snd -- )
\
\ mix-channel                     ( file-data :optional beg dur snd chn edpos -- val )
\ insert-channel                  ( file-data :optional beg 0 dur snd chn edpos -- val )
\ redo-channel                    ( :optional edits snd chn -- )
\ undo-channel                    ( :optional edits snd chn -- )
\ any-env-channel                 ( en func :optional beg dur snd chn edpos origin -- val )
\ sine-ramp                       ( rmp0 rmp1 :optional beg dur snd chn edpos -- val )
\ sine-env-channel                ( en :optional beg dur snd chn edpos -- val )
\ blackman4-ramp                  ( rmp0 rmp1 :optional beg dur snd chn edpos -- val )
\ blackman4-env-channel           ( en :optional beg dur snd chn edpos -- val )
\ ramp-squared                    ( rmp0 rmp1 :optional symmetric beg dur snd chn edpos -- val )
\ env-squared-channel             ( en :optional symmetric beg dur snd chn edpos -- val )
\ ramp-expt                       ( rmp0 rmp1 exponent ... )
\ env-expt-channel                ( en exponent ... )
\ offset-channel                  ( amount :optional beg dur snd chn edpos -- val )
\ offset-sound                    ( offset :optional beg dur snd -- )
\ pad-sound                       ( beg dur :optional snd -- )
\ dither-channel                  ( :optional amount beg dur snd chn edpos -- val )
\ dither-sound                    ( :optional amount beg dur snd -- )
\ contrast-channel                ( index :optional beg dur snd chn edpos -- val )
\ contrast-sound                  ( index :optional beg dur snd -- )
\ scale-sound                     ( scl :optional beg dur snd -- )
\ normalize-sound                 ( amp :optional beg dur snd -- )
\ 
\ channels=                       ( snd1 chn1 snd2 chn2 :optional allowable-difference -- f )
\ channels-equal?                 ( snd1 chn1 snd2 chn2 :optional allowable-difference -- f )
\ mono->stereo                    ( new-name snd1 chn1 snd2 chn2 -- snd )
\ mono-files->stereo              ( new-name chan1-name chan2-name -- snd )
\ stereo->mono                    ( orig-snd chan1-name chan2-name -- snd0 snd1 )
\
\ with-reopen-menu     		  ( -- )
\ with-buffers-menu    		  ( -- )
\ 
\ if-cursor-follows-play-it-stays-where-play-stopped ( :optional enable -- )

require clm
require examp

\ === Snd-7 compatibility stuff (snd7.scm) ===
: mus-a0     ( gen     -- val ) 0          mus-xcoeff ;
: set-mus-a0 ( gen val -- val ) 0 swap set-mus-xcoeff ;
: mus-a1     ( gen     -- val ) 1          mus-xcoeff ;
: set-mus-a1 ( gen val -- val ) 1 swap set-mus-xcoeff ;
: mus-a2     ( gen     -- val ) 2          mus-xcoeff ;
: set-mus-a2 ( gen val -- val ) 2 swap set-mus-xcoeff ;
: mus-b1     ( gen     -- val ) 1          mus-ycoeff ;
: set-mus-b1 ( gen val -- val ) 1 swap set-mus-ycoeff ;
: mus-b2     ( gen     -- val ) 2          mus-ycoeff ;
: set-mus-b2 ( gen val -- val ) 2 swap set-mus-ycoeff ;

: back-or-forth-graph ( count -- lst )
  { count }
  sounds if
    #f snd-snd { cursnd }
    #f snd-chn { curchn }
    0 { curpos }
    0 { pos }
    #() { sndlst }
    sounds each { snd }
      snd channels 0 ?do
	cursnd snd =
	curchn i = && if pos to curpos then
	pos 1+ to pos
	sndlst #( snd i ) array-push drop
      loop
    end-each
    curpos count + sndlst object-length mod { newpos }
    sndlst newpos array-ref { vals }
    vals 0 array-ref    set-selected-sound   drop
    vals 1 array-ref #f set-selected-channel drop
    #( selected-sound #f selected-channel )
  else
    nil
  then
;

: forward-graph  ( count -- lst )        back-or-forth-graph ;
: backward-graph ( count -- lst ) negate back-or-forth-graph ;

hide
: sort-mix-pos ( a b -- n )
  { a b }
  a mix-position { am }
  b mix-position { bm }
  am bm < if
    1
  else am bm > if
      -1
    else
      0
    then
  then
;
set-current

: back-or-forth-mix <{ count :optional snd #f chn #f -- mx|#f }>
  snd chn mixes { mx }
  count 0<> mx nil? not && if
    mx length 1 = if
      mx 0 array-ref mix-position snd chn #f set-cursor	drop
      mx 0 array-ref				\ retval
    else
      mx <'> sort-mix-pos object-sort { sorted-mx }
      snd chn #f cursor { pos }
      count 0> if -1 else 0 then { curpos }
      pos sorted-mx 0 array-ref mix-position >= if
	sorted-mx each { m }
	  count 0>
	  pos m mix-position < &&
	  count 0<
	  pos m mix-position <= && || if
	    leave
	  else
	    curpos 1+ to curpos
	  then
	end-each
      then
      curpos count + mx length mod to curpos
      sorted-mx curpos object-ref { val }
      val mix-position snd chn #f set-cursor drop
      val				\ retval
    then
  else
    #f
  then
;
previous

: forward-mix  <{ count :optional snd #f chn #f -- mx|#f }>
  count        snd snd-snd chn snd-chn back-or-forth-mix
;

: backward-mix <{ count :optional snd #f chn #f -- mx|#f }>
  count negate snd snd-snd chn snd-chn back-or-forth-mix
;

hide
: sort-mark-sample ( a b -- n )
  { a b }
  a #f mark-sample { am }
  b #f mark-sample { bm }
  am bm < if
    1
  else am bm > if
      -1
    else
      0
    then
  then
;
set-current

: back-or-forth-mark <{ count :optional snd #f chn #f -- mk|#f }>
  snd chn #f marks { mk }
  count 0<>
  mk empty? not && if
    mk length 1 = if
      mk 0 array-ref #f mark-sample snd chn #f set-cursor drop
      mk 0 array-ref				\ retval
    else
      mk <'> sort-mark-sample object-sort { sorted-mk }
      snd chn #f cursor { pos }
      count 0> if -1 else 0 then { curpos }
      pos sorted-mk 0 array-ref #f mark-sample >= if
	sorted-mk each { m }
	  count 0>
	  pos m #f mark-sample < &&
	  count 0<
	  pos m #f mark-sample <= && || if
	    leave
	  else
	    curpos 1+ to curpos
	  then
	end-each
      then
      curpos count + mk length mod to curpos
      sorted-mk curpos object-ref { val }
      val mark-sample snd chn #f set-cursor
      val				\ retval
    then
  else
    #f
  then
;
previous

: forward-mark  <{ count :optional snd #f chn #f -- mk|#f }>
  count        snd snd-snd chn snd-chn back-or-forth-mark
;

: backward-mark <{ count :optional snd #f chn #f -- mk|#f }>
  count negate snd snd-snd chn snd-chn back-or-forth-mark
;

: mus-bank ( gens amps in1 in2 -- sum )
  { gens amps in1 in2 }
  0.0					\ sum
  gens each ( gen ) in1 i object-ref in2 i object-ref mus-run amps i vct-ref f* ( sum ) f+ end-each
;

: oscil-bank ( amps gens in1 in2 -- sum )
  { amps gens in1 in2 }
  0.0					\ sum
  gens each ( gen ) in1 i object-ref in2 i object-ref oscil amps i vct-ref f* ( sum ) f+ end-each
;

: after-save-as-hook-replace-sound ( snd fname from-dialog -- )
   { snd fname from-dialog }
  from-dialog if
    snd revert-sound drop
    snd close-sound drop
    fname open-sound drop
  then
;

: emacs-style-save-as ( -- f )
  after-save-as-hook <'> after-save-as-hook-replace-sound hook-member?
;

: set-emacs-style-save-as ( f -- )
  if
    emacs-style-save-as unless
      after-save-as-hook <'> after-save-as-hook-replace-sound add-hook!
    then
  else
    after-save-as-hook <'> after-save-as-hook-replace-sound remove-hook! drop
  then
;
\ #t set-emacs-style-save-as ==> installs
\ #f set-emacs-style-save-as ==> deinstalls

\ === Snd-8 compatibility stuff (snd8.scm) ===

: samples->sound-data <{ :optional beg 0 len #f snd #f chn #f sd #f edpos #f sd-chan 0 -- sd }>
  beg len snd chn edpos channel->vct
  sd sound-data? if
    sd
  else
    1
    len integer? if
      len
    else
      snd chn #f frames
    then
    make-sound-data
  then
  sd-chan
  vct->sound-data
;

: open-sound-file <{ :key
     file        "test.snd"
     channels    1
     srate       22050
     comment     ""
     header-type mus-next -- fd }>
  file srate channels mus-lfloat header-type comment mus-sound-open-output
;

<'> mus-sound-close-output alias close-sound-file ( fd bytes -- n )

: vct->sound-file <{ fd v samps -- n }>
  fd 0 samps 1- 1 v vct->sound-data mus-sound-write
;

\ === End of compatibility stuff ===

\ ;;; -------- sound-property

: remove-sound-property <{ key :optional snd #f -- alist }>
  doc" Removes key-value pair in the given sound's property list.  \
Returns altered list."
  snd sound-properties key array-assoc-remove!
;

: set-sound-property-save-state-ignore <{ key :optional snd #f -- alist }>
  \ 'save-state-ignore key snd set-sound-property
  'save-state-ignore snd sound-property { ign }
  ign array? if ign key array-push else #( 'save-state-ignore key ) then to ign
  'save-state-ignore ign snd set-sound-property
;

\ ;;; -------- channel-property

: remove-channel-property <{ key :optional snd #f chn #f -- alist }>
  doc" Removes key-value pair in the given channel's property list.  \
Returns altered list."
  snd chn channel-properties key array-assoc-remove!
;

: set-channel-property-save-state-ignore <{ key :optional snd #f chn #f -- alist }>
  'save-state-ignore snd chn channel-property { ign }
  ign array? if ign key array-push else #( 'save-state-ignore key ) then to ign
  'save-state-ignore ign snd chn set-channel-property
;

: channel-sync     { snd chn -- val } 'sync     snd chn     channel-property ;
: set-channel-sync { snd chn val -- } 'sync val snd chn set-channel-property ;

\ ;;; -------- mix with result at original peak amp

: normalize-mix ( filename beg in-chn snd chn -- scl )
  doc" It is like mix but the mix result has same peak amp as unmixed SND/CHN (returns scaler)."
  { filename beg in-chan snd chn }
  snd chn #f maxamp { original-maxamp }
  filename beg in-chan snd chn undef undef undef mix drop
  snd chn #f maxamp { new-maxamp }
  original-maxamp new-maxamp f<> if
    original-maxamp new-maxamp f/ { scaler }
    snd sync { old-sync }
    0 snd set-sync drop
    scaler snd chn scale-by drop
    old-sync snd set-sync drop
    scaler
  else
    1.0
  then
;

\ ;;;-------- mix with envelope on mixed-in file
\ ;;;
\ ;;; there are lots of ways to do this; this version uses functions from Snd, CLM, and Sndlib.

: enveloped-mix ( filename beg env -- )
  doc" Mixes FILENAME starting at BEG with amplitude envelope ENV.\n\
\"pistol.snd\" 0 #( 0 0 1 1 2 0 ) enveloped-mix"
  { filename beg env }
  filename mus-sound-frames { len }
  temp-dir empty? if "" else temp-dir then "tmp.snd" $+ { tmp-name }
  tmp-name 22050 1 mus-bshort mus-next "" mus-sound-open-output 0 mus-sound-close-output drop
  #( #( :envelope env :length len make-env ) ) { envs }
  1 1.0 make-mixer { mx }
  tmp-name filename 0 len 0 mx envs mus-mix drop
  tmp-name beg undef undef undef undef undef undef mix drop
  tmp-name file-delete
;

\ ;;; -------- map-sound-files, match-sound-files
\ ;;;
\ ;;; apply a function to each sound in dir
\ ;;;
\ ;;;   (map-sound-files (lambda (n) (if (> (mus-sound-duration n) 10.0) (snd-print n))))

: map-sound-files <{ func :optional dir "." -- lst }>
  doc" Applies FUNC to each sound file in DIR."
  dir sound-files-in-directory map func #( *key* ) run-proc end-map
;
\ lambda: <{ n -- str|#f }> n mus-sound-duration 10.0 f> if n snd-print cr then ; map-sound-files

: for-each-sound-file <{ func :optional dir "." -- }>
  doc" Applies FUNC to each sound file in DIR."
  dir sound-files-in-directory each { f } func #( f ) run-proc drop end-each
;
0 [if]
"/home/bil/sf" value loop-path
lambda: <{ n -- }>
  loop-path "/" $+ n $+ <'> mus-sound-loop-info #t nil fth-catch if
    stack-reset
    exit
  then
  empty? unless n snd-print drop cr then
; loop-path for-each-sound-file
[then]

: match-sound-files <{ func :optional dir "." -- ary }>
  doc" Applies FUNC to each sound file in DIR and returns an array of files \
for which FUNC does not return #f."
  #() { matches }
  dir sound-files-in-directory each { f }
    func #( f ) run-proc if matches f array-push drop then
  end-each
  matches
;
\ lambda: <{ n -- f }> /\.(wav?|snd)$/ n regexp-match ; "." match-sound-files

\ ;;; -------- selection-members
\ ;;;
\ ;;; returns a list of lists of (snd chn): channels in current selection

: selection-members ( -- array-of-lists )
  doc" Returns an array of lists of #( snd chn ) indicating the channels \
participating in the current selection."
  #() { sndlist }
  undef selection? if
    sounds each { snd }
      snd channels 0 ?do
	snd i selection-member? if sndlist #( snd i ) array-push drop then
      loop
    end-each
  then
  sndlist
;

\ ;;; -------- make-selection
\ ;;;
\ ;;; the regularized form of this would use dur not end

: make-selection <{ :optional beg 0 end #f snd #f chn #f -- }>
  doc" Makes a selection like make-region but without creating a region.  \
It follows SND's sync field, and applies to all SND's channels if CHN is not specified. \
END defaults to end of channel, BEG defaults to 0, SND defaults to the currently selected sound."
  snd snd-snd { current-sound }
  current-sound sound? unless 'no-such-sound #( get-func-name beg end snd chn ) fth-throw then
  current-sound sync { current-sync }
  undef selection? if
    sounds each { s }
      s channels 0 ?do
	s i selection-member? ( need-update )
	#f s i set-selection-member? drop
	( need-update ) if s i update-time-graph drop then
      loop
    end-each
    chn integer? if
      end integer? if end 1+ else snd chn #f frames then beg - { len }
      #t  snd chn set-selection-member?  drop
      beg snd chn set-selection-position drop
      len snd chn set-selection-frames   drop
    else
      sounds each { s }
	snd #t                                  =
	s current-sound                         = ||
	current-sync 0=  current-sync s sync = && || if
	  s channels 0 ?do
	    #t  s i set-selection-member?  drop
	    beg s i set-selection-position drop
	    len s i set-selection-frames   drop
	  loop
	then
      end-each
    then
  then
;

\ ;;; -------- delete selected portion and smooth the splice

: delete-selection-and-smooth ( -- )
  doc" Deletes the current selection and smooths the splice."
  undef selection? if
    #f #f selection-position { beg }
    #f #f selection-frames   { len }
    all-chans each { lst }
      lst 0 array-ref  { snd }
      lst 1 array-ref { chn }
      snd chn selection-member? if
	beg           len snd chn #f delete-samples drop
	beg 16 - 0 max 32 snd chn    smooth-sound   drop
      then
    end-each
  then
;

\ ;;; -------- eval over selection, replacing current samples,
\ ;;;          mapped to "C-x x" key using prompt-in-minibuffer
\ ;;;
\ ;;; when the user types C-x x (without modifiers) and there is a current selection,
\ ;;;   the minibuffer prompts "selection eval:".  Eventually the user responds,
\ ;;;   hopefully with a function of one argument, the current selection sample
\ ;;;   the value returned by the function becomes the new selection value.

: eval-over-selection <{ func -- val }>
  doc" Evaluates FUNC on each sample in the current selection."
  func word?
  undef selection? && if
    #f #f selection-position { beg }
    #f #f selection-frames   { len }
    $" <'> %s %s" #( func get-func-name ) string-format { origin }
    all-chans each { lst }
      lst 0 array-ref  { snd }
      lst 1 array-ref { chn }
      snd chn selection-member? if
	beg len snd chn #f channel->vct ( old-data ) map
	  func #( *key* ) run-proc
	end-map ( new-data ) beg len snd chn #f origin vct->channel drop
      then
    end-each
  then
;
0 [if]
"x" 0 lambda: <{ -- val }>
  undef selection? if
    $" selection-eval:" <'> eval-over-selection #f #f prompt-in-minibuffer
  else
    $" no selection" #f #f report-in-minibuffer
  then
; #t $" eval over selection" $" eval over selection" bind-key
[then]

hide
: response-cb { snd action-if-yes action-if-no -- proc; response self -- val }
  1 proc-create action-if-no , action-if-yes , snd ,
 does> { response self -- val }
  self @ { action-if-no }
  self cell+ @ { action-if-yes }
  self 2 cells + @ { snd }
  snd clear-minibuffer drop
  response 0 string-ref [char] y = if
    action-if-yes snd run-proc
  else
    action-if-no snd run-proc
  then
;
set-current

: yes-or-no? ( question action-if-yes action-if-no snd -- )
  { question action-if-yes action-if-no snd }
  snd clear-minibuffer drop
  question
  snd action-if-yes action-if-no response-cb
  snd
  #t
  prompt-in-minibuffer drop
;
previous

\ ;;; -------- mix-channel, insert-channel, c-channel

hide
: mc-cb { rd -- prc; y self -- val }
  1 proc-create rd , ( prc )
 does> { y self -- val }
  self @ ( rd ) next-sample y f+
;
set-current

: mix-channel <{ file-data :optional beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Mixes in FILE-DATA.  \
FILE-DATA can be the file name or a list #( file-name [beg [channel]] )"
  file-data string? if file-data else file-data 0 array-ref then { file-name }
  file-name find-file to file-name
  file-name false? if 'no-such-file #( get-func-name file-name ) fth-throw then
  file-data string? file-data length 2 < || if 0 else file-data 1 array-ref then { file-beg }
  file-data string? file-data length 3 < || if 0 else file-data 2 array-ref then { file-channel }
  dur file-name mus-sound-frames file-beg - || { len }
  beg 0< if 'no-such-sample #( get-func-name beg ) fth-throw then
  len 0> if
    file-beg file-name file-channel 1 #f make-sampler { reader }
    $" %S %s %s %s" #( file-data beg dur get-func-name ) string-format { origin }
    reader mc-cb beg len snd chn edpos origin map-channel
  else
    #f
  then
;
previous

: insert-channel <{ file-data :optional beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Inserts the FILE-DATA.  \
FILE-DATA can be the file name or a list #( file-name [beg [channel]] )"
  file-data string? if file-data else file-data 0 array-ref then { file-name }
  file-name find-file to file-name
  file-name false? if 'no-such-file #( get-func-name file-name ) fth-throw then
  file-data string? file-data length 2 < || if 0 else file-data 1 array-ref then { file-beg }
  file-data string? file-data length 3 < || if 0 else file-data 2 array-ref then { file-channel }
  dur file-name mus-sound-frames file-beg - || { len }
  beg 0< if 'no-such-sample #( get-func-name beg ) fth-throw then
  len 0> if
    file-beg file-name file-channel 1 #f make-sampler { reader }
    len 0.0 make-vct map! reader next-sample end-map { data }
    reader free-sampler drop
    $" %S %s %s %s" #( file-data beg dur get-func-name ) string-format { origin }
    beg len data snd chn edpos #f origin insert-samples
  else
    #f
  then
;

\ ;;; -------- redo-channel, undo-channel

: redo-channel <{ :optional edits 1 snd #f chn #f -- }>
  doc" It's the regularized version of redo."
  snd fixnum?
  snd sync 0<> &&
  chn fixnum?  && if
    snd chn edit-position edits + snd chn set-edit-position drop
  else
    edits snd chn redo drop
  then
;

: undo-channel <{ :optional edits 1 snd #f chn #f -- }>
  doc" It's the regularized version of undo."
  snd fixnum?
  snd sync 0<> &&
  chn fixnum?  && if
    snd chn edit-position edits - 0 max snd chn set-edit-position drop
  else
    edits snd chn undo drop
  then
;

\ ;;; -------- any-env-channel

hide
: aec-cb { en func beg dur snd chn edpos -- prc; self -- val }
  0 proc-create en , func , beg , dur , snd , chn , edpos , ( prc )
 does> { self -- val }
  self           @ { en }
  self   cell+   @ { func }
  self 2 cells + @ { beg }
  self 3 cells + @ { dur }
  self 4 cells + @ { snd }
  self 5 cells + @ { chn }
  self 6 cells + @ { edpos }
  0.0 0.0 { x0 y0 }
  en 0 array-ref { x1 }
  en 1 array-ref { y1 }
  en envelope-last-x en 0 array-ref f- { xrange }
  beg { ramp-beg }
  0 { ramp-dur }
  en length 1- 2 ?do
    x1 to x0
    y1 to y0
    en i    array-ref to x1
    en i 1+ array-ref to y1
    x1 x0 f- xrange f/ dur f* fround->s to ramp-dur
    y0 y1 f= if
      y0 ramp-beg ramp-dur snd chn edpos scale-channel
    else
      func #( y0 y1 ramp-beg ramp-dur snd chn edpos ) run-proc
    then
    ramp-dur +to ramp-beg
  2 +loop
;
set-current

: any-env-channel <{ en func :optional beg 0 dur #f snd #f chn #f edpos #f origin #f -- val }>
  en nil? if
    #f
  else
    en envelope-length ( pts ) 1 = if
      en 0 array-ref beg dur snd chn edpos scale-channel
    else
      dur integer? unless snd chn #f frames to dur then
      en func beg dur snd chn edpos aec-cb origin as-one-edit
    then
  then
;
previous

\ ;;; -------- sine-ramp sine-env-channel 

hide
: sr3-cb <{ y data forward -- val }>
  data 0 vct-ref { angle }
  data 1 vct-ref { incr }
  angle fcos f2/ 0.5 f+ data 3 vct-ref f* data 2 vct-ref f+ y f* ( val )
  data 0  forward angle incr if f+ else f- then  vct-set! drop
  ( val )
;

: sr2-cb { rmp0 rmp1 -- prc; frag-beg frag-dur self -- vct }
  2 proc-create rmp0 , rmp1 ,
 does> { frag-beg frag-dur self -- vct }
  self @ { rmp0 }
  self cell+ @ { rmp1 }
  pi frag-dur f/ { incr }
  vct( pi fnegate frag-beg incr f* f+
     incr
     rmp0
     rmp1 rmp0 f- )
;
set-current

: sine-ramp <{ rmp0 rmp1 :optional beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Produces a sinsusoidal connection from RMP0 to RMP1."
  \ ;; vct: angle incr off scl
  $" %s %s %s %s %s" #( rmp0 rmp1 beg dur get-func-name ) string-format { origin }
  <'> sr3-cb beg dur snd chn edpos #t  rmp0 rmp1 sr2-cb  origin ptree-channel
;
previous

: sine-env-channel <{ en :optional beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Connects ENV's dots with sinusoids."
  $" %s %s %s %s" #( en beg dur get-func-name ) string-format { origin }
  en <'> sine-ramp beg dur snd chn edpos origin any-env-channel
;
\ #( 0 0 1 1 2 -0.5 3 1 ) sine-env-channel

\ ;;; an obvious extension of this idea is to use the blackman fft window formulas
\ ;;;   to get sharper sinusoids (i.e. use the sum of n cosines, rather than just 1)
\ 
\ ;;; -------- blackman4-ramp, blackman4-env-channel

hide
: b4r3-cb <{ y data forward -- val }>
  data 0 vct-ref { angle }
  data 1 vct-ref { incr }
  angle fcos { cx }
  cx 0.041194 f* -0.20762 f+ cx f* 0.375696 f+ cx f* -0.29145 f+ cx f* 0.084037 f+
  data 3 vct-ref f* data 2 vct-ref f+ y f* ( val )
  data 0  forward angle incr if f+ else f- then  vct-set! drop
  ( val )
;

: b4r2-cb { rmp0 rmp1 -- prc; frag-beg frag-dur self -- vct }
  2 proc-create rmp0 , rmp1 , ( prc )
 does> { frag-beg frag-dur self -- vct }
  self       @ { rmp0 }
  self cell+ @ { rmp1 }
  pi frag-dur f/ { incr }
  vct( frag-beg incr f*
     incr
     rmp0
     rmp1 rmp0 f- )
;
set-current

: blackman4-ramp <{ rmp0 rmp1 :optional beg 0 dur #f snd #f chn #f edpos #f -- val }>
  $" %s %s %s %s %s" #( rmp0 rmp1 beg dur get-func-name ) string-format { origin }
  <'> b4r3-cb beg dur snd chn edpos #f rmp0 rmp1 b4r2-cb origin ptree-channel
;
previous

: blackman4-env-channel <{ en :optional beg 0 dur #f snd #f chn #f edpos #f -- val }>
  $" %s %s %s %s" #( en beg dur get-func-name ) string-format { origin }
  en <'> blackman4-ramp beg dur snd chn edpos origin any-env-channel
;

\ ;;; any curve can be used as the connecting line between envelope breakpoints in the
\ ;;;   same manner -- set up each ramp to take the current position and increment,
\ ;;;   then return the value in ptree-channel.  A simple one would have a table of
\ ;;;   values and use array-interp.
\ 
\ ;;; -------- ramp-squared, env-squared-channel

hide
: rsq3-cb <{ y data forward -- val }>
  data 0 vct-ref { angle }
  data 1 vct-ref { incr }
  angle dup f* data 3 vct-ref f* data 2 vct-ref f+ y f* ( val )
  data 0  forward angle incr if f+ else f- then  vct-set! drop
  ( val )
;

: rsq2-cb { rmp0 rmp1 symmetric -- prc; frag-beg frag-dur self -- vct }
  2 proc-create rmp0 , rmp1 , symmetric , ( prc )
 does> { frag-beg frag-dur self -- vct }
  self           @ { rmp0 }
  self cell+     @ { rmp1 }
  self 2 cells + @ { symmetric }
  frag-dur 1/f { incr }
  symmetric rmp1 rmp0 f< && if
    vct( frag-dur frag-beg f- incr f*  incr fnegate  rmp1  rmp0 rmp1 f- )
  else
    vct( frag-beg incr f*  incr  rmp0  rmp1 rmp0 f- )
  then
;
set-current

: ramp-squared <{ rmp0 rmp1 :optional symmetric #t beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Connects RMP0 and RMP1 with an x^2 curve."
  $" %s %s %s %s %s %s" #( rmp0 rmp1 symmetric beg dur get-func-name ) string-format { origin }
  <'> rsq3-cb beg dur snd chn edpos #t  rmp0 rmp1 symmetric rsq2-cb  origin ptree-channel
;
previous

hide
: esqc-cb { symmetric -- prc; r0 r1 b d s c e self -- val }
  7 proc-create symmetric , ( prc )
 does> { r0 r1 b d s c e self -- val }
  r0 r1 self @ ( symmetric ) b d s c e ramp-squared
;
set-current

: env-squared-channel <{ en :optional symmetric #t beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Connects ENV's dots with x^2 curves."
  $" %s %s %s %s %s" #( en symmetric beg dur get-func-name ) string-format { origin }
  en  symmetric esqc-cb beg dur snd chn edpos origin any-env-channel
;
previous
\ #( 0 0 1 1 2 -0.5 3 1 ) env-squared-channel

\ ;;; -------- ramp-expt, env-expt-channel

hide
: rex3-cb <{ y data forward -- val }>
  data 0 vct-ref { angle }
  data 1 vct-ref { incr }
  data 4 vct-ref angle flog f* fexp data 3 vct-ref f* data 2 vct-ref f+ y f* ( val )
  data 0  forward angle incr if f+ else f- then  vct-set! drop
  ( val )
;

: rex2-cb { rmp0 rmp1 symmetric exponent -- prc; frag-beg frag-dur self -- vct }
  2 proc-create rmp0 , rmp1 , symmetric , exponent , ( prc )
 does> { frag-beg frag-dur self -- vct }
  self           @ { rmp0 }
  self cell+     @ { rmp1 }
  self 2 cells + @ { symmetric }
  self 3 cells + @ { exponent }
  frag-dur 1/f { incr }
  symmetric rmp1 rmp0 f< && if
    vct( frag-dur frag-beg f- incr f*  incr fnegate  rmp1  rmp0 rmp1 f-  exponent )
  else
    vct( frag-beg incr f*  incr  rmp0  rmp1 rmp0 f-  exponent )
  then
;
set-current

: ramp-expt <{ rmp0 rmp1 exponent
     :optional symmetric #t beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Connects RMP0 and RMP1 with an x^exponent curve."
  \ ;; vct: start incr off scl exponent
  \ ;; a^x = exp(x * log(a))
  $" %s %s %s %s %s %s %s"
  #( rmp0 rmp1 exponent symmetric beg dur get-func-name ) string-format { origin }
  <'> rex3-cb beg dur snd chn edpos #t  rmp0 rmp1 symmetric exponent rex2-cb origin ptree-channel
;
previous

hide
: expc-cb { symmetric exponent -- prc; r0 r1 b d s c e self -- val }
  7 proc-create symmetric , exponent , ( prc )
 does> { r0 r1 b d s c e self -- val }
  r0 r1 self cell+ @ ( exponent ) self @ ( symmetric ) b d s c e ramp-expt
;
set-current

: env-expt-channel <{ en exponent
     :optional symmetric #t beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Connects ENV's dots with x^exponent curves."
  $" %s %s %s %s %s %s" #( en exponent symmetric beg dur get-func-name ) string-format { origin }
  en  symmetric exponent expc-cb beg dur snd chn edpos origin any-env-channel
;
previous

\ ;;; -------- offset-channel

hide
: offc-cb { dc -- prc; y self -- val }
  1 proc-create dc , ( prc )
 does> { y self -- val }
  self @ ( dc ) y f+
;
set-current

: offset-channel <{ amount :optional beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Adds AMOUNT to each sample."
  $" %s %s %s %s" #( amount beg dur get-func-name ) string-format { origin }
  amount offc-cb beg dur snd chn edpos #t #f origin ptree-channel
;
previous

: offset-sound <{ offset :optional beg 0 dur #f snd #f -- }>
  doc" Adds OFFSET to every sample in SND."
  snd snd-snd to snd
  snd sound? if
    snd channels 0 ?do offset beg dur snd i ( chn ) #f offset-channel drop loop
  else
    'no-such-sound #( get-func-name snd ) fth-throw
  then
;

\ ;;; -------- pad-sound

: pad-sound <{ beg dur :optional snd #f -- }>
  doc" Places a block of DUR zeros in every channel of SND starting at BEG."
  snd snd-snd to snd
  snd sound? if
    snd channels 0 ?do beg dur snd i ( chn ) #f pad-channel drop loop
  else
    'no-such-sound #( get-func-name snd ) fth-throw
  then
;

\ ;;; -------- dither-channel

hide
: dith-cb { dither -- prc; y self -- val }
  1 proc-create dither , ( prc )
 does> { y self -- val }
  self @ ( dither ) dup mus-random swap mus-random f+ y f+
;
set-current

: dither-channel <{ :optional amount 0.00006 beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Adds AMOUNT dither to each sample."
  $" %s %s %s %s" #( amount beg dur get-func-name ) string-format { origin }
  amount f2/ dith-cb beg dur snd chn edpos #t #f origin ptree-channel
;

: dither-sound <{ :optional amount 0.00006 beg 0 dur #f snd #f -- }>
  doc" Adds dithering to every sample of SND."
  snd snd-snd to snd
  snd sound? if
    snd channels 0 ?do amount beg dur snd i ( chn ) #f dither-channel drop loop
  else
    'no-such-sound #( get-func-name snd ) fth-throw
  then
;

\ ;;; -------- contrast-channel

hide
: cntr-cb { index -- prc; y self -- val }
  1 proc-create index , ( prc )
 does> { y self -- val }
  y two-pi f* fsin self @ ( index ) f*  y half-pi f*  f+ fsin
;
set-current

: contrast-channel <{ index :optional beg 0 dur #f snd #f chn #f edpos #f -- val }>
  doc" Applies contrast enhancement to the sound."
  $" %s %s %s %s" #( index beg dur get-func-name ) string-format { origin }
  index cntr-cb beg dur snd chn edpos #f #f origin ptree-channel
;

: contrast-sound <{ index :optional beg 0 dur #f snd #f -- }>
  doc" Applies contrast-enhancement to every channel of SND."
  snd snd-snd to snd
  snd sound? if
    snd channels 0 ?do index beg dur snd i ( chn ) #f contrast-channel drop loop
  else
    'no-such-sound #( get-func-name snd ) fth-throw
  then
;

\ ;;; -------- scale-sound

: scale-sound <{ scl :optional beg 0 dur #f snd #f -- }>
  doc" Multiplies every sample in SND by SCL."
  snd snd-snd to snd
  snd sound? if
    snd channels 0 ?do scl beg dur snd i ( chn ) #f scale-channel drop loop
  else
    'no-such-sound #( get-func-name snd ) fth-throw
  then
;

\ ;;; -------- normalize-sound

: normalize-sound <{ amp :optional beg 0 dur #f snd #f -- }>
  doc" Scales SND to peak amplitude AMP."
  snd snd-snd to snd
  snd sound? if
    amp 0.0 snd #t #f maxamp each fabs fmax end-each f/ { scl }
    snd channels 0 ?do scl beg dur snd i ( chn ) #f scale-channel drop loop
  else
    'no-such-sound #( get-func-name snd ) fth-throw
  then
;

\ ;;; -------- channels-equal

hide
: c-equal-cb { rd diff -- prc; y self -- f }
  1 proc-create rd , diff , ( prc )
 does> { y self -- f }
  self @ ( rd ) read-sample y f- fabs self cell+ @ ( diff ) f>
;
set-current

: channels= <{ snd1 chn1 snd2 chn2 :optional allowable-difference 0.0 -- f }>
  doc" Returns #t if the two channels are the same (within diff) modulo trailing 0's."
  snd1 snd2 =
  chn1 chn2 = && if
    #t
  else
    snd1 chn1 #f maxamp { mx1 }
    snd2 chn2 #f maxamp { mx2 }
    mx1 mx2 f- fabs allowable-difference f> if
      #f
    else
      snd1 chn1 #f frames { len1 }
      snd2 chn2 #f frames { len2 }
      len1 len2 >= if
	len1
	snd1
	snd2
	chn1
	chn2
      else
	len2
	snd2
	snd1
	chn2
	chn1
      then { len s1 s2 c1 c2 }
      0 s2 c2 1 #f make-sampler { read2 }
      read2 allowable-difference c-equal-cb 0 len s1 c1 #f #f scan-channel not
    then
  then
;
previous

: channels-equal? <{ snd1 chn1 snd2 chn2 :optional allowable-difference 0.0 -- f }>
  doc" Returns #t if the two channels are the same (within diff)."
  snd1 chn1 #f frames snd2 chn2 #f frames <> if
    #f
  else
    snd1 chn1 snd2 chn2 allowable-difference channels=
  then
;

\ ;;; -------- mono->stereo, mono-files->stereo

: mono->stereo ( new-name snd1 chn1 snd2 chn2 -- snd )
  doc" Takes the two channels and combines them into a stereo sound NEW-NAME."
  { new-name snd1 chn1 snd2 chn2 }
  snd1 chn1 edit-position { old-ed1 }
  snd2 chn2 edit-position { old-ed2 }
  :file new-name :channels 2 :srate snd1 srate new-sound { ind }
  ind 0 snd1 chn1 0 #f #f swap-channels drop
  ind 1 snd2 chn2 0 #f #f swap-channels drop
  old-ed1 snd1 chn1 set-edit-position drop
  old-ed2 snd2 chn2 set-edit-position drop
  ind
;
\ "test.snd" 0 0 1 0 mono->stereo

: mono-files->stereo ( new-name chan1-name chan2-name -- snd )
  doc" Combines two mono files into the stereo file NEW-NAME."
  {  new-name chan1-name chan2-name }
  chan1-name find-file to chan1-name
  chan1-name false? if 'no-such-file #( get-func-name chan1-name ) fth-throw then
  chan1-name open-sound { ind1 }
  chan2-name find-file to chan2-name
  chan2-name false? if 'no-such-file #( get-func-name chan2-name ) fth-throw then
  chan2-name open-sound { ind2 }
  new-name ind1 0 ind2 0 mono->stereo { ind3 }
  ind1 close-sound drop
  ind2 close-sound drop
  ind3
;
\ "test.snd" "oboe.snd" "pistol.snd" mono-files->stereo

: stereo->mono ( orig-snd chan1-name chan2-name -- snd0 snd1 )
  doc" Splits a stereo sound into two mono sounds named CHAN1-NAME and CHAN2-NAME."
  {  orig-snd chan1-name chan2-name }
  orig-snd 0 edit-position { old-ed0 }
  orig-snd 1 edit-position { old-ed1 }
  :file chan1-name :srate orig-snd srate new-sound { chan1 }
  :file chan2-name :srate orig-snd srate new-sound { chan2 }
  orig-snd 0 chan1 0 0 #f #f swap-channels drop
  orig-snd 1 chan2 0 0 #f #f swap-channels drop
  old-ed0 orig-snd 0 set-edit-position drop
  old-ed1 orig-snd 1 set-edit-position drop
  chan1 chan2
;
\ 0 "hi1.snd" "hi2.snd" stereo->mono

\ === PREFERENCES DIALOG ===

\ --- reopen menu ---

hide
"empty" value reopen-empty
#() value reopen-names
#f  value reopen-menu
16  value reopen-max-length

<'> noop 0 make-proc constant extensions-noop

: reopen-select-cb { brief-name long-name -- proc; self -- }
  0 proc-create long-name , brief-name ,
 does> ( self -- )
  { self }
  self @ { long-name }
  self cell+ @ { brief-name }
  reopen-menu brief-name remove-from-menu drop
  long-name file-exists? if long-name open-sound drop then
;

: add-to-reopen-menu <{ snd -- #f }>
  snd short-file-name { brief-name }
  snd file-name { long-name }
  reopen-names brief-name array-member? unless
    reopen-names reopen-empty array-member? if
      reopen-menu reopen-empty remove-from-menu drop
      #() to reopen-names
    then
    reopen-menu brief-name brief-name long-name reopen-select-cb 0 add-to-menu drop
    reopen-names brief-name array-push to reopen-names
    reopen-names length reopen-max-length > if
      reopen-menu reopen-names array-shift remove-from-menu drop
    then
  then
  #f
;

: check-reopen-menu <{ file -- }>
  file #f file-basename { brief-name }
  reopen-names brief-name array-member? if
    reopen-menu brief-name remove-from-menu drop
    reopen-names brief-name array-index { idx }
    idx 0>= if reopen-names idx array-delete! drop then
  then
  reopen-names empty? if
    reopen-menu reopen-empty extensions-noop undef add-to-menu drop
    reopen-names reopen-empty array-push to reopen-names
  then
;
set-current

#f value including-reopen-menu		\ for prefs

: with-reopen-menu ( -- )
  including-reopen-menu unless
    #() to reopen-names
    reopen-menu false? if "Reopen" extensions-noop add-to-main-menu to reopen-menu then
    reopen-menu reopen-empty extensions-noop 0 add-to-menu drop
    reopen-names reopen-empty array-push to reopen-names
    #t to including-reopen-menu
    close-hook <'> add-to-reopen-menu add-hook!
    open-hook  <'> check-reopen-menu  add-hook!
  then
;
previous

\ --- buffers menu ---

hide
"empty" value buffer-empty
#() value buffer-names
#f  value buffer-menu

: buffer-select-cb ( file -- proc ; self -- )
  0 proc-create swap ,
 does> ( self -- )
  { self }
  self @ ( file ) 0 find-sound dup sound? if select-sound then drop
;

: open-buffer <{ file -- }>
  buffer-names buffer-empty array-member? if
    buffer-menu buffer-empty remove-from-menu drop
    #() to buffer-names
  then
  buffer-menu file file buffer-select-cb -1 add-to-menu drop
  buffer-names file array-push to buffer-names
;

: close-buffer <{ snd -- #f }>
  buffer-menu snd file-name remove-from-menu drop
  buffer-names snd file-name array-index { idx }
  idx 0>= if buffer-names idx array-delete! drop then
  buffer-names empty? if
    buffer-menu buffer-empty extensions-noop 0 add-to-menu drop
    buffer-names buffer-empty array-push to buffer-names
  then
  #f
;
set-current

#f value including-buffers-menu		\ for prefs

: with-buffers-menu ( -- )
  including-buffers-menu unless
    #() to buffer-names
    buffer-menu false? if "Buffers" extensions-noop add-to-main-menu to buffer-menu then
    buffer-menu buffer-empty extensions-noop 0 add-to-menu drop
    buffer-names buffer-empty array-push to buffer-names
    #t to including-buffers-menu
    open-hook  <'> open-buffer  add-hook!
    close-hook <'> close-buffer add-hook!
  then
;
previous

\ ;;; -------- cursor-follows-play and stays where it was when the play ended
\ moved from examp.fs to extensions.fs [ms]

hide
: current-cursor      { snd chn -- cur } 'cursor snd chn channel-property ;
: set-current-cursor  { snd chn val -- } 'cursor val snd chn set-channel-property ;
: original-cursor     { snd chn -- cur } 'original-cursor snd chn channel-property ;
: set-original-cursor { snd chn val -- } 'original-cursor val snd chn set-channel-property ;

: local-dac-func <{ data -- val }>
  sounds each { snd }
    snd channels 0 ?do
      snd i #f cursor snd i original-cursor <> if
	snd i  snd i #f cursor set-current-cursor
      then
    loop
  end-each
  #f
;

: local-start-playing-func <{ snd -- val }>
  snd channels 0 ?do
    snd i #f cursor { cur }
    snd i cur set-original-cursor
    snd i cur set-current-cursor
  loop
  #f
;

: local-stop-playing-func <{ snd -- val }>
  snd 0 current-cursor snd #t #f set-cursor
;
set-current

: if-cursor-follows-play-it-stays-where-play-stopped <{ :optional enable #t -- }>
  enable if
    dac-hook           <'> local-dac-func           add-hook!
    start-playing-hook <'> local-start-playing-func add-hook!
    stop-playing-hook  <'> local-stop-playing-func  add-hook!
  else
    dac-hook           <'> local-dac-func           remove-hook! drop
    start-playing-hook <'> local-start-playing-func remove-hook! drop
    stop-playing-hook  <'> local-stop-playing-func  remove-hook! drop
  then
;
previous

\ extensions.fs ends here
