: _TYPE ( b u -- )
    FOR AFT DUP C@ >CHAR EMIT 1 + THEN NEXT DROP ;

: dm+ ( b u -- b )
    OVER 4 U.R SPACE FOR AFT DUP C@ 3 U.R 1 + THEN NEXT ;

: DUMP ( b u -- )
    BASE @ >R HEX 16 /
    FOR CR 16 2DUP dm+ ROT ROT 2 SPACES _TYPE NUF? NOT WHILE
    NEXT ELSE R> DROP THEN DROP R> BASE ! ;

: .S ( -- ) 
	CR DEPTH FOR AFT R@ PICK . THEN NEXT ." <sp" ;

: .BASE ( -- ) 
	BASE @ DECIMAL DUP . BASE ! ;

: .FREE ( -- ) 
	CP 2@ - U. ;

: !CSP ( -- ) 
	SP@ CSP ! ;

: ?CSP ( -- ) 
	SP@ CSP @ XOR ABORT" stack depth" ;

: >NAME ( xt -- na | F )
    CURRENT
    BEGIN CELL+ @ ?DUP WHILE 2DUP
        BEGIN @ DUP WHILE 2DUP NAME> XOR
        WHILE CELL-
        REPEAT THEN SWAP DROP ?DUP
    UNTIL SWAP DROP SWAP DROP EXIT THEN DROP 0 ;

: .ID ( a -- )
    ?DUP IF COUNT $01F AND _TYPE EXIT THEN ." {noName}" ;

: SEE ( -- ; <string> )
    ' CR CELL+
    BEGIN CELL+ DUP @ DUP IF >NAME THEN ?DUP
        IF SPACE .ID ELSE DUP @ U. THEN NUF?
    UNTIL DROP ;

: WORDS ( -- )
    CR CONTEXT @
    BEGIN @ ?DUP
    WHILE DUP SPACE .ID CELL- NUF?
    UNTIL DROP THEN ;

: VER ( -- u ) $101 ;

: hi ( -- )
    !IO BASE @ HEX \ initialize IO device & sign on
    CR ." eFORTH V" VER <# # # 46 HOLD # #> TYPE
    CR ;

: EMPTY ( -- )
    FORTH CONTEXT @ DUP CURRENT 2! 6 CP 3 MOVE OVERT ;
    CREATE 'BOOT ' hi , \ application vector

: COLD ( -- )
    BEGIN
    U0 UP 74 CMOVE
    PRESET 'BOOT @EXECUTE
    FORTH CONTEXT @ DUP CURRENT 2! OVERT
    QUIT
    AGAIN ;

