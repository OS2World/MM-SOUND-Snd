#ifndef CLM2XEN_H
#define CLM2XEN_H

#include "vct.h"

typedef struct mus_xen {
  mus_any *gen;
  XEN *vcts; /* one for each accessible mus_float_t array (wrapped up here in a vct) */
  int nvcts;
  bool dont_free_gen;
  struct ptree *input_ptree; /* added 24-Apr-02 for run optimizer */
  struct ptree *edit_ptree;  /* ditto 26-Jul-04 */
  struct ptree *analyze_ptree;
  struct ptree *synthesize_ptree;
} mus_xen;

#define XEN_TO_MUS_XEN(arg) ((mus_xen *)XEN_OBJECT_REF(arg))
#define XEN_TO_MUS_ANY(obj) ((mus_any *)((XEN_TO_MUS_XEN(obj))->gen))
#define MUS_CLM_DEFAULT_TABLE_SIZE 512
#define MUS_CLM_DEFAULT_FREQUENCY 0.0

#ifdef __cplusplus
extern "C" {
#endif

MUS_EXPORT mus_long_t clm_default_table_size_c(void);
MUS_EXPORT double clm_default_frequency_c(void);

MUS_EXPORT bool mus_xen_p(XEN obj);
MUS_EXPORT const char *mus_fft_window_xen_name(mus_fft_window_t i);
MUS_EXPORT XEN mus_xen_to_object(mus_xen *gn);
MUS_EXPORT XEN mus_xen_to_object_with_vct(mus_xen *gn, XEN v);
MUS_EXPORT mus_any *mus_optkey_to_mus_any(XEN key, const char *caller, int n, mus_any *def);
MUS_EXPORT int mus_optkey_unscramble(const char *caller, int nkeys, XEN *keys, XEN *args, int *orig);
MUS_EXPORT mus_float_t mus_optkey_to_float(XEN key, const char *caller, int n, mus_float_t def);
MUS_EXPORT int mus_optkey_to_int(XEN key, const char *caller, int n, int def);
MUS_EXPORT bool mus_optkey_to_bool(XEN key, const char *caller, int n, bool def);
MUS_EXPORT mus_long_t mus_optkey_to_mus_long_t(XEN key, const char *caller, int n, mus_long_t def);
MUS_EXPORT const char *mus_optkey_to_string(XEN key, const char *caller, int n, char *def);
MUS_EXPORT XEN mus_optkey_to_procedure(XEN key, const char *caller, int n, XEN def, int required_args, const char *err);
MUS_EXPORT mus_xen *mus_any_to_mus_xen(mus_any *ge);
MUS_EXPORT mus_float_t mus_locsig_or_move_sound_to_vct_or_sound_data(mus_xen *ms, mus_any *loc_gen, mus_long_t pos, mus_float_t fval, bool from_locsig);
MUS_EXPORT mus_float_t *mus_vct_to_partials(vct *v, int *npartials, int *error_code);
MUS_EXPORT XEN mus_clm_output(void);
MUS_EXPORT XEN mus_clm_reverb(void);

MUS_EXPORT XEN g_mus_channels(XEN obj);
MUS_EXPORT XEN g_mus_length(XEN gen);
MUS_EXPORT XEN g_mus_file_name(XEN gen);
MUS_EXPORT XEN g_mus_data(XEN gen);

MUS_EXPORT void Init_sndlib(void);

#ifdef __cplusplus
}
#endif

#endif
