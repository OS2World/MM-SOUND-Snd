\ -*- snd-forth -*-
\ mix.fs -- mix.scm -> mix.fs

\ Translator: Michael Scholz <mi-scholz@users.sourceforge.net>
\ Created: Tue Oct 11 18:23:12 CEST 2005
\ Changed: Thu Apr 15 15:58:17 CEST 2010

\ Commentary:
\
\ ;;; various mix related functions
\
\ mix-sound            ( file start -- mix-id )
\ silence-all-mixes     ( -- )
\ find-mix             ( sample snd chn -- mx )
\ mix->vct             ( id -- vct )
\ save-mix             ( id filename -- )
\ mix-maxamp           ( id -- max-amp )
\ snap-mix-to-beat     ( at-tag-position -- )
\
\ mix-click-sets-amp   ( id -- #t )
\ mix-click-info       ( id -- #t )
\ mix-name->id         ( name -- id )
\ 
\ delete-mix           ( id -- val )
\ scale-mixes          ( mix-list scl -- )
\ silence-mixes        ( mix-list -- )
\ move-mixes           ( mix-list samps -- )
\ src-mixes            ( mix-list sr -- )
\ transpose-mixes      ( mix-list semitones -- )
\ color-mixes          ( mix-list col -- )
\ set-mixes-tag-y      ( mix-list new-y -- )
\ mixes-maxamp         ( mix-list -- mx )
\ scale-tempo          ( mix-list tempo-scl -- )
\ mixes-length         ( mix-list -- len )
\ save-mixes           ( mix-list filename -- )

require clm
require examp

: tree-for-each ( proc-or-xt tree -- )
  doc" Applies PROC-OR-XT to every leaf of TREE."
  { proc-or-xt tree }
  tree nil? unless
    tree cons? if
      proc-or-xt tree car recurse
      proc-or-xt tree cdr recurse
    else
      proc-or-xt proc? if proc-or-xt '( tree ) run-proc drop then
    then
  then
;

: mix-sound <{ file :optional start 0 -- id }>
  doc" Mixes FILE (all chans) at START in the currently selected sound."
  file start #t undef undef undef undef mix
;

hide
: silence-mix-xt       <{ id -- }> id 0.0 set-mix-amp drop ;
: silence-all-mixes-cb <{ -- }> <'> silence-mix-xt undef undef mixes tree-for-each ;
set-current
: silence-all-mixes ( -- )
  doc" Sets all mix amps to 0."
  <'> silence-all-mixes-cb undef as-one-edit drop
;
previous

: find-mix <{ samp :optional snd #f chn #f -- mx }>
  doc" Returns the id of the mix at the given SAMPLE, or #f."
  #f					\ flag
  snd snd-snd chn snd-chn mixes each { id }
    id mix-position samp d= if
      drop				\ drop flag
      id
      leave
    then
  end-each
;

: mix->vct ( id -- v )
  doc" Returns mix's data in vct."
  { id }
  id mix? false? if 'no-such-mix #( get-func-name id ) fth-throw then
  id 0 make-mix-sampler { reader }
  id mix-length 0.0 make-vct map! reader read-mix-sample end-map
  reader free-sampler drop
;

: save-mix ( id fname -- )
  doc" Saves mix data (as floats) in FILENAME."
  { id fname }
  id mix->vct { v }
  fname #f srate mus-sound-open-output { fd }
  fd 0 v vct-length 1- 1 v undef undef vct->sound-data mus-sound-write drop
  fd v vct-length 4 * mus-sound-close-output drop
;

: mix-maxamp ( id -- max-amp )
  doc" Returns the max amp in the given mix."
  mix->vct vct-peak
;

hide
: snap-mix-to-beat-cb ( id samps self -- #t )
  { id samps self }
  id mix-position samps + { samp }
  id mix-home 0 array-ref { snd }
  id mix-home 1 array-ref { chn }
  snd chn beats-per-minute 60.0 f/ { bps }
  snd srate { sr }
  samp bps f* sr f/ floor { beat }
  beat sr f* bps f/ floor f>s { lower }
  beat 1.0 f+ sr f* bps f/ floor f>s { higher }
  id
  samp lower - higher samp - < if 0 lower max else higher then set-mix-position drop
  #t
;
set-current
: snap-mix-to-beat <{ -- }>
  doc" Forces a dragged mix to end up on a beat (see beats-per-minute).  \
Resets mix-release-hook to cancel."
  mix-release-hook snap-mix-to-beat-cb add-hook!
;
previous

\ --- Mix Property ---
: mix-click-sets-amp <{ id -- #t }>
  'zero id mix-property not if
    'amp  id  id mix-amp  set-mix-property drop
    id 0.0 set-mix-amp drop
    'zero id #t set-mix-property
  else
    id  'amp id mix-property  set-mix-amp drop
    'zero id #f set-mix-property
  then drop
  #t					\ #t --> omit default action
;
\ mix-click-hook <'> mix-click-sets-amp add-hook!

\ mix-click-info

: mix-click-info <{ id -- #t }>
  doc" A mix-click-hook function that describes a mix and its properties.\n\
mix-click-hook <'> mix-click-info add-hook!"
  id mix-home 0 array-ref { mid }
  id mix-name empty? if "" else $"  (%S)" #( id mix-name ) string-format then { mname }
  $"        mix id: %s%s\n" #( id mname ) string-format make-string-output-port { prt }
  prt $"      position: %d (%.3f secs)\n" #( id mix-position dup mid srate f/ ) port-puts-format
  prt $"        length: %d (%.3f secs)\n" #( id mix-length   dup mid srate f/ ) port-puts-format
  prt $"            in: %s[%d]\n" #( mid short-file-name id mix-home 1 array-ref ) port-puts-format
  prt $"        scaler: %s\n"   #( id mix-amp )     port-puts-format
  prt $"         speed: %.3f\n" #( id mix-speed )   port-puts-format
  prt $"           env: %s\n"   #( id mix-amp-env ) port-puts-format
  id mix-properties { props }
  props empty? unless prt $"    properties: %s\n" #( props ) port-puts-format then
  $" Mix info" prt port->string info-dialog drop
  #t
;
\ mix-click-hook <'> mix-click-info add-hook!

\ ;;; -------- mix-name->id

: mix-name->id { name -- mx }
  doc" Returns the mix id associated with NAME."
  #f
  sounds each { snd }
    snd channels 0 do
      snd i ( chn ) mixes each { mx }
	mx mix-name name string= if drop mx exit then
      end-each
    loop
  end-each dup false? if drop 'no-such-mix #( get-func-name name ) fth-throw then
;

\ ;;; ---------------- backwards compatibilty

: delete-mix ( id -- val ) 0.0 set-mix-amp ;

\ ;;; -------- mix lists (used to be "tracks"
hide
: scale-mixes-cb { mix-list scl -- prc; self -- }
  0 proc-create mix-list , scl , ( prc )
 does> { self -- }
  self cell+ @ { scl }
  self @ each dup mix-amp scl f* set-mix-amp drop end-each
;
set-current
: scale-mixes ( mix-list scl -- ) scale-mixes-cb undef as-one-edit drop ;
previous

: silence-mixes ( mix-list -- ) 0.0 scale-mixes ;

hide
: move-mixes-cb { mix-list samps -- prc; self -- }
  0 proc-create mix-list , samps , ( prc )
 does> { self -- }
  self cell+ @ { samps }
  self @ each dup mix-position samps + set-mix-position drop end-each
;
set-current
: move-mixes ( mix-list samps -- ) move-mixes-cb undef as-one-edit drop ;
previous

hide
: src-mixes-cb { mix-list sr -- prc; self -- }
  0 proc-create mix-list , sr , ( prc )
 does> { self -- }
  self cell+ @ { sr }
  self @ each dup mix-speed sr f* set-mix-speed drop end-each
;
set-current
: src-mixes ( mix-list sr -- ) src-mixes-cb undef as-one-edit drop ;
previous

: transpose-mixes ( mix-list semitones -- )
  doc" Transposes each mix in mix-list by semitones."
  12.0 f/ 2.0 swap f** src-mixes
;

'snd-nogui provided? [unless]
  : color-mixes     ( mix-list col -- )     { col } each { m } m col set-mix-color drop end-each ;
  : set-mixes-tag-y ( mix-list new-y -- ) { new-y } each { m } m new-y set-mix-tag-y drop end-each ;
  : mixes-maxamp    ( mix-list -- mx )     0.0 swap each { m } m mix-maxamp fmax end-each ;
[then]

hide
: scale-tempo-cb { mix-list tempo-scl first-beg -- prc; self -- }
  0 proc-create mix-list , tempo-scl , first-beg , ( prc )
 does> { self -- }
  self cell+ @ { tempo-scl }
  self 2 cells + @ { first-beg }
  self @ each { m }
    m mix-position first-beg - tempo-scl f* f>s { diff }
    diff 0<> if m first-beg diff + set-mix-position drop then
  end-each
;
set-current
: scale-tempo ( mix-list tempo-scl -- )
  { mix-list tempo-scl }
  mix-list 0 array-ref mix-position dup { first-beg last-beg }
  mix-list 1 nil array-subarray each { m }
    m mix-position { pos }
    first-beg pos min to first-beg
    last-beg pos max to last-beg
  end-each
  mix-list tempo-scl first-beg scale-tempo-cb undef as-one-edit drop
;
previous

\ ;;; reverse-mix-list is mix-list -1.0 scale-tempo

: mixes-length ( mix-list -- len )
  { mix-list }
  0 mix-list each { m } m mix-position m mix-length + max end-each
  0 mix-list each mix-position min end-each - 1+
;

: save-mixes ( mix-list filename -- )
  { mix-list filename }
  mix-list mix-length { len }
  0 mix-list each mix-position min end-each { beg }
  len 0.0 make-vct { data }
  mix-list each { m } data  m mix->vct  m mix-position beg -  vct-add! drop end-each
  filename #f srate 1 #f #f "" mus-sound-open-output { fd }
  fd 0 len 1- 1 data vct->sound-data mus-sound-write drop
  fd 4 data vct-length * mus-sound-close-output drop
;

\ mix.fs ends here
