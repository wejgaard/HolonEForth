CODE BYE ( -- , exit Forth )
	INT 020H \ return to DOS

CODE ?RX ( -- c T | F ) 
	$CODE 3,'?RX',QRX
	XOR BX,BX \ BX=0 setup for false flag
	MOV DL,0FFH \ input command
	MOV AH,6 \ MS-DOS Direct Console I/O
	INT 021H
	JZ QRX3 \ ?key ready
	OR AL,AL \ AL=0 if extended char
	JNZ QRX1 \ ?extended character code
	INT 021H
	MOV BH,AL \ extended code in msb
	JMP QRX2
		QRX1: MOV BL,AL
		QRX2: PUSH BX \ save character
	MOV BX,-1 \ true flag
		QRX3: PUSH BX
	$NEXT

CODE TX! ( c -- ) 
	POP DX \ char in DL
	CMP DL,0FFH \ 0FFH is interpreted as input
	JNZ TX1 \ do NOT allow input
	MOV DL,32 \ change to blank
	TX1: MOV AH,6 \ MS-DOS Direct Console I/O
	INT 021H \ display character
	$NEXT

CODE !IO ( -- ) 
	$NEXT

$NEXT MACRO
 	LODSW \ load next word into WP (AX)
 	JMP AX \ jump directly to the word thru WP
 	ENDM \ IP (SI) now points to the next word

doLIST ( a -- ) 
	XCHG BP,SP \ exchange pointers
	PUSH SI \ push return stack
	XCHG BP,SP \ restore the pointers
	POP SI \ new list address
	$NEXT

CODE EXIT 
	XCHG BP,SP \ exchange pointers
	POP SI \ pop return stack
	XCHG BP,SP \ restore the pointers
	$NEXT

CODE EXECUTE ( ca -- ) 
	POP BX
	JMP BX \ jump to the code address
	CODE doLIT ( -- w ) \ Push inline literal on data stack.
	LODSW \ get the literal compiled in-line
	PUSH AX \ push literal on the stack
	$NEXT \ execute next word after literal

CODE doLIT ( -- w )   
	LODSW   \ get the literal compiled in-line
	PUSH AX \ push literal on the stack
	$NEXT    \ execute next word after literal

CODE nextword ( -- ) 
	SUB WORD PTR [BP],1 \ decrement the index
	JC NEXT1 \ ?decrement below 0
	MOV SI,0[SI] \ no, continue loop
	$NEXT

	NEXT1:ADD BP,2 \ yes, pop the index
	ADD SI,2 \ exit loop
	$NEXT

CODE ?branch ( f -- ) 
	POP BX \ pop flag
	OR BX,BX \ ?flag=0
	JZ BRAN1 \ yes, so branch
	ADD SI,2 \ point IP to next cell
	$NEXT

	BRAN1:MOV SI,0[SI] \ IP:=(IP), jump to new address
	$NEXT

CODE branch ( -- ) 
	MOV SI,0[SI] \ jump to new address unconditionally
	$NEXT

CODE ! ( w a -- ) 
	POP BX \ get address from tos
	POP 0[BX] \ store data to that adddress
	$NEXT

CODE @ ( a -- w )
	POP BX \ get address
	PUSH 0[BX] \ fetch data
	$NEXT

CODE C! ( c b -- ) 
	POP BX \ get address
	POP AX \ get data in a cell
	MOV 0[BX],AL \ store one byte
	$NEXT

CODE C@ ( b -- c ) 
	POP BX \ get address
	XOR AX,AX \ AX=0 zero the hi byte
	MOV AL,0[BX] \ get low byte
	PUSH AX \ push on stack
	$NEXT

CODE RP@ ( -- a ) 
	PUSH BP \ copy address to return stack
	$NEXT \ pointer register BP

CODE RP! ( a -- ) 
	POP BP \ copy (BP) to tos
	$NEXT

CODE R> ( -- w ) 
	PUSH 0[BP] \ copy w to data stack
	ADD BP,2 \ adjust RP for popping
	$NEXT

CODE R@ ( -- w ) 
	PUSH 0[BP] \ copy w to data stack
	$NEXT

CODE >R ( w -- ) 
	SUB BP,2 \ adjust RP for pushing
	POP 0[BP] \ push w to return stack
	$NEXT

CODE DROP ( w -- ) 
	ADD SP,2 \ adjust SP to pop
	$NEXT

CODE DUP ( w -- w w ) 
	MOV BX,SP \ use BX to index the stack
	PUSH 0[BX]
	$NEXT

CODE SWAP ( w1 w2 -- w2 w1 ) 
	POP BX \ get w2
	POP AX \ get w1
	PUSH BX \ push w2
	PUSH AX \ push w1
	$NEXT

CODE OVER ( w1 w2 -- w1 w2 w1 )
	MOV BX,SP \ use BX to index the stack
	PUSH 2[BX] \ get w1 and push on stack
	$NEXT

CODE SP@ ( -- a ) 
	MOV BX,SP \ use BX to index the stack
	PUSH BX \ push SP back
	$NEXT

CODE SP! ( a -- ) 
	POP SP \ safety
	$NEXT

CODE 0< ( n -- f ) 
	POP AX
	CWD \ sign extend AX into DX
	PUSH DX \ push 0 or -1
	$NEXT

CODE AND ( w w -- w ) 
	POP BX
	POP AX
	AND BX,AX
	PUSH BX
	$NEXT

CODE OR ( w w -- w ) 
	POP BX
	POP AX
	OR BX,AX
	PUSH BX
	$NEXT

CODE XOR ( w w -- w ) 
	POP BX
	POP AX
	XOR BX,AX
	PUSH BX
	$NEXT

CODE UM+ ( w w -- w cy )
	XOR CX,CX \ CX=0 initial carry flag
	POP BX
	POP AX
	ADD AX,BX
	RCL CX,1 \ get carry
	PUSH AX \ push sum
	PUSH CX \ push carry
	$NEXT

