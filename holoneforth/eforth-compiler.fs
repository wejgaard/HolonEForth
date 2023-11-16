: [ ( -- )
    [ ' $INTERPRET ] LITERAL
    'EVAL ! ( vector EVAL to $INTERPRET )
    ; IMMEDIATE ( enter into text interpreter mode )

: ] ( -- )
    [ ' $COMPILE ] LITERAL
    'EVAL ! ( vector EVAL to $COMPILE )
;

: ' ( -- xt ) 
	TOKEN NAME? IF EXIT THEN THROW ;

: ALLOT ( n -- ) 
	CP +! ;

: , ( w -- ) 
	HERE DUP CELL+ CP ! ! ; \ ???ALIGNED

: [COMPILE] ( -- ; <string> ) ' , ; IMMEDIATE

: COMPILE ( -- )
	R> DUP @ , CELL+ >R ;

: LITERAL ( w -- ) 
	COMPILE doLIT , ; IMMEDIATE

: $," ( -- ) 
	34 WORD COUNT ALIGNED CP ! ;

: RECURSE ( -- ) 
	LAST @ NAME> , ; IMMEDIATE

: <MARK ( -- a ) 
	HERE ;

: <RESOLVE ( a -- ) 
	, ;

: >MARK ( -- A ) 
	HERE 0 , ;

: >RESOLVE ( A -- ) 
	<MARK SWAP ! ;

: FOR ( -- a ) 
	COMPILE >R <MARK ; IMMEDIATE

: BEGIN ( -- a ) 
	<MARK ; IMMEDIATE

: NEXT ( a -- ) 
	COMPILE nextword <RESOLVE ; IMMEDIATE

: UNTIL ( a -- ) 
	COMPILE ?branch <RESOLVE ; IMMEDIATE

: AGAIN ( a -- ) 
	COMPILE branch <RESOLVE ; IMMEDIATE

: IF ( -- A ) 
	COMPILE ?branch >MARK ; IMMEDIATE

: AHEAD ( -- A ) 
	COMPILE branch >MARK ; IMMEDIATE

: REPEAT ( A a -- ) 
	[COMPILE] AGAIN >RESOLVE ; IMMEDIATE

: THEN ( A -- ) 
	>RESOLVE ; IMMEDIATE

: AFT ( a -- a A ) 
	DROP [COMPILE] AHEAD [COMPILE] BEGIN SWAP ; IMMEDIATE

: ELSE ( A -- A ) 
	[COMPILE] AHEAD SWAP [COMPILE] THEN ; IMMEDIATE

: WHEN ( a A -- a A a ) 
	[COMPILE] IF OVER ; IMMEDIATE

: WHILE ( a -- A a ) 
	[COMPILE] IF SWAP ; IMMEDIATE

: ABORT" ( -- ; <string> ) 
	COMPILE abort" $," ; IMMEDIATE

: $" ( -- ; <string> ) 
	COMPILE $"| $," ; IMMEDIATE

: ." ( -- ; <string> ) 
	COMPILE ."| $," ; IMMEDIATE

: ?UNIQUE ( a -- a ) 
	DUP NAME? IF ." reDef " OVER COUNT TYPE THEN DROP ;

: $,n ( a -- )
    DUP C@
    IF ?UNIQUE
        ( na) DUP LAST ! \ for OVERT
        ( na) HERE ALIGNED SWAP
        ( cp na) CELL-
        ( cp la) CURRENT @ @
        ( cp la na') OVER !
        ( cp la) CELL- DUP NP ! ( ptr) ! EXIT
    THEN $" name" THROW ;

: $COMPILE ( a -- )
    NAME? ?DUP
    IF @ $80 AND
        IF EXECUTE ELSE , THEN EXIT
    THEN 'NUMBER @EXECUTE
    IF [COMPILE] LITERAL EXIT
    THEN THROW ;

: OVERT ( -- ) 
	LAST @ CURRENT @ ! ;

: ; ( -- )
    COMPILE EXIT [COMPILE] [ OVERT ; IMMEDIATE

: ] ( -- ) 
	doLIT $COMPILE 'EVAL ! ;

: call, ( xt -- ) 
    $E890 , HERE CELL+ - , ;

: ( -- ; <string> ) 
	TOKEN $,n doLIT doLIST call, ] ;

: IMMEDIATE ( -- ) 
	$80 LAST @ @ OR LAST @ ! ;

: USER ( n -- ; <string> )
    TOKEN $,n OVERT
    doLIT doLIST COMPILE doUSER , ;

: CREATE ( -- ; <string> )
    TOKEN $,n OVERT
    doLIT doLIST COMPILE doVAR ;

: VARIABLE ( -- ; <string> ) 
	CREATE 0 , ;

