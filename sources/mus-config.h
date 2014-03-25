/* mus-config.h.  Generated from mus-config.h.in by configure.  */
#ifndef MUS_CONFIG_H_LOADED
#define MUS_CONFIG_H_LOADED

/* Define to `int' or something if <sys/types.h> doesn't define.  */
/* #undef mode_t */
/* #undef pid_t */
/* #undef size_t */
/* #undef ssize_t */
/* #undef mus_long_t */
/* #undef int64_t */

#define HAVE_GETCWD 1
#define HAVE_STRFTIME 1
#define HAVE_STRERROR 1
#define HAVE_READLINK 1
#define HAVE_GETLINE 1
#define HAVE_SETLOCALE 1
#define HAVE_SLEEP 1
#define HAVE_ACCESS 1
#define HAVE_OPENDIR 1
#define HAVE_SIGNAL 1
#define USE_STATVFS 1
/* #undef USE_STATFS */
#define HAVE_DIFFTIME 1
#define HAVE_GETTIMEOFDAY 1
#define HAVE_VSNPRINTF 1
#define HAVE_VASPRINTF 1
#define HAVE_SNPRINTF 1
#define HAVE_MEMMOVE 1
#define HAVE_LSTAT 1
#define HAVE_STRCASECMP 1
#define HAVE_PATHCONF 1
#define HAVE_DECL_HYPOT 1
#define HAVE_DECL_ISNAN 1

#define HAVE_COMPLEX_TRIG 1
#define WITH_COMPLEX 1

#define HAVE_SPECIAL_FUNCTIONS 1
/* #undef WORDS_BIGENDIAN */
/* #undef _FILE_OFFSET_BITS */
/* #undef _LARGE_FILES */
#define HAVE___FUNC__ 1
#define SIZEOF_OFF_T 8
#define SIZEOF_LONG 4
#define SIZEOF_LONG_LONG 8
#define SIZEOF_UNSIGNED_LONG 4
#define SIZEOF_VOID_P 4
#define SIZEOF_INT 4
#define SIZEOF_INT64_T 8
#define SIZEOF_SSIZE_T 4
/* #undef HAVE_READLINE */
#define HAVE_NESTED_FUNCTIONS 1

#define HAVE_FCNTL_H 1
#define HAVE_LIMITS_H 1
#define HAVE_STRING_H 1
#define HAVE_UNISTD_H 1
#define HAVE_STDBOOL_H 1
#define HAVE_DLFCN_H 1
/* #undef HAVE_SYS_SOUNDCARD_H */
/* #undef HAVE_MACHINE_SOUNDCARD_H */
/* #undef HAVE_SYS_MIXER_H */
/* #undef MUS_HAVE_USR_LIB_OSS */
/* #undef MUS_HAVE_USR_LOCAL_LIB_OSS */
/* #undef MUS_HAVE_OPT_OSS */
/* #undef MUS_HAVE_VAR_LIB_OSS */
/* #undef HAVE_LIBC_H */
#define HAVE_SETJMP_H 1
/* #undef HAVE_GNU_LIBC_VERSION_H */
#define HAVE_SYS_PARAM_H 1
/* #undef HAVE_SYS_MOUNT_H */
/* #undef HAVE_ALSA_ASOUNDLIB_H */
/* #undef HAVE_BYTESWAP_H */
#define HAVE_STDINT_H 1
/* #undef HAVE_FAM_H */
#define HAVE_SYS_TIME_H 1
#define HAVE_DIRENT_H 1
#define FGREP_PROG "U:/MOZTOOLS/grep.exe -F"

#define HAVE_PTHREAD_H 1

#define SND_PACKAGE "Snd"
#define SND_VERSION "12.4"
#define SND_HOST "i386-pc-os2-emx"

/* #undef HAVE_OSX */
/* #undef HAVE_WINDOZE */
/* #undef HAVE_SUN */
/* #undef HAVE_NETBSD */
/* #undef HAVE_LINUX */

/* #undef MUS_LINUX */
/* #undef MUS_SUN */
/* #undef MUS_OPENBSD */
/* #undef MUS_NETBSD */
/* #undef MUS_WINDOZE */
/* #undef HAVE_OSS */
/* #undef HAVE_ALSA */
/* #undef HAVE_JACK_IN_LINUX */
/* #undef MUS_JACK */
/* #undef MUS_MAC_OSX */
/* #undef MUS_ESD */
/* #undef MUS_HPUX */
/* #undef MUS_ESD_VERSION */
/* #undef MUS_AUDIOFILE_VERSION */
/* #undef HAVE_KAUDIODEVICEPROPERTYTRANSPORTTYPE */
/* #undef HAVE_KLINEARPCMFORMATFLAGISNONINTERLEAVED */
/* #undef HAVE_AUDIODEVICEDESTROYIOPROCID */
/* #undef MUS_JACK_VERSION */
/* #undef MUS_PULSEAUDIO */
/* #undef MUS_PORTAUDIO */

/* #undef HAVE_FFTW3 */
/* #undef WITH_GMP */

