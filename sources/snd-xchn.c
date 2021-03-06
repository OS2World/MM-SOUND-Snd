#include "snd.h"

enum {W_top, W_form, W_main_window, W_edhist, W_wf_buttons, W_f, W_w, W_left_scrollers, W_zy, W_sy,
      W_bottom_scrollers, W_sx, W_zx, W_graph, W_gzy, W_gsy,
      NUM_CHAN_WIDGETS
};


#if ((XmVERSION >= 2) && (XmREVISION >= 3))
  #define DEFAULT_EDIT_HISTORY_WIDTH 2
#else
  #define DEFAULT_EDIT_HISTORY_WIDTH 1
#endif


Widget channel_main_pane(chan_info *cp)
{
  if (cp) return(cp->chan_widgets[W_form]);
  return(NULL);
}


Widget channel_graph(chan_info *cp)      {return(cp->chan_widgets[W_graph]);}
static Widget channel_sx(chan_info *cp)  {return(cp->chan_widgets[W_sx]);}
static Widget channel_sy(chan_info *cp)  {return(cp->chan_widgets[W_sy]);}
static Widget channel_zx(chan_info *cp)  {return(cp->chan_widgets[W_zx]);}
static Widget channel_zy(chan_info *cp)  {return(cp->chan_widgets[W_zy]);}
static Widget channel_gsy(chan_info *cp) {return(cp->chan_widgets[W_gsy]);}
static Widget channel_gzy(chan_info *cp) {return(cp->chan_widgets[W_gzy]);}
Widget channel_w(chan_info *cp)          {return(cp->chan_widgets[W_w]);}
Widget channel_f(chan_info *cp)          {return(cp->chan_widgets[W_f]);}


bool channel_graph_is_visible(chan_info *cp)
{
  return((cp) &&
	 (channel_graph(cp)) &&
	 (XtIsManaged(channel_graph(cp))) &&
	 (cp->sound) &&
	 /* here we may have a sound wrapper for variable display in which case the sound widgets are not implemented */
	 (((cp->sound->inuse == SOUND_WRAPPER) || (cp->sound->inuse == SOUND_REGION)) ||
	  ((cp->sound->inuse == SOUND_NORMAL) &&
	   /* other choice: SOUND_IDLE -> no display */
	   (w_snd_pane(cp->sound)) &&
	   (XtIsManaged(w_snd_pane(cp->sound))))));
}


#define EDIT_HISTORY_LIST(Cp) (Cp)->chan_widgets[W_edhist]


static mus_float_t sqr(mus_float_t a) {return(a * a);}

static mus_float_t cube(mus_float_t a) {return(a * a * a);}


static mus_float_t get_scrollbar(Widget w, int val, int scrollbar_max)
{
  int size;
  if (val == 0) return(0.0);
  XtVaGetValues(w, XmNsliderSize, &size, NULL);
  return((mus_float_t)val / (mus_float_t)(scrollbar_max - size));
}


static void sy_changed(int value, chan_info *cp)
{
  axis_info *ap;
  mus_float_t low;
  ap = cp->axis;
  low = get_scrollbar(channel_sy(cp), value, SCROLLBAR_MAX);
  ap->sy = (1.0 - ap->zy) * low;
  apply_y_axis_change(cp);
}


static void sx_changed(int value, chan_info *cp)
{
  /* treat as centered with non-slider trough as defining current bounds */
  axis_info *ap;
  double low;
  ap = cp->axis;
  low = get_scrollbar(channel_sx(cp), value, SCROLLBAR_MAX);
  ap->sx = low * (1.0 - ap->zx);
  apply_x_axis_change(cp);
}


static void zy_changed(int value, chan_info *cp)
{ 
  axis_info *ap;
  mus_float_t old_zy;
  ap = cp->axis;
  if (value < 1) value = 1;
  old_zy = ap->zy;
  ap->zy = sqr(get_scrollbar(channel_zy(cp), value, SCROLLBAR_MAX));
  if (ap->zy < 1e-5) ap->zy = 1e-5;   /* if this goes to 0, X can hang */
  ap->sy += (.5 * (old_zy - ap->zy)); /* try to keep wave centered */
  if (ap->sy < 0) ap->sy = 0;
  apply_y_axis_change(cp);
  resize_sy(cp);
}


#define X_RANGE_CHANGEOVER 20.0

static void zx_changed(int value, chan_info *cp)
{ /* scrollbar change */
  axis_info *ap;
  static int old_zx_value = -1;
  #define ZX_MIN 20

  if (value < ZX_MIN) value = ZX_MIN; /* less than this causes underflow in snd-axis describe_ticks */
                                      /* snd-gchn uses .01 -- its equivalent here would be 100 */
                                      /* perhaps the definition should be ((int)(0.002 * MAX_SCROLLBAR)) */
  if (old_zx_value == value) return;  /* try to keep click on slider from moving the window! */
  old_zx_value = value;

  ap = cp->axis;
  if (ap->xmax == 0.0) return;
  if (ap->xmax <= ap->xmin) 
    {
      ap->xmax = ap->xmin + .001;
      ap->x_ambit = .001;
    }
  if (ap->x_ambit < X_RANGE_CHANGEOVER)
    ap->zx = sqr(get_scrollbar(channel_zx(cp), value, SCROLLBAR_MAX));
  else ap->zx = cube(get_scrollbar(channel_zx(cp), value, SCROLLBAR_MAX));
  /* if cursor visible, focus on that, else selection, else mark, else left side */
  focus_x_axis_change(cp, zoom_focus_style(ss));
  resize_sx(cp);
}


static void set_scrollbar(Widget w, mus_float_t position, mus_float_t range, int scrollbar_max) /* position and range 0 to 1.0 */
{
  int size, val;
  size = (int)(scrollbar_max * range);
  if (size > scrollbar_max) 
    size = scrollbar_max; /* this can't happen!?! */
  if (size < 1) size = 1;
  val = (int)(scrollbar_max * position);
  if ((val + size) > scrollbar_max) val = scrollbar_max - size;
  if (val < 0) val = 0;
  XtVaSetValues(w,
		XmNsliderSize, size,
		XmNvalue, val,
		NULL);
}


static void change_gzy_1(mus_float_t val, chan_info *cp)
{
  mus_float_t chan_frac, new_gsy, new_size;
  cp->gzy = val;
  chan_frac = 1.0 / ((mus_float_t)(((snd_info *)(cp->sound))->nchans));
  new_size = chan_frac + ((1.0 - chan_frac) * cp->gzy);
  if ((cp->gsy + new_size) > 1.0) 
    new_gsy = 1.0 - new_size; 
  else new_gsy = cp->gsy;
  if (new_gsy < 0.0) new_gsy = 0.0;
  set_scrollbar(channel_gsy(cp), new_gsy, new_size, SCROLLBAR_MAX);
}


static void gzy_changed(int value, chan_info *cp)
{
  change_gzy_1(get_scrollbar(channel_gzy(cp), value, SCROLLBAR_MAX), cp);
  for_each_sound_chan(cp->sound, update_graph_or_warn);
}


void change_gzy(mus_float_t val, chan_info *cp)
{
  change_gzy_1(val, cp);
  set_scrollbar(channel_gzy(cp), val, 1.0 / (mus_float_t)(cp->sound->nchans), SCROLLBAR_MAX);
}


static void gsy_changed(int value, chan_info *cp)
{
  mus_float_t low;
  low = get_scrollbar(channel_gsy(cp), value, SCROLLBAR_MAX);
  cp->gsy = (1.0 - cp->gzy) * low;
  for_each_sound_chan(cp->sound, update_graph_or_warn);
}


mus_float_t gsy_value(chan_info *cp)
{
  Widget wcp;
  int ival;
  wcp = channel_gsy(cp);
  XtVaGetValues(wcp, XmNvalue, &ival, NULL);
  return((mus_float_t)ival / (mus_float_t)(SCROLLBAR_MAX));
}


mus_float_t gsy_size(chan_info *cp)
{
  Widget wcp;
  int ival;
  wcp = channel_gsy(cp);
  XtVaGetValues(wcp, XmNsliderSize, &ival, NULL);
  return((mus_float_t)ival / (mus_float_t)(SCROLLBAR_MAX));
}


static void set_zx_scrollbar(chan_info *cp, axis_info *ap)
{
  if (ap->x_ambit < X_RANGE_CHANGEOVER)
    set_scrollbar(channel_zx(cp), sqrt(ap->zx), .1, SCROLLBAR_MAX);  /* assume size is 10% of scrollbar length */
  else set_scrollbar(channel_zx(cp), pow(ap->zx, .333), .1, SCROLLBAR_MAX);
}


void set_z_scrollbars(chan_info *cp, axis_info *ap)
{
  set_zx_scrollbar(cp, ap);
  set_scrollbar(channel_zy(cp), sqrt(ap->zy), .1, SCROLLBAR_MAX);
}


