%{
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
    public BlockNode root; // �������� ���� ��������������� ������ 
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%union { 
            public double dVal; 
            public int iVal; 
            public string sVal; 
            public Node nVal;
            public ExprNode eVal;
            public StatementNode stVal;
            public LogicExprNode logVal;
            public BlockNode blVal;
       }

%using ProgramTree;

%namespace SimpleParser

%token BEGIN END ASSIGN SEMICOLON WHILE FOR TO WRITE PRINTLN SKOBKA_O SKOBKA_C IF THEN ELSE VAR ZP ADD SUB MULT DIV EQUALS LOGIC_AND TRUE FALSE LOGIC_OR
%token <iVal> INUM 
%token <dVal> RNUM 
%token <sVal> ID

%type <eVal> expr ident params T F
%type <logVal> logic_expr logic_T  logic_F
%type <stVal> assign statement while for write if var
%type <blVal> stlist block

%%

progr   : block { root = $1; }
        ;

stlist  :
            { 
                $$ = new BlockNode(); 
            }
        | stlist statement 
            { 
                $1.Add($2); 
                $$ = $1; 
            }
        ;

statement: assign SEMICOLON { $$ = $1; }
        | while   { $$ = $1; }
        | for     { $$ = $1; }
        | write SEMICOLON   { $$ = $1; }
        | if     { $$ = $1; }
        | var SEMICOLON     { $$ = $1; }
        | block   { $$ = $1; }
    ;

ident   : ID { $$ = new IdNode($1); }   
        ;

write   : WRITE SKOBKA_O expr SKOBKA_C { $$ = new WriteNode($3); }
        ;
    
assign  : ident ASSIGN expr { $$ = new AssignNode($1 as IdNode, $3); }
        ;
        
        
var     : VAR params { $$ = new VarNode($2 as ParamsNode); }
        ;

//expr  : ident  { $$ = $1 as IdNode; }
//      | INUM { $$ = new IntNumNode($1); }
//      ;

logic_expr  :   logic_T { $$ = $1 as LogicOperationNode; }
            |   logic_expr LOGIC_OR logic_T { $$ = new LogicOperationNode($1, $3, SimpleParser.Tokens.LOGIC_OR); }
            ;
            
            //logic_equals  : expr EQUALS expr { $$ = new EqualsNode($1, $3); }
//        ;
logic_T     :   logic_F { $$ = $1 as LogicExprNode; }
            |   logic_F EQUALS logic_F  { $$ = new LogicOperationNode($1, $3, SimpleParser.Tokens.EQUALS); }
            |   logic_T LOGIC_AND logic_F { $$ = new LogicOperationNode($1, $3, SimpleParser.Tokens.LOGIC_AND); }
            ;
            
logic_F     :   ident {$$ = new LogicIdNode($1 as IdNode); }
            |   TRUE { $$ = new LogicNumNode(true); }
            |   FALSE { $$ = new LogicNumNode(false); }
            |   SKOBKA_O logic_expr SKOBKA_C { $$ = $2 as LogicExprNode; }
            ;
            

expr    : T { $$ = $1 as OperationNode; }
        | expr ADD T { $$ = new OperationNode($1, $3, SimpleParser.Tokens.ADD); }
        | expr SUB T { $$ = new OperationNode($1, $3, SimpleParser.Tokens.SUB); }
        ;
        
T       : F { $$ = $1 as ExprNode; }
        | T MULT F { $$ = new OperationNode ( $1, $3, SimpleParser.Tokens.MULT); }
        | T DIV F { $$ = new OperationNode ($1, $3, SimpleParser.Tokens.DIV); }
        ;
        
F       : ident { $$ = $1 as IdNode; }
        | INUM { $$ = new IntNumNode($1); }
        | RNUM { $$ = new DoubleNumNode($1); }
        | SKOBKA_O expr SKOBKA_C { $$ = $2 as ExprNode; }
        ;
        
params  : expr { $$ =  new ParamsNode($1); }
        | params ZP expr {$$ = new ParamsNode($3, $1 as ParamsNode); }
        ;
        
if      : IF SKOBKA_O logic_expr SKOBKA_C statement { $$ = new IfNode($3, $5); }
        | IF SKOBKA_O logic_expr SKOBKA_C statement ELSE statement { $$ = new IfNode($3, $5, $7); }
        ;

block   : BEGIN stlist END { $$ = $2; }
        ;
        
        
while   : WHILE SKOBKA_O logic_expr SKOBKA_C statement { $$ = new WhileNode($3, $5); }
        ;
        
        
for     : FOR SKOBKA_O assign TO expr SKOBKA_C statement { $$ = new ForNode($3 as AssignNode, $5, $7); }
        ;
    
%%