#define HAVE_GSL 1
#define MUS_GSL_VERSION "1.9"
#define HAVE_GSL_EIGEN_NONSYMMV_WORKSPACE 1

#define HAVE_SCHEME 1
/* #undef HAVE_RUBY */
/* #undef HAVE_FORTH */
#define HAVE_EXTENSION_LANGUAGE 1

/* #undef HAVE_FAM */
/* #undef MUS_GAMIN_VERSION */

/* #undef HAVE_GL */
/* #undef JUST_GL */
/* #undef HAVE_GLU */
/* #undef WITH_GL2PS */

/* #undef MUS_DEFAULT_TEMP_DIR */
/* #undef MUS_DEFAULT_SAVE_DIR */
/* #undef MUS_DEFAULT_DOC_DIR */
#define MUS_TRAP_SEGFAULT 1
/* #undef SND_AS_WIDGET */
#define mus_float_t double
#define mus_long_t int64_t
#define WITH_DOUBLES 1
#define SNDLIB_USE_FLOATS 1
/* #undef MUS_DEBUGGING */
/* #undef HAVE_LADSPA */
#define MUS_SAMPLE_BITS 24
/* #undef WITH_SHARED_SNDLIB */

/* #undef USE_GTK */
/* #undef USE_MOTIF */
#define USE_NO_GUI 1

/* #undef HAVE_X */
/* #undef HAVE_XmToolTipGetLabel */
/* #undef HAVE_XmCreateDataField */
/* #undef HAVE_XmCreateButtonBox */
/* #undef HAVE_XmCreateTabStack */
/* #undef HAVE_XmCreateDropDown */
/* #undef HAVE_XmCreateColumn */
/* #undef HAVE_XmCreateFontSelector */
/* #undef HAVE_XmCreateColorSelector */
/* #undef HAVE_STATIC_XM */
/* #undef MUS_WITH_EDITRES */
/* #undef HAVE_XSHAPEQUERYEXTENSION */

/* #undef MUS_RUBY_VERSION */
/* #undef RUBY_RELEASE_DATE */
/* #undef RUBY_SEARCH_PATH */
/* #undef HAVE_RB_PROC_NEW */

/* #undef WITH_RELATIVE_PANES */
#define HAVE_MAKE_RATIO 1
#define HAVE_MAKE_RECTANGULAR 1

/* #undef HAVE_GTK_TEST_WIDGET_CLICK */
/* #undef HAVE_GTK_ADJUSTMENT_GET_UPPER */
/* #undef HAVE_GTK_SCALE_ADD_MARK */
/* #undef HAVE_GTK_INFO_BAR_NEW */
/* #undef HAVE_GTK_STATUS_ICON_GET_TITLE */
/* #undef HAVE_GTK_WIDGET_GET_VISIBLE */
/* #undef HAVE_GTK_WIDGET_GET_MAPPED */
/* #undef HAVE_GTK_COMBO_BOX_NEW_WITH_AREA */
/* #undef HAVE_GTK_GRID_NEW */
/* #undef HAVE_GTK_ADJUSTMENT_GET_MINIMUM_INCREMENT */
/* #undef HAVE_GTK_FONT_CHOOSER_NEW */
/* #undef HAVE_GTK_3 */

/* #undef HAVE_CAIRO_GLYPH_ALLOCATE */
/* #undef HAVE_CAIRO_REGION_XOR */

/* #undef MUS_PANGO_VERSION */
/* #undef MUS_CAIRO_VERSION */

/* #undef XM_DISABLE_DEPRECATED */
/* #undef CLM_DISABLE_DEPRECATED */
/* #undef SNDLIB_DISABLE_DEPRECATED */
/* #undef XEN_DISABLE_DEPRECATED */
/* #undef S7_DISABLE_DEPRECATED */
/* #undef SND_DISABLE_DEPRECATED */

/* #undef WITH_PROFILING */

/* #undef HAVE_OGG */
/* #undef PATH_OGGDEC */
/* #undef PATH_OGGENC */
/* #undef HAVE_FLAC */
/* #undef PATH_FLAC */
/* #undef HAVE_SPEEX */
/* #undef PATH_SPEEXDEC */
/* #undef PATH_SPEEXENC */
/* #undef HAVE_MPEG */
/* #undef PATH_MPG123 */
/* #undef HAVE_TIMIDITY */
/* #undef PATH_TIMIDITY */
/* #undef HAVE_MPG321 */
/* #undef PATH_MPG321 */
/* #undef HAVE_SHORTEN */
/* #undef PATH_SHORTEN */
/* #undef HAVE_TTA */
/* #undef PATH_TTA */
/* #undef HAVE_WAVPACK */
/* #undef PATH_WAVPACK */
/* #undef PATH_WVUNPACK */

#ifndef USE_SND
  #define USE_SND 1
#endif

#ifdef _MSC_VER
  typedef long mus_long_t;
  #define ssize_t int 
  #define snprintf _snprintf 
  #define strtoll strtol
  #if _MSC_VER > 1200
    #define _CRT_SECURE_NO_DEPRECATE 1
    #define _CRT_NONSTDC_NO_DEPRECATE 1
    #define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1
  #endif
#endif

#endif
