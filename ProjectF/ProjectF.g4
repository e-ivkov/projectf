grammar ProjectF;

program
   : (declaration|statement)  (';' (declaration|statement))* (';')? EOF
   ;

declaration
   : variable (':' type)? ('is' expression)?
   ;

variable
   : Identifier;

expressions
   : expression (',' expression)* 
   ;

logicalOp
   : 'and' | 'or' | 'xor';

expression
   : relation (logicalOp relation)?
   ;

relationOp
   : '<' | '<=' | '>' | '>=' | '=' | '/=';

relation
   : factor (relationOp factor)?
   ;

factorOp
	: '+' | '-';

factor
   : term  (factorOp term)*
   ;


termOp
	: '*' | '/';

term
   : unary  (termOp unary)*
   ;

unary
   : (factorOp)?  secondary
   ;

secondary
   : primary tail*
   ;

primary
   :    '(' expression ')'
   |    elementary
   |    function
   |    tuple
   |    map
   |    list
   ;

tail
   :    '(' expressions? ')'
   |    '[' expression ']'
   |    '.' Identifier
   |    '.' IntegerLiteral
   ;

elementary
   :    'false' | 'true'
   |    IntegerLiteral
   |    RealLiteral
   |    RationalLiteral
   |    ComplexLiteral
   |    StringLiteral
   |    Identifier
   ;

function
   : 'func' '(' parameters? ')' (':' type)? body
   ;

parameters
   : variable ':' type (',' variable ':' type)*
   ;

body
   : 'do' statements 'end'
   | '=>' expression
   ;

tuple
   : '[' (tupleElement (',' tupleElement)*)? ']'
   ;

tupleElement
   : (Identifier 'is')? expression
   ;

map
   : '{' (mapElement (',' mapElement)*)? '}'
   ;

mapElement
   : expression ':' expression
   ;

list
   : '(' (expressions)? ')'
   ;

type
   :    'bool'
   |    'integer'
   |    'real'
   |    'rational'
   |    'complex'
   |    'string'
   |	'void'
   |    'func' '(' (type (',' type)*)? ')' (':' type)?
   |    '{}'
   |    '[' type ':' type ']'
   |    '(' type ')'
   ;

statements
   : (declaration|statement)  (';' (declaration|statement))* (';')?
   ;

statement
   : assignmentOrCall
   | conditional
   | loop
   | 'return' expression?
   | 'break'
   | declaration
   ;

assignmentOrCall
   : secondary (':=' expression)?
   ;

conditional
   : 'if' expression 'then' statements ('else' statements)? 'end'
   ;

whileloop
	: ('while' expression) loopbody
	;

forloop
	: 'for' (variable 'in')? expression ('..' expression) loopbody
	;

loop
   : forloop
   | whileloop
   ;

loopbody
   : 'loop' statements 'end'
   ;

StringLiteral
    :   '"' StringCharacters? '"'
    ;
fragment
StringCharacters
    :   StringCharacter+
    ;
fragment
StringCharacter
    :   ~["\\]
    |   EscapeSequence
    ;

fragment
EscapeSequence
    :   '\\' [btnfr"'\\]
    |   UnicodeEscape
    ;

fragment
UnicodeEscape
    :   '\\' 'u' HexDigit HexDigit HexDigit HexDigit
    ;

fragment
HexDigit
    :   [0-9a-fA-F]
    ;

ComplexLiteral
    :   (Digits '.' Digits?)? 'i' (Digits '.' Digits?)
    |   IntegerLiteral? 'i' Digits
    |   (Digits '.' Digits?)? 'i' Digits
    |   Digits? 'i' (Digits '.' Digits?)
    ;

RationalLiteral
    :   Digits '\\' Digits?
    ;

RealLiteral
    :   Digits '.' Digits? ExponentPart? FloatTypeSuffix?
    |   '.' Digits ExponentPart? FloatTypeSuffix?
    |   Digits ExponentPart FloatTypeSuffix?
    |   Digits FloatTypeSuffix
    ;

fragment
Digits
    :   Digit+
    ;

fragment
Digit
    :   '0'
    |   NonZeroDigit
    ;

fragment
NonZeroDigit
    :   [1-9]
    ;

fragment
ExponentPart
    :   ExponentIndicator SignedInteger
    ;

fragment
ExponentIndicator
    :   [eE]
    ;

fragment
FloatTypeSuffix
    :   [fFdD]
    ;

fragment
SignedInteger
    :   Sign? Digits
    ;

fragment
Sign
    :   [+-]
    ;

Identifier
    :   Letter (Letter | Digit)*
    ;

IntegerLiteral
    :   Digit+
    ;

fragment
Letter
    :   [a-zA-Z$_]
    ;


WS
	: [ \t\r\n]+ -> channel(HIDDEN)
	;
