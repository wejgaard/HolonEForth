: doVAR ( -- a ) R> ;

VARIABLE UP ( -- a)

: doUSER ( -- a )
    R> @ 	    \ retrieve user area offset
    UP @ + ; \ add to user area base addr

: doVOC ( -- ) 
	R> CONTEXT ! ;

: FORTH ( -- ) 
	doVOC [ 0 , 0 ,

SP0 ( -- a )

RP0 ( -- a )

'?KEY ( -- a )

'EMIT ( -- a )

'EXPECT ( -- a )

'TAP ( -- a )

'ECHO ( -- a )

'PROMPT ( -- a )

BASE ( -- a )

tmp ( -- a )

SPAN ( -- a )

>IN ( -- a )

#TIB ( -- a )

CSP ( -- a )

'EVAL ( -- a )

'NUMBER ( -- a )

HLD ( -- a )

HANDLER ( -- a )

CONTEXT ( -- a )

CURRENT ( -- a )

CP ( -- a )

NP ( -- a )

LAST ( -- a )

: ?DUP ( w -- w w | 0 ) 
	DUP IF DUP THEN ;

: ROT ( w1 w2 w3 -- w2 w3 w1 ) 
	>R SWAP R> SWAP ;

: 2DROP ( w w -- ) 
	DROP DROP ;

: 2DUP ( w1 w2 -- w1 w2 w1 w2 ) 
	OVER OVER ;

: + ( w w -- w ) 
	UM+ DROP ;

: NOT ( w -- w ) 
	-1 XOR ;

: NEGATE ( n -- -n ) 
	NOT 1 + ;

: DNEGATE ( d -- -d ) 
	NOT >R NOT 1 UM+ R> + ;

: D+ ( d d -- d ) 
	>R SWAP >R UM+ R> R> + + ;

: - ( w w -- w ) 
	NEGATE + ;

: ABS ( n -- +n ) 
	DUP 0< IF NEGATE THEN ;

: = ( w w -- t ) 
	XOR IF 0 EXIT THEN -1 ;

: U< ( u u -- t ) 
	2DUP XOR 0< IF SWAP DROP 0< EXIT THEN - 0< ;

: < ( n n -- t ) 
	2DUP XOR 0< IF DROP 0< EXIT THEN - 0< ;

: MAX ( n n -- n ) 
	2DUP < IF SWAP THEN DROP ;

: MIN ( n n -- n ) 
	2DUP SWAP < IF SWAP THEN ROP ;

: WITHIN ( u ul uh -- t ) 
         OVER - >R - R> U< ;

: UM/MOD ( ud u -- ur uq )
    2DUP U< 
    IF NEGATE 15   
        FOR >R DUP UM+ >R >R DUP UM+ R> + DUP
            R> R@ SWAP >R UM+ R> OR
            IF >R DROP 1 + R> ELSE DROP THEN R>
        NEXT DROP SWAP EXIT
    THEN DROP 2DROP -1 DUP ;

: M/MOD ( d n -- r q ) 
    DUP 0< DUP >R
    IF NEGATE >R DNEGATE R>
    THEN >R DUP 0< IF R@ + THEN R> UM/MOD R>
    IF SWAP NEGATE SWAP THEN ;

: /MOD ( n n -- r q ) 
	OVER 0< SWAP M/MOD ;

: MOD ( n n -- r ) 
	/MOD DROP ;

: / ( n n -- q ) 
	/MOD SWAP DROP ;

: UM* ( u u -- ud )
    0 SWAP ( u1 0 u2 ) 15
    FOR DUP UM+ >R >R DUP UM+ R> + R>
        IF >R OVER UM+ R> + THEN
    NEXT ROT DROP ;

: * ( n n -- n ) 
	UM* DROP ;

: M* ( n n -- d )
    2DUP XOR 0< >R ABS SWAP ABS UM* R> IF DNEGATE THEN ;

: */MOD ( n n n -- r q ) 
	>R M* R> M/MOD ;

: */ ( n n n -- q ) 
	*/MOD SWAP DROP ;

: CELL- ( a -- a ) -2 + ;

: CELL+ ( a -- a ) 2 + ;

: CELLS ( n -- n ) 2 * ;

: ALIGNED ( b -- a )
    DUP 0 2 UM/MOD DROP DUP
    IF 2 SWAP - THEN + ;

: BL ( -- 32 ) 32 ;

: >CHAR ( c -- c )
    $7F AND DUP 127 BL WITHIN IF DROP 95 THEN ;

: DEPTH ( -- n ) 
	SP@ SP0 @ SWAP - 2 / ;

: PICK ( +n -- w ) 
	1 + CELLS SP@ + @ ;

: +! ( n a -- ) 
	SWAP OVER @ + SWAP ! ;

: 2! ( d a -- ) 
	SWAP OVER ! CELL+ ! ;

: 2@ ( a -- d ) 
	DUP CELL+ @ SWAP @ ;

: COUNT ( b -- b +n ) 
	DUP 1 + SWAP C@ ;

: HERE ( -- a ) 
	CP @ ;

: PAD ( -- a ) 
	HERE 80 + ;

: TIB ( -- a ) 
	#TIB CELL+ @ ;

: @EXECUTE ( a -- ) 
	@ ?DUP IF EXECUTE THEN ;

: CMOVE ( b b u -- )
    FOR AFT >R DUP C@ R@ C! 1 + R> 1 + THEN NEXT 2DROP ;

: FILL ( b u c -- )
    SWAP FOR SWAP AFT 2DUP C! 1 + THEN NEXT 2DROP ;

: -TRAILING ( b u -- b u )
    FOR AFT BL OVER R@ + C@ <
        IF R> 1 + EXIT THEN THEN
    NEXT 0 ;

: PACK$ ( b u a -- a ) 
    ALIGNED DUP >R OVER
    DUP 0 2 UM/MOD DROP
    - OVER + 0 SWAP ! 2DUP C! 1 + SWAP CMOVE R> ;

