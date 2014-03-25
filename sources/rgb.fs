\ -*- snd-forth -*-
\ rgb.fs -- rgb.scm|rb --> rgb.fs

\ Author: Michael Scholz <mi-scholz@users.sourceforge.net>
\ Created: Fri Aug 18 16:57:43 CEST 2006
\ Changed: Wed Nov 25 01:22:08 CET 2009

\ Commentary:
\
\ ;;; X11 color names converted to Snd colors
\ ;; tan -> tawny 24-Aug-01
\
\ e.g.: jet-colormap set-colormap drop
\

\ Code:

1.00 0.98 0.98 make-color constant snow
0.97 0.97 1.00 make-color constant ghost-white
0.96 0.96 0.96 make-color constant white-smoke
0.86 0.86 0.86 make-color constant gainsboro
1.00 0.98 0.94 make-color constant floral-white
0.99 0.96 0.90 make-color constant old-lace
0.98 0.94 0.90 make-color constant linen
0.98 0.92 0.84 make-color constant antique-white
1.00 0.93 0.83 make-color constant papaya-whip
1.00 0.92 0.80 make-color constant blanched-almond
1.00 0.89 0.77 make-color constant bisque
1.00 0.85 0.72 make-color constant peach-puff
1.00 0.87 0.68 make-color constant navajo-white
1.00 0.89 0.71 make-color constant moccasin
1.00 0.97 0.86 make-color constant cornsilk
1.00 1.00 0.94 make-color constant ivory
1.00 0.98 0.80 make-color constant lemon-chiffon
1.00 0.96 0.93 make-color constant seashell
0.94 1.00 0.94 make-color constant honeydew
0.96 1.00 0.98 make-color constant mint-cream
0.94 1.00 1.00 make-color constant azure
0.94 0.97 1.00 make-color constant alice-blue
0.90 0.90 0.98 make-color constant lavender
1.00 0.94 0.96 make-color constant lavender-blush
1.00 0.89 0.88 make-color constant misty-rose
1.00 1.00 1.00 make-color constant white
0.00 0.00 0.00 make-color constant black
0.18 0.31 0.31 make-color constant dark-slate-gray
0.18 0.31 0.31 make-color constant dark-slate-grey
0.41 0.41 0.41 make-color constant dim-gray
0.41 0.41 0.41 make-color constant dim-grey
0.44 0.50 0.56 make-color constant slate-gray
0.44 0.50 0.56 make-color constant slate-grey
0.46 0.53 0.60 make-color constant light-slate-gray
0.46 0.53 0.60 make-color constant light-slate-grey
0.74 0.74 0.74 make-color constant gray
0.74 0.74 0.74 make-color constant grey
0.82 0.82 0.82 make-color constant light-grey
0.82 0.82 0.82 make-color constant light-gray
0.10 0.10 0.44 make-color constant midnight-blue
0.00 0.00 0.50 make-color constant navy
0.00 0.00 0.50 make-color constant navy-blue
0.39 0.58 0.93 make-color constant cornflower-blue
0.28 0.24 0.54 make-color constant dark-slate-blue
0.41 0.35 0.80 make-color constant slate-blue
0.48 0.41 0.93 make-color constant medium-slate-blue
0.52 0.44 1.00 make-color constant light-slate-blue
0.00 0.00 0.80 make-color constant medium-blue
0.25 0.41 0.88 make-color constant royal-blue
0.00 0.00 1.00 make-color constant blue
0.12 0.56 1.00 make-color constant dodger-blue
0.00 0.75 1.00 make-color constant deep-sky-blue
0.53 0.80 0.92 make-color constant sky-blue
0.53 0.80 0.98 make-color constant light-sky-blue
0.27 0.51 0.70 make-color constant steel-blue
0.69 0.77 0.87 make-color constant light-steel-blue
0.68 0.84 0.90 make-color constant light-blue
0.69 0.87 0.90 make-color constant powder-blue
0.68 0.93 0.93 make-color constant pale-turquoise
0.00 0.80 0.82 make-color constant dark-turquoise
0.28 0.82 0.80 make-color constant medium-turquoise
0.25 0.87 0.81 make-color constant turquoise
0.00 1.00 1.00 make-color constant cyan
0.87 1.00 1.00 make-color constant light-cyan
0.37 0.62 0.62 make-color constant cadet-blue
0.40 0.80 0.66 make-color constant medium-aquamarine
0.50 1.00 0.83 make-color constant aquamarine
0.00 0.39 0.00 make-color constant dark-green
0.33 0.42 0.18 make-color constant dark-olive-green
0.56 0.73 0.56 make-color constant dark-sea-green
0.18 0.54 0.34 make-color constant sea-green
0.23 0.70 0.44 make-color constant medium-sea-green
0.12 0.70 0.66 make-color constant light-sea-green
0.59 0.98 0.59 make-color constant pale-green
0.00 1.00 0.50 make-color constant spring-green
0.48 0.98 0.00 make-color constant lawn-green
0.00 1.00 0.00 make-color constant green
0.50 1.00 0.00 make-color constant chartreuse
0.00 0.98 0.60 make-color constant medium-spring-green
0.68 1.00 0.18 make-color constant green-yellow
0.20 0.80 0.20 make-color constant lime-green
0.60 0.80 0.20 make-color constant yellow-green
0.13 0.54 0.13 make-color constant forest-green
0.42 0.55 0.14 make-color constant olive-drab
0.74 0.71 0.42 make-color constant dark-khaki
0.94 0.90 0.55 make-color constant khaki
0.93 0.91 0.66 make-color constant pale-goldenrod
0.98 0.98 0.82 make-color constant light-goldenrod-yellow
1.00 1.00 0.87 make-color constant light-yellow
1.00 1.00 0.00 make-color constant yellow
1.00 0.84 0.00 make-color constant gold
0.93 0.86 0.51 make-color constant light-goldenrod
0.85 0.64 0.12 make-color constant goldenrod
0.72 0.52 0.04 make-color constant dark-goldenrod
0.73 0.56 0.56 make-color constant rosy-brown
0.80 0.36 0.36 make-color constant indian-red
0.54 0.27 0.07 make-color constant saddle-brown
0.62 0.32 0.18 make-color constant sienna
0.80 0.52 0.25 make-color constant peru
0.87 0.72 0.53 make-color constant burlywood
0.96 0.96 0.86 make-color constant beige
0.96 0.87 0.70 make-color constant wheat
0.95 0.64 0.37 make-color constant sandy-brown
\ tan collides with Scheme built-in -- tawny suggested by Dave Phillips
0.82 0.70 0.55 make-color constant tawny
0.82 0.41 0.12 make-color constant chocolate
0.70 0.13 0.13 make-color constant firebrick
0.64 0.16 0.16 make-color constant brown
0.91 0.59 0.48 make-color constant dark-salmon
0.98 0.50 0.45 make-color constant salmon
1.00 0.62 0.48 make-color constant light-salmon
1.00 0.64 0.00 make-color constant orange
1.00 0.55 0.00 make-color constant dark-orange
1.00 0.50 0.31 make-color constant coral
0.94 0.50 0.50 make-color constant light-coral
1.00 0.39 0.28 make-color constant tomato
1.00 0.27 0.00 make-color constant orange-red
1.00 0.00 0.00 make-color constant red
1.00 0.41 0.70 make-color constant hot-pink
1.00 0.08 0.57 make-color constant deep-pink
1.00 0.75 0.79 make-color constant pink
1.00 0.71 0.75 make-color constant light-pink
0.86 0.44 0.57 make-color constant pale-violet-red
0.69 0.19 0.37 make-color constant maroon
0.78 0.08 0.52 make-color constant medium-violet-red
0.81 0.12 0.56 make-color constant violet-red
1.00 0.00 1.00 make-color constant magenta
0.93 0.51 0.93 make-color constant violet
0.86 0.62 0.86 make-color constant plum
0.85 0.44 0.84 make-color constant orchid
0.73 0.33 0.82 make-color constant medium-orchid
0.60 0.20 0.80 make-color constant dark-orchid
0.58 0.00 0.82 make-color constant dark-violet
0.54 0.17 0.88 make-color constant blue-violet
0.62 0.12 0.94 make-color constant purple
0.57 0.44 0.86 make-color constant medium-purple
0.84 0.75 0.84 make-color constant thistle
1.00 0.98 0.98 make-color constant snow1
0.93 0.91 0.91 make-color constant snow2
0.80 0.79 0.79 make-color constant snow3
0.54 0.54 0.54 make-color constant snow4
1.00 0.96 0.93 make-color constant seashell1
0.93 0.89 0.87 make-color constant seashell2
0.80 0.77 0.75 make-color constant seashell3
0.54 0.52 0.51 make-color constant seashell4
1.00 0.93 0.86 make-color constant antiquewhite1
0.93 0.87 0.80 make-color constant antiquewhite2
0.80 0.75 0.69 make-color constant antiquewhite3
0.54 0.51 0.47 make-color constant antiquewhite4
1.00 0.89 0.77 make-color constant bisque1
0.93 0.83 0.71 make-color constant bisque2
0.80 0.71 0.62 make-color constant bisque3
0.54 0.49 0.42 make-color constant bisque4
1.00 0.85 0.72 make-color constant peachpuff1
0.93 0.79 0.68 make-color constant peachpuff2
0.80 0.68 0.58 make-color constant peachpuff3
0.54 0.46 0.39 make-color constant peachpuff4
1.00 0.87 0.68 make-color constant navajowhite1
0.93 0.81 0.63 make-color constant navajowhite2
0.80 0.70 0.54 make-color constant navajowhite3
0.54 0.47 0.37 make-color constant navajowhite4
1.00 0.98 0.80 make-color constant lemonchiffon1
0.93 0.91 0.75 make-color constant lemonchiffon2
0.80 0.79 0.64 make-color constant lemonchiffon3
0.54 0.54 0.44 make-color constant lemonchiffon4
1.00 0.97 0.86 make-color constant cornsilk1
0.93 0.91 0.80 make-color constant cornsilk2
0.80 0.78 0.69 make-color constant cornsilk3
0.54 0.53 0.47 make-color constant cornsilk4
1.00 1.00 0.94 make-color constant ivory1
0.93 0.93 0.87 make-color constant ivory2
0.80 0.80 0.75 make-color constant ivory3
0.54 0.54 0.51 make-color constant ivory4
0.94 1.00 0.94 make-color constant honeydew1
0.87 0.93 0.87 make-color constant honeydew2
0.75 0.80 0.75 make-color constant honeydew3
0.51 0.54 0.51 make-color constant honeydew4
1.00 0.94 0.96 make-color constant lavenderblush1
0.93 0.87 0.89 make-color constant lavenderblush2
0.80 0.75 0.77 make-color constant lavenderblush3
0.54 0.51 0.52 make-color constant lavenderblush4
1.00 0.89 0.88 make-color constant mistyrose1
0.93 0.83 0.82 make-color constant mistyrose2
0.80 0.71 0.71 make-color constant mistyrose3
0.54 0.49 0.48 make-color constant mistyrose4
0.94 1.00 1.00 make-color constant azure1
0.87 0.93 0.93 make-color constant azure2
0.75 0.80 0.80 make-color constant azure3
0.51 0.54 0.54 make-color constant azure4
0.51 0.43 1.00 make-color constant slateblue1
0.48 0.40 0.93 make-color constant slateblue2
0.41 0.35 0.80 make-color constant slateblue3
0.28 0.23 0.54 make-color constant slateblue4
0.28 0.46 1.00 make-color constant royalblue1
0.26 0.43 0.93 make-color constant royalblue2
0.23 0.37 0.80 make-color constant royalblue3
0.15 0.25 0.54 make-color constant royalblue4
0.00 0.00 1.00 make-color constant blue1
0.00 0.00 0.93 make-color constant blue2
0.00 0.00 0.80 make-color constant blue3
0.00 0.00 0.54 make-color constant blue4
0.12 0.56 1.00 make-color constant dodgerblue1
0.11 0.52 0.93 make-color constant dodgerblue2
0.09 0.45 0.80 make-color constant dodgerblue3
0.06 0.30 0.54 make-color constant dodgerblue4
0.39 0.72 1.00 make-color constant steelblue1
0.36 0.67 0.93 make-color constant steelblue2
0.31 0.58 0.80 make-color constant steelblue3
0.21 0.39 0.54 make-color constant steelblue4
0.00 0.75 1.00 make-color constant deepskyblue1
0.00 0.70 0.93 make-color constant deepskyblue2
0.00 0.60 0.80 make-color constant deepskyblue3
0.00 0.41 0.54 make-color constant deepskyblue4
0.53 0.80 1.00 make-color constant skyblue1
0.49 0.75 0.93 make-color constant skyblue2
0.42 0.65 0.80 make-color constant skyblue3
0.29 0.44 0.54 make-color constant skyblue4
0.69 0.88 1.00 make-color constant lightskyblue1
0.64 0.82 0.93 make-color constant lightskyblue2
0.55 0.71 0.80 make-color constant lightskyblue3
0.37 0.48 0.54 make-color constant lightskyblue4
0.77 0.88 1.00 make-color constant slategray1
0.72 0.82 0.93 make-color constant slategray2
0.62 0.71 0.80 make-color constant slategray3
0.42 0.48 0.54 make-color constant slategray4
0.79 0.88 1.00 make-color constant lightsteelblue1
0.73 0.82 0.93 make-color constant lightsteelblue2
0.63 0.71 0.80 make-color constant lightsteelblue3
0.43 0.48 0.54 make-color constant lightsteelblue4
0.75 0.93 1.00 make-color constant lightblue1
0.70 0.87 0.93 make-color constant lightblue2
0.60 0.75 0.80 make-color constant lightblue3
0.41 0.51 0.54 make-color constant lightblue4
0.87 1.00 1.00 make-color constant lightcyan1
0.82 0.93 0.93 make-color constant lightcyan2
0.70 0.80 0.80 make-color constant lightcyan3
0.48 0.54 0.54 make-color constant lightcyan4
0.73 1.00 1.00 make-color constant paleturquoise1
0.68 0.93 0.93 make-color constant paleturquoise2
0.59 0.80 0.80 make-color constant paleturquoise3
0.40 0.54 0.54 make-color constant paleturquoise4
0.59 0.96 1.00 make-color constant cadetblue1
0.55 0.89 0.93 make-color constant cadetblue2
0.48 0.77 0.80 make-color constant cadetblue3
0.32 0.52 0.54 make-color constant cadetblue4
0.00 0.96 1.00 make-color constant turquoise1
0.00 0.89 0.93 make-color constant turquoise2
0.00 0.77 0.80 make-color constant turquoise3
0.00 0.52 0.54 make-color constant turquoise4
0.00 1.00 1.00 make-color constant cyan1
0.00 0.93 0.93 make-color constant cyan2
0.00 0.80 0.80 make-color constant cyan3
0.00 0.54 0.54 make-color constant cyan4
0.59 1.00 1.00 make-color constant darkslategray1
0.55 0.93 0.93 make-color constant darkslategray2
0.47 0.80 0.80 make-color constant darkslategray3
0.32 0.54 0.54 make-color constant darkslategray4
0.50 1.00 0.83 make-color constant aquamarine1
0.46 0.93 0.77 make-color constant aquamarine2
0.40 0.80 0.66 make-color constant aquamarine3
0.27 0.54 0.45 make-color constant aquamarine4
0.75 1.00 0.75 make-color constant darkseagreen1
0.70 0.93 0.70 make-color constant darkseagreen2
0.61 0.80 0.61 make-color constant darkseagreen3
0.41 0.54 0.41 make-color constant darkseagreen4
0.33 1.00 0.62 make-color constant seagreen1
0.30 0.93 0.58 make-color constant seagreen2
0.26 0.80 0.50 make-color constant seagreen3
0.18 0.54 0.34 make-color constant seagreen4
0.60 1.00 0.60 make-color constant palegreen1
0.56 0.93 0.56 make-color constant palegreen2
0.48 0.80 0.48 make-color constant palegreen3
0.33 0.54 0.33 make-color constant palegreen4
0.00 1.00 0.50 make-color constant springgreen1
0.00 0.93 0.46 make-color constant springgreen2
0.00 0.80 0.40 make-color constant springgreen3
0.00 0.54 0.27 make-color constant springgreen4
0.00 1.00 0.00 make-color constant green1
0.00 0.93 0.00 make-color constant green2
0.00 0.80 0.00 make-color constant green3
0.00 0.54 0.00 make-color constant green4
0.50 1.00 0.00 make-color constant chartreuse1
0.46 0.93 0.00 make-color constant chartreuse2
0.40 0.80 0.00 make-color constant chartreuse3
0.27 0.54 0.00 make-color constant chartreuse4
0.75 1.00 0.24 make-color constant olivedrab1
0.70 0.93 0.23 make-color constant olivedrab2
0.60 0.80 0.20 make-color constant olivedrab3
0.41 0.54 0.13 make-color constant olivedrab4
0.79 1.00 0.44 make-color constant darkolivegreen1
0.73 0.93 0.41 make-color constant darkolivegreen2
0.63 0.80 0.35 make-color constant darkolivegreen3
0.43 0.54 0.24 make-color constant darkolivegreen4
1.00 0.96 0.56 make-color constant khaki1
0.93 0.90 0.52 make-color constant khaki2
0.80 0.77 0.45 make-color constant khaki3
0.54 0.52 0.30 make-color constant khaki4
1.00 0.92 0.54 make-color constant lightgoldenrod1
0.93 0.86 0.51 make-color constant lightgoldenrod2
0.80 0.74 0.44 make-color constant lightgoldenrod3
0.54 0.50 0.30 make-color constant lightgoldenrod4
1.00 1.00 0.87 make-color constant lightyellow1
0.93 0.93 0.82 make-color constant lightyellow2
0.80 0.80 0.70 make-color constant lightyellow3
0.54 0.54 0.48 make-color constant lightyellow4
1.00 1.00 0.00 make-color constant yellow1
0.93 0.93 0.00 make-color constant yellow2
0.80 0.80 0.00 make-color constant yellow3
0.54 0.54 0.00 make-color constant yellow4
1.00 0.84 0.00 make-color constant gold1
0.93 0.79 0.00 make-color constant gold2
0.80 0.68 0.00 make-color constant gold3
0.54 0.46 0.00 make-color constant gold4
1.00 0.75 0.14 make-color constant goldenrod1
0.93 0.70 0.13 make-color constant goldenrod2
0.80 0.61 0.11 make-color constant goldenrod3
0.54 0.41 0.08 make-color constant goldenrod4
1.00 0.72 0.06 make-color constant darkgoldenrod1
0.93 0.68 0.05 make-color constant darkgoldenrod2
0.80 0.58 0.05 make-color constant darkgoldenrod3
0.54 0.39 0.03 make-color constant darkgoldenrod4
1.00 0.75 0.75 make-color constant rosybrown1
0.93 0.70 0.70 make-color constant rosybrown2
0.80 0.61 0.61 make-color constant rosybrown3
0.54 0.41 0.41 make-color constant rosybrown4
1.00 0.41 0.41 make-color constant indianred1
0.93 0.39 0.39 make-color constant indianred2
0.80 0.33 0.33 make-color constant indianred3
0.54 0.23 0.23 make-color constant indianred4
1.00 0.51 0.28 make-color constant sienna1
0.93 0.47 0.26 make-color constant sienna2
0.80 0.41 0.22 make-color constant sienna3
0.54 0.28 0.15 make-color constant sienna4
1.00 0.82 0.61 make-color constant burlywood1
0.93 0.77 0.57 make-color constant burlywood2
0.80 0.66 0.49 make-color constant burlywood3
0.54 0.45 0.33 make-color constant burlywood4
1.00 0.90 0.73 make-color constant wheat1
0.93 0.84 0.68 make-color constant wheat2
0.80 0.73 0.59 make-color constant wheat3
0.54 0.49 0.40 make-color constant wheat4
1.00 0.64 0.31 make-color constant tan1
0.93 0.60 0.29 make-color constant tan2
0.80 0.52 0.25 make-color constant tan3
0.54 0.35 0.17 make-color constant tan4
1.00 0.50 0.14 make-color constant chocolate1
0.93 0.46 0.13 make-color constant chocolate2
0.80 0.40 0.11 make-color constant chocolate3
0.54 0.27 0.07 make-color constant chocolate4
1.00 0.19 0.19 make-color constant firebrick1
0.93 0.17 0.17 make-color constant firebrick2
0.80 0.15 0.15 make-color constant firebrick3
0.54 0.10 0.10 make-color constant firebrick4
1.00 0.25 0.25 make-color constant brown1
0.93 0.23 0.23 make-color constant brown2
0.80 0.20 0.20 make-color constant brown3
0.54 0.14 0.14 make-color constant brown4
1.00 0.55 0.41 make-color constant salmon1
0.93 0.51 0.38 make-color constant salmon2
0.80 0.44 0.33 make-color constant salmon3
0.54 0.30 0.22 make-color constant salmon4
1.00 0.62 0.48 make-color constant lightsalmon1
0.93 0.58 0.45 make-color constant lightsalmon2
0.80 0.50 0.38 make-color constant lightsalmon3
0.54 0.34 0.26 make-color constant lightsalmon4
1.00 0.64 0.00 make-color constant orange1
0.93 0.60 0.00 make-color constant orange2
0.80 0.52 0.00 make-color constant orange3
0.54 0.35 0.00 make-color constant orange4
1.00 0.50 0.00 make-color constant darkorange1
0.93 0.46 0.00 make-color constant darkorange2
0.80 0.40 0.00 make-color constant darkorange3
0.54 0.27 0.00 make-color constant darkorange4
1.00 0.45 0.34 make-color constant coral1
0.93 0.41 0.31 make-color constant coral2
0.80 0.36 0.27 make-color constant coral3
0.54 0.24 0.18 make-color constant coral4
1.00 0.39 0.28 make-color constant tomato1
0.93 0.36 0.26 make-color constant tomato2
0.80 0.31 0.22 make-color constant tomato3
0.54 0.21 0.15 make-color constant tomato4
1.00 0.27 0.00 make-color constant orangered1
0.93 0.25 0.00 make-color constant orangered2
0.80 0.21 0.00 make-color constant orangered3
0.54 0.14 0.00 make-color constant orangered4
1.00 0.00 0.00 make-color constant red1
0.93 0.00 0.00 make-color constant red2
0.80 0.00 0.00 make-color constant red3
0.54 0.00 0.00 make-color constant red4
1.00 0.08 0.57 make-color constant deeppink1
0.93 0.07 0.54 make-color constant deeppink2
0.80 0.06 0.46 make-color constant deeppink3
0.54 0.04 0.31 make-color constant deeppink4
1.00 0.43 0.70 make-color constant hotpink1
0.93 0.41 0.65 make-color constant hotpink2
0.80 0.37 0.56 make-color constant hotpink3
0.54 0.23 0.38 make-color constant hotpink4
1.00 0.71 0.77 make-color constant pink1
0.93 0.66 0.72 make-color constant pink2
0.80 0.57 0.62 make-color constant pink3
0.54 0.39 0.42 make-color constant pink4
1.00 0.68 0.72 make-color constant lightpink1
0.93 0.63 0.68 make-color constant lightpink2
0.80 0.55 0.58 make-color constant lightpink3
0.54 0.37 0.39 make-color constant lightpink4
1.00 0.51 0.67 make-color constant palevioletred1
0.93 0.47 0.62 make-color constant palevioletred2
0.80 0.41 0.54 make-color constant palevioletred3
0.54 0.28 0.36 make-color constant palevioletred4
1.00 0.20 0.70 make-color constant maroon1
0.93 0.19 0.65 make-color constant maroon2
0.80 0.16 0.56 make-color constant maroon3
0.54 0.11 0.38 make-color constant maroon4
1.00 0.24 0.59 make-color constant violetred1
0.93 0.23 0.55 make-color constant violetred2
0.80 0.20 0.47 make-color constant violetred3
0.54 0.13 0.32 make-color constant violetred4
1.00 0.00 1.00 make-color constant magenta1
0.93 0.00 0.93 make-color constant magenta2
0.80 0.00 0.80 make-color constant magenta3
0.54 0.00 0.54 make-color constant magenta4
1.00 0.51 0.98 make-color constant orchid1
0.93 0.48 0.91 make-color constant orchid2
0.80 0.41 0.79 make-color constant orchid3
0.54 0.28 0.54 make-color constant orchid4
1.00 0.73 1.00 make-color constant plum1
0.93 0.68 0.93 make-color constant plum2
0.80 0.59 0.80 make-color constant plum3
0.54 0.40 0.54 make-color constant plum4
0.87 0.40 1.00 make-color constant mediumorchid1
0.82 0.37 0.93 make-color constant mediumorchid2
0.70 0.32 0.80 make-color constant mediumorchid3
0.48 0.21 0.54 make-color constant mediumorchid4
0.75 0.24 1.00 make-color constant darkorchid1
0.70 0.23 0.93 make-color constant darkorchid2
0.60 0.20 0.80 make-color constant darkorchid3
0.41 0.13 0.54 make-color constant darkorchid4
0.61 0.19 1.00 make-color constant purple1
0.57 0.17 0.93 make-color constant purple2
0.49 0.15 0.80 make-color constant purple3
0.33 0.10 0.54 make-color constant purple4
0.67 0.51 1.00 make-color constant mediumpurple1
0.62 0.47 0.93 make-color constant mediumpurple2
0.54 0.41 0.80 make-color constant mediumpurple3
0.36 0.28 0.54 make-color constant mediumpurple4
1.00 0.88 1.00 make-color constant thistle1
0.93 0.82 0.93 make-color constant thistle2
0.80 0.71 0.80 make-color constant thistle3
0.54 0.48 0.54 make-color constant thistle4
0.00 0.00 0.00 make-color constant gray0
0.00 0.00 0.00 make-color constant grey0
0.01 0.01 0.01 make-color constant gray1
0.01 0.01 0.01 make-color constant grey1
0.02 0.02 0.02 make-color constant gray2
0.02 0.02 0.02 make-color constant grey2
0.03 0.03 0.03 make-color constant gray3
0.03 0.03 0.03 make-color constant grey3
0.04 0.04 0.04 make-color constant gray4
0.04 0.04 0.04 make-color constant grey4
0.05 0.05 0.05 make-color constant gray5
0.05 0.05 0.05 make-color constant grey5
0.06 0.06 0.06 make-color constant gray6
0.06 0.06 0.06 make-color constant grey6
0.07 0.07 0.07 make-color constant gray7
0.07 0.07 0.07 make-color constant grey7
0.08 0.08 0.08 make-color constant gray8
0.08 0.08 0.08 make-color constant grey8
0.09 0.09 0.09 make-color constant gray9
0.09 0.09 0.09 make-color constant grey9
0.10 0.10 0.10 make-color constant gray10
0.10 0.10 0.10 make-color constant grey10
0.11 0.11 0.11 make-color constant gray11
0.11 0.11 0.11 make-color constant grey11
0.12 0.12 0.12 make-color constant gray12
0.12 0.12 0.12 make-color constant grey12
0.13 0.13 0.13 make-color constant gray13
0.13 0.13 0.13 make-color constant grey13
0.14 0.14 0.14 make-color constant gray14
0.14 0.14 0.14 make-color constant grey14
0.15 0.15 0.15 make-color constant gray15
0.15 0.15 0.15 make-color constant grey15
0.16 0.16 0.16 make-color constant gray16
0.16 0.16 0.16 make-color constant grey16
0.17 0.17 0.17 make-color constant gray17
0.17 0.17 0.17 make-color constant grey17
0.18 0.18 0.18 make-color constant gray18
0.18 0.18 0.18 make-color constant grey18
0.19 0.19 0.19 make-color constant gray19
0.19 0.19 0.19 make-color constant grey19
0.20 0.20 0.20 make-color constant gray20
0.20 0.20 0.20 make-color constant grey20
0.21 0.21 0.21 make-color constant gray21
0.21 0.21 0.21 make-color constant grey21
0.22 0.22 0.22 make-color constant gray22
0.22 0.22 0.22 make-color constant grey22
0.23 0.23 0.23 make-color constant gray23
0.23 0.23 0.23 make-color constant grey23
0.24 0.24 0.24 make-color constant gray24
0.24 0.24 0.24 make-color constant grey24
0.25 0.25 0.25 make-color constant gray25
0.25 0.25 0.25 make-color constant grey25
0.26 0.26 0.26 make-color constant gray26
0.26 0.26 0.26 make-color constant grey26
0.27 0.27 0.27 make-color constant gray27
0.27 0.27 0.27 make-color constant grey27
0.28 0.28 0.28 make-color constant gray28
0.28 0.28 0.28 make-color constant grey28
0.29 0.29 0.29 make-color constant gray29
0.29 0.29 0.29 make-color constant grey29
0.30 0.30 0.30 make-color constant gray30
0.30 0.30 0.30 make-color constant grey30
0.31 0.31 0.31 make-color constant gray31
0.31 0.31 0.31 make-color constant grey31
0.32 0.32 0.32 make-color constant gray32
0.32 0.32 0.32 make-color constant grey32
0.33 0.33 0.33 make-color constant gray33
0.33 0.33 0.33 make-color constant grey33
0.34 0.34 0.34 make-color constant gray34
0.34 0.34 0.34 make-color constant grey34
0.35 0.35 0.35 make-color constant gray35
0.35 0.35 0.35 make-color constant grey35
0.36 0.36 0.36 make-color constant gray36
0.36 0.36 0.36 make-color constant grey36
0.37 0.37 0.37 make-color constant gray37
0.37 0.37 0.37 make-color constant grey37
0.38 0.38 0.38 make-color constant gray38
0.38 0.38 0.38 make-color constant grey38
0.39 0.39 0.39 make-color constant gray39
0.39 0.39 0.39 make-color constant grey39
0.40 0.40 0.40 make-color constant gray40
0.40 0.40 0.40 make-color constant grey40
0.41 0.41 0.41 make-color constant gray41
0.41 0.41 0.41 make-color constant grey41
0.42 0.42 0.42 make-color constant gray42
0.42 0.42 0.42 make-color constant grey42
0.43 0.43 0.43 make-color constant gray43
0.43 0.43 0.43 make-color constant grey43
0.44 0.44 0.44 make-color constant gray44
0.44 0.44 0.44 make-color constant grey44
0.45 0.45 0.45 make-color constant gray45
0.45 0.45 0.45 make-color constant grey45
0.46 0.46 0.46 make-color constant gray46
0.46 0.46 0.46 make-color constant grey46
0.47 0.47 0.47 make-color constant gray47
0.47 0.47 0.47 make-color constant grey47
0.48 0.48 0.48 make-color constant gray48
0.48 0.48 0.48 make-color constant grey48
0.49 0.49 0.49 make-color constant gray49
0.49 0.49 0.49 make-color constant grey49
0.50 0.50 0.50 make-color constant gray50
0.50 0.50 0.50 make-color constant grey50
0.51 0.51 0.51 make-color constant gray51
0.51 0.51 0.51 make-color constant grey51
0.52 0.52 0.52 make-color constant gray52
0.52 0.52 0.52 make-color constant grey52
0.53 0.53 0.53 make-color constant gray53
0.53 0.53 0.53 make-color constant grey53
0.54 0.54 0.54 make-color constant gray54
0.54 0.54 0.54 make-color constant grey54
0.55 0.55 0.55 make-color constant gray55
0.55 0.55 0.55 make-color constant grey55
0.56 0.56 0.56 make-color constant gray56
0.56 0.56 0.56 make-color constant grey56
0.57 0.57 0.57 make-color constant gray57
0.57 0.57 0.57 make-color constant grey57
0.58 0.58 0.58 make-color constant gray58
0.58 0.58 0.58 make-color constant grey58
0.59 0.59 0.59 make-color constant gray59
0.59 0.59 0.59 make-color constant grey59
0.60 0.60 0.60 make-color constant gray60
0.60 0.60 0.60 make-color constant grey60
0.61 0.61 0.61 make-color constant gray61
0.61 0.61 0.61 make-color constant grey61
0.62 0.62 0.62 make-color constant gray62
0.62 0.62 0.62 make-color constant grey62
0.63 0.63 0.63 make-color constant gray63
0.63 0.63 0.63 make-color constant grey63
0.64 0.64 0.64 make-color constant gray64
0.64 0.64 0.64 make-color constant grey64
0.65 0.65 0.65 make-color constant gray65
0.65 0.65 0.65 make-color constant grey65
0.66 0.66 0.66 make-color constant gray66
0.66 0.66 0.66 make-color constant grey66
0.67 0.67 0.67 make-color constant gray67
0.67 0.67 0.67 make-color constant grey67
0.68 0.68 0.68 make-color constant gray68
0.68 0.68 0.68 make-color constant grey68
0.69 0.69 0.69 make-color constant gray69
0.69 0.69 0.69 make-color constant grey69
0.70 0.70 0.70 make-color constant gray70
0.70 0.70 0.70 make-color constant grey70
0.71 0.71 0.71 make-color constant gray71
0.71 0.71 0.71 make-color constant grey71
0.72 0.72 0.72 make-color constant gray72
0.72 0.72 0.72 make-color constant grey72
0.73 0.73 0.73 make-color constant gray73
0.73 0.73 0.73 make-color constant grey73
0.74 0.74 0.74 make-color constant gray74
0.74 0.74 0.74 make-color constant grey74
0.75 0.75 0.75 make-color constant gray75
0.75 0.75 0.75 make-color constant grey75
0.76 0.76 0.76 make-color constant gray76
0.76 0.76 0.76 make-color constant grey76
0.77 0.77 0.77 make-color constant gray77
0.77 0.77 0.77 make-color constant grey77
0.78 0.78 0.78 make-color constant gray78
0.78 0.78 0.78 make-color constant grey78
0.79 0.79 0.79 make-color constant gray79
0.79 0.79 0.79 make-color constant grey79
0.80 0.80 0.80 make-color constant gray80
0.80 0.80 0.80 make-color constant grey80
0.81 0.81 0.81 make-color constant gray81
0.81 0.81 0.81 make-color constant grey81
0.82 0.82 0.82 make-color constant gray82
0.82 0.82 0.82 make-color constant grey82
0.83 0.83 0.83 make-color constant gray83
0.83 0.83 0.83 make-color constant grey83
0.84 0.84 0.84 make-color constant gray84
0.84 0.84 0.84 make-color constant grey84
0.85 0.85 0.85 make-color constant gray85
0.85 0.85 0.85 make-color constant grey85
0.86 0.86 0.86 make-color constant gray86
0.86 0.86 0.86 make-color constant grey86
0.87 0.87 0.87 make-color constant gray87
0.87 0.87 0.87 make-color constant grey87
0.87 0.87 0.87 make-color constant gray88
0.87 0.87 0.87 make-color constant grey88
0.89 0.89 0.89 make-color constant gray89
0.89 0.89 0.89 make-color constant grey89
0.89 0.89 0.89 make-color constant gray90
0.89 0.89 0.89 make-color constant grey90
0.91 0.91 0.91 make-color constant gray91
0.91 0.91 0.91 make-color constant grey91
0.92 0.92 0.92 make-color constant gray92
0.92 0.92 0.92 make-color constant grey92
0.93 0.93 0.93 make-color constant gray93
0.93 0.93 0.93 make-color constant grey93
0.94 0.94 0.94 make-color constant gray94
0.94 0.94 0.94 make-color constant grey94
0.95 0.95 0.95 make-color constant gray95
0.95 0.95 0.95 make-color constant grey95
0.96 0.96 0.96 make-color constant gray96
0.96 0.96 0.96 make-color constant grey96
0.96 0.96 0.96 make-color constant gray97
0.96 0.96 0.96 make-color constant grey97
0.98 0.98 0.98 make-color constant gray98
0.98 0.98 0.98 make-color constant grey98
0.98 0.98 0.98 make-color constant gray99
0.98 0.98 0.98 make-color constant grey99
1.00 1.00 1.00 make-color constant gray100
1.00 1.00 1.00 make-color constant grey100
0.66 0.66 0.66 make-color constant dark-grey
0.66 0.66 0.66 make-color constant dark-gray
0.00 0.00 0.54 make-color constant dark-blue
0.00 0.54 0.54 make-color constant dark-cyan
0.54 0.00 0.54 make-color constant dark-magenta
0.54 0.00 0.00 make-color constant dark-red
0.56 0.93 0.56 make-color constant light-green

\ rgb.fs ends here
