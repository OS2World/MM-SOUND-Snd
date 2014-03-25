;;; backwards compatibility for snd 12

(define verbose-cursor with-verbose-cursor)

#|
;;; this was in snd-motif.scm but it could not have worked anytime recently
(define (show-font-name font)
  "(show-font-name font-list) shows the Snd-related name and the X-related name of each font in a font list"
  (define (show-next-font context)
    (let ((next-font (XmFontListGetNextFont context)))
      (if (and next-font (car next-font))
	  (begin
	    (if (XFontStruct? (caddr next-font))
		(let ((name (XGetFontProperty (caddr next-font) XA_FULL_NAME)))
		  (if (not (car name))
		      (set! name (XGetFontProperty (caddr next-font) XA_FAMILY_NAME)))
		  (snd-print 
		   (format #f "~A: ~A~%"
			   (cadr next-font)
			   (XGetAtomName 
			    (XtDisplay (cadr (main-widgets)))
			    (list 'Atom (cadr name))))))
		(snd-print (format #f "no font found!~%")))
	    (show-next-font context)))))
  (let ((context (XmFontListInitFontContext font))) ; what are XmFontListInitFontContext and friends?
    (if context
	(begin
	  (show-next-font context)
	  (XmFontListFreeFontContext context))
	"no fonts?")))
|#


#|
;;;
;;;    (install-searcher (lambda (file) (= (srate file) 44100)))
;;;    (install-searcher (lambda (file) (= (channels file) 4)))
;;;
;;; this is obsolete -- use the file-filter mechanism instead

(define (install-searcher proc)
  "(install-searcher proc) replaces the current file search procedure in the File Selection 
Box: (install-searcher (lambda (file) (= (srate file) 44100)))"
  (define match-sound-files
    (lambda args
      "(match-sound-files func dir) applies func to each sound file in dir and returns a list of files for which func does not return #f"
      (let* ((func (car args))
	     (matches '()))
	(for-each
	 (lambda (file)
	   (if (func file)
	       (set! matches (cons file matches))))
	 (sound-files-in-directory (if (null? (cdr args)) "." (cadr args))))
	matches)))
  (define (XmString->string str)
    (XmStringUnparse str #f XmCHARSET_TEXT XmCHARSET_TEXT #f 0 XmOUTPUT_ALL))
  (define (XmStringTable->list st len)
    (XmStringTableUnparse st len #f XmCHARSET_TEXT XmCHARSET_TEXT #f 0 XmOUTPUT_ALL))
  (define (list->XmStringTable strs)
    (XmStringTableParseStringArray strs (length strs) #f XmCHARSET_TEXT #f 0 #f))
  (XtSetValues (open-file-dialog #f)
	       (list XmNfileSearchProc
		     (lambda (widget info)
		       (let* ((dir (XmString->string (.dir info)))
			      (files (match-sound-files proc dir))
			      (fileTable (list->XmStringTable 
					  (map (lambda (n) 
						 (string-append dir n)) 
					       files))))
			 (XtSetValues widget
				      (list XmNfileListItems fileTable
					    XmNfileListItemCount (length files)
					    XmNlistUpdated #t))
			 (for-each (lambda (n) (XmStringFree n)) fileTable))))))


;;; here's a fancier version that gets rid of the useless directory list,
;;;   and shows multi-channel files in color
|#
