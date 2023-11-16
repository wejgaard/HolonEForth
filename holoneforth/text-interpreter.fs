: DIGIT ( u -- c ) 
	9 OVER < 7 AND + 48 + ;

: EXTRACT ( n base -- n c ) 
	0 SWAP UM/MOD SWAP DIGIT ;

: <# ( -- ) 
	PAD HLD ! ;

: HOLD ( c -- ) 
	HLD @ 1 - DUP HLD ! C! ;

: # ( u -- u ) 
	BASE @ EXTRACT HOLD ;

: #S ( u -- 0 ) 
	BEGIN # DUP WHILE REPEAT ;

: SIGN ( n -- ) 
	0< IF 45 HOLD THEN ;

: #> ( w -- b u ) 
	DROP HLD @ PAD OVER - ;

: str ( n -- b u )
    DUP >R ( save a copy for sign)
    ABS ( use absolute of n)
    <# #S ( convert all digits)
    R> SIGN ( add sign from n)
    #> ; ( return number string addr and length)

: HEX ( -- )
     16 BASE ! ;

: DECIMAL ( -- )
    10 BASE ! ;

: .R ( n +n -- )
    >R str ( convert n to a number string)
    R> OVER - SPACES ( print leading spaces)
    TYPE ; ( print number in +n column format)

: U.R ( u +n -- )
    >R ( save column number)
    <# #S #> R> ( convert unsigned number)
    OVER - SPACES ( print leading spaces)
    TYPE ; ( print number in +n columns)

: U. ( u -- )
    <# #S #> ( convert unsigned number)
    SPACE ( print one leading space)
    TYPE ; ( print number)

: . ( w -- )
    BASE @ 10 XOR ( if not in decimal mode)
    IF U. EXIT THEN ( print unsigned number)
    str SPACE TYPE ; ( print signed number if decimal)

: ? ( a -- )
    @ . ;

: DIGIT? ( c base -- u t )
    >R 48 - 9 OVER <
    IF 7 - DUP 10 < OR THEN DUP R> U< ;

: NUMBER? ( a -- n T | a F )
    BASE @ >R 0 OVER COUNT ( a 0 b n)
    OVER C@ 36 =
    IF HEX SWAP 1 + SWAP 1 - THEN ( a 0 b' n')
        OVER C@ 45 = >R ( a 0 b n)
        SWAP R@ - SWAP R@ + ( a 0 b" n") ?DUP
        IF 1 - ( a 0 b n)
            FOR DUP >R C@ BASE @ DIGIT?
            WHILE SWAP BASE @ * + R> 1 +
            NEXT DROP R@ ( b ?sign) IF NEGATE THEN SWAP
        ELSE R> R> ( b index) 2DROP ( digit number) 2DROP 0
        THEN DUP
    THEN R> ( n ?sign) 2DROP R> BASE ! ;

: ?KEY ( -- c T | F )
	'?KEY @EXECUTE ;

: KEY ( -- c ) 
	BEGIN ?KEY UNTIL ;

: EMIT ( c -- ) 
	'EMIT @EXECUTE ;

: NUF? ( -- f ) 
	?KEY DUP IF 2DROP KEY 13 = THEN ;

: PACE ( -- ) 
	11 EMIT ;

: SPACE ( -- ) 
	BL EMIT ;

: CHARS ( +n c -- ) 
    	SWAP 0 MAX FOR AFT DUP EMIT THEN NEXT DROP ;

: SPACES ( +n -- ) 
	BL CHARS ;

: TYPE ( b u -- ) 
	FOR AFT DUP C@ EMIT 1 + THEN NEXT DROP ;

: CR ( -- ) 
	13 EMIT 10 EMIT ;

: do$ ( -- a )
    R> R@ R> COUNT + ALIGNED >R SWAP >R ;

: $"| ( -- a ) 
	do$ ;

: ."| ( -- ) 
	do$ COUNT TYPE ; COMPILE-ONLY

: parseword ( b u c -- b u delta ; <string> )
    tmp ! OVER >R DUP \ b u u
    IF 1 - tmp @ BL =
        IF \ b u' \ 'skip'
            FOR BL OVER C@ - 0< NOT WHILE 1 +
            NEXT ( b) R> DROP 0 DUP EXIT \ all delim
            THEN R>
        THEN OVER SWAP \ b' b' u' \ 'scan'
        FOR tmp @ OVER C@ - tmp @ BL =
           IF 0< THEN WHILE 1 +
           NEXT DUP >R ELSE R> DROP DUP 1 + >R
        THEN OVER - R> R> - EXIT
    THEN ( b u) OVER R> - ;

: PARSE ( c -- b u ; <string> )
    >R TIB >IN @ + #TIB @ >IN @ - R> parse >IN +! ;