void initialize_scrollbars(chan_info *cp)
{
  axis_info *ap;
  snd_info *sp;

  cp->gzy = 1.0;
  cp->gsy = 1.0;

  ap = cp->axis;
  sp = cp->sound;

  set_scrollbar(channel_sx(cp), ap->sx, ap->zx, SCROLLBAR_MAX);
  set_scrollbar(channel_sy(cp), ap->sy, ap->zy, SCROLLBAR_MAX);
  set_z_scrollbars(cp, ap);

  if ((sp->nchans > 1) && 
      (cp->chan == 0) && 
      (channel_gsy(cp)))
    {
      set_scrollbar(channel_gsy(cp), 1.0, 1.0, SCROLLBAR_MAX);
      set_scrollbar(channel_gzy(cp), 1.0, 1.0 / (mus_float_t)(sp->nchans), SCROLLBAR_MAX);
    }
}


void resize_sy(chan_info *cp)
{
  /* something changed the y axis view, so the scale scroller needs to reflect that change (in size and position) */
  axis_info *ap;
  ap = cp->axis;
  if (ap->y_ambit != 0.0)
    set_scrollbar(channel_sy(cp),
		  (ap->y0 - ap->ymin) / ap->y_ambit,
		  (ap->y1 - ap->y0) / ap->y_ambit,
		  SCROLLBAR_MAX);
}


void resize_sy_and_zy(chan_info *cp)
{
  resize_sy(cp);
  set_scrollbar(channel_zy(cp), sqrt(cp->axis->zy), .1, SCROLLBAR_MAX);  
}


void resize_sx(chan_info *cp)
{
  axis_info *ap;
  ap = cp->axis;
  if (ap->x_ambit != 0.0)
    set_scrollbar(channel_sx(cp),
		  (ap->x0 - ap->xmin) / ap->x_ambit,
		  (ap->x1 - ap->x0) / ap->x_ambit,
		  SCROLLBAR_MAX);
}


void resize_sx_and_zx(chan_info *cp)
{
  resize_sx(cp);
  /* change zx position (not its size) */
  set_zx_scrollbar(cp, cp->axis);
}


void channel_open_pane(chan_info *cp)
{
  XtManageChild(channel_main_pane(cp));
}


void channel_unlock_pane(chan_info *cp)
{
  XtVaSetValues(channel_main_pane(cp),
		XmNpaneMinimum, 5,
		XmNpaneMaximum, LOTSA_PIXELS,
		NULL);
}


static void channel_lock_pane(chan_info *cp, int val)
{
  if (val < 6) val = 6;
  XtUnmanageChild(channel_main_pane(cp));
  XtVaSetValues(channel_main_pane(cp),
		XmNpaneMinimum, val - 5,
		XmNpaneMaximum, val + 5,
		NULL);
}


static void sy_drag_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    sy_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}


static void sy_valuechanged_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    sy_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}


static void sx_drag_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    sx_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}


static void sx_valuechanged_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    sx_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}


static void sx_increment_callback(Widget w, XtPointer context, XtPointer info) 
{
  /* problem here is that in large files these increments, if determined via scrollbar values, are huge */
  /* so, move ahead one-tenth window on each tick */
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    sx_incremented(cp, 0.1);
}


static void sx_decrement_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    sx_incremented(cp, -0.1);
}


static void zy_drag_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    zy_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}


static void zy_valuechanged_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    zy_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}


static void zx_drag_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    zx_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}


/* can't use value changed callback in scrollbars because they are called upon mouse release
 *   even when nothing changed, and the value passed appears to be the right edge of the
 *   slider -- this is too unpredictable, and the result is the window moves when the user
 *   did not want it to.
 */


static void gzy_drag_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    gzy_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}


static void gsy_drag_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    gsy_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}


static void gsy_valuechanged_callback(Widget w, XtPointer context, XtPointer info) 
{
  chan_info *cp = (chan_info *)(context);
  if (cp->active == CHANNEL_HAS_AXES)
    gsy_changed(((XmScrollBarCallbackStruct *)info)->value, cp);
}

/* anything special for increment?  XmNincrementCallback sx_increment_callback */


static void f_toggle_callback(Widget w, XtPointer context, XtPointer info)
{
  XmToggleButtonCallbackStruct *cb = (XmToggleButtonCallbackStruct *)info;
  XButtonEvent *ev;
  ev = (XButtonEvent *)(cb->event);
  f_button_callback((chan_info *)context, cb->set, (ev->state & snd_ControlMask));
}


static void w_toggle_callback(Widget w, XtPointer context, XtPointer info)
{
  XmToggleButtonCallbackStruct *cb = (XmToggleButtonCallbackStruct *)info;
  XButtonEvent *ev;
  ev = (XButtonEvent *)(cb->event);
  w_button_callback((chan_info *)context, cb->set, (ev->state & snd_ControlMask));
}


static void channel_expose_callback(Widget w, XtPointer context, XtPointer info)
{
  static oclock_t last_expose_event_time = 0;
  static chan_info *last_cp = NULL;
  snd_info *sp;
  chan_info *cp = (chan_info *)context;
  XmDrawingAreaCallbackStruct *cb = (XmDrawingAreaCallbackStruct *)info;
  XExposeEvent *ev;
  oclock_t curtime;

  if ((cp == NULL) || (cp->active < CHANNEL_HAS_AXES) || (cp->sound == NULL)) return;

  ev = (XExposeEvent *)(cb->event);

  /* if multiple channels/sounds displayed, each gets an expose event, but the earlier ones
   *   have count>0, and the last can get more than 1, causing our notion of last_cp to
   *   be useless, and we'll drop the earlier ones anyway, so if cp != last_cp, expose
   *   last_cp if last_count>0 or times equal (not sure which is safest).
   */

  curtime = XtLastTimestampProcessed(MAIN_DISPLAY(ss));

  if ((ev->count == 0) ||
      (last_expose_event_time != curtime) ||
      (cp != last_cp))
    {
      sp = cp->sound;
      if (sp->channel_style != CHANNELS_SEPARATE)
	{
	  if ((cp->chan == 0) && (ev->width > 10) && (ev->height > 10))
	    for_each_sound_chan(sp, update_graph_or_warn);
	}
      else update_graph_or_warn(cp);
    }

  last_cp = cp;
  last_expose_event_time = curtime;
}


static void channel_resize_callback(Widget w, XtPointer context, XtPointer info)
{
  channel_resize((chan_info *)context);
}


static XEN mouse_enter_graph_hook;
static XEN mouse_leave_graph_hook;

static void graph_mouse_enter(Widget w, XtPointer context, XEvent *event, Boolean *flag)
{
  pointer_or_int_t data;
  XEnterWindowEvent *ev = (XEnterWindowEvent *)event;

  if (with_pointer_focus(ss))
    goto_window(w);

  XtVaGetValues(w, XmNuserData, &data, NULL);
  if (XEN_HOOKED(mouse_enter_graph_hook))
    run_hook(mouse_enter_graph_hook,
	     XEN_LIST_2(C_INT_TO_XEN_SOUND(UNPACK_SOUND(data)),
			C_TO_XEN_INT(UNPACK_CHANNEL(data))),
	     S_mouse_enter_graph_hook);

  check_cursor_shape((chan_info *)context, ev->x, ev->y);
}


static void graph_mouse_leave(Widget w, XtPointer context, XEvent *event, Boolean *flag)
{
  pointer_or_int_t data;
  XLeaveWindowEvent *ev = (XLeaveWindowEvent *)event;

  XtVaGetValues(w, XmNuserData, &data, NULL);
  if (XEN_HOOKED(mouse_leave_graph_hook))
    run_hook(mouse_leave_graph_hook,
	     XEN_LIST_2(C_INT_TO_XEN_SOUND(UNPACK_SOUND(data)),
			C_TO_XEN_INT(UNPACK_CHANNEL(data))),
	     S_mouse_leave_graph_hook);

  /*
  XUndefineCursor(XtDisplay(w), XtWindow(w));
  */
  check_cursor_shape((chan_info *)context, ev->x, ev->y);
}


static void graph_button_press(Widget w, XtPointer context, XEvent *event, Boolean *cont) 
{
  XButtonEvent *ev = (XButtonEvent *)event;
  graph_button_press_callback((chan_info *)context, (void *)ev, ev->x, ev->y, ev->state, ev->button, ev->time);
}


static void graph_button_release(Widget w, XtPointer context, XEvent *event, Boolean *cont) /* cont = "continue to dispatch" */
{
  XButtonEvent *ev = (XButtonEvent *)event;
  graph_button_release_callback((chan_info *)context, ev->x, ev->y, ev->state, ev->button);
}

