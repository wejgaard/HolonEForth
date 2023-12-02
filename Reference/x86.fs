$NEXT MACRO
 	LODSW 			\ load next word into WP (AX)
 	JMP AX 			\ jump directly to the word thru WP
 	ENDM 			\ IP (SI) now points to the next word

doLIST ( a -- )	 \ Run address list in a colon word.
	XCHG BP,SP \ exchange pointers
	PUSH SI \ push return stack
	XCHG BP,SP \ restore the pointers
	POP SI \ new list address
	$NEXT

CODE EXIT    \ Terminate a colon definition.
	XCHG BP,SP \ exchange pointers
	POP SI \ pop return stack
	XCHG BP,SP \ restore the pointers
	$NEXT

CODE EXECUTE ( ca -- ) \ Execute the word at ca.
	POP BX
	JMP BX \ jump to the code address
	CODE doLIT ( -- w ) \ Push inline literal on data stack.
	LODSW \ get the literal compiled in-line
	PUSH AX \ push literal on the stack
	$NEXT \ execute next word after literal

CODE next ( -- ) \ Decrement index and exit loop
	\ if index is less than 0.
	SUB WORD PTR [BP],1 \ decrement the index
	JC NEXT1 \ ?decrement below 0
	MOV SI,0[SI] \ no, continue loop
	$NEXT

NEXT1:ADD BP,2 \ yes, pop the index
	ADD SI,2 \ exit loop
	$NEXT

CODE ?branch ( f -- ) \ Branch if flag is zero.
	POP BX \ pop flag
	OR BX,BX \ ?flag=0
	JZ BRAN1 \ yes, so branch
	ADD SI,2 \ point IP to next cell
	$NEXT

BRAN1:MOV SI,0[SI] \ IP:=(IP), jump to new address
	$NEXT

CODE branch ( -- ) \ Branch to an inline address.
	MOV SI,0[SI] \ jump to new address unconditionally
	$NEXT


CODE ! ( w a -- ) \ Pop the data stack to memory.
	POP BX \ get address from tos
	POP 0[BX] \ store data to that adddress
	$NEXT

CODE @ ( a -- w ) \ Push memory location to data stack.
	POP BX \ get address
	PUSH 0[BX] \ fetch data
	$NEXT

CODE C! ( c b -- ) \ Pop data stack to byte memory.
	POP BX \ get address
	POP AX \ get data in a cell
	MOV 0[BX],AL \ store one byte
	$NEXT

CODE C@ ( b -- c ) \ Push byte memory content on data stack.
	POP BX \ get address
	XOR AX,AX \ AX=0 zero the hi byte
	MOV AL,0[BX] \ get low byte
	PUSH AX \ push on stack
	$NEXT


CODE RP@ ( -- a ) \ Push current RP to data stack.
	PUSH BP \ copy address to return stack
	$NEXT \ pointer register BP

CODE RP! ( a -- ) \ Set the return stack pointer.
	POP BP \ copy (BP) to tos
	$NEXT

CODE R> ( -- w ) \ Pop return stack to data stack.
	PUSH 0[BP] \ copy w to data stack
	ADD BP,2 \ adjust RP for popping
	$NEXT

CODE R@ ( -- w ) \ Copy top of return stack to data stack.
	PUSH 0[BP] \ copy w to data stack
	$NEXT

CODE >R ( w -- ) \ Push data stack to return stack.
	SUB BP,2 \ adjust RP for pushing
	POP 0[BP] \ push w to return stack
	$NEXT


CODE DROP ( w -- ) \ Discard top stack item.
	ADD SP,2 \ adjust SP to pop
	$NEXT

CODE DUP ( w -- w w ) \ Duplicate the top stack item.
	MOV BX,SP \ use BX to index the stack
	PUSH 0[BX]
	$NEXT

CODE SWAP ( w1 w2 -- w2 w1 ) \ Exchange top two stack items.
	POP BX \ get w2
	POP AX \ get w1
	PUSH BX \ push w2
	PUSH AX \ push w1
	$NEXT

CODE OVER ( w1 w2 -- w1 w2 w1 ) \ Copy second stack item to top.
	MOV BX,SP \ use BX to index the stack
	PUSH 2[BX] \ get w1 and push on stack
	$NEXT

CODE SP@ ( -- a ) \ Push the current data stack pointer.
	MOV BX,SP \ use BX to index the stack
	PUSH BX \ push SP back
	$NEXT

CODE SP! ( a -- ) \ Set the data stack pointer.
	POP SP \ safety
	$NEXT
	
( Logical Words )

CODE 0< ( n -- f ) \ Return true if n is negative.
	POP AX
	CWD \ sign extend AX into DX
	PUSH DX \ push 0 or -1
	$NEXT

CODE AND ( w w -- w ) \ Bitwise AND.
	POP BX
	POP AX
	AND BX,AX
	PUSH BX
	$NEXT

CODE OR ( w w -- w ) \ Bitwise inclusive OR.
	POP BX
	POP AX
	OR BX,AX
	PUSH BX
	$NEXT

CODE XOR ( w w -- w ) \ Bitwise exclusive OR.
	POP BX
	POP AX
	XOR BX,AX
	PUSH BX
	$NEXT

( Primitive Arithmetic )

CODE UM+ ( w w -- w cy )
\ Add two numbers, return the sum and carry flag.
	XOR CX,CX \ CX=0 initial carry flag
	POP BX
	POP AX
	ADD AX,BX
	RCL CX,1 \ get carry
	PUSH AX \ push sum
	PUSH CX \ push carry
	$NEXT