: .( ( -- ) 
	41 PARSE TYPE ; IMMEDIATE

: ( ( -- ) 
	41 PARSE 2DROP ; IMMEDIATE

: \ ( -- ) 
	#TIB @ >IN ! ; IMMEDIATE

: CHAR ( -- c )
	BL PARSE DROP C@ ;

: TOKEN ( -- a ; <string> )
    BL PARSE 31 MIN NP @ OVER - CELL- PACK$ ;

: WORD ( c -- a ; <string> ) 
	PARSE HERE PACK$ ;

: NAME> ( a -- xt ) 
	CELL- CELL- @ ;

: SAME? ( a a u -- a a f \ -0+ )
    FOR AFT OVER R@ CELLS + @
      OVER R@ CELLS + @ - ?DUP
      IF R> DROP EXIT THEN THEN
    NEXT 0 ;

: find ( a va -- xt na | a F )
    	SWAP 				    \ va a
    	DUP C@ 2 / tmp !  \ va a   \ get cell count
    	DUP @ >R             \ va a   \ count byte & 1st char
    	CELL+ SWAP          \ a' va
    	BEGIN @ DUP         \ a' na na
        	IF DUP @ [ =MASK ] LITERAL AND R@ XOR      \ ignore lexicon bits
        	IF CELL+ -1 ELSE CELL+ tmp @ SAME? THEN
        	ELSE R> DROP EXIT
        	THEN
    	WHILE CELL- CELL- \ a' la
    	REPEAT R> DROP SWAP DROP CELL- DUP NAME> SWAP ;

: NAME? ( a -- xt na | a F )
    CONTEXT DUP 2@ XOR IF CELL- THEN >R       \ context<>also
    BEGIN R> CELL+ DUP >R @ ?DUP
    WHILE find ?DUP
    UNTIL R> DROP EXIT THEN R> DROP 0 ;

: ^H ( b b b -- b b b ) 
	>R OVER R> SWAP OVER XOR
    	IF 8 'ECHO @EXECUTE
        	32 'ECHO @EXECUTE \ distructive
        	8 'ECHO @EXECUTE \ backspace
    	THEN ;

: TAP ( bot eot cur c -- bot eot cur )
    	DUP 'ECHO @EXECUTE OVER C! 1 + ;

: kTAP ( bot eot cur c -- bot eot cur )
    	DUP 13 XOR
    	IF 8 XOR IF BL TAP ELSE ^H THEN EXIT
   	THEN DROP SWAP DROP DUP ;

: accept ( b u -- b u )
    	OVER + OVER
    	BEGIN 2DUP XOR
    	WHILE KEY DUP BL - 95 U<
     	  	IF TAP ELSE 'TAP @EXECUTE THEN
    	REPEAT DROP OVER - ;

: EXPECT ( b u -- ) 
	'EXPECT @EXECUTE SPAN ! DROP ;

: QUERY ( -- )
    TIB 80 'EXPECT @EXECUTE #TIB ! DROP 0 >IN ! ;

: CATCH ( ca -- err#/0 )
    ( Execute word at ca and set up an error frame for it.)
    SP@ >R ( save current stack pointer on return stack )
    HANDLER @ >R ( save the handler pointer on return stack )
    RP@ HANDLER ! ( save the handler frame pointer in HANDLER )
    ( ca ) EXECUTE ( execute the assigned word over this safety net )
    R> HANDLER ! ( normal return from the executed word )
    ( restore HANDLER from the return stack )
    R> DROP ( discard the saved data stack pointer )
    0 ; ( push a no-error flag on data stack )

: THROW ( err# -- err# )
    ( Reset system to current local error frame an update error flag.)
    HANDLER @ RP! ( expose latest error handler frame on return stack)
    R> HANDLER ! ( restore previously saved error handler frame )
    R> SWAP >R ( retrieve the data stack pointer saved )
    SP! ( restore the data stack )
    DROP
    R> ; ( retrived err# )

CREATE NULL$ 0 , $," coyote"

: ABORT ( -- ) NULL$ THROW ;

: abort" ( f -- ) IF do$ THROW THEN do$ DROP ;

: ?STACK DEPTH 0< IF $" underflow" THROW THEN ;

: $INTERPRET ... 'NUMBER @EXECUTE IF EXIT THEN THROW ;

: $INTERPRET ( a -- )
    NAME? ?DUP
    IF @ $40 AND
        ABORT" compile ONLY" EXECUTE EXIT
    THEN 'NUMBER @EXECUTE IF EXIT THEN THROW ;

: [ ( -- ) 
	doLIT $INTERPRET 'EVAL ! ; IMMEDIATE

: .OK ( -- ) 
	doLIT $INTERPRET 'EVAL @ = IF ." ok" THEN CR ;

: ?STACK ( -- ) 
	DEPTH 0< ABORT" underflow" ;

: EVAL ( -- )
    BEGIN TOKEN DUP C@
    WHILE 'EVAL @EXECUTE ?STACK
    REPEAT DROP 'PROMPT @EXECUTE ;

: PRESET ( -- ) 
	SP0 @ SP! TIB #TIB CELL+ ! ;

: xio ( a a a -- ) 
    doLIT accept 'EXPECT 2! 'ECHO 2! ; COMPILE-ONLY

: FILE ( -- )
    doLIT PACE doLIT DROP doLIT kTAP xio ;

: HAND ( -- )
    doLIT .OK doLIT EMIT [ kTAP xio ;
    CREATE I/O ' ?RX , ' TX! , \ defaults

: CONSOLE ( -- ) 
	I/O 2@ '?KEY 2! HAND ;

: QUIT ( -- )
    RP0 @ RP!
    BEGIN [COMPILE] [
        BEGIN QUERY doLIT EVAL CATCH ?DUP
        UNTIL 'PROMPT @ SWAP CONSOLE NULL$ OVER XOR
        IF CR #TIB 2@ TYPE
            CR >IN @ 94 CHARS
            CR COUNT TYPE ." ? "
        THEN doLIT .OK XOR
        IF $1B EMIT THEN
        PRESET
    AGAIN ;