#if 0
static void graph_button_motion(Widget w, XtPointer context, XEvent *event, Boolean *cont) 
{ /* mouse drag */
  XMotionEvent *ev = (XMotionEvent *)event;
  graph_button_motion_callback((chan_info *)context, ev->x, ev->y, ev->time);
}
#endif


static void graph_mouse_motion(Widget w, XtPointer context, XEvent *event, Boolean *cont) 
{ /* mouse movement */
  XMotionEvent *ev = (XMotionEvent *)event;
  if ((ev->state & Button1Mask) == 0)
    check_cursor_shape((chan_info *)context, ev->x, ev->y);
  else graph_button_motion_callback((chan_info *)context, ev->x, ev->y, ev->time);
}


static int no_padding(Arg *args, int n)
{
  XtSetArg(args[n], XmNmarginHeight, 0); n++;
  XtSetArg(args[n], XmNmarginWidth, 0); n++;
  XtSetArg(args[n], XmNmarginTop, 0); n++;
  XtSetArg(args[n], XmNmarginBottom, 0); n++;
  XtSetArg(args[n], XmNmarginLeft, 0); n++;
  XtSetArg(args[n], XmNmarginRight, 0); n++;
  return(n);
}


static void hide_gz_scrollbars(snd_info *sp)
{
  Widget w;
  w = channel_gsy(sp->chans[0]);
  if ((w) && (XtIsManaged(w))) XtUnmanageChild(w);
  w = channel_gzy(sp->chans[0]);
  if ((w) && (XtIsManaged(w))) XtUnmanageChild(w);
}


static void show_gz_scrollbars(snd_info *sp)
{
  Widget w;
  w = channel_gsy(sp->chans[0]);
  if ((w) && (!XtIsManaged(w))) XtManageChild(w);
  w = channel_gzy(sp->chans[0]);
  if ((w) && (!XtIsManaged(w))) XtManageChild(w);
}


/* edit history support */

static void history_select_callback(Widget w, XtPointer context, XtPointer info) 
{
  /* undo/redo to reach selected position */
  XmListCallbackStruct *cbs = (XmListCallbackStruct *)info;
  edit_history_select((chan_info *)context, cbs->item_position - 1);
}


#if WITH_RELATIVE_PANES
#include <Xm/SashP.h>

/* using undocumented callback here, as in snd-xsnd.c */
static void remake_edit_history(Widget lst, chan_info *cp, int from_graph)
{
  snd_info *sp;
  int i, eds;
  XmString *edits;

  if (cp->squelch_update) return;
  XmListDeleteAllItems(lst);
  sp = cp->sound;

  if (sp->channel_style != CHANNELS_SEPARATE)
    {
      chan_info *ncp;
      int k, all_eds = 0, ed, filelen;
      char *title;

      for (k = 0; k < sp->nchans; k++)
	{
	  ncp = sp->chans[k];
	  eds = ncp->edit_ctr;
	  while ((eds < (ncp->edit_size - 1)) && (ncp->edits[eds + 1])) eds++;
	  all_eds += eds;
	}
      all_eds += 3 * sp->nchans;
      edits = (XmString *)calloc(all_eds, sizeof(XmString));
      filelen = 16 + strlen(sp->filename);
      title = (char *)calloc(filelen, sizeof(char));
      for (k = 0, ed = 0; k < sp->nchans; k++)
	{
	  ncp = sp->chans[k];
	  ncp->edhist_base = ed;
	  snprintf(title, filelen, "chan %d: %s", k + 1, sp->filename);
	  edits[ed++] = XmStringCreateLocalized(title);
	  eds = ncp->edit_ctr;
	  while ((eds < (ncp->edit_size - 1)) && (ncp->edits[eds + 1])) eds++;
	  for (i = 1; i <= eds; i++)
	    {
	      char *temp;
	      temp = edit_to_string(ncp, i);
	      edits[ed++] = XmStringCreateLocalized(temp);
	      free(temp);
	    }
	  if (k < sp->nchans - 1)
	    edits[ed++] = XmStringCreateLocalized((char *)"______________________________");
	}
      free(title);
      XtVaSetValues(lst, 
		    XmNitems, edits, 
		    XmNitemCount, ed, 
		    NULL);
      for (i = 0; i < ed; i++) 
	XmStringFree(edits[i]);
      free(edits);
      XmListSelectPos(lst, cp->edhist_base + cp->edit_ctr + 1, false);
      if (from_graph) goto_graph(cp);
    }
  else
    {
      int items = 0;
      eds = cp->edit_ctr;
      while ((eds < (cp->edit_size - 1)) && (cp->edits[eds + 1])) eds++;
      edits = (XmString *)calloc(eds + 1, sizeof(XmString));
      edits[0] = XmStringCreateLocalized(sp->filename);
      for (i = 1; i <= eds; i++)
	{
	  char *temp;
	  temp = edit_to_string(cp, i);
	  edits[i] = XmStringCreateLocalized(temp);
	  free(temp);
	}
      XtVaSetValues(lst, 
		    XmNitems, edits, 
		    XmNitemCount, eds + 1, 
		    NULL);
      for (i = 0; i <= eds; i++) 
	XmStringFree(edits[i]);
      free(edits);
      XmListSelectPos(lst, cp->edit_ctr + 1, false);
      XtVaGetValues(lst, XmNvisibleItemCount, &items, NULL);
      if (items <= eds)
	XtVaSetValues(lst, XmNtopItemPosition, eds - items + 2, NULL);
      if (from_graph) goto_graph(cp);
    }
}


static void watch_edit_history_sash(Widget w, XtPointer closure, XtPointer info)
{
  SashCallData call_data = (SashCallData)info;
  /* call_data->params[0]: Commit, Move, Key, Start (as strings) */
  if ((call_data->params) && 
      (mus_strcmp(call_data->params[0], "Start")))
    {
      chan_info *cp = (chan_info *)closure;
      Widget edhist;
      if ((cp) && (cp->chan_widgets))
	{
	  edhist = EDIT_HISTORY_LIST(cp);
	  if (edhist)
	    remake_edit_history(edhist, cp, false);
	}
    }
}
#endif


void reflect_edit_history_change(chan_info *cp)
{
  /* new edit so it is added, and any trailing lines removed */
  snd_info *sp;
  Widget lst;

  if (cp->squelch_update) return;
  if (cp->in_as_one_edit > 0) return;
  sp = cp->sound;
  lst = EDIT_HISTORY_LIST(cp);
#if WITH_RELATIVE_PANES
  if ((lst) && (widget_width(lst) > 1))
    remake_edit_history(lst, cp, true);
  else
    {
      if ((cp->chan > 0) && (sp->channel_style != CHANNELS_SEPARATE))
	{
	  lst = EDIT_HISTORY_LIST(sp->chans[0]);
	  if ((lst) && (widget_width(lst) > 1))
	    remake_edit_history(lst, sp->chans[0], true);
	}
    }
#else
  /* old form */
  if (lst)
    {
      int i, eds, items = 0;
      XmString *edits;
      eds = cp->edit_ctr;
      while ((eds < (cp->edit_size - 1)) && (cp->edits[eds + 1])) eds++;
      if (eds >= 0)
	{
	  if ((eds == cp->edit_ctr) && (eds > 1)) /* need to force 0 (1) case to start list with sound file name */
	    {
	      XmString edit;
	      /* special common case -- we're appending a new edit description */
	      XtVaGetValues(lst, XmNitemCount, &items, NULL);
	      if (items > eds )
		XmListDeleteItemsPos(lst, cp->edit_size, eds + 1); 
	      /* cp->edit_size is too large, but the manual says this is the way to delete to the end */
	      {
		char *temp;
		temp = edit_to_string(cp, eds);
		edit = XmStringCreateLocalized(temp);
		free(temp);
	      }
	      XmListAddItemUnselected(lst, edit, eds + 1);
	      XmStringFree(edit);
	    }
	  else
	    {
	      edits = (XmString *)calloc(eds + 1, sizeof(XmString));
	      edits[0] = XmStringCreateLocalized(sp->filename);
	      for (i = 1; i <= eds; i++) 
		{
		  char *temp;
		  temp = edit_to_string(cp, i);
		  edits[i] = XmStringCreateLocalized(temp);
		  free(temp);
		}
	      XtVaSetValues(lst, 
			    XmNitems, edits, 
			    XmNitemCount, eds + 1, 
			    NULL);
	      for (i = 0; i <= eds; i++) 
		XmStringFree(edits[i]);
	      free(edits);
	    }
	  XmListSelectPos(lst, cp->edit_ctr + 1, false);
	  XtVaGetValues(lst, XmNvisibleItemCount, &items, NULL);
	  if (items <= eds)
	    XtVaSetValues(lst, XmNtopItemPosition, eds - items + 2, NULL);
	  goto_graph(cp);
	}
    }
#endif
}


