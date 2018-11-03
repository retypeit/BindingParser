Program = 
		TextBlock ( "{" BindingBlock "}" (TextBlock) )*
		|"{" BindingBlock "}"

BindingBlock= 
		Condition (? Expression : expression)
		| condition
		| expression
		
Condition =
		Expression ( "==" | "!=" | "<" | ">" ) expression

Expression = 
		Term ( "+" | "-" ) term

Term =
		Factor (("*" | "/" ) factor)*

Factor =
		Function
		| Identity
		| Identity ?? Factor	// Value with null default
		| Identity ??? Factor	// Value with null or undefined default
		|(-|+)? number
		| """ string """
		| "'" char "'"
		| "(" Statement")"

Function =
		Ident "(" params ")"

Params =
		( Statement ( "," BindingBlock )* )*

Identity = 
        (@)? (a-z|A-z)+ (a-z|A-Z|_|0-9)*

TextBlock =
       [^{]*

ValueWithDefault= 
        