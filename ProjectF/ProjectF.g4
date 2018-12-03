grammar ProjectF;

program
   : declaration (';' declaration)* EOF
   ;

declaration
   : variable (':' type)? 'is' expression
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

factor
   : term  ('+' | '-' term)*
   ;

term
   : unary  ('*' | '/' unary)*
   ;

unary
   : ('+' | '-')?  secondary
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
   : declaration (',' declaration)*
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
   : '(' expressions? ')'
   ;

type
   :    'bool'
   |    'integer'
   |    'real'
   |    'rational'
   |    'complex'
   |    'string'
   |    'func' '(' (type (',' type)*)? ')' (':' type)?
   |    '{}'
   |    '[' type ':' type ']'
   |    '(' type ')'
   ;

statements
   : statement (';' statement)*
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

loop
   : 'for' (Identifier 'in')? expression ('..' expression) loopbody
   | ('while' expression) loopbody
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
    :   Letter+
    ;

IntegerLiteral
    :   Digit+
    ;

fragment
Letter
    :   [a-zA-Z$_]
    ;


WS
	:	' ' -> channel(HIDDEN)
	;