void reflect_edit_counter_change(chan_info *cp)
{
  /* undo/redo/revert -- change which line is highlighted */
  Widget lst;

  if (cp->squelch_update) return;
  lst = EDIT_HISTORY_LIST(cp);
  if ((lst) && (widget_width(lst) > 1))
    {
      int len, top;
      XmListSelectPos(lst, cp->edit_ctr + 1, false);
      XtVaGetValues(lst, 
		    XmNvisibleItemCount, &len, 
		    XmNtopItemPosition, &top, 
		    NULL);
      if ((cp->edit_ctr + 1) < top) 
	XtVaSetValues(lst, XmNtopItemPosition, cp->edit_ctr + 1, NULL);
      else
	if ((cp->edit_ctr + 1) >= (top + len))
	  XtVaSetValues(lst, XmNtopItemPosition, cp->edit_ctr, NULL);
      goto_graph(cp);
    }
}


/* for combined cases, the incoming chan_info pointer is always chan[0], 
 * but the actual channel depends on placement if mouse oriented.
 * virtual_selected_channel(cp) (snd-chn.c) retains the current selected channel
 */

void graph_key_press(Widget w, XtPointer context, XEvent *event, Boolean *cont) 
{
  XKeyEvent *ev = (XKeyEvent *)event;
  KeySym keysym;
  int key_state;
  snd_info *sp = (snd_info *)context;
  key_state = ev->state;
  keysym = XKeycodeToKeysym(XtDisplay(w),
			    (int)(ev->keycode),
			    (key_state & snd_ShiftMask) ? 1 : 0);
  key_press_callback(any_selected_channel(sp), ev->x, ev->y, ev->state, keysym);
}
 

static void cp_graph_key_press(Widget w, XtPointer context, XEvent *event, Boolean *cont) 
{
  /* called by every key-intercepting widget in the entire sound pane */
  XKeyEvent *ev = (XKeyEvent *)event;
  KeySym keysym;
  int key_state;
  chan_info *cp = (chan_info *)context;
  if ((cp == NULL) || (cp->sound == NULL)) return; /* can't happen */
  key_state = ev->state;
  keysym = XKeycodeToKeysym(XtDisplay(w),
			    (int)(ev->keycode),
			    (key_state & snd_ShiftMask) ? 1 : 0);
  key_press_callback(cp, ev->x, ev->y, ev->state, keysym);
}


static void channel_drop_watcher(Widget w, const char *str, Position x, Position y, void *context)
{
  pointer_or_int_t data;
  data = (pointer_or_int_t)context;
  drag_and_drop_mix_at_x_y((int)data, str, x, y);
}


static void channel_drag_watcher(Widget w, const char *str, Position x, Position y, drag_style_t dtype, void *context)
{
  int snd, chn;
  pointer_or_int_t data;
  snd_info *sp;
  chan_info *cp;
  float seconds;
  XtVaGetValues(w, XmNuserData, &data, NULL);
  chn = UNPACK_CHANNEL(data);
  snd = UNPACK_SOUND(data);

  sp = ss->sounds[snd];
  if (snd_ok(sp))
    {
      switch (dtype)
	{
	case DRAG_ENTER:
	case DRAG_MOTION:
	  cp = sp->chans[chn];
	  if ((sp->nchans > 1) && (sp->channel_style == CHANNELS_COMBINED))
	    cp = which_channel(sp, y);    
	  seconds = (float)(ungrf_x(cp->axis, x));
	  if (seconds < 0.0) seconds = 0.0;
	  if (sp->nchans > 1)
	    report_in_minibuffer(sp, "drop to mix file in chan %d at %.4f", cp->chan + 1, seconds);
	  else report_in_minibuffer(sp, "drop to mix file at %.4f", seconds);
	  break;

	case DRAG_LEAVE:
	  string_to_minibuffer(sp, " "); /* not clear_minibuffer here! => segfault */
	  break;
	}
    }
}


int add_channel_window(snd_info *sp, int channel, int chan_y, int insertion, Widget main, fw_button_t button_style, bool with_events)
{
  int i;
  bool need_extra_scrollbars;
  Widget *cw;
  chan_info *cp;
  graphics_context *cax;
  bool make_widgets;

  /* if ((main) && ((!(XmIsForm(main))) || (!(XtWindow(main))))) return(-1); */ /* new gcc complains about the XmIsForm for some reason */
  if ((main) && (!(XtWindow(main)))) return(-1); /* can this happen? */

  make_widgets = ((sp->chans[channel]) == NULL);
  sp->chans[channel] = make_chan_info(sp->chans[channel], channel, sp);
  cp = sp->chans[channel];

  if (cp->chan_widgets == NULL) 
    cp->chan_widgets = (Widget *)calloc(NUM_CHAN_WIDGETS, sizeof(Widget));
  cw = cp->chan_widgets;
  need_extra_scrollbars = ((!main) && (channel == 0));

  if (make_widgets)
    {
      XtCallbackList n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n14, n15;
      int n;
      Arg args[32];

      /* allocate the entire widget apparatus for this channel of this sound */

      if (!main)
	{
	  n = 0;
	  XtSetArg(args[n], XmNbackground, ss->basic_color); n++;
	  if (insertion) {XtSetArg(args[n], XmNpositionIndex, (short)channel); n++;}
	  XtSetArg(args[n], XmNpaneMinimum, chan_y); n++;

	  cw[W_form] = XtCreateManagedWidget("chn-form", xmFormWidgetClass, w_snd_pane(sp), args, n);
	  if ((sp->channel_style == CHANNELS_COMBINED) && (channel > 0)) XtUnmanageChild(cw[W_form]);

	  n = 0;
	  XtSetArg(args[n], XmNbackground, ss->basic_color); n++;
	  /* n = no_padding(args, n); */
	  n = attach_all_sides(args, n);
	  XtSetArg(args[n], XmNsashIndent, 2); n++;
	  XtSetArg(args[n], XmNorientation, XmHORIZONTAL); n++;
	  XtSetArg(args[n], XmNpaneMaximum, LOTSA_PIXELS); n++; 
	  cw[W_top] = XtCreateManagedWidget("chn-top", xmPanedWindowWidgetClass, cw[W_form], args, n);
	  XtAddEventHandler(cw[W_top], KeyPressMask, false, graph_key_press, (XtPointer)sp);

	  n = 0;
	  XtSetArg(args[n], XmNbackground, ss->white); n++;
	  XtSetArg(args[n], XmNpaneMaximum, DEFAULT_EDIT_HISTORY_WIDTH); n++;
	  XtSetArg(args[n], XmNlistSizePolicy, XmCONSTANT); n++;
	  cw[W_edhist] = XmCreateScrolledList(cw[W_top], (char *)"chn-edhist", args, n);
	  XtManageChild(cw[W_edhist]);

	  XtAddCallback(cw[W_edhist], XmNbrowseSelectionCallback, history_select_callback, cp);
	  XtAddEventHandler(cw[W_edhist], KeyPressMask, false, graph_key_press, (XtPointer)sp);
	  XtAddEventHandler(XtParent(cw[W_edhist]), KeyPressMask, false, graph_key_press, (XtPointer)sp);

	  n = 0;
	  XtSetArg(args[n], XmNbackground, ss->basic_color); n++;
	  XtSetArg(args[n], XmNpaneMaximum, LOTSA_PIXELS); n++;
	  cw[W_main_window] = XtCreateManagedWidget("chn-main-window", xmFormWidgetClass, cw[W_top], args, n);
	  XtAddEventHandler(cw[W_main_window], KeyPressMask, false, graph_key_press, (XtPointer)sp);

#if WITH_RELATIVE_PANES
	{
	  int k;
	  Widget child;
	  CompositeWidget w = (CompositeWidget)(cw[W_top]);
	  for (k = w->composite.num_children - 1; k >= 0; k--)
	    {
	      child = w->composite.children[k];
	      if ((XtIsWidget(child)) && 
		  (XtIsSubclass(child, xmSashWidgetClass)))
		{
		  XtAddCallback(child, XmNcallback, watch_edit_history_sash, (XtPointer)cp);
		  break; /* there seems to be more than 1?? */
		}
	    }
	}
#endif
	}
      else cw[W_main_window] = main;

      n = 0;  
      XtSetArg(args[n], XmNbackground, ss->basic_color); n++;
      XtSetArg(args[n], XmNtopAttachment, XmATTACH_NONE); n++;
      XtSetArg(args[n], XmNbottomAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNleftAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNrightAttachment, XmATTACH_NONE); n++;
      n = no_padding(args, n);
      XtSetArg(args[n], XmNpacking, XmPACK_COLUMN); n++;
      XtSetArg(args[n], XmNnumColumns, 1); n++;
      XtSetArg(args[n], XmNorientation, XmVERTICAL); n++;
      cw[W_wf_buttons] = XtCreateManagedWidget("chn-buttons", xmRowColumnWidgetClass, cw[W_main_window], args, n);	

      if (button_style == WITH_FW_BUTTONS)
	{
	  n = 0;
	  XtSetArg(args[n], XmNbackground, ss->basic_color); n++;
	  XtSetArg(args[n], XmNspacing, 1); n++;
	  XtSetArg(args[n], XmNselectColor, ss->selection_color); n++;
	  cw[W_f] = make_togglebutton_widget("f", cw[W_wf_buttons], args, n);
	  XtAddCallback(cw[W_f], XmNvalueChangedCallback, f_toggle_callback, cp);
	  XtAddEventHandler(cw[W_f], KeyPressMask, false, graph_key_press, (XtPointer)sp);
	  
	  XtSetArg(args[n], XmNset, true); n++;
	  cw[W_w] = make_togglebutton_widget("w", cw[W_wf_buttons], args, n);
	  XtAddCallback(cw[W_w], XmNvalueChangedCallback, w_toggle_callback, cp);
	  XtAddEventHandler(cw[W_w], KeyPressMask, false, graph_key_press, (XtPointer)sp);
	}
      else
	{
	  n = 0;
	  XtSetArg(args[n], XmNbackground, ss->basic_color); n++;
	  XtSetArg(args[n], XmNarrowDirection, XmARROW_UP); n++;
	  XtSetArg(args[n], XmNsensitive, false); n++;
	  cw[W_f] = XtCreateManagedWidget("up", xmArrowButtonWidgetClass, cw[W_wf_buttons], args, n);

	  n = 0;
	  XtSetArg(args[n], XmNbackground, ss->basic_color); n++;
	  XtSetArg(args[n], XmNarrowDirection, XmARROW_DOWN); n++;
	  XtSetArg(args[n], XmNsensitive, false); n++;
	  cw[W_w] = XtCreateManagedWidget("down", xmArrowButtonWidgetClass, cw[W_wf_buttons], args, n);
	}

      n = 0;
      XtSetArg(args[n], XmNbackground, ss->basic_color); n++;
      XtSetArg(args[n], XmNorientation, XmHORIZONTAL); n++;
      XtSetArg(args[n], XmNtopAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNbottomAttachment, XmATTACH_WIDGET); n++;
      XtSetArg(args[n], XmNbottomWidget, cw[W_wf_buttons]); n++;
      XtSetArg(args[n], XmNleftAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNrightAttachment, XmATTACH_NONE); n++;
      XtSetArg(args[n], XmNspacing, 0); n++;
      cw[W_left_scrollers] = XtCreateManagedWidget("chn-left", xmRowColumnWidgetClass, cw[W_main_window], args, n);
      XtAddEventHandler(cw[W_left_scrollers], KeyPressMask, false, graph_key_press, (XtPointer)sp);

      n = 0;
      XtSetArg(args[n], XmNbackground, ss->zoom_color); n++;
      XtSetArg(args[n], XmNwidth, ss->position_slider_width); n++;
      XtSetArg(args[n], XmNtopAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNbottomAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNleftAttachment, XmATTACH_FORM); n++; 
      XtSetArg(args[n], XmNrightAttachment, XmATTACH_NONE); n++;
      XtSetArg(args[n], XmNorientation, XmVERTICAL); n++;
      XtSetArg(args[n], XmNmaximum, SCROLLBAR_MAX); n++; 
      XtSetArg(args[n], XmNincrement, 1); n++;
      XtSetArg(args[n], XmNprocessingDirection, XmMAX_ON_TOP); n++;
      XtSetArg(args[n], XmNdragCallback, n1 = make_callback_list(zy_drag_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNvalueChangedCallback, n2 = make_callback_list(zy_valuechanged_callback, (XtPointer)cp)); n++;
      cw[W_zy] = XtCreateManagedWidget("chn-zy", xmScrollBarWidgetClass, cw[W_left_scrollers], args, n);
      XtAddEventHandler(cw[W_zy], KeyPressMask, false, graph_key_press, (XtPointer)sp);

      n = 0;
      XtSetArg(args[n], XmNbackground, ss->position_color); n++;
      XtSetArg(args[n], XmNwidth, ss->zoom_slider_width); n++;
      XtSetArg(args[n], XmNtopAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNbottomAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNleftAttachment, XmATTACH_WIDGET); n++;
      XtSetArg(args[n], XmNleftWidget, cw[W_zy]); n++;
      XtSetArg(args[n], XmNrightAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNorientation, XmVERTICAL); n++;
      XtSetArg(args[n], XmNmaximum, SCROLLBAR_MAX); n++;
      XtSetArg(args[n], XmNincrement, 1); n++;
      XtSetArg(args[n], XmNprocessingDirection, XmMAX_ON_TOP); n++;
      XtSetArg(args[n], XmNdragCallback, n3 = make_callback_list(sy_drag_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNvalueChangedCallback, n4 = make_callback_list(sy_valuechanged_callback, (XtPointer)cp)); n++;
      cw[W_sy] = XtCreateManagedWidget("chn-sy", xmScrollBarWidgetClass, cw[W_left_scrollers], args, n);
      XtAddEventHandler(cw[W_sy], KeyPressMask, false, graph_key_press, (XtPointer)sp);

      n = 0;
      XtSetArg(args[n], XmNbackground, ss->basic_color); n++;
      XtSetArg(args[n], XmNorientation, XmVERTICAL); n++;
      XtSetArg(args[n], XmNtopAttachment, XmATTACH_NONE); n++;
      XtSetArg(args[n], XmNbottomAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNleftAttachment, XmATTACH_WIDGET); n++;
      XtSetArg(args[n], XmNleftWidget, cw[W_wf_buttons]); n++;
      XtSetArg(args[n], XmNrightAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNspacing, 0); n++;
      cw[W_bottom_scrollers] = XtCreateManagedWidget("chn-bottom", xmRowColumnWidgetClass, cw[W_main_window], args, n);
      XtAddEventHandler(cw[W_bottom_scrollers], KeyPressMask, false, graph_key_press, (XtPointer)sp);

      n = 0;
      XtSetArg(args[n], XmNbackground, ss->position_color); n++;
      XtSetArg(args[n], XmNheight, ss->position_slider_width); n++;
      XtSetArg(args[n], XmNtopAttachment, XmATTACH_NONE); n++;
      XtSetArg(args[n], XmNbottomAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNleftAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNrightAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNorientation, XmHORIZONTAL); n++;
      XtSetArg(args[n], XmNmaximum, SCROLLBAR_MAX); n++;
      XtSetArg(args[n], XmNincrement, 1); n++;
      XtSetArg(args[n], XmNdragCallback, n5 = make_callback_list(sx_drag_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNincrementCallback, n6 = make_callback_list(sx_increment_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNdecrementCallback, n7 = make_callback_list(sx_decrement_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNvalueChangedCallback, n8 = make_callback_list(sx_valuechanged_callback, (XtPointer)cp)); n++;
      cw[W_sx] = XtCreateManagedWidget("chn-sx", xmScrollBarWidgetClass, cw[W_bottom_scrollers], args, n);
      XtAddEventHandler(cw[W_sx], KeyPressMask, false, graph_key_press, (XtPointer)sp);

      n = 0;
      XtSetArg(args[n], XmNbackground, ss->zoom_color); n++;
      XtSetArg(args[n], XmNheight, ss->zoom_slider_width + 2); n++;
      XtSetArg(args[n], XmNorientation, XmHORIZONTAL); n++;
      XtSetArg(args[n], XmNtopAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNbottomAttachment, XmATTACH_WIDGET); n++;
      XtSetArg(args[n], XmNbottomWidget, cw[W_sx]); n++;
      XtSetArg(args[n], XmNleftAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNrightAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNmaximum, SCROLLBAR_MAX); n++;
      XtSetArg(args[n], XmNincrement, 1); n++;
      XtSetArg(args[n], XmNdragCallback, n9 = make_callback_list(zx_drag_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNincrementCallback, n10 = make_callback_list(zx_drag_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNdecrementCallback, n11 = make_callback_list(zx_drag_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNpageIncrementCallback, n12 = make_callback_list(zx_drag_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNpageDecrementCallback, n13 = make_callback_list(zx_drag_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNtoTopCallback, n14 = make_callback_list(zx_drag_callback, (XtPointer)cp)); n++;
      XtSetArg(args[n], XmNtoBottomCallback, n15 = make_callback_list(zx_drag_callback, (XtPointer)cp)); n++;

      cw[W_zx] = XtCreateManagedWidget("chn-zx", xmScrollBarWidgetClass, cw[W_bottom_scrollers], args, n);
      XtAddEventHandler(cw[W_zx], KeyPressMask, false, graph_key_press, (XtPointer)sp);

      n = 0;
      XtSetArg(args[n], XmNbackground, ss->graph_color); n++;
      XtSetArg(args[n], XmNforeground, ss->data_color); n++;
      XtSetArg(args[n], XmNtopAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNbottomAttachment, XmATTACH_WIDGET); n++;
      XtSetArg(args[n], XmNbottomWidget, cw[W_bottom_scrollers]); n++;
      XtSetArg(args[n], XmNleftAttachment, XmATTACH_WIDGET); n++;
      XtSetArg(args[n], XmNleftWidget, cw[W_left_scrollers]); n++;
      XtSetArg(args[n], XmNrightAttachment, XmATTACH_FORM); n++;
      XtSetArg(args[n], XmNuserData, PACK_SOUND_AND_CHANNEL(sp->index, cp->chan)); n++;
      /* this collides with W_gzy below, but a consistent version came up with half a window blank */
      XtSetArg(args[n], XmNnavigationType, XmNONE); n++;
      cw[W_graph] = XtCreateManagedWidget("chn-graph", xmDrawingAreaWidgetClass, cw[W_main_window], args, n);

      if (with_events)
	{
	  /* region regraph sets up its own callbacks */
	  XtAddCallback(cw[W_graph], XmNresizeCallback, channel_resize_callback, (XtPointer)cp);
	  XtAddCallback(cw[W_graph], XmNexposeCallback, channel_expose_callback, (XtPointer)cp);
	}
      /* allow cursor in all cases (zoom to cursor in region window for example, or fft axis drag in variable display) */
      XtAddEventHandler(cw[W_graph], ButtonPressMask, false, graph_button_press, (XtPointer)cp);
      XtAddEventHandler(cw[W_graph], ButtonReleaseMask, false, graph_button_release, (XtPointer)cp);
      /* XtAddEventHandler(cw[W_graph], ButtonMotionMask, false, graph_button_motion, (XtPointer)cp); */
      XtAddEventHandler(cw[W_graph], PointerMotionMask, false, graph_mouse_motion, (XtPointer)cp);
      if (main == NULL)
	{
	  pointer_or_int_t data;
	  XtAddEventHandler(cw[W_graph], EnterWindowMask, false, graph_mouse_enter, (XtPointer)cp);
	  XtAddEventHandler(cw[W_graph], LeaveWindowMask, false, graph_mouse_leave, (XtPointer)cp);
	  XtAddEventHandler(cw[W_graph], KeyPressMask, false, cp_graph_key_press, (XtPointer)cp);

	  data = (pointer_or_int_t)PACK_SOUND_AND_CHANNEL(sp->index, cp->chan);
	  add_drag_and_drop(cw[W_graph], channel_drop_watcher, channel_drag_watcher, (void *)data);
	}

      free(n1);
      free(n2);
      free(n3);
      free(n4);
      free(n5);
      free(n6);
      free(n7);
      free(n8);
      free(n9);
      free(n10);
      free(n11);
      free(n12);
      free(n13);
      free(n14);
      free(n15);

      if (need_extra_scrollbars)
	{
	  /* that is: not region browser chan, might need combined graph, channel 0 is the controller in that case */
	  /* this is independent of sp->nchans because these structs are re-used and added to as needed */
	  n = 0;
	  XtSetArg(args[n], XmNbackground, ss->zoom_color); n++;
	  XtSetArg(args[n], XmNwidth, ss->position_slider_width); n++;
	  XtSetArg(args[n], XmNtopAttachment, XmATTACH_FORM); n++;
	  XtSetArg(args[n], XmNbottomAttachment, XmATTACH_WIDGET); n++;
	  XtSetArg(args[n], XmNbottomWidget, cw[W_bottom_scrollers]); n++;
	  XtSetArg(args[n], XmNleftAttachment, XmATTACH_NONE); n++;
	  XtSetArg(args[n], XmNrightAttachment, XmATTACH_FORM); n++;
	  XtSetArg(args[n], XmNorientation, XmVERTICAL); n++;
	  XtSetArg(args[n], XmNmaximum, SCROLLBAR_MAX); n++; 
	  XtSetArg(args[n], XmNincrement, 1); n++;
	  XtSetArg(args[n], XmNprocessingDirection, XmMAX_ON_TOP); n++;

	  XtSetArg(args[n], XmNincrementCallback, n1 = make_callback_list(gzy_drag_callback, (XtPointer)cp)); n++;
	  XtSetArg(args[n], XmNdecrementCallback, n2 = make_callback_list(gzy_drag_callback, (XtPointer)cp)); n++;
	  XtSetArg(args[n], XmNpageIncrementCallback, n3 = make_callback_list(gzy_drag_callback, (XtPointer)cp)); n++;
	  XtSetArg(args[n], XmNpageDecrementCallback, n4 = make_callback_list(gzy_drag_callback, (XtPointer)cp)); n++;
	  XtSetArg(args[n], XmNtoTopCallback, n5 = make_callback_list(gzy_drag_callback, (XtPointer)cp)); n++;
	  XtSetArg(args[n], XmNtoBottomCallback, n6 = make_callback_list(gzy_drag_callback, (XtPointer)cp)); n++;
	  XtSetArg(args[n], XmNdragCallback, n7 = make_callback_list(gzy_drag_callback, (XtPointer)cp)); n++;

	  cw[W_gzy] = XtCreateManagedWidget("chn-gzy", xmScrollBarWidgetClass, cw[W_main_window], args, n);
	  XtAddEventHandler(cw[W_gzy], KeyPressMask, false, graph_key_press, (XtPointer)sp);


	  n = 0;
	  XtSetArg(args[n], XmNbackground, ss->position_color); n++;
	  XtSetArg(args[n], XmNwidth, ss->position_slider_width); n++;
	  XtSetArg(args[n], XmNtopAttachment, XmATTACH_FORM); n++;
	  XtSetArg(args[n], XmNbottomAttachment, XmATTACH_WIDGET); n++;
	  XtSetArg(args[n], XmNbottomWidget, cw[W_bottom_scrollers]); n++;
	  XtSetArg(args[n], XmNleftAttachment, XmATTACH_NONE); n++;
	  XtSetArg(args[n], XmNrightAttachment, XmATTACH_WIDGET); n++;
	  XtSetArg(args[n], XmNrightWidget, cw[W_gzy]); n++;
	  XtSetArg(args[n], XmNorientation, XmVERTICAL); n++;
	  XtSetArg(args[n], XmNmaximum, SCROLLBAR_MAX); n++;
	  XtSetArg(args[n], XmNincrement, 1); n++;
	  XtSetArg(args[n], XmNprocessingDirection, XmMAX_ON_TOP); n++;
	  XtSetArg(args[n], XmNdragCallback, n8 = make_callback_list(gsy_drag_callback, (XtPointer)cp)); n++;
	  XtSetArg(args[n], XmNvalueChangedCallback, n9 = make_callback_list(gsy_valuechanged_callback, (XtPointer)cp)); n++;
	  cw[W_gsy] = XtCreateManagedWidget("chn-gsy", xmScrollBarWidgetClass, cw[W_main_window], args, n);
	  XtAddEventHandler(cw[W_gsy], KeyPressMask, false, graph_key_press, (XtPointer)sp);
	  
	  free(n1);
	  free(n2);
	  free(n3);
	  free(n4);
	  free(n5);
	  free(n6);
	  free(n7);

	  free(n8);
	  free(n9);
	}
      else
	{
	  cw[W_gsy] = NULL;
	  cw[W_gzy] = NULL;
	}
      run_new_widget_hook(cw[W_main_window]);
      /* also position of current graph in overall sound as window */

    } /* end alloc new chan */

  else

    { 
      /* re-manage currently inactive chan */
      XtVaSetValues(cw[W_main_window], XmNpaneMinimum, chan_y, NULL);
      if (cw[W_edhist]) 
	XtVaSetValues(XtParent(cw[W_edhist]), XmNpaneMaximum, 1, NULL);
      if ((sp->channel_style != CHANNELS_COMBINED) || 
	  (channel == 0))
	for (i = 0; i < NUM_CHAN_WIDGETS - 1; i++)
	  if ((cw[i]) && (!XtIsManaged(cw[i])))
	    XtManageChild(cw[i]);
      recolor_graph(cp, false); /* in case selection color left over from previous use */
    }

  if (cw[W_edhist]) 
    XtVaSetValues(XtParent(cw[W_edhist]), XmNpaneMaximum, LOTSA_PIXELS, NULL);

  if ((need_extra_scrollbars) && 
      (sp->channel_style != CHANNELS_COMBINED)) 
    hide_gz_scrollbars(sp); /* default is on in this case */  

  cax = cp->ax;
  cax->wn = XtWindow(cw[W_graph]);
  cax->dp = XtDisplay(cw[W_graph]);
  cax->gc = ss->basic_gc;
  return(0);
}


static void set_graph_font(chan_info *cp, graphics_context *ax, XFontStruct *bf)
{
  if (!ax) return;
  ax->current_font = bf->fid;  
  XSetFont(XtDisplay(channel_to_widget(cp)), copy_GC(cp), bf->fid);
}


void set_peak_numbers_font(chan_info *cp, graphics_context *ax) {set_graph_font(cp, ax, PEAKS_FONT(ss));}

void set_tiny_numbers_font(chan_info *cp, graphics_context *ax) {set_graph_font(cp, ax, TINY_FONT(ss));}

void set_bold_peak_numbers_font(chan_info *cp, graphics_context *ax) {set_graph_font(cp, ax, BOLD_PEAKS_FONT(ss));}


color_t get_foreground_color(graphics_context *ax)
{
  XGCValues gv;
  XGetGCValues(MAIN_DISPLAY(ss), ax->gc, GCForeground, &gv);
  return(gv.foreground);
}


void set_foreground_color(graphics_context *ax, Pixel color)
{
  XSetForeground(MAIN_DISPLAY(ss), ax->gc, color);
}


GC copy_GC(chan_info *cp)
{
  if (cp->selected) return(ss->selected_basic_gc);
  return(ss->basic_gc);
}


GC erase_GC(chan_info *cp)
{
  /* used only to clear partial bgs in chan graphs */
  snd_info *sp;
  sp = cp->sound;
  if ((cp->selected) ||
      ((sp) && 
       (sp->channel_style == CHANNELS_SUPERIMPOSED) && 
       (sp->index == ss->selected_sound)))
    return(ss->selected_erase_gc);
  return(ss->erase_gc);
}


void free_fft_pix(chan_info *cp)
{
  if ((cp->fft_pix != None) &&
      (channel_graph(cp)))
    XFreePixmap(XtDisplay(channel_graph(cp)),
		cp->fft_pix);
  cp->fft_pix = None;
  cp->fft_pix_ready = false;
}


bool restore_fft_pix(chan_info *cp, graphics_context *ax)
{
  XCopyArea(ax->dp,
	    cp->fft_pix, 
	    ax->wn,
	    copy_GC(cp),
	    0, 0,                          /* source x y */
	    cp->fft_pix_width, cp->fft_pix_height,
	    cp->fft_pix_x0, cp->fft_pix_y0);
  return(true);
}


void save_fft_pix(chan_info *cp, graphics_context *ax, int fwidth, int fheight, int x0, int y1)
{
  if ((fwidth == 0) || (fheight == 0)) return;
  if (cp->fft_pix == None)
    {
      /* make new pixmap */
      cp->fft_pix_width = fwidth;
      cp->fft_pix_height = fheight;
      cp->fft_pix_x0 = x0;
      cp->fft_pix_y0 = y1;
      cp->fft_pix_cutoff = cp->spectrum_end;
      cp->fft_pix = XCreatePixmap(ax->dp,
				       RootWindowOfScreen(XtScreen(channel_graph(cp))),
				       fwidth, fheight,
				       DefaultDepthOfScreen(XtScreen(channel_graph(cp))));
    }
  XCopyArea(ax->dp,
	    ax->wn,
	    cp->fft_pix, 
	    copy_GC(cp),
	    cp->fft_pix_x0, cp->fft_pix_y0,
	    cp->fft_pix_width, cp->fft_pix_height,
	    0, 0);
  cp->fft_pix_ready = true;
}


void cleanup_cw(chan_info *cp)
{
  if (cp)
    {
      Widget *cw;
      free_fft_pix(cp);

      cp->selected = false;
      cw = cp->chan_widgets;
      if (cw)
	{
	  if (cw[W_w])
	    {
	      XtVaSetValues(cw[W_w], XmNset, true, NULL);
	      XtVaSetValues(cw[W_f], XmNset, false, NULL);
	    }
	  if (channel_main_pane(cp))
	    XtUnmanageChild(channel_main_pane(cp));
	}
    }
}


void change_channel_style(snd_info *sp, channel_style_t new_style)
{
  if ((sp) && 
      (sp->nchans > 1))
    {
      int i;
      channel_style_t old_style;
      chan_info *selected_cp;

      selected_cp = any_selected_channel(sp); /* chan 0 is none is selected */
      old_style = sp->channel_style;
      sp->channel_style = new_style;

      if (new_style != old_style)
	{
	  int height;

#if WITH_RELATIVE_PANES
	  if ((new_style == CHANNELS_SEPARATE) || (old_style == CHANNELS_SEPARATE))
	    {
	      Widget lst;
	      lst = EDIT_HISTORY_LIST(sp->chans[0]);
	      if ((lst) && (widget_width(lst) > 1))
		remake_edit_history(lst, sp->chans[0], true);
	    }
#endif

	  if (old_style == CHANNELS_COMBINED)
	    {
	      hide_gz_scrollbars(sp);
	      for (i = 1; i < sp->nchans; i++) channel_set_mix_tags_erased(sp->chans[i]);
	    }
	  else 
	    {
	      if (new_style == CHANNELS_COMBINED)
		{
		  show_gz_scrollbars(sp);
		  for (i = 1; i < sp->nchans; i++) channel_set_mix_tags_erased(sp->chans[i]);
		}
	    }

	  if (old_style == CHANNELS_SUPERIMPOSED)
	    {
	      syncb(sp, sp->previous_sync);
	      XtVaSetValues(unite_button(sp), XmNselectColor, ss->selection_color, NULL);
	    }
	  else
	    {
	      if (new_style == CHANNELS_SUPERIMPOSED)
		{
		  sp->previous_sync = sp->sync;
		  if (sp->sync == 0) syncb(sp, 1);
		  XtVaSetValues(unite_button(sp), XmNselectColor, ss->green, NULL);

		  apply_y_axis_change(selected_cp);
		  apply_x_axis_change(selected_cp);

		  for (i = 0; i < sp->nchans; i++) 
		    {
		      if (i != selected_cp->chan)
			{
			  chan_info *ncp;
			  ncp = sp->chans[i];
			  CURSOR(ncp) = CURSOR(selected_cp);
			  if (selected_cp->graph_transform_p != ncp->graph_transform_p)
			    {
			      ncp->graph_transform_p = selected_cp->graph_transform_p;
			      set_toggle_button(channel_f(ncp), selected_cp->graph_transform_p, false, (void *)ncp);
			    }
			  if (selected_cp->graph_time_p != ncp->graph_time_p)
			    {
			      ncp->graph_time_p = selected_cp->graph_time_p;
			      set_toggle_button(channel_w(ncp), selected_cp->graph_time_p, false, (void *)ncp);
			    }
			}
		    }
		}
	    }

	  height = widget_height(w_snd_pane(sp)) - control_panel_height(sp);
	  if (old_style == CHANNELS_SEPARATE)
	    {
	      axis_info *ap;
	      ap = selected_cp->axis;
	      channel_lock_pane(sp->chans[0], height);

	      for (i = 0; i < sp->nchans; i++) 
		{
		  if (i != selected_cp->chan)
		    set_axes(sp->chans[i], ap->x0, ap->x1, ap->y0, ap->y1);
		  if (i > 0)
		    cleanup_cw(sp->chans[i]);
		}

	      channel_open_pane(sp->chans[0]);
	      channel_unlock_pane(sp->chans[0]);
	      XmToggleButtonSetState(unite_button(sp), true, false);
	    }
	  else
	    {
	      if (new_style == CHANNELS_SEPARATE)
		{
		  /* height = total space available */
		  height /= sp->nchans;
		  for_each_sound_chan_with_int(sp, channel_lock_pane, height);
		  for_each_sound_chan(sp, channel_open_pane);
		  for_each_sound_chan(sp, channel_unlock_pane);

		  for (i = 1; i < sp->nchans; i++)
		    {
		      Widget *cw;
		      chan_info *cp;
		      int j;

		      cp = sp->chans[i];
		      cw = cp->chan_widgets;

		      for (j = 0; j < NUM_CHAN_WIDGETS - 1; j++)
			if ((cw[j]) && (!XtIsManaged(cw[j]))) 
			  XtManageChild(cw[j]);

		      XmToggleButtonSetState(cw[W_f], (Boolean)(cp->graph_transform_p), false);
		      XmToggleButtonSetState(cw[W_w], (Boolean)(cp->graph_time_p), false);
		      /* these can get out of sync if changes are made in the unseparated case */
		    }

		  XmToggleButtonSetState(unite_button(sp), false, false);
		  if (sp->selected_channel > 0) color_selected_channel(sp);
		}
	    }

	  if ((new_style == CHANNELS_COMBINED) && 
	      (sp->selected_channel > 0)) 
	    color_selected_channel(sp);
	}
    }
}


static XEN g_channel_widgets(XEN snd, XEN chn)
{
  #define H_channel_widgets "(" S_channel_widgets " :optional snd chn): a list of widgets: ((0)graph (1)w (2)f (3)sx (4)sy (5)zx (6)zy (7)edhist)"
  chan_info *cp;
  ASSERT_CHANNEL(S_channel_widgets, snd, chn, 1);
  cp = get_cp(snd, chn, S_channel_widgets);
  if (!cp) return(XEN_FALSE);
  return(XEN_CONS(XEN_WRAP_WIDGET(channel_graph(cp)),
	   XEN_CONS(XEN_WRAP_WIDGET(channel_w(cp)),
	     XEN_CONS(XEN_WRAP_WIDGET(channel_f(cp)),
	       XEN_CONS(XEN_WRAP_WIDGET(channel_sx(cp)),
	         XEN_CONS(XEN_WRAP_WIDGET(channel_sy(cp)),
	           XEN_CONS(XEN_WRAP_WIDGET(channel_zx(cp)),
	             XEN_CONS(XEN_WRAP_WIDGET(channel_zy(cp)),
		       XEN_CONS(XEN_WRAP_WIDGET(EDIT_HISTORY_LIST(cp)),
			 XEN_CONS(XEN_WRAP_WIDGET(channel_gsy(cp)),
			   XEN_CONS(XEN_WRAP_WIDGET(channel_gzy(cp)),
			     XEN_CONS(XEN_WRAP_WIDGET(channel_main_pane(cp)),
	                       XEN_EMPTY_LIST))))))))))));
}


/* previous snd-xxen.c contents) */

static void timed_eval(XtPointer in_code, XtIntervalId *id)
{
#if HAVE_EXTENSION_LANGUAGE
  XEN lst = (XEN)in_code;
  XEN_CALL_0(XEN_CADR(lst), "timed callback func");
  snd_unprotect_at(XEN_TO_C_INT(XEN_CAR(lst)));
#endif
}


static XEN g_in(XEN ms, XEN code)
{
  #define H_in "(" S_in " msecs thunk): invoke thunk in msecs milliseconds (named call_in in Ruby)"

#if HAVE_EXTENSION_LANGUAGE
  XEN_ASSERT_TYPE(XEN_NUMBER_P(ms), ms, XEN_ARG_1, S_in, "a number");
  XEN_ASSERT_TYPE(XEN_PROCEDURE_P(code), code, XEN_ARG_2, S_in, "a procedure");

  if (XEN_REQUIRED_ARGS_OK(code, 0))
    {
      int secs;
      secs = XEN_TO_C_INT(ms);
      if (secs < 0) 
	XEN_OUT_OF_RANGE_ERROR(S_in, XEN_ARG_1, ms, "a positive integer");
      else
	{
	  XEN lst;
	  lst = XEN_LIST_2(XEN_FALSE, code);
	  XEN_LIST_SET(lst, 0, C_TO_XEN_INT(snd_protect(lst)));
	  XtAppAddTimeOut(MAIN_APP(ss), 
			  (unsigned long)secs,
			  (XtTimerCallbackProc)timed_eval, 
			  (XtPointer)lst);
	  /* the "code" arg can still be something misleading like an applicable smob */
	  /*   there's a way to catch that in run.c line 129, but I'm not sure it's the "right thing" here */
	}
    }
  else XEN_BAD_ARITY_ERROR(S_in, 2, code, "should take no args");
#endif

  return(ms);
}


void color_unselected_graphs(color_t color)
{
  int i;
  for (i = 0; i < ss->max_sounds; i++)
    {
      snd_info *sp;
      int j;
      sp = ss->sounds[i];
      if ((sp) && (sp->inuse != SOUND_WRAPPER))
	for (j = 0; j < sp->allocated_chans; j++)
	  {
	    chan_info *cp;
	    cp = sp->chans[j];
	    if ((cp) && ((i != ss->selected_sound) || (j != sp->selected_channel)))
	      XtVaSetValues(channel_graph(cp), XmNbackground, color, NULL);
	  }
    }
}


void color_chan_components(color_t color, slider_choice_t which_component)
{
  int i;
  for (i = 0; i < ss->max_sounds; i++)
    {
      snd_info *sp;
      int j;
      sp = ss->sounds[i];
      if ((sp) && (sp->inuse != SOUND_WRAPPER))
	for (j = 0; j < sp->allocated_chans; j++)
	  {
	    chan_info *cp;
	    cp = sp->chans[j];
	    if (cp)
	      {
		if (which_component == COLOR_POSITION)
		  {
		    XtVaSetValues(channel_sx(cp), XmNbackground, color, NULL);
		    XtVaSetValues(channel_sy(cp), XmNbackground, color, NULL);
		  }
		else
		  {
		    XtVaSetValues(channel_zy(cp), XmNbackground, color, NULL);
		    XtVaSetValues(channel_zx(cp), XmNbackground, color, NULL);
		  }
	      }
	  }
    }
}


static XEN g_graph_cursor(void)
{
  #define H_graph_cursor "(" S_graph_cursor "): current graph cursor shape"
  return(C_TO_XEN_INT(in_graph_cursor(ss)));
}


#include <X11/cursorfont.h>

static XEN g_set_graph_cursor(XEN curs)
{
  int val;
  XEN_ASSERT_TYPE(XEN_INTEGER_P(curs), curs, XEN_ONLY_ARG, S_setB S_graph_cursor, "an integer");
  /* X11/cursorfont.h has various even-numbered glyphs, but the odd numbers are ok, and XC_num_glyphs is a lie */
  /*   if you use too high a number here, X dies */
  val = XEN_TO_C_INT(curs);
  if ((val >= 0) && (val <= XC_xterm))
    {
      ss->Graph_Cursor = val;
      ss->graph_cursor = XCreateFontCursor(XtDisplay(MAIN_SHELL(ss)), in_graph_cursor(ss));
    }
  else XEN_OUT_OF_RANGE_ERROR(S_setB S_graph_cursor, 1, curs, "~A: invalid cursor");
  return(curs);
}


#ifdef XEN_ARGIFY_1
XEN_NARGIFY_2(g_in_w, g_in)
XEN_NARGIFY_0(g_graph_cursor_w, g_graph_cursor)
XEN_NARGIFY_1(g_set_graph_cursor_w, g_set_graph_cursor)
XEN_ARGIFY_2(g_channel_widgets_w, g_channel_widgets)
#else
#define g_in_w g_in
#define g_graph_cursor_w g_graph_cursor
#define g_set_graph_cursor_w g_set_graph_cursor
#define g_channel_widgets_w g_channel_widgets
#endif


void g_init_gxchn(void)
{
  XEN_DEFINE_PROCEDURE(S_in,            g_in_w,             2, 0, 0, H_in);

  XEN_DEFINE_PROCEDURE_WITH_SETTER(S_graph_cursor, g_graph_cursor_w, H_graph_cursor,
				   S_setB S_graph_cursor, g_set_graph_cursor_w,  0, 0, 1, 0);

  XEN_DEFINE_PROCEDURE(S_channel_widgets, g_channel_widgets_w, 0, 2, 0, H_channel_widgets);

#if HAVE_SCHEME
  #define H_mouse_enter_graph_hook S_mouse_enter_graph_hook " (snd chn): called when the mouse \
enters the drawing area (graph pane) of the given channel.\n\
  (add-hook! " S_mouse_enter_graph_hook "\n\
    (lambda (snd chn)\n\
      (" S_focus_widget " (car (" S_channel_widgets " snd chn)))))"

  #define H_mouse_leave_graph_hook S_mouse_leave_graph_hook " (snd chn): is called when the mouse \
leaves the drawing area (graph pane) of the given channel."
#endif
#if HAVE_RUBY
  #define H_mouse_enter_graph_hook S_mouse_enter_graph_hook " (snd chn): called when the mouse \
enters the drawing area (graph pane) of the given channel.\n\
  $mouse_enter_graph_hook.add-hook!(\"focus\") do |snd chn|\n\
    focus_widget(channel_widgets(snd, chn)[0])\n\
    end"

  #define H_mouse_leave_graph_hook S_mouse_leave_graph_hook " (snd chn): called when the mouse \
leaves the drawing area (graph pane) of the given channel."
#endif
#if HAVE_FORTH
  #define H_mouse_enter_graph_hook S_mouse_enter_graph_hook " (snd chn): called when the mouse \
enters the drawing area (graph pane) of the given channel.\n\
" S_mouse_enter_graph_hook " lambda: <{ snd chn }>\n\
  snd chn " S_channel_widgets " car " S_focus_widget "\n\
; add-hook!"
  #define H_mouse_leave_graph_hook S_mouse_leave_graph_hook " (snd chn): is called when the mouse \
leaves the drawing area (graph pane) of the given channel."
#endif

  mouse_enter_graph_hook = XEN_DEFINE_HOOK(S_mouse_enter_graph_hook, 2, H_mouse_enter_graph_hook);    /* args = snd chn */
  mouse_leave_graph_hook = XEN_DEFINE_HOOK(S_mouse_leave_graph_hook, 2, H_mouse_leave_graph_hook);    /* args = snd chn */
}
